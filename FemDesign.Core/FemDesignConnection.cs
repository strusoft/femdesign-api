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
using System.Threading.Tasks;

using FemDesign;
using FemDesign.Calculate;

namespace FemDesign
{
    /// <summary>
    /// FEM-Design real-time connection. Use this to open a <see cref="Model"/>, run <see cref="Analysis"/>, <see cref="Design"/>, extract results and more.
    /// </summary>
    public class FemDesignConnection : IDisposable
    {
        private readonly PipeConnection _connection;
        private readonly Process _process;
        public bool HasExited { get; private set; }
        public bool IsConnected => _connection._inputPipe.IsConnected;
        public bool IsDisconnected => !IsConnected;

        /// <summary>
        /// Keep FEM-Design open after <see cref="Dispose"/> is called.
        /// </summary>
        private bool _keepOpen;


        public delegate void OnOutputEvent(string output);
        /// <summary>
        /// Occurs whenever FEM-Design writes a new log message.
        /// 
        /// Verbosity may be adjusted using <see cref="SetVerbosity(Verbosity)"/>
        /// </summary>
        public OnOutputEvent OnOutput { get; set; } = null;

        /// <summary>
        /// Open a new instance of FEM-Design and connect to it.
        /// </summary>
        /// <param name="fdInstallationDir"></param>
        /// <param name="minimized">Open FEM-Design as a minimized window.</param>
        /// <param name="keepOpen">If true FEM-Design will be left open and have to be manually exited.</param>
        /// <param name="outputDir">The directory to save script files. If set to null, the files will be will be written to a temporary directory and deleted after.</param>
        /// <param name="tempOutputDir"><code>BE CAREFUL!</code>If true the <paramref name="outputDir"/> will be deleted on exit. This option has no effect unless <paramref name="outputDir"/> has been specified.</param>
        public FemDesignConnection(
            string fdInstallationDir = @"C:\Program Files\StruSoft\FEM-Design 21\",
            bool minimized = false,
            bool keepOpen = false,
            string outputDir = null,
            bool tempOutputDir = false)
        {
            string pathToFemDesign = Path.Combine(fdInstallationDir, "fd3dstruct.exe");

            string pipeName = "FdPipe" + Guid.NewGuid().ToString();
            var startInfo = new ProcessStartInfo()
            {
                FileName = pathToFemDesign,
                Arguments = "/p " + pipeName,
                UseShellExecute = false,
                Verb = "open",
            };
            if (minimized)
                startInfo.EnvironmentVariables["FD_NOGUI"] = "1";

            OutputDir = outputDir;
            if (string.IsNullOrEmpty(outputDir) == false && tempOutputDir)
                _outputDirsToBeDeleted.Add(OutputDir);

            _keepOpen = keepOpen;

            _connection = new PipeConnection(pipeName);
            _process = Process.Start(startInfo);
            _process.Exited += ProcessExited;
            _connection.WaitForConnection();

            // Forward all output messages from pipe (except echo guid commands).
            _connection.OnOutput += (message) => {
                message = message.Replace(">echo ", "");
                bool isGuid = Guid.TryParse(message, out Guid _);
                if (isGuid == false)
                    OnOutput?.Invoke(message);
            };
        }

        private void ProcessExited(object sender, EventArgs e)
        {
            this.HasExited = true;
        }

        /// <summary>
        /// Disconnects the current connection. FEM-Design will be left open for normal usage.
        /// </summary>
        public void Disconnect()
        {
            this._connection.Send("detach"); // Tell FEM-Design to detach from the pipe
            this._connection.Disconnect();
        }

        /// <summary>
        /// FEM-Design will be left open, when the <see cref="FemDesignConnection"/> is disposed.
        /// </summary>
        public void KeepOpen()
        {
            _keepOpen = true;
        }

        /// <summary>
        /// Run a script and wait for it to finish.
        /// </summary>
        /// <param name="script"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void RunScript(FdScript script)
        {
            if (script == null) throw new ArgumentNullException("script");

            string scriptPath = OutputFileHelper.GetFdScriptPath(OutputDir);

            script.Serialize(scriptPath);
            this._connection.Send("run " + scriptPath);
            this._connection.WaitForCommandToFinish();
        }

        /// <summary>
        /// Run a script and wait for it to finish.
        /// </summary>
        /// <param name="script"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task RunScriptAsync(FdScript script)
        {
            if (script == null) throw new ArgumentNullException("script");

            string scriptPath = OutputFileHelper.GetFdScriptPath(OutputDir);

            script.Serialize(scriptPath);
            this._connection.Send("run " + scriptPath);
            await this._connection.WaitForCommandToFinishAsync();
        }

        /// <summary>
        /// Open a file in FEM-Design application.
        /// </summary>
        /// <param name="filePath">The model file to be opened. Typically a .str or .struxml file, but any filetype supported in FEM-Design is valid.</param>
        /// <param name="disconnect">Set to True to disconnect to the pipe and leave FEM-Design Open.</param>
        public void Open(string filePath, bool disconnect = false)
        {
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            this.RunScript(new FdScript(logfile, new CmdOpen(filePath)));
            if (disconnect) this.Disconnect();
        }

        /// <summary>
        /// Open a file in FEM-Design application.
        /// </summary>
        /// <param name="filePath">The model file to be opened. Typically a .str or .struxml file, but any filetype supported in FEM-Design is valid.</param>
        public async Task OpenAsync(string filePath)
        {
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            await this.RunScriptAsync(new FdScript(logfile, new CmdOpen(filePath)));

        }

        /// <summary>
        /// Open a <see cref="Model"/> in FEM-Design application.
        /// </summary>
        /// <param name="model">Model to be opened.</param>
        /// <param name="disconnect">Set to True to disconnect to the pipe and leave FEM-Design Open.</param>
        public void Open(Model model, bool disconnect = false)
        {
            var struxml = OutputFileHelper.GetStruxmlPath(OutputDir);
            // Model must be serialized to a file to be opened in FEM-Design.
            model.SerializeModel(struxml);
            this.Open(struxml, disconnect);
        }

        public void SetGlobalConfig(Calculate.CmdGlobalCfg cmdglobalconfig)
        {
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            var script = new FdScript(
                logfile,
                cmdglobalconfig
            );
            this.RunScript(script);
        }

        /// <summary>
        /// Open a <see cref="Model"/> in FEM-Design application.
        /// </summary>
        /// <param name="model">Model to be opened.</param>
        public async Task OpenAsync(Model model)
        {
            var struxml = OutputFileHelper.GetStruxmlPath(OutputDir);
            // Model must be serialized to a file to be opened in FEM-Design.
            model.SerializeModel(struxml);
            await this.OpenAsync(struxml);
        }

        /// <summary>
        /// Runs an analysis task on the current model in FEM-Design.
        /// </summary>
        /// <param name="analysis">The analysis to be run. Defaults to static analysis (<see cref="Analysis.StaticAnalysis(Comb, bool, bool)"/>)</param>
        public void RunAnalysis(Analysis analysis = null)
        {
            if (analysis is null)
                analysis = Analysis.StaticAnalysis();

            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            var script = new FdScript(
                logfile,
                new CmdUser(CmdUserModule.RESMODE),
                new CmdCalculation(analysis)
            );
            this.RunScript(script);
        }

        /// <summary>
        /// Opens <paramref name="model"/> in FEM-Design and runs the analysis.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="analysis"></param>
        public void RunAnalysis(Model model, Analysis analysis)
        {
            this.Open(model);
            this.RunAnalysis(analysis);
        }

        /// <summary>
        /// Runs a design task on the current model in FEM-Design.
        /// </summary>
        /// <param name="userModule"></param>
        /// <param name="design"></param>
        /// <exception cref="ArgumentException"></exception>
        public void RunDesign(CmdUserModule userModule, Design design)
        {
            if (userModule == CmdUserModule.RESMODE)
            {
                throw new ArgumentException("User Module can not be 'RESMODE'!");
            }

            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);

            var script = new FdScript(
                logfile,
                new CmdUser(userModule),
                new CmdCalculation(design)
            );

            if (design.ApplyChanges == true) { script.Add(new CmdDesignDesignChanges()); }

            this.RunScript(script);
        }

        /// <summary>
        /// Opens <paramref name="model"/> in FEM-Design and runs the design.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="design"></param>
        /// <param name="userModule"></param>
        public void RunDesign(Model model, Design design, CmdUserModule userModule)
        {
            this.Open(model);
            this.RunDesign(userModule, design);
        }

        public void SetVerbosity(Verbosity verbosity)
        {
            _connection.Send("v " + (int)verbosity);
        }

        public void EndSession()
        {
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);

            var script = new FdScript(
                logfile,
                new CmdEndSession()
            );

            this.RunScript(script);
        }

        /// <summary>
        /// Retrieves the currently opened model with all available elements as a <see cref="Model"/> object.
        /// </summary>
        public Model GetModel()
        {
            string struxmlPath = OutputFileHelper.GetStruxmlPath(OutputDir, "model_saved");
            string logfilePath = OutputFileHelper.GetLogfilePath(OutputDir);
            RunScript(new FdScript(logfilePath, new CmdSave(struxmlPath)));
            return Model.DeserializeFromFilePath(struxmlPath);
        }

        /// <summary>
        /// Retreive results from the opened model.
        /// </summary>
        /// <typeparam name="T">Result type to retrieve. Must be a type that implements the <see cref="Results.IResult"/> interface</typeparam>
        /// <param name="units">Optional. Unit setting for the results.</param>
        /// <returns>List of results of type <typeparamref name="T"/> if any could be retrieved. If the model has no results of type <typeparamref name="T"/> or cannot access them at the moment, then the list will be empty.</returns>
        public List<T> GetResults<T>(Results.UnitResults units = null) where T : Results.IResult
        {
            if (units is null)
                units = Results.UnitResults.Default();

            // Input bsc files and output csv files
            var listProcs = typeof(T).GetCustomAttribute<Results.ResultAttribute>()?.ListProcs ?? Enumerable.Empty<ListProc>();
            var bscPaths = listProcs.Select(l => OutputFileHelper.GetBscPath(OutputDir, l.ToString())).ToList();
            var csvPaths = listProcs.Select(l => OutputFileHelper.GetCsvPath(OutputDir, l.ToString())).ToList();

            var bscs = listProcs.Zip(bscPaths, (l, p) => new Bsc(l, p, units, true)).ToList();
            bscs.ForEach(b => b.SerializeBsc());

            // FdScript commands
            List<CmdCommand> listGenCommands = new List<CmdCommand>();
            listGenCommands.Add(new CmdUser(CmdUserModule.RESMODE));
            for (int i = 0; i < bscPaths.Count; i++)
                //listGenCommands.Add(new CmdListGen(bscPaths[i], csvPaths[i]));
                listGenCommands.Add(new CmdListGen(bscPaths[i], csvPaths[i]));

            // Run the script
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            var script = new FdScript(logfile, listGenCommands.ToArray());
            this.RunScript(script);

            // Read csv results files
            List<T> results = new List<T>();
            foreach (string resultFile in csvPaths)
            {
                results.AddRange(
                    Results.ResultsReader.Parse(resultFile).ConvertAll(r => (T)r)
                );
            }

            return results;
        }



        public List<T> GetLoadCaseResults<T>(Loads.LoadCase loadCase, Results.UnitResults units = null) where T : Results.IResult
        {
            var mapCase = new MapCase(loadCase.Name);

            if (units is null)
                units = Results.UnitResults.Default();

            // Input bsc files and output csv files
            var listProcs = typeof(T).GetCustomAttribute<Results.ResultAttribute>()?.ListProcs.Where(p =>  p.IsLoadCase() == true) ?? Enumerable.Empty<ListProc>();

            // listproc that are only load case

            var bscPaths = listProcs.Select(l => OutputFileHelper.GetBscPath(OutputDir, l.ToString())).ToList();
            var csvPaths = listProcs.Select(l => OutputFileHelper.GetCsvPath(OutputDir, l.ToString())).ToList();

            var bscs = listProcs.Zip(bscPaths, (l, p) => new Bsc(l, p, units, false)).ToList();
            bscs.ForEach(b => b.SerializeBsc());

            // FdScript commands
            List<CmdCommand> listGenCommands = new List<CmdCommand>();
            listGenCommands.Add(new CmdUser(CmdUserModule.RESMODE));
            for (int i = 0; i < bscPaths.Count; i++)
                listGenCommands.Add(new CmdListGen(bscs[i], csvPaths[i], false, mapCase));

            // Run the script
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            var script = new FdScript(logfile, listGenCommands.ToArray());
            this.RunScript(script);

            // Read csv results files
            List<T> results = new List<T>();
            foreach (string resultFile in csvPaths)
            {
                results.AddRange(
                    Results.ResultsReader.Parse(resultFile).ConvertAll(r => (T)r)
                );
            }

            return results;
        }


        public List<T> GetLoadCombinationResults<T>(Loads.LoadCombination loadCombination, Results.UnitResults units = null) where T : Results.IResult
        {
            var mapComb = new MapComb(loadCombination.Name);

            if (units is null)
                units = Results.UnitResults.Default();

            // Input bsc files and output csv files
            var listProcs = typeof(T).GetCustomAttribute<Results.ResultAttribute>()?.ListProcs.Where(p => p.IsLoadCombination() == true) ?? Enumerable.Empty<ListProc>();

            // listproc that are only load case

            var bscPaths = listProcs.Select(l => OutputFileHelper.GetBscPath(OutputDir, l.ToString())).ToList();
            var csvPaths = listProcs.Select(l => OutputFileHelper.GetCsvPath(OutputDir, l.ToString())).ToList();

            var bscs = listProcs.Zip(bscPaths, (l, p) => new Bsc(l, p, units, false)).ToList();
            bscs.ForEach(b => b.SerializeBsc());

            // FdScript commands
            List<CmdCommand> listGenCommands = new List<CmdCommand>();
            listGenCommands.Add(new CmdUser(CmdUserModule.RESMODE));
            for (int i = 0; i < bscPaths.Count; i++)
                listGenCommands.Add(new CmdListGen(bscs[i], csvPaths[i], false, mapComb));

            // Run the script
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            var script = new FdScript(logfile, listGenCommands.ToArray());
            this.RunScript(script);

            // Read csv results files
            List<T> results = new List<T>();
            foreach (string resultFile in csvPaths)
            {
                results.AddRange(
                    Results.ResultsReader.Parse(resultFile).ConvertAll(r => (T)r)
                );
            }

            return results;
        }

        public void Save(string filePath)
        {
            string logfile = OutputFileHelper.GetLogfilePath(OutputDir);
            var script = new FdScript(logfile, new CmdSave(filePath));
            this.RunScript(script);
        }

        public void Dispose()
        {
            if (_keepOpen) Disconnect();

            this._connection.Dispose();

            // TODO: Delete the files when they are not locked by FEM-Design
            //_deleteOutputDirectories();
        }

        private void _deleteOutputDirectories()
        {
            foreach (string dir in _outputDirsToBeDeleted)
                if (Directory.Exists(dir))
                    Directory.Delete(dir, true);
        }

        public string OutputDir
        {
            get { return _outputDir; }
            set
            {
                if (string.IsNullOrEmpty(value)) // Use temp dir
                {
                    _outputDir = Path.Combine(Directory.GetCurrentDirectory(), "FEM-Design API");
                    _outputDirsToBeDeleted.Add(_outputDir);
                }
                else // Use given directory
                    _outputDir = Path.GetFullPath(value);
            }
        }
        private string _outputDir;
        private List<string> _outputDirsToBeDeleted = new List<string>();
    }

    /*
    FEM-Design usage with named pipe.

    To initiate:
    1:  Create a WIN32 named pipe for duplex mode, message oriented
    1a: optional: Create another pipe for back channel, named appending 'b'.
    2:  Launch FD with command line argument `/p Name` passing the name you used at creation. FD will open it right at start and exit if can't. After successful launch it listens to commands while the usual interface is active. You can combine it with the windowless / minimized mode to hide the window. It also attaches to the back channel pipe at this moment, if unable, all output is permanently disabled.
    3:  Send commands through the pipe.
    4:  FD will exit if 'exit' command received or the pipe is closed on this end.

    FD only reads the main pipe and only writes the back channel (if supplied), allowing this end to never read.
    While the pipe is duplexand, it can be used in both direction. If it gets clogged in one direction (by not reading what the other end sends), the write can get blocked too.
    The document recommends using another pipe for a back channel.
    By default nothing is written to the back channel, you need to set output level (using verbosity, v, comand) or use commands with implicit reply.
    FD buffers all outgoing messages till they can be sent over, if this end is lazy to read, it will not clog, however they will accumulate in memory.
     */
    internal class PipeConnection : IDisposable
    {
        /// <summary>
        /// Connect to FEM-Design using Named Pipe
        /// </summary>
        /// <param name="pipeBaseName"></param>
        /// <exception cref="Exception"></exception>
        public PipeConnection(string pipeBaseName = "FdPipe1")
        {
            // todo(Gustav): figure out 9-bit encoding?
            // encoding = System.Text.Encoding.GetEncoding(1252); // https://nicolaiarocci.com/how-to-read-windows-1252-encoded-files-with-.netcore-and-.net5-/
            _encoding = System.Text.Encoding.ASCII;

            string input_name = pipeBaseName;
            string output_name = pipeBaseName + "b";

            _inputPipe = create_pipe(input_name);
            _outputPipe = create_pipe(output_name);

            _startOutputThread();

            // this is what check status does...
            if (_inputPipe == null) { throw new Exception("setup failed"); }
        }

        public void WaitForConnection()
        {
            _inputPipe.WaitForConnection();
        }

        public delegate void OnOutputEvent(string output);
        public OnOutputEvent OnOutput { get; set; } = null;

        /// <summary>
        /// Send command to FEM-Design using the named pipe.
        /// </summary>
        /// <param name="command">Command to excecute in FEM-Design.</param>
        public void Send(string command)
        {
            /*
            The command format is
            [!]<cmd> [args]
            there is no delimiter at the end, the pipe message counts.
            FEM-Design reads the pipe immediately and puts the commands in a queue. The queue is processed when it's READYSTATE
            is ready for another command, finishing execution of the previous or a current script.
            
            The !requests out of bound execution. That is not supported by very command and mainly
            serves to manipulate the queue itself, verbosity or check the communicaiton is alive.
        
            Messages are text in 9bit ANSI (codepage), limited to 4096 bytes.

            Commands:
        
            exit
            Stop the FD process
        
            detach
            Close the pipe and continue in normal interface mode
        
            clear [in|out]
            Flush the FD mesage queue for the direction, or both if send without in or out parameters.
            Has no Effect on what is already issued to the pipe
        
            echo [txt]
            Write txt to output
        
            stat
            Write queue and processing status to output
        
            v [N]
            Set verbosity control (bits)
               1: Enable basic output
               2: Echo all INPUT commands
               4: FEM-Design log-lines
               8: Script log lines
              16: Calculation window messages (except fortran)
             *32: Progress window title
        
            Commands echo and stat always creates output, otherwise nothing is written at V = 0
            * Not yet supported
        
            run [scriptfile]
            Execute script as from tools / run script menu.
        
            cmd [command]
            Execute command as if typed into the command window. No warranty!
        
            esc
            Escape during calculation to break/stop it.
            */

            if (_inputPipe.CanWrite == false) throw new Exception("Can't write to pipe");
            var buffer = _encoding.GetBytes(command);
            _inputPipe.Write(buffer, 0, buffer.Length);
            _inputPipe.Flush();
        }

        internal void WaitForCommandToFinish()
        {
            var guid = Guid.NewGuid();
            this.Send("echo " + guid);
            this._waitForOutput(guid);
        }

        internal async Task WaitForCommandToFinishAsync()
        {
            var guid = Guid.NewGuid();
            this.Send("echo " + guid);
            await this._waitForOutputAsync(guid);
        }

        // ----------------------------------------------------------------------------------------

        public void Disconnect()
        {
            _inputPipe.Disconnect();
        }

        public void Dispose()
        {
            _inputPipe.Dispose();
            _outputPipe.Dispose();
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

        private async Task _waitForOutputAsync(Guid guid)
        {
            await this._waitForOutputAsync(guid.ToString());
        }

        private void _waitForOutput(string output)
        {
            _waitForOutputAsync(output).Wait();
        }

        private async Task _waitForOutputAsync(string output)
        {
            var tcs = new TaskCompletionSource<bool>();
            void onOutput(string msg)
            {
                if (msg == output)
                {
                    this.OnOutput -= onOutput;
                    tcs.SetResult(true);
                }
            }
            this.OnOutput += onOutput;

            await tcs.Task;
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

        //internal void Disconnect()
        //{
        //    this._inputPipe.Disconnect();
        //    this._outputPipe.Disconnect();
        //}

        // ----------------------------------------------------------------------------------------

        internal readonly NamedPipeServerStream _inputPipe;
        internal readonly NamedPipeServerStream _outputPipe;
        readonly BackgroundWorker _outputWorker = new BackgroundWorker();
        private readonly System.Text.Encoding _encoding;
        private const int BUFFER_SIZE = 4096;
        private bool _sendOutputToEvent = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public enum Verbosity
    {
        EchoAndStatOnly = 0,
        /// <summary>
        /// Enable basic output
        /// </summary>
        BasicOnly = 1,
        /// <summary>
        /// Echo all INPUT commands
        /// </summary>
        InputOnly = 2,
        /// <summary>
        /// FEM-Design log-lines
        /// </summary>
        LogLinesOnly = 4,
        /// <summary>
        /// Script log lines
        /// </summary>
        ScriptLogLinesOnly = 8,
        /// <summary>
        /// Calculation window messages (except fortran)
        /// </summary>
        CalculationMessagesOnly = 16,
        /// <summary>
        /// Progress window title
        /// </summary>
        ProgressWindowTitleOnly = 32,

        None = EchoAndStatOnly,
        Low = BasicOnly | InputOnly,
        Normal = BasicOnly | InputOnly | LogLinesOnly | ScriptLogLinesOnly,
        High = BasicOnly | InputOnly | LogLinesOnly | ScriptLogLinesOnly | CalculationMessagesOnly,
        All = BasicOnly | InputOnly | LogLinesOnly | ScriptLogLinesOnly | CalculationMessagesOnly | ProgressWindowTitleOnly,
    }

    public static class OutputFileHelper
    {
        private const string _scriptsDirectory = "scripts";
        private const string _resultsDirectory = "results";
        private const string _bscDirectory = "bsc";

        private const string _logFileName = "logfile.log";
        private const string _struxmlFileName = "model.struxml";
        private const string _strFileName = "model.str";

        private const string _fdscriptFileExtension = ".fdscript";
        private const string _bscFileExtension = ".bsc";
        private const string _csvFileExtension = ".csv";
        public static string GetLogfilePath(string baseDir)
        {
            string logfilePath = Path.Combine(baseDir, _logFileName);
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);
            return Path.GetFullPath(Path.Combine(baseDir, _logFileName));
        }
        public static string GetStruxmlPath(string baseDir, string modelName = null)
        {
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);
            string path;
            if (string.IsNullOrEmpty(modelName))
                path = Path.GetFullPath(Path.Combine(baseDir, _struxmlFileName));
            else
                path = Path.GetFullPath(Path.Combine(baseDir, Path.ChangeExtension(Path.GetFileName(modelName), "struxml")));
                
            return path;
        }
        public static string GetStrPath(string baseDir)
        {
            if (!Directory.Exists(baseDir))
                Directory.CreateDirectory(baseDir);
            string path = Path.GetFullPath(Path.Combine(baseDir, _strFileName));
            return path;
        }
        public static string GetBscPath(string baseDir, string fileName)
        {
            string dir = Path.Combine(baseDir, _scriptsDirectory, _bscDirectory);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            fileName = Path.ChangeExtension(fileName, _bscFileExtension);
            string path = Path.GetFullPath(Path.Combine(dir, fileName));
            return path;
        }
        public static string GetCsvPath(string baseDir, string fileName)
        {
            string dir = Path.Combine(baseDir, _resultsDirectory);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            fileName = Path.ChangeExtension(fileName, _csvFileExtension);
            string path = Path.GetFullPath(Path.Combine(dir, fileName));
            return path;
        }
        public static string GetFdScriptPath(string baseDir, string fileName = "script")
        {
            string dir = Path.Combine(baseDir, _scriptsDirectory);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            fileName = Path.GetFileName(Path.ChangeExtension(fileName, _fdscriptFileExtension));
            string path = Path.GetFullPath(Path.Combine(dir, fileName));
            return path;
        }
    }
}
