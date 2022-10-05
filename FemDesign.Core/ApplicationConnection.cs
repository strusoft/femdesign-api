using System;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Pipes;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using FemDesign;
using FemDesign.Calculate;

namespace FemDesign
{
    /*
     FEM-Design usage with pipe
        To initiate :
        1: create a WIN32 named pipe for duplex mode, message oriented
        1a: optional : create another pipe for back channel, named appending 'b'.
        2: launch FD with command line argument /p Name
        passing the name you used at creation.FD will open it right at start and exit if can't
        after successful open it listens to commands while the usual interface is active
        you can combine it with the windowless / minimized mode to hide the window
        it also attaches to the back channel pipe at this moment, if unable, all output is permanently disabled
        3: send commands through the pipe
        4: FD will exit if 'exit' command received or the pipe is closed on this end

        FD only reads the main pipe and only writes the back channel(if supplied), allowing this end to never
        read.While the pipe is duplexand can be used in both direction, if it gets clogged in
        one direction(by not reading what the other end sends), the write can get blocked too.
        The document recommends using another pipe for a back channel.
        By default nothing is written to the back channel, you need to set output level or commands with implicit reply.
        FD buffers all outgoing messages till they can be sent over, if this end is lazy to read it will not clog,
        however they will accumulate in memory.
     */
    public class ApplicationConnection : IDisposable
    {
        public ApplicationConnection(string fd_installation_folder, string pipe_base_name = "FdPipe1")
        {
            // todo(Gustav): figure out 9-bit encoding?
            // encoding = System.Text.Encoding.GetEncoding(1252); // https://nicolaiarocci.com/how-to-read-windows-1252-encoded-files-with-.netcore-and-.net5-/
            _encoding = System.Text.Encoding.ASCII;

            string input_name = pipe_base_name;
            string output_name = pipe_base_name + "b";

            _inputPipe = create_pipe(input_name);
            _outputPipe = create_pipe(output_name);

            _startOutputThread();

            // this is what check status does...
            if (_inputPipe == null) { throw new Exception("setup failed"); }

            string path_to_fd_struct = Path.Combine(fd_installation_folder, "fd3dstruct.exe");
            this._process = Process.Start(path_to_fd_struct, "/p " + input_name);
            this._process.Exited += Process_Exited;

            _inputPipe.WaitForConnection();
        }

        private void Process_Exited(object sender, EventArgs e)
        {
            this.HasExited = true;
        }

        public delegate void OnOutputEvent(string output);
        public OnOutputEvent OnOutput { get; set; } = null;


        public void Send(string command)
        {
            if (_inputPipe.CanWrite == false) throw new Exception("Can't write to pipe");
            var buffer = _encoding.GetBytes(command);
            _inputPipe.Write(buffer, 0, buffer.Length);
            _inputPipe.Flush();
        }

        /// <summary>
        /// Run a script and wait for it to finish.
        /// </summary>
        /// <param name="script"></param>
        public void RunScript(FdScript script)
        {
            if (script == null) throw new ArgumentNullException("script");
            if (script.FdScriptPath == null) throw new ArgumentNullException("script.FdScriptPath");

            script.SerializeFdScript();
            this.Send("run " + script.FdScriptPath);
            this.WaitForCommandToFinish();
        }

        /// <summary>
        /// Run a script and wait for it to finish.
        /// </summary>
        /// <param name="script"></param>
        public void RunScript(FdScript2 script, string tempPath = null)
        {
            if (script == null) throw new ArgumentNullException("script");

            if (tempPath is null)
            {
                using (var temp = new TemporaryFile(".fdscript"))
                {
                    script.Serialize(temp.FilePath);
                    this.Send("run " + temp.FilePath);
                    this.WaitForCommandToFinish();
                }
            }
            else
            {
                if (File.Exists(tempPath)) throw new ArgumentException("tempPath already exists and would be overritten");
                if (Path.GetExtension(tempPath).ToLower() != ".fdscript")
                    throw new ArgumentException("tempPath must have extension .fdscript");

                script.Serialize(tempPath);
                this.Send("run " + tempPath);
                this.WaitForCommandToFinish();
                File.Delete(tempPath);
            }
        }

        /// <summary>
        /// Open a file in FEM-Design application.
        /// </summary>
        /// <param name="filePath">The model file to be opened. Typically a .str or .struxml file, but any filetype sopported in FEM-Design can be used.</param>
        public void Open(string filePath)
        {
            this.RunScript(FdScript.OpenModel(Path.GetFullPath(filePath)));
        }

        /// <summary>
        /// Open a <see cref="Model"/> in FEM-Design application.
        /// </summary>
        /// <param name="model">Model to be opened.</param>
        /// <param name="tempPath">Optionally override where the temporary struxml file will be saved.</param>
        public void Open(Model model, string tempPath = null)
        {
            using (var temp = new TemporaryFile("struxml"))
            {
                // Model must be serialized to a file to be opened in FEM-Design.
                model.SerializeModel(temp.FilePath);

                this.Open(temp.FilePath);
            }
        }

        public List<T> GetResults<T>(Results.UnitResults units = null) where T : Results.IResult
        {
            if (units is null)
                units = Results.UnitResults.Default();

            var listProcs = typeof(T).GetCustomAttribute<Results.ResultAttribute>()?.ListProcs ?? Enumerable.Empty<ListProc>();
            var tempBscs = listProcs.Select(l => new TemporaryFile(".bsc")).ToList();
            var tempCsvs = listProcs.Select(l => new TemporaryFile(".csv")).ToList();
            var bscs = listProcs.Zip(tempBscs, (l, p) => new Bsc(l, p.FilePath, units)).ToList();
            bscs.ForEach(b => b.SerializeBsc());
            var listGenCommands = tempBscs.Zip(tempCsvs, (b, c) =>
                {
                    return new List<CmdCommand> {
                        new CmdUserModule2(CmdUserModule.RESMODE),
                        new CmdListGen2(c.FilePath, b.FilePath)
                    };
                }).SelectMany(l => l);

            List<T> results = new List<T>();
            using (var tempLog = new TemporaryFile(".log"))
            {
                var script = new FdScript2(tempLog.FilePath, listGenCommands.ToArray());

                this.RunScript(script);

                foreach (string resultFile in tempCsvs.Select(c => c.FilePath))
                {
                    results.AddRange(
                        Results.ResultsReader.Parse(resultFile).ConvertAll(r => (T)r)
                    );
                }
            }

            tempBscs.Concat(tempCsvs).ToList().ForEach(t => t.Dispose());
            return results;
        }

        public void WaitForCommandToFinish()
        {
            var guid = Guid.NewGuid();
            this.Send("echo " + guid);
            this._waitForOutput(guid);
        }

        // ----------------------------------------------------------------------------------------

        public void Dispose()
        {
            _disposePipes();
            _disposeWorker();
        }

        // ----------------------------------------------------------------------------------------

        private static NamedPipeServerStream create_pipe(string name)
        {
            // it seems that c# autoprefixes the \\.\pipe\ string, no need to add
#pragma warning disable CA1416 // Validate platform compatibility
            var pipe = new NamedPipeServerStream(name,
                PipeDirection.InOut,
                NamedPipeServerStream.MaxAllowedServerInstances,
                PipeTransmissionMode.Message,
                PipeOptions.None,
                BUFFER_SIZE, BUFFER_SIZE
                );
#pragma warning restore CA1416 // Validate platform compatibility
            return pipe;
        }

        private void _waitForOutput(Guid guid)
        {
            this._waitForOutput(guid.ToString());
        }

        private void _waitForOutput(string output)
        {
            bool isDone = false;
            void onOutput(string msg)
            {
                if (msg == output)
                {
                    this.OnOutput -= onOutput;
                    isDone = true;
                }
            }
            this.OnOutput += onOutput;

            while (!isDone)
            {
                System.Threading.Thread.Sleep(10); // ms
            }
        }

        private void _disposePipes()
        {
            _inputPipe.Dispose();
            _outputPipe.Dispose();
        }

        // ----------------------------------------------------------------------------------------

        private void _disposeWorker()
        {
            _sendOutputToEvent = false;
            if (_outputWorker.IsBusy)
            {
                _outputWorker.CancelAsync();
            }
        }

        private void _startOutputThread()
        {
            _outputWorker.WorkerSupportsCancellation = true;

            _outputWorker.DoWork += _onOutputThread;

            _outputWorker.WorkerReportsProgress = true;
            _outputWorker.ProgressChanged += _onOuputThreadEvent;

            if (_outputWorker.IsBusy == true) { throw new Exception("background thread already started... bug?"); }
            _outputWorker.RunWorkerAsync();
        }

        private void _onOutputThread(object sender, DoWorkEventArgs e)
        {
            try
            {
                _outputPipe.WaitForConnection();
                while (_outputWorker.CancellationPending == false)
                {
                    var buffer = new byte[BUFFER_SIZE + 1];
                    var read = _outputPipe.Read(buffer, 0, BUFFER_SIZE);
                    var line = _encoding.GetString(buffer, 0, read);
                    if (line == null) { continue; }

                    _outputWorker.ReportProgress(0, line);
                }
            }
            catch (Exception x)
            {
                _outputWorker.ReportProgress(1, x);
            }
        }

        private void _onOuputThreadEvent(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is string line)
            {
                if (_sendOutputToEvent)
                {
                    OnOutput?.Invoke(line);
                }
            }
            else if (e.UserState is Exception ex)
            {
                if (_sendOutputToEvent)
                {
                    throw ex;
                }
            }
            else
            {
                throw new Exception("invalid progress object");
            }
        }

        // ----------------------------------------------------------------------------------------

        private readonly NamedPipeServerStream _inputPipe;
        private readonly NamedPipeServerStream _outputPipe;
        readonly BackgroundWorker _outputWorker = new BackgroundWorker();
        private readonly System.Text.Encoding _encoding;
        private const int BUFFER_SIZE = 4096;
        private readonly Process _process;
        public bool HasExited { get; private set; }

        // ---

        private bool _sendOutputToEvent = true;
    }
}
