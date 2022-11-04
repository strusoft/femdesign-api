using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Runtime;

namespace FemDesign.Calculate
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Application
    {
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
        [MultiReturn(new[] { "FdModel", "FdFeaModel", "Results", "HasExited" })]
        public static Dictionary<string, object> RunAnalysis(Model fdModel, string struxmlPath, Calculate.Analysis analysis, [DefaultArgument("[]")] List<Type> resultTypes, [DefaultArgument("[]")] Results.UnitResults units, string docxTemplatePath = "", bool endSession = true, bool closeOpenWindows = false, bool runNode = true)
        {
            if (!runNode)
            {
                throw new System.ArgumentException("runNode is set to false!");
            }
            fdModel.SerializeModel(struxmlPath);
            analysis.SetLoadCombinationCalculationParameters(fdModel);

            units = Results.UnitResults.Default();
            // It needs to check if model has been runned
            // Always Return the FeaNode Result
            resultTypes.Insert(0, typeof(Results.FeaNode));
            resultTypes.Insert(1, typeof(Results.FeaBar));
            resultTypes.Insert(2, typeof(Results.FeaShell));

            // Create Bsc files from resultTypes
            //var listProcs = resultTypes.Select(r => Results.ResultAttributeExtentions.ListProcs[r]);
            var bscPathsFromResultTypes = Calculate.Bsc.BscPathFromResultTypes(resultTypes, struxmlPath, units);
            var rtn = fdModel.FdApp.RunAnalysis(struxmlPath, analysis, bscPathsFromResultTypes, docxTemplatePath, endSession, closeOpenWindows);


            // Create FdScript
            var fdScript = FemDesign.Calculate.FdScript.ReadStr(struxmlPath, bscPathsFromResultTypes);

            IEnumerable<Results.IResult> results = Enumerable.Empty<Results.IResult>();

            List<Results.FeaNode> feaNodeRes = new List<Results.FeaNode>();
            List<Results.FeaBar> feaBarRes = new List<Results.FeaBar>();
            List<Results.FeaShell> feaShellRes = new List<Results.FeaShell>();

            if (resultTypes != null && resultTypes.Any())
            {
                foreach (var cmd in fdScript.CmdListGen)
                {
                    string path = cmd.OutFile;
                    try
                    {
                        if (path.Contains("FeaNode"))
                        {
                            feaNodeRes = Results.ResultsReader.Parse(path).Cast<Results.FeaNode>().ToList();
                        }
                        else if (path.Contains("FeaBar"))
                        {
                            feaBarRes = Results.ResultsReader.Parse(path).Cast<Results.FeaBar>().ToList();
                        }
                        else if (path.Contains("FeaShell"))
                        {
                            feaShellRes = Results.ResultsReader.Parse(path).Cast<Results.FeaShell>().ToList();
                        }
                        else
                        {
                            var _results = Results.ResultsReader.Parse(path);
                            results = results.Concat(_results);
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception(e.InnerException.Message);
                    }
                }
            }

            var fdFeaModel = new FemDesign.Results.FDfea(feaNodeRes, feaBarRes, feaShellRes);

            var resultGroups = results.GroupBy(t => t.GetType()).ToList();

            // Convert Data in NestedList structure
            var resultsTree = new List<List<Results.IResult>>();
            var i = 0;
            foreach (var resGroup in resultGroups)
            {
                resultsTree.Add(resGroup.ToList());
                i++;
            }

            return new Dictionary<string, object>
            {
                {"FdModel", fdModel },
                {"FdFeaModel", fdFeaModel },
                {"Results", resultsTree },
                {"HasExited", rtn }
            };
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
        public static bool RunDesign(string mode, Model fdModel, string struxmlPath, Calculate.Analysis analysis, Calculate.Design design, [DefaultArgument("[]")] List<string> bscPath, string docxTemplatePath = "", bool endSession = false, bool closeOpenWindows = false, bool runNode = true)
        {
            if (!runNode)
            {
                throw new System.ArgumentException("runNode is set to false!");
            }

            fdModel.SerializeModel(struxmlPath);
            analysis.SetLoadCombinationCalculationParameters(fdModel);
            return fdModel.FdApp.RunDesign(mode, struxmlPath, analysis, design, bscPath, docxTemplatePath, endSession, closeOpenWindows);
        }
        #endregion
    }
}
