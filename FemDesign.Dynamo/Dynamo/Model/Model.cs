
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
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
            return Country.ToString();
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
            // Deep clone model
            var clone = fdModel.DeepClone();

            // Add entities
            var _supports = supports.Cast<ISupportElement>().ToList();
            clone.AddEntities(bars, fictitiousBars, shells, fictitiousShells, panels, covers, loads, loadCases, loadCombinations, _supports, storeys, axes, null, overwrite);
            return clone;
        }

        /// <summary>
        /// Add elements to model. Nested lists are not supported, use flatten.
        /// </summary>
        /// <param name="fdModel"> Model to add elements to.</param>
        /// <param name="elements">Structure elements to add. Nested lists are not supported, use flatten.</param>
        /// <param name="loads">Load elements to add. Nested lists are not supported, use flatten.</param>
        /// <param name="loadCases">Load cases to add. Nested lists are not supported, use flatten.</param>
        /// <param name="loadCombinations">Load combinations to add. Nested lists are not supported, use flatten.</param>
        /// <param name="overwrite">Overwrite elements sharing GUID and mark as modified?</param>
        [IsLacingDisabled()]
        [IsVisibleInDynamoLibrary(true)]
        public static Model AddElements(Model fdModel, [DefaultArgument("[]")] List<object> elements, [DefaultArgument("[]")] List<object> loads, [DefaultArgument("[]")] List<Loads.LoadCase> loadCases, [DefaultArgument("[]")] List<Loads.LoadCombination> loadCombinations, bool overwrite = false)
        {
            // Deep clone model
            var clone = fdModel.DeepClone();

            var _elements = elements.Cast<IStructureElement>().ToList();
            var _loads = loads.Cast<ILoadElement>().ToList();

            clone.AddElements(_elements, overwrite);
            clone.AddLoads(_loads, overwrite);
            clone.AddLoadCases(loadCases, overwrite);
            clone.AddLoadCombinations(loadCombinations, overwrite);

            return clone;
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
        public static Model CreateNewModel([DefaultArgument("\"S\"")] string countryCode, [DefaultArgument("[]")] List<Bars.Bar> bars, [DefaultArgument("[]")] List<ModellingTools.FictitiousBar> fictitiousBars, [DefaultArgument("[]")] List<Shells.Slab> shells, [DefaultArgument("[]")] List<ModellingTools.FictitiousShell> fictitiousShells, [DefaultArgument("[]")] List<Shells.Panel> panels, [DefaultArgument("[]")] List<Cover> covers, [DefaultArgument("[]")] List<object> loads, [DefaultArgument("[]")] List<Loads.LoadCase> loadCases, [DefaultArgument("[]")] List<Loads.LoadCombination> loadCombinations, [DefaultArgument("[]")] List<object> supports, [DefaultArgument("[]")] List<StructureGrid.Storey> storeys, [DefaultArgument("[]")] List<StructureGrid.Axis> axes)
        {
            // Create model
            Model model = new Model(EnumParser.Parse<Country>(countryCode));
            var _supports = supports.Cast<GenericClasses.ISupportElement>().ToList();
            model.AddEntities(bars, fictitiousBars, shells, fictitiousShells, panels, covers, loads, loadCases, loadCombinations, _supports, storeys, axes, null, false);
            return model;
        }

        /// <summary>
        /// Create new model. Add entities to model. Nested lists are not supported, use flatten.
        /// </summary>
        /// <param name="countryCode">National annex of calculation code ("D"/"DK"/"EST"/"FIN"/"GB"/"H"/"N"/"PL"/"RO"/"S"/"TR")</param>
        /// <param name="elements">Elements found under the Structure tab in FEM-Design.</param>
        /// <param name="loads">Loads found under the Loads tab in FEM-Design.</param>
        /// <param name="loadCases">Load cases.</param>
        /// <param name="loadCombinations">Load combinations.</param>
        [IsLacingDisabled()]
        [IsVisibleInDynamoLibrary(true)]
        public static Model CreateNewModel([DefaultArgument("\"S\"")] string countryCode, [DefaultArgument("[]")] List<object> elements, [DefaultArgument("[]")] List<object> loads, [DefaultArgument("[]")] List<Loads.LoadCase> loadCases, [DefaultArgument("[]")] List<Loads.LoadCombination> loadCombinations)
        {
            var _elements = elements.Cast<IStructureElement>().ToList();
            var _loads = loads.Cast<ILoadElement>().ToList();
            Model fdModel = new Model(EnumParser.Parse<Country>(countryCode), _elements, _loads, loadCases, loadCombinations);

            return fdModel;
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