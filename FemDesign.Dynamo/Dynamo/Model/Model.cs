
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Model
    {
        // Attribute getters
        [IsVisibleInDynamoLibrary(true)]
        public string GetStandard()
        {
            return Standard;
        }

        [IsVisibleInDynamoLibrary(true)]
        public string GetCountry()
        {
            return Country;
        }

        #region dynamo
        /// <summary>
        /// Add elements to model. Nested lists are not supported, use flatten.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="fdModel"> Model to add elements to.</param>
        /// <param name="bars"> Single bar element or list of bar elements to add. Nested lists are not supported, use flatten.</param>
        /// <param name="shells"> Single shell element or list of shell elements to add. Nested lists are not supported, use flatten.</param>
        /// <param name="covers"> Single cover element or list of cover elements to add. Nested lists are not supported, use flatten.</param>
        /// <param name="loads"> Loads", "Single PointLoad, LineLoad, SurfaceLoad or PressureLoad element or list of PointLoad, LineLoad, SurfaceLoad or PressureLoad to add. Nested lists are not supported, use flatten.</param>
        /// <param name="loadCases"> Single loadCase element or list of loadCase-elements to add. Nested lists are not supported, use flatten.</param>
        /// <param name="loadCombinations"> Single loadCombination element or list of loadCombination elements to add. Nested lists are not supported, use flatten.</param>
        /// <param name="supports"> Single support element or list of support elements to add. Nested lists are not supported, use flatten.</param>
        /// <param name="storeys"> Single storey element or list of storey elements to add. Nested lists are not supported, use flatten.</param>
        /// <param name="axes"> Single axis element or list of axis elements to add. Nested lists are not supported, use flatten.</param>
        /// <param name="overwrite">Overwrite elements sharing GUID and mark as modified?</param>
        [IsLacingDisabled()]
        [IsVisibleInDynamoLibrary(true)]
        public static Model AddElements(Model fdModel, [DefaultArgument("[]")] List<Bars.Bar> bars, [DefaultArgument("[]")] List<ModellingTools.FictitiousBar> fictitiousBars, [DefaultArgument("[]")] List<Shells.Slab> shells, [DefaultArgument("[]")] List<ModellingTools.FictitiousShell> fictitiousShells, [DefaultArgument("[]")] List<Shells.Panel> panels, [DefaultArgument("[]")] List<Cover> covers, [DefaultArgument("[]")] List<object> loads, [DefaultArgument("[]")] List<Loads.LoadCase> loadCases, [DefaultArgument("[]")] List<Loads.LoadCombination> loadCombinations, [DefaultArgument("[]")] List<object> supports, [DefaultArgument("[]")] List<StructureGrid.Storey> storeys, [DefaultArgument("[]")] List<StructureGrid.Axis> axes, bool overwrite = false)
        {
            // deep clone model
            Model model = fdModel.DeepClone();

            // add entities
            model.AddEntities(bars, fictitiousBars, shells, fictitiousShells, panels, covers, loads, loadCases, loadCombinations, supports, storeys, axes, overwrite);
            return model;
        }

        /// <summary>
        /// Add ConnectedLines elements to model. Nested lists are not supported, use flatten.
        /// </summary>
        /// <param name="fdModel">Model to add elements to.</param>
        /// <param name="connectedLines">Single connected lines element or list of connected lines to add. Nested lists are not supported, use flatten.</param>
        /// <param name="overwrite">Overwrite elements sharing GUID and mark as modified?</param>
        [IsLacingDisabled()]
        [IsVisibleInDynamoLibrary(true)]
        public static Model ModelAddConnectedLine(Model fdModel, List<ModellingTools.ConnectedLines> connectedLines, bool overwrite = false)
        {
            // add connectedLines
            foreach (ModellingTools.ConnectedLines item in connectedLines)
            {
                fdModel.AddConnectedLine(item, overwrite);
            }

            // return
            return fdModel;
            
        }

        /// <summary>
        /// Add ConnectedPoints elements to model. Nested lists are not supported, use flatten.
        /// </summary>
        /// <param name="fdModel">Model to add elements to.</param>
        /// <param name="connectedPoints">Single connected points element or list of connected lines to add. Nested points are not supported, use flatten.</param>
        /// <param name="overwrite">Overwrite elements sharing GUID and mark as modified?</param>
        [IsLacingDisabled()]
        [IsVisibleInDynamoLibrary(true)]
        public static Model ModelAddConnectedPoints(Model fdModel, List<ModellingTools.ConnectedPoints> connectedPoints, bool overwrite = false)
        {
            // add connectedLines
            foreach (ModellingTools.ConnectedPoints item in connectedPoints)
            {
                fdModel.AddConnectedPoints(item, overwrite);
            }

            // return
            return fdModel;
        }

        /// <summary>
        /// Create new model. Add entities to model. Nested lists are not supported, use flatten.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="countryCode">National annex of calculation code ("D"/"DK"/"EST"/"FIN"/"GB"/"H"/"N"/"PL"/"RO"/"S"/"TR")</param>
        /// <param name="bars"> Single bar element or list of bar elements to add. Nested lists are not supported, use flatten.</param>
        /// <param name="shells"> Single shell element or list of shell elements to add. Nested lists are not supported, use flatten.</param>
        /// <param name="covers"> Single cover element or list of cover elements to add. Nested lists are not supported, use flatten.</param>
        /// <param name="loads"> Loads", "Single PointLoad, LineLoad, SurfaceLoad or PressureLoad element or list of PointLoad, LineLoad, SurfaceLoad or PressureLoad to add. Nested lists are not supported, use flatten.</param>
        /// <param name="loadCases"> Single loadCase element or list of loadCase-elements to add. Nested lists are not supported, use flatten.</param>
        /// <param name="loadCombinations"> Single loadCombination element or list of loadCombination elements to add. Nested lists are not supported, use flatten.</param>
        /// <param name="supports"> Single support element or list of support elements to add. Nested lists are not supported, use flatten.</param>
        /// <param name="storeys"> Single storey element or list of storey elements to add. Nested lists are not supported, use flatten.</param>
        /// <param name="axes"> Single axis element or list of axis elements to add. Nested lists are not supported, use flatten.</param>
        [IsLacingDisabled()]
        [IsVisibleInDynamoLibrary(true)]
        public static Model CreateNewModel([DefaultArgument("S")] string countryCode, [DefaultArgument("[]")] List<Bars.Bar> bars, [DefaultArgument("[]")] List<ModellingTools.FictitiousBar> fictitiousBars, [DefaultArgument("[]")] List<Shells.Slab> shells, [DefaultArgument("[]")] List<ModellingTools.FictitiousShell> fictitiousShells, [DefaultArgument("[]")] List<Shells.Panel> panels, [DefaultArgument("[]")] List<Cover> covers, [DefaultArgument("[]")] List<object> loads, [DefaultArgument("[]")] List<Loads.LoadCase> loadCases, [DefaultArgument("[]")] List<Loads.LoadCombination> loadCombinations, [DefaultArgument("[]")] List<object> supports, [DefaultArgument("[]")] List<StructureGrid.Storey> storeys, [DefaultArgument("[]")] List<StructureGrid.Axis> axes)
        {
            //
            if (countryCode == null)
            {
                countryCode = "S";
            }

            // create model
            Model _model = new Model(countryCode);
            _model.AddEntities(bars, fictitiousBars, shells, fictitiousShells, panels, covers, loads, loadCases, loadCombinations, supports, storeys, axes, false);
            return _model;
        }
        /// <summary>
        /// Open model in FEM-Design.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="fdModel">FdModel.</param>
        /// <param name="struxmlPath">File path where to save the model as .struxml</param>
        /// <param name="closeOpenWindows">If true all open windows will be closed without prior warning.</param>
        /// <param name="runNode">If true node will execute. If false node will not execute. </param>
        /// <returns>Bool. True if session has exited. False if session is still open or was closed manually.</returns>
        [IsVisibleInDynamoLibrary(true)]
        public static void OpenModel(Model fdModel, string struxmlPath, bool closeOpenWindows = false, bool runNode = true)
        {
            if (runNode)
            {
                fdModel.SerializeModel(struxmlPath);
                fdModel.FdApp.OpenStruxml(struxmlPath, closeOpenWindows);
            }
            else
            {
                throw new System.ArgumentException("runNode is set to false!");
            }
        }
        /// <summary>
        /// Load model from .struxml. Add entities to model. Nested lists are not supported, use flatten. Note: Only supported elements will loaded from the .struxml model.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="filePathStruxml">File path to .struxml file.</param>
        [IsLacingDisabled()]
        [IsVisibleInDynamoLibrary(true)]
        public static Model ReadStruxml(string filePathStruxml)
        {

            Model _model = Model.DeserializeFromFilePath(filePathStruxml);
            return _model;
        }
        /// <summary>
        /// Read model from .str file. Note: Only supported elements will loaded from the .struxml model.
        /// </summary>
        /// <param name="strPath">File path to .str file.</param>
        /// <param name="bscPath">File path to .bsc batch-file. Item or list.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        [MultiReturn(new[]{"Model", "HasExited"})]
        public static Dictionary<string, object> ReadStr(string strPath, [DefaultArgument("[]")] List<string> bscPath)
        {
            Calculate.FdScript fdScript = Calculate.FdScript.ReadStr(strPath, bscPath);
            Calculate.Application fdApp = new Calculate.Application();
            bool hasExited =  fdApp.RunFdScript(fdScript, false, true, false);
            if (hasExited)
            {
                return new Dictionary<string, object>
                {
                    {"Model", Model.DeserializeFromFilePath(fdScript.StruxmlPath)},
                    {"HasExited", hasExited}
                };
            }
            else
            {
                throw new System.ArgumentException("Process did not exit, unable to load .struxml.");
            }
        }  
            
        /// <summary>
        /// Save model to .struxml. Returns true if model was serialized.
        /// </summary>
        /// <remarks>Action</remarks>
        /// <param name="fdModel">FdModel.</param>
        /// <param name="struxmlPath">File path where to save the model as .struxml.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static bool SaveModel(Model fdModel, string struxmlPath)
        {
            fdModel.SerializeModel(struxmlPath);
            return true;
        }
        #endregion
    }
}