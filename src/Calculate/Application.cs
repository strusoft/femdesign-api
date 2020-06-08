// https://strusoft.com/
using System.Diagnostics;
using System.IO;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Calculate
{
    /// <summary>
    /// This is a simple "application" for fd3dstruct.
    /// Retreive process information of a running fd3dstruct. 
    /// Start and run processes (open, analysis, design etc.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class Application
    {
        /// <summary>
        /// Path to fd3dstruct.
        /// </summary>
        /// <value></value>
        internal string fdPath { get; set; }

        /// <summary>
        /// Version of running fd3dstruct process.
        /// </summary>
        /// <value></value>
        internal string fdVersion { get; set; }

        /// <summary>
        /// Target version of class library.
        /// </summary>
        internal string fdTargetVersion = "19";

        internal Application()
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
                this.fdPath = firstProcess.MainModule.FileName;
                this.fdVersion = firstProcess.MainModule.FileVersionInfo.FileVersion.Split(new char[] { '.' })[0];

                // check if process inforamtion matches target version
                if (this.fdVersion != null && this.fdVersion == this.fdTargetVersion && this.fdPath != null)
                {
                    return;
                }
            }
            else
            {
                throw new System.ArgumentException("FEM-Design " + this.fdTargetVersion + " - 3D Structure must be running! Start FEM-Design " + this.fdTargetVersion + " - 3D Structure and reload script.");
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
        /// Open a .struxml file in fd3dstruct.
        /// </summary>
        /// <param name="struxmlPath"></param>
        /// <param name="killProcess"></param>
        internal void OpenStruxml(string struxmlPath, bool killProcess)
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
                FileName = this.fdPath,
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
        internal bool RunFdScript(FdScript fdScript, bool killProcess, bool endSession)
        {
            // serialize script
            fdScript.SerializeFdScript();

            // kill processes
            if (killProcess)
            {
                this.KillProcesses();
            }

            string arguments = "/s " + fdScript.fdScriptPath;
            string processPath = fdScript.fdScriptPath;

            ProcessStartInfo processStartInfo = new ProcessStartInfo(processPath)
            {
                Arguments = arguments,
                UseShellExecute = false,
                WorkingDirectory = Path.GetDirectoryName(fdScript.fdScriptPath),
                FileName = this.fdPath,
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

        internal bool RunAnalysis(string struxmlPath, Analysis analysis, string bscPath, string docxTemplatePath, bool endSession, bool closeOpenWindows)
        {
            FdScript fdScript = FdScript.Analysis(struxmlPath, analysis, bscPath, docxTemplatePath, endSession);
            return this.RunFdScript(fdScript, closeOpenWindows, endSession);
        }
        internal bool RunDesign(string mode,string struxmlPath, Analysis analysis, Design design, string bscPath, string docxTemplatePath, bool endSession, bool closeOpenWindows)
        {
            FdScript fdScript = FdScript.Design(mode, struxmlPath, analysis, design, bscPath, docxTemplatePath, endSession);
            return this.RunFdScript(fdScript, closeOpenWindows, endSession);
        }

        #region dynamo
        /// <summary>
        /// Run analysis of model.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="fdModel">FdModel.</param>
        /// <param name="struxmlPath">File path where to save the model as .struxml</param>
        /// <param name="analysis">Analysis.</param>
        /// <param name="bscPath">File path to batch-file (.bsc) to run</param>
        /// <param name="docxTemplatePath">File path to documenation template file (.dsc) to run.</param>
        /// <param name="endSession">If true FEM-Design will close after execution.</param>
        /// <param name="closeOpenWindows">If true all open windows will be closed without prior warning.</param>
        /// <param name="runNode">If true node will execute. If false node will not execute. </param>
        /// <returns>Bool. True if session has exited. False if session is still open or was closed manually.</returns>
        [IsVisibleInDynamoLibrary(true)]
        public static bool RunAnalysis(Model fdModel, string struxmlPath, Calculate.Analysis analysis, string bscPath = "", string docxTemplatePath = "", bool endSession = true, bool closeOpenWindows = false, bool runNode = true)
        {
            if (!runNode)
            {
                throw new System.ArgumentException("runNode is set to false!");
            }
            fdModel.SerializeModel(struxmlPath);
            analysis.SetLoadCombinationCalculationParameters(fdModel);
            return fdModel.fdApp.RunAnalysis(struxmlPath, analysis, bscPath, docxTemplatePath, endSession, closeOpenWindows);
        }
        /// <summary>
        /// Run analysis and design of a model.
        /// </summary>
        /// <param name="mode">Design mode: rc, steel or timber.</param>
        /// <param name="fdModel">FdModel.</param>
        /// <param name="struxmlPath">File path where to save the model as .struxml</param>
        /// <param name="analysis">Analysis.</param>
        /// <param name="design">Design.</param>
        /// <param name="bscPath">File path to batch-file (.bsc) to run.</param>
        /// <param name="docxTemplatePath">File path to documenation template file (.dsc) to run.</param>
        /// <param name="endSession">If true FEM-Design will close after execution.</param>
        /// <param name="closeOpenWindows">If true all open windows will be closed without prior warning.</param>
        /// <param name="runNode">If true node will execute. If false node will not execute. </param>
        /// <returns>Bool. True if session has exited. False if session is still open or was closed manually.</returns>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static bool RunDesign(string mode, Model fdModel, string struxmlPath, Calculate.Analysis analysis, Calculate.Design design, string bscPath = "", string docxTemplatePath = "", bool endSession = true, bool closeOpenWindows = false, bool runNode = true)
        {
            if (!runNode)
            {
                throw new System.ArgumentException("runNode is set to false!");
            }

            fdModel.SerializeModel(struxmlPath);
            analysis.SetLoadCombinationCalculationParameters(fdModel);
            return fdModel.fdApp.RunDesign(mode, struxmlPath, analysis, design, bscPath, docxTemplatePath, endSession, closeOpenWindows);
        }
        #endregion
    }
}