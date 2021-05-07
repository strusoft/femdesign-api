using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Runtime;

namespace FemDesign.Calculate
{
    public partial class Analysis
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
        public static bool RunAnalysis(Model fdModel, string struxmlPath, Calculate.Analysis analysis, [DefaultArgument("[]")] List<string> bscPath, string docxTemplatePath = "", bool endSession = true, bool closeOpenWindows = false, bool runNode = true)
        {
            if (!runNode)
            {
                throw new System.ArgumentException("runNode is set to false!");
            }
            fdModel.SerializeModel(struxmlPath);
            analysis.SetLoadCombinationCalculationParameters(fdModel);
            return fdModel.FdApp.RunAnalysis(struxmlPath, analysis, bscPath, docxTemplatePath, endSession, closeOpenWindows);
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
