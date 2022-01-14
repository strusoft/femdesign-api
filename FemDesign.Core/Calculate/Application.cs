// https://strusoft.com/
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;


namespace FemDesign.Calculate
{
    /// <summary>
    /// This is a simple "application" for fd3dstruct.
    /// Retreive process information of a running fd3dstruct. 
    /// Start and run processes (open, analysis, design etc.
    /// </summary>
    [System.Serializable]
    public partial class Application
    {
        /// <summary>
        /// Path to fd3dstruct.
        /// </summary>
        /// <value></value>
        internal string FdPath { get; set; }

        /// <summary>
        /// Version of running fd3dstruct process.
        /// </summary>
        /// <value></value>
        internal string FdVersion { get; set; }

        /// <summary>
        /// Target version of class library.
        /// </summary>
        internal string FdTargetVersion = "21";

        public Application()
        {
            this.GetProcessInformation();
        }

        /// <summary>
        /// Retreive process information of a running fd3dstruct process.
        /// </summary>
        private void GetProcessInformation()
        {
            Process[] processes = Process.GetProcessesByName("fd3dstruct");
            if (processes.Length > 0)
            {
                // get process information
                Process firstProcess = processes[0];
                try
                {
                    this.FdPath = firstProcess.MainModule.FileName;
                }
                catch (System.Exception e)
                {
                    throw e;
                }
                this.FdVersion = firstProcess.MainModule.FileVersionInfo.FileVersion.Split(new char[] { '.' })[0];
            }
            
            // Check if process information matches target version
            if (this.FdVersion == null || this.FdVersion != this.FdTargetVersion || this.FdPath == null)
            {
                throw new System.ArgumentException("FEM-Design " + this.FdTargetVersion + " - 3D Structure must be running! Start FEM-Design " + this.FdTargetVersion + " - 3D Structure and reload script.");
            }
        }

        /// <summary>
        /// Force shutdown of any running fd3dstruct processes.
        /// </summary>
        private void KillProcesses()
        {
            Process[] processes = Process.GetProcessesByName("fd3dstruct");
            foreach (Process process in processes)
            {
                process.Kill();
            }
        }

        /// <summary>
        /// Check if a fd3dstruct process is running.
        /// </summary>
        internal static void CheckProcess()
        {
            var app = new Application();
            app.GetProcessInformation();
        }

        /// <summary>
        /// Check if a fd3dstruct process is running.
        /// </summary>
        public static bool IsRunning()
        {
            try
            {
                CheckProcess();
                return true;
            }
            catch (System.ArgumentException)
            {
                return false;
            }
        }
        /// <summary>
        /// Get a list of all files open in a fd3dstruct process.
        /// </summary>
        public static List<string> GetOpenFileNames()
        {
            Process[] processes = Process.GetProcessesByName("fd3dstruct");
            return processes.Select(p => p.MainWindowTitle.Split(new string[] { " - " }, System.StringSplitOptions.None)[2]).ToList();
        }

        /// <summary>
        /// Open a .struxml file in fd3dstruct.
        /// </summary>
        /// <param name="struxmlPath"></param>
        /// <param name="killProcess"></param>
        public void OpenStruxml(string struxmlPath, bool killProcess)
        {
            // kill processes
            if (killProcess)
            {
                this.KillProcesses();
            }

            string arguments = struxmlPath;
            string processPath = struxmlPath;

            ProcessStartInfo processStartInfo = new ProcessStartInfo(processPath)
            {
                Arguments = arguments,
                UseShellExecute = false,
                WorkingDirectory = Path.GetDirectoryName(struxmlPath),
                FileName = this.FdPath,
                Verb = "open"
            };

            // start process
            Process.Start(processStartInfo);
        }

        /// <summary>
        /// Run fd3dstruct with a .fdscript.
        /// </summary>
        /// <param name="fdScript"></param>
        /// <param name="killProcess"></param>
        /// <param name="endSession"></param>
        /// <returns></returns>
        public bool RunFdScript(FdScript fdScript, bool killProcess, bool endSession, bool checkOpenFiles = true)
        {
            // serialize script
            fdScript.SerializeFdScript();

            // kill processes
            if (killProcess)
            {
                this.KillProcesses();
            }

            // Check if files are already open
            if (checkOpenFiles) {
                var openFiles = GetOpenFileNames();
                var filename = fdScript.CmdOpen?.Filename;
                filename = Path.GetFileName(filename);
                if (filename != null && openFiles.Contains(filename))
                {
                    throw new System.Exception($"File {filename} already open in fd3dstruct process. Please close the file and try again. ");
                }

                filename = fdScript.CmdSave?.FilePath;
                filename = Path.GetFileName(filename);
                if (filename != null && openFiles.Contains(filename))
                {
                    throw new System.Exception($"File {filename} already open in fd3dstruct process. Please close the file and try again. ");
                }
            }

            string arguments = "/s " + fdScript.FdScriptPath;
            string processPath = fdScript.FdScriptPath;

            ProcessStartInfo processStartInfo = new ProcessStartInfo(processPath)
            {
                Arguments = arguments,
                UseShellExecute = false,
                WorkingDirectory = Path.GetDirectoryName(fdScript.FdScriptPath),
                FileName = this.FdPath,
                Verb = "open"
            };

            // start process
            Process process = Process.Start(processStartInfo);

            if (endSession)
            {
                process.WaitForExit();
                return process.HasExited;
            }
            else
            {
                return process.HasExited;
            }
        }

        public bool RunAnalysis(string struxmlPath, Analysis analysis, List<string> bscPath, string docxTemplatePath, bool endSession, bool closeOpenWindows)
        {
            FdScript fdScript = FdScript.Analysis(struxmlPath, analysis, bscPath, docxTemplatePath, endSession);
            return this.RunFdScript(fdScript, closeOpenWindows, endSession, false);
        }

        public bool RunDesign(string mode,string struxmlPath, Analysis analysis, Design design, List<string> bscPath, string docxTemplatePath, bool endSession, bool closeOpenWindows)
        {
            FdScript fdScript = FdScript.Design(mode, struxmlPath, analysis, design, bscPath, docxTemplatePath, endSession);
            return this.RunFdScript(fdScript, closeOpenWindows, endSession, false);
        }

    }
}