// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign
{
    /// <summary>
    /// Model. Represents a complete struxml model.
    /// </summary>
    [System.Serializable]
    [XmlRoot("database", Namespace = "urn:strusoft")]
    public partial class Model
    {
        [XmlIgnore]
        public Calculate.Application FdApp = new Calculate.Application(); // start a new FdApp to get process information.
        /// <summary>
        /// The actual struXML version;  should be equal to the schema version the xml file is conformed to.
        /// </summary>
        [XmlAttribute("struxml_version")]
        public string StruxmlVersion { get; set; } // versiontype
        /// <summary>
        /// Name of the StruSoft or 3rd party product what generated this XML file.
        /// </summary>
        [XmlAttribute("source_software")]
        public string SourceSoftware { get; set; } // string
        /// <summary>
        /// The data is partial data, so the oldest entity latest modification date and time is the
        /// value in UTC. If the current XML contains the whole database, the start_time value is
        /// "1970-01-01T00:00:00Z". The date and time always in UTC!
        /// </summary>
        [XmlAttribute("start_time")]
        public string StartTime { get; set; } // dateTime
        /// <summary>
        /// The data is partial data, so the newest entity latest modification date and time is this
        /// value in UTC. This date and time always in UTC!
        /// </summary>
        [XmlAttribute("end_time")]
        public string EndTime { get; set; } // dateTime
        [XmlAttribute("guid")]
        public System.Guid Guid { get; set; } // guidtype
        [XmlAttribute("convertid")]
        public string ConvertId { get; set; } // guidtype
        /// <summary>Calculation code</summary>
        [XmlAttribute("standard")]
        public string Standard { get; set; } // standardtype
        /// <summary>National annex of calculation code</summary>
        [XmlAttribute("country")]
        public Country Country { get; set; } // eurocodetype
        [XmlAttribute("xmlns")]
        public string Xmlns { get; set; }

        [XmlElement("construction_stages", Order = 1)]
        public ConstructionStages ConstructionStages { get; set; }

        [XmlElement("entities", Order = 2)]
        public Entities Entities { get; set; }
        [XmlElement("sections", Order = 3)]
        public Sections.ModelSections Sections { get; set; }
        [XmlElement("materials", Order = 4)]
        public Materials.Materials Materials { get; set; }
        [XmlElement("reinforcing_materials", Order = 5)]
        public Materials.ReinforcingMaterials ReinforcingMaterials { get; set; }
        [XmlElement("composites", Order = 6)]
        public StruSoft.Interop.StruXml.Data.DatabaseComposites Composites { get; set; }
        [XmlElement("point_connection_types", Order = 7)]
        public LibraryItems.PointConnectionTypes PointConnectionTypes { get; set; }
        [XmlElement("point_support_group_types", Order = 8)]
        public LibraryItems.PointSupportGroupTypes PointSupportGroupTypes { get; set; }
        [XmlElement("line_connection_types", Order = 9)]
        public LibraryItems.LineConnectionTypes LineConnectionTypes { get; set; }
        [XmlElement("line_support_group_types", Order = 10)]
        public LibraryItems.LineSupportGroupTypes LineSupportGroupTypes { get; set; }
        [XmlElement("surface_connection_types", Order = 11)]
        public LibraryItems.SurfaceConnectionTypes SurfaceConnectionTypes { get; set; }
        [XmlElement("surface_support_types", Order = 12)]
        public LibraryItems.SurfaceSupportTypes SurfaceSupportTypes { get; set; }
        [XmlElement("timber_panel_types", Order = 13)]
        public Materials.OrthotropicPanelTypes OrthotropicPanelTypes { get; set; }
        [XmlElement("glc_panel_types", Order = 14)]
        public Materials.GlcPanelTypes GlcPanelTypes { get; set; }

        [XmlElement("clt_panel_types", Order = 15)]
        public Materials.CltPanelTypes CltPanelTypes { get; set; }

        [XmlElement("ptc_strand_types", Order = 16)]
        public Reinforcement.PtcStrandType PtcStrandTypes { get; set; }

        [XmlElement("vehicle_types", Order = 17)]
        public List<StruSoft.Interop.StruXml.Data.Vehicle_lib_type> VehicleTypes { get; set; }

        [XmlElement("bolt_types", Order = 18)]
        public List<StruSoft.Interop.StruXml.Data.Bolt_lib_type> BoltTypes { get; set; }

        [XmlElement("geometry", Order = 19)]
        public StruSoft.Interop.StruXml.Data.DatabaseGeometry Geometry { get; set; }

        [XmlElement("user_defined_filter", Order = 20)]
        public List<StruSoft.Interop.StruXml.Data.Userfilter_type> UserDefinedFilters { get; set; }

        [XmlElement("user_defined_views", Order = 21)]
        public StruSoft.Interop.StruXml.Data.DatabaseUser_defined_views UserDefinedViews { get; set; }

        [XmlElement("end", Order = 22)]
        public string End { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Model()
        {

        }

        /// <summary>
        /// Initialize a model with elements.
        /// </summary>
        /// <param name="country">Country/Annex of the FEM-Design model.</param>
        /// <param name="elements">Structural elements.</param>
        /// <param name="loads">Load elements</param>
        /// <param name="loadCases">Load cases</param>
        /// <param name="loadCombinations">Load combinations</param>
        /// <param name="loadGroups">Load groups</param>
        /// <param name="constructionStage">Construction stages object instance.</param>
        public Model(Country country, List<IStructureElement> elements = null, List<ILoadElement> loads = null, List<Loads.LoadCase> loadCases = null, List<Loads.LoadCombination> loadCombinations = null, List<Loads.ModelGeneralLoadGroup> loadGroups = null, ConstructionStages constructionStage = null)
        {
            Initialize(country);

            if (elements != null)
                AddElements(elements, overwrite: false);
            if (loads != null)
                AddLoads(loads, overwrite: false);
            if (loadCases != null)
                AddLoadCases(loadCases, overwrite: false);
            if (loadCombinations != null)
                AddLoadCombinations(loadCombinations, overwrite: false);
            if (loadGroups != null)
                AddLoadGroupTable(loadGroups, overwrite: false);
            if (constructionStage != null)
                SetConstructionStages(constructionStage);
        }

        private void Initialize(Country country)
        {
            this.StruxmlVersion = "01.00.000";
            this.SourceSoftware = "FEM-Design 18.00.004";
            this.StartTime = "1970-01-01T00:00:00.000";
            this.EndTime = System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
            this.Guid = System.Guid.NewGuid();
            this.ConvertId = "00000000-0000-0000-0000-000000000000";
            this.Standard = "EC";
            this.Country = country;
            this.End = "";

            // Check if model contains entities, sections and materials, else these needs to be initialized.
            if (this.Entities == null)
            {
                this.Entities = new Entities();
            }
            if (this.Sections == null)
            {
                this.Sections = new Sections.ModelSections();
            }
            if (this.Materials == null)
            {
                this.Materials = new Materials.Materials();
            }
            if (this.ReinforcingMaterials == null)
            {
                this.ReinforcingMaterials = new Materials.ReinforcingMaterials();
            }
            if (this.LineConnectionTypes == null)
            {
                this.LineConnectionTypes = new LibraryItems.LineConnectionTypes();
                this.LineConnectionTypes.PredefinedTypes = new List<Releases.RigidityDataLibType3>();
            }
            if (this.PtcStrandTypes == null)
            {
                this.PtcStrandTypes = new Reinforcement.PtcStrandType();
            }
        }

        #region serialization
        /// <summary>
        /// Deserialize model from file (.struxml).
        /// </summary>
        public static Model DeserializeFromFilePath(string filePath)
        {
            // check file extension
            if (Path.GetExtension(filePath) != ".struxml")
            {
                throw new System.ArgumentException("File extension must be .struxml! Model.DeserializeModel failed.");
            }

            //
            XmlSerializer deserializer = new XmlSerializer(typeof(Model));
            TextReader reader = new StreamReader(filePath);

            object obj;
            try
            {
                obj = deserializer.Deserialize(reader);
            }
            catch (System.InvalidOperationException ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(System.Reflection.TargetInvocationException))
                {
                    if (ex.InnerException.InnerException != null && ex.InnerException.InnerException.GetType() == typeof(Calculate.ProgramNotStartedException))
                    {
                        throw ex.InnerException.InnerException; // FEM-Design 21 - 3D Structure must be running! Start FEM-Design " + this.FdTargetVersion + " - 3D Structure and reload script
                    }
                }
                throw ex; // There is an error in XML document (3, 2).
            }
            finally
            {
                // close reader
                reader.Close();
            }

            // cast type
            Model model = (Model)obj;

            if (model.Entities == null) model.Entities = new Entities();

            // prepare elements with library reference
            // Check if there are any elements of type to avoid null checks on each library type (sections, materials etc.) in each method below
            if (model.Entities.Bars.Any())
                model.GetBars();
            if (model.Entities.AdvancedFem.FictitiousShells.Any())
                model.GetFictitiousShells();
            if (model.Entities.Supports.LineSupport.Any())
                model.GetLineSupports();
            if (model.Entities.Panels.Any())
                model.GetPanels();
            if (model.Entities.Supports.PointSupport.Any())
                model.GetPointSupports();
            if (model.Entities.Slabs.Any())
                model.GetSlabs();
            if (model.Entities.Supports.SurfaceSupport.Any())
                model.GetSurfaceSupports();
            if (model.Entities.AdvancedFem.ConnectedPoints.Any())
                model.GetPointConnections();
            if (model.Entities.AdvancedFem.ConnectedLines.Any())
                model.GetLineConnections();
            if (model.ConstructionStages != null && model.ConstructionStages.Stages.Any())
                model.GetConstructionStages();
            if (model.Entities?.Loads?.LoadCombinations != null && model.Entities.Loads.LoadCombinations.Any())
                model.GetLoadCombinations();
            return model;
        }

        /// <summary>
        /// Serialize Model to file (.struxml).
        /// </summary>
        /// <param name="filePath"></param>
        public void SerializeModel(string filePath)
        {
            if (filePath == null)
            {
                var currentDirectory = System.IO.Directory.GetCurrentDirectory();
                filePath = System.IO.Path.Combine(currentDirectory, "myModel.struxml");
            }

            // Relavive paths will be converted to full paths
            filePath = System.IO.Path.GetFullPath(filePath);

            // If path has no file extension "struxml" will be used
            if (Path.GetExtension(filePath) == "")
                filePath = Path.ChangeExtension(filePath, "struxml");

            // Check file extension
            if (Path.GetExtension(filePath) != ".struxml")
            {
                throw new System.ArgumentException("File extension must be .struxml! Model.SerializeModel failed.");
            }

            // Serialize
            XmlSerializer serializer = new XmlSerializer(typeof(Model));
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, this);
            }
        }

        /// <summary>
        /// Reads a .str model, then saves and deserialize it from file (.struxml).
        /// </summary>
        /// <param name="strPath">FEM-Design model (.str) to be read.</param>
        /// <param name="resultTypes">Results that should be read. (Might require model to have been analysed)</param>
        /// <param name="killProcess"></param>
        /// <param name="endSession"></param>
        /// <param name="checkOpenFiles"></param>
        public static (Model fdModel, IEnumerable<Results.IResult> results) ReadStr(string strPath, IEnumerable<Type> resultTypes, bool killProcess = false, bool endSession = true, bool checkOpenFiles = true)
        {
            if (resultTypes != null)
            {
                var notAResultType = resultTypes.Where(r => !typeof(Results.IResult).IsAssignableFrom(r)).FirstOrDefault();
                if (notAResultType != null)
                    throw new ArgumentException($"{notAResultType.Name} is not a result type. (It does not inherit from {typeof(FemDesign.Results.IResult).FullName})");
            }

            var fdScript = Calculate.FdScript.ExtractResults(strPath, resultTypes);

            var app = new Calculate.Application();
            app.RunFdScript(fdScript, killProcess, endSession, checkOpenFiles);

            Model model = Model.DeserializeFromFilePath(fdScript.CmdSave.FilePath);

            IEnumerable<Results.IResult> results = Enumerable.Empty<Results.IResult>();
            if (resultTypes != null && resultTypes.Any())
            {
                results = fdScript.CmdListGen.Select(cmd => cmd.OutFile).SelectMany(path =>
                {
                    try
                    {
                        return Results.ResultsReader.Parse(path);
                    }
                    catch (System.ApplicationException)
                    {
                        return Enumerable.Empty<Results.IResult>();
                    }
                });
            }

            return (model, results);
        }

        /// <summary>
        /// Open a Model in FemDesign
        /// </summary>
        /// <param name="filePath">if null, the file will be created in the current directory with the name "myModel.struxml"</param>
        /// <param name="closeOpenWindows"></param>
        public void Open(string filePath = null, bool closeOpenWindows = false)
        {
            if (filePath == null)
            {
                var currentDirectory = System.IO.Directory.GetCurrentDirectory();
                filePath = System.IO.Path.Combine(currentDirectory, "myModel.struxml");
            }

            this.SerializeModel(filePath);
            this.FdApp.OpenStruxml(filePath, closeOpenWindows);
        }

        public bool RunAnalysis(Calculate.Analysis analysis, IEnumerable<Type> resultTypes = null, Results.UnitResults units = null, string struxmlPath = null, string docxTemplatePath = null, bool endSession = false, bool closeOpenWindows = false, Calculate.CmdGlobalCfg cmdGlobalCfg = null)
        {
            if (struxmlPath == null)
            {
                var currentDirectory = System.IO.Directory.GetCurrentDirectory();
                struxmlPath = System.IO.Path.Combine(currentDirectory, "myModel.struxml");
            }

            List<string> bscPath = null;
            if (resultTypes != null)
            {
                bscPath = FemDesign.Calculate.Bsc.BscPathFromResultTypes(resultTypes, struxmlPath, units);
            }

            this.SerializeModel(struxmlPath);
            analysis.SetLoadCombinationCalculationParameters(this);
            return this.FdApp.RunAnalysis(struxmlPath, analysis, bscPath, docxTemplatePath, endSession, closeOpenWindows, cmdGlobalCfg);
        }

        public bool RunDesign(Calculate.CmdUserModule mode, Calculate.Analysis analysis, Calculate.Design design, IEnumerable<Type> resultTypes = null, Results.UnitResults units = null, string struxmlPath = null, string docxTemplatePath = null, bool endSession = false, bool closeOpenWindows = false, Calculate.CmdGlobalCfg cmdGlobalCfg = null)
        {
            if (struxmlPath == null)
            {
                var currentDirectory = System.IO.Directory.GetCurrentDirectory();
                struxmlPath = System.IO.Path.Combine(currentDirectory, "myModel.struxml");
            }

            List<string> bscPath = null;
            if (resultTypes != null)
            {
                var notAResultType = resultTypes.Where(r => !typeof(Results.IResult).IsAssignableFrom(r)).FirstOrDefault();
                if (notAResultType != null)
                    throw new ArgumentException($"{notAResultType.Name} is not a result type. (It does not inherit from {typeof(FemDesign.Results.IResult).FullName})");
                bscPath = FemDesign.Calculate.Bsc.BscPathFromResultTypes(resultTypes, struxmlPath, units);
            }

            this.SerializeModel(struxmlPath);
            analysis.SetLoadCombinationCalculationParameters(this);
            return this.FdApp.RunDesign(mode.ToString(), struxmlPath, analysis, design, bscPath, docxTemplatePath, endSession, closeOpenWindows, cmdGlobalCfg);
        }

        /// <summary>
        /// Serialize Model to string.
        /// </summary>
        public string SerializeToString()
        {
            // serialize
            XmlSerializer serializer = new XmlSerializer(typeof(Model));
            using (TextWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, this);
                return writer.ToString();
            }
        }
        #endregion

        #region addEntities

        /// <summary>
        /// Add entities to Model.
        /// </summary>
        public Model AddEntities(List<Bars.Bar> bars, List<ModellingTools.FictitiousBar> fictitiousBars, List<Shells.Slab> shells, List<ModellingTools.FictitiousShell> fictitiousShells, List<Shells.Panel> panels, List<Cover> covers, List<object> loads, List<Loads.LoadCase> loadCases, List<Loads.LoadCombination> loadCombinations, List<ISupportElement> supports, List<StructureGrid.Storey> storeys, List<StructureGrid.Axis> axes, List<Loads.ModelGeneralLoadGroup> loadGroups, bool overwrite)
        {
            // check if model contains entities, sections and materials
            if (this.Entities == null)
            {
                this.Entities = new Entities();
            }
            if (this.Sections == null)
            {
                this.Sections = new Sections.ModelSections();
            }
            if (this.Materials == null)
            {
                this.Materials = new Materials.Materials();
            }
            if (this.ReinforcingMaterials == null)
            {
                this.ReinforcingMaterials = new Materials.ReinforcingMaterials();
            }
            if (this.LineConnectionTypes == null)
            {
                this.LineConnectionTypes = new LibraryItems.LineConnectionTypes();
                this.LineConnectionTypes.PredefinedTypes = new List<Releases.RigidityDataLibType3>();
            }

            if (bars != null)
            {
                foreach (Bars.Bar bar in bars)
                {
                    this.AddBar(bar, overwrite);
                }
            }

            if (fictitiousBars != null)
            {
                foreach (ModellingTools.FictitiousBar fictBar in fictitiousBars)
                {
                    this.AddFictBar(fictBar, overwrite);
                }
            }

            if (shells != null)
            {
                foreach (Shells.Slab shell in shells)
                {
                    this.AddSlab(shell, overwrite);
                }
            }

            if (fictitiousShells != null)
            {
                foreach (ModellingTools.FictitiousShell fictShell in fictitiousShells)
                {
                    this.AddFictShell(fictShell, overwrite);
                }
            }

            if (panels != null)
            {
                foreach (Shells.Panel panel in panels)
                {
                    this.AddPanel(panel, overwrite);
                }
            }

            if (covers != null)
            {
                foreach (Cover cover in covers)
                {
                    this.AddCover(cover, overwrite);
                }
            }

            if (loads != null)
            {
                foreach (object load in loads)
                {
                    this.AddLoad(load, overwrite);
                }
            }

            if (loadCases != null)
            {
                foreach (Loads.LoadCase loadCase in loadCases)
                {
                    this.AddLoadCase(loadCase, overwrite);
                }
            }

            this.AddLoadCombinations(loadCombinations, overwrite);

            if (loadGroups != null)
            {
                this.AddLoadGroupTable(loadGroups, overwrite);
            }

            if (supports != null)
            {
                foreach (ISupportElement support in supports)
                {
                    this.AddSupport(support, overwrite);
                }
            }

            if (storeys != null)
            {
                foreach (StructureGrid.Storey storey in storeys)
                {
                    this.AddStorey(storey, overwrite);
                }
            }

            if (axes != null)
            {
                foreach (StructureGrid.Axis axis in axes)
                {
                    this.AddAxis(axis, overwrite);
                }
            }

            return this;
        }

        /// <summary>
        /// Add Bar to Model.
        /// </summary>
        private void AddBar(Bars.Bar obj, bool overwrite)
        {
            // in model?
            bool inModel = this.BarInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.Bars.RemoveAll(x => x.Guid == obj.Guid);
            }

            // if truss
            if (obj.BarPart.SectionType == Bars.SectionType.Truss)
            {
                this.AddSection(obj.BarPart.TrussUniformSectionObj, overwrite);
                this.AddMaterial(obj.BarPart.ComplexMaterialObj, overwrite);
            }
            // if complex composite and not delta beam type
            else if (obj.BarPart.SectionType == Bars.SectionType.CompositeBeamColumn)
            {
                this.AddComplexComposite(obj.BarPart.ComplexCompositeObj, overwrite);
            }
            // if complex section but delta beam type
            else if (obj.BarPart.SectionType == Bars.SectionType.DeltaBeamColumn)
            {
                // do nothing
            }
            // if complex section
            else if (obj.BarPart.SectionType == Bars.SectionType.RegularBeamColumn)
            {
                this.AddComplexSection(obj.BarPart.ComplexSectionObj, overwrite);
                this.AddMaterial(obj.BarPart.ComplexMaterialObj, overwrite);
            }
            else
            {
                throw new System.ArgumentException("Type of bar is not supported.");
            }

            // add reinforcement
            this.AddBarReinforcements(obj, overwrite);

            // add ptc
            this.AddBarPtcs(obj, overwrite);

            // add bar
            this.Entities.Bars.Add(obj);
        }

        /// <summary>
        /// Check if Bar in Model.
        /// </summary>
        private bool BarInModel(Bars.Bar obj)
        {
            foreach (Bars.Bar elem in this.Entities.Bars)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add BarReinforcement(s) from Bar to Model.
        /// </summary>
        private void AddBarPtcs(Bars.Bar obj, bool overwrite)
        {
            foreach (Reinforcement.Ptc ptc in obj.Ptc)
            {
                this.AddPtc(ptc, overwrite);
            }
        }

        /// <summary>
        /// Add Post-tensioned cable to Model.
        /// </summary>
        private void AddPtc(Reinforcement.Ptc obj, bool overwrite)
        {
            // in model?
            bool inModel = this.PtcInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.PostTensionedCables.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add material
            this.AddPtcStrandType(obj.StrandType, overwrite);

            // add ptc
            this.Entities.PostTensionedCables.Add(obj);
        }

        private bool PtcInModel(Reinforcement.Ptc obj)
        {
            foreach (Reinforcement.Ptc elem in this.Entities.PostTensionedCables)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Fictitious Bar to Model.
        /// </summary>
        private void AddFictBar(ModellingTools.FictitiousBar obj, bool overwrite)
        {
            // in model?
            bool inModel = this.FictBarInModel(obj);

            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            else if (inModel && overwrite == true)
            {
                this.Entities.AdvancedFem.FictitiousBars.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add fictitious bar
            this.Entities.AdvancedFem.FictitiousBars.Add(obj);
        }

        /// <summary>
        /// Check if Fictitious Bar in Model.
        /// </summary>
        private bool FictBarInModel(ModellingTools.FictitiousBar obj)
        {
            foreach (ModellingTools.FictitiousBar elem in this.Entities.AdvancedFem.FictitiousBars)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Fictitious Shell to Model.
        /// </summary>
        private void AddFictShell(ModellingTools.FictitiousShell obj, bool overwrite)
        {
            // in model?
            bool inModel = this.FictShellInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.AdvancedFem.FictitiousShells.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add line connection types (predefined rigidity)
            foreach (Releases.RigidityDataLibType3 predef in obj.Region.GetPredefinedRigidities())
            {
                this.AddPredefinedRigidity(predef, overwrite);
            }

            // add shell
            this.Entities.AdvancedFem.FictitiousShells.Add(obj);
        }

        /// <summary>
        /// Check if Fictitious Bar in Model.
        /// </summary>
        private bool FictShellInModel(ModellingTools.FictitiousShell obj)
        {
            foreach (ModellingTools.FictitiousShell elem in this.Entities.AdvancedFem.FictitiousShells)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add ComplexSection (from Bar) to Model.
        /// </summary>
        private void AddComplexSection(Sections.ComplexSection complexSection, bool overwrite)
        {
            // in model?
            bool inModel = this.ComplexSectionInModel(complexSection);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{complexSection.GetType().FullName} with guid: {complexSection.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Sections.ComplexSection.RemoveAll(x => x.Guid == complexSection.Guid);
            }

            // add complex section
            this.Sections.ComplexSection.Add(complexSection);

            // add sections in complex section
            foreach (Sections.Section section in complexSection.Sections)
            {
                this.AddSection(section, overwrite);
            }
        }

        /// <summary>
        /// Check if ComplexSection in Model.
        /// </summary>
        private bool ComplexSectionInModel(FemDesign.Sections.ComplexSection obj)
        {
            foreach (Sections.ComplexSection elem in this.Sections.ComplexSection)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Check if CompositeSection in Model.
        /// </summary>
        private void AddCompositeSection(StruSoft.Interop.StruXml.Data.Complex_composite_type obj, bool overwrite)
        {
            // in model?
            // obj.Composite_section.Unique(x => x.Guid);
            var uniqueCompositeSection = obj.Composite_section.Where(x => x.Guid != null).GroupBy(x => x.Guid).Select(grp => grp.FirstOrDefault());


            foreach (var compositeSection in uniqueCompositeSection)
            {
                // initialise variable as false
                bool inModel = false;

                if (this.Composites.Composite_section != null)
                {
                    inModel = this.Composites.Composite_section.Any(x => x.Guid == compositeSection.Guid);
                }
                else
                {
                    this.Composites.Composite_section = new List<StruSoft.Interop.StruXml.Data.Composite_data>();
                }


                // in model, don't overwrite
                if (inModel && overwrite == false)
                {
                    throw new System.ArgumentException($"{compositeSection.GetType().FullName} with guid: {compositeSection.Guid} has already been added to model. Are you adding the same element twice?");
                }

                // in model, overwrite
                else if (inModel && overwrite == true)
                {
                    this.Composites.Composite_section.RemoveAll(x => x.Guid == compositeSection.Guid);
                }

                // add complex composite
                this.Composites.Composite_section.Add(compositeSection.CompositeSectionDataObj);
                foreach (var part in compositeSection.CompositeSectionDataObj.Part)
                {
                    this.AddMaterial(part.MaterialObj, overwrite);
                    this.AddSection(part.SectionObj, overwrite);
                }
            }
        }



        /// <summary>
        /// Add ComplexComposite to Model.
        /// if ComplexComposite is present, also compositeSection will be created 
        /// </summary>
        private void AddComplexComposite(StruSoft.Interop.StruXml.Data.Complex_composite_type obj, bool overwrite)
        {
            // in model?
            bool inModel = false;
            // if composites present
            if (this.Composites != null)
            {
                inModel = this.Composites.Complex_composite.Any(x => x.Guid == obj.Guid);
            }
            // if composites not present
            else
            {
                this.Composites = new StruSoft.Interop.StruXml.Data.DatabaseComposites();
                this.Composites.Complex_composite = new List<StruSoft.Interop.StruXml.Data.Complex_composite_type>();
            }

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Composites.Complex_composite.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add complex composite
            this.Composites.Complex_composite.Add(obj);

            // add composite section
            this.AddCompositeSection(obj, overwrite);
        }


        /// <summary>
        /// Add Cover to Model.
        /// </summary>
        private void AddCover(Cover obj, bool overwrite)
        {
            // in model?
            bool inModel = this.CoverInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.AdvancedFem.Covers.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add cover
            this.Entities.AdvancedFem.Covers.Add(obj);
        }

        /// <summary>
        /// Check if Cover in Model.
        /// </summary>
        private bool CoverInModel(Cover obj)
        {
            foreach (Cover elem in this.Entities.AdvancedFem.Covers)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddConnectedLine(ModellingTools.ConnectedLines obj, bool overwrite)
        {
            // advanced fem null?
            if (this.Entities.AdvancedFem == null)
            {
                this.Entities.AdvancedFem = new AdvancedFem();
            }

            // connected lines null?
            if (this.Entities.AdvancedFem.ConnectedLines == null)
            {
                this.Entities.AdvancedFem.ConnectedLines = new List<ModellingTools.ConnectedLines>();
            }

            // in model?
            bool inModel = this.Entities.AdvancedFem.ConnectedLines.Any(x => x.Guid == obj.Guid);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.AdvancedFem.ConnectedLines.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add connected line
            this.Entities.AdvancedFem.ConnectedLines.Add(obj);

            // add predefined rigidity
            if (obj.PredefRigidity != null)
            {
                this.AddConnectedLinesLibItem(obj.PredefRigidity, overwrite);
            }
        }

        private void AddConnectedLinesLibItem(Releases.RigidityDataLibType3 obj, bool overwrite)
        {
            // if null create new element
            if (this.LineConnectionTypes == null)
            {
                this.LineConnectionTypes = new LibraryItems.LineConnectionTypes();
                this.LineConnectionTypes.PredefinedTypes = new List<Releases.RigidityDataLibType3>();
            }

            // in model?
            bool inModel = this.LineConnectionTypes.PredefinedTypes.Any(x => x.Guid == obj.Guid);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.LineConnectionTypes.PredefinedTypes.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add lib item
            this.LineConnectionTypes.PredefinedTypes.Add(obj);
        }

        private void AddConnectedPoints(ModellingTools.ConnectedPoints obj, bool overwrite)
        {
            // advanced fem null?
            if (this.Entities.AdvancedFem == null)
            {
                this.Entities.AdvancedFem = new AdvancedFem();
            }

            // connected points null?
            if (this.Entities.AdvancedFem.ConnectedPoints == null)
            {
                this.Entities.AdvancedFem.ConnectedPoints = new List<ModellingTools.ConnectedPoints>();
            }

            // in model?
            bool inModel = this.Entities.AdvancedFem.ConnectedPoints.Any(x => x.Guid == obj.Guid);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.AdvancedFem.ConnectedPoints.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add connected point
            this.Entities.AdvancedFem.ConnectedPoints.Add(obj);

            // add predefined rigidity
            if (obj.PredefRigidity != null)
            {
                this.AddConnectedPointsLibItem(obj.PredefRigidity, overwrite);
            }
        }

        private void AddConnectedPointsLibItem(Releases.RigidityDataLibType2 obj, bool overwrite)
        {
            // if null create new element
            if (this.PointConnectionTypes == null)
            {
                this.PointConnectionTypes = new LibraryItems.PointConnectionTypes();
                this.PointConnectionTypes.PredefinedTypes = new List<Releases.RigidityDataLibType2>();
            }

            // in model?
            bool inModel = this.PointConnectionTypes.PredefinedTypes.Any(x => x.Guid == obj.Guid);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.PointConnectionTypes.PredefinedTypes.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add lib item
            this.PointConnectionTypes.PredefinedTypes.Add(obj);
        }

        /// <summary>
        /// Add Fictitious Shell to Model.
        /// </summary>
        private void AddDiaphragm(ModellingTools.Diaphragm obj, bool overwrite)
        {
            // in model?
            bool inModel = this.DiaphragmInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.AdvancedFem.Diaphragms.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add line connection types (predefined rigidity)
            foreach (Releases.RigidityDataLibType3 predef in obj.Region.GetPredefinedRigidities())
            {
                this.AddPredefinedRigidity(predef, overwrite);
            }

            this.Entities.AdvancedFem.Diaphragms.Add(obj);
        }

        /// <summary>
        /// Check if Fictitious Bar in Model.
        /// </summary>
        private bool DiaphragmInModel(ModellingTools.Diaphragm obj)
        {
            foreach (ModellingTools.Diaphragm elem in this.Entities.AdvancedFem.Diaphragms)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Add Load to Model.
        /// </summary>
        /// <param name="obj">PointLoad, LineLoad, PressureLoad, SurfaceLoad</param>
        /// <param name="overwrite"></param>
        private void AddLoad(object obj, bool overwrite)
        {
            if (obj == null)
            {
                throw new System.ArgumentException("Passed object is null");
            }
            else if (obj.GetType() == typeof(Loads.PointLoad))
            {
                this.AddPointLoad((Loads.PointLoad)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Loads.LineLoad))
            {
                this.AddLineLoad((Loads.LineLoad)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Loads.LineStressLoad))
            {
                this.AddLineStressLoad((Loads.LineStressLoad)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Loads.LineTemperatureLoad))
            {
                this.AddLineTemperatureLoad((Loads.LineTemperatureLoad)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Loads.PressureLoad))
            {
                this.AddPressureLoad((Loads.PressureLoad)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Loads.SurfaceLoad))
            {
                this.AddSurfaceLoad((Loads.SurfaceLoad)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Loads.SurfaceTemperatureLoad))
            {
                this.AddSurfaceTemperatureLoad((Loads.SurfaceTemperatureLoad)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Loads.MassConversionTable))
            {
                this.AddMassConversionTable((Loads.MassConversionTable)obj);
            }
            else if (obj.GetType() == typeof(Loads.Footfall))
            {
                this.AddFootfall((Loads.Footfall)obj, overwrite);
            }
            else
            {
                throw new System.ArgumentException("Passed object must be PointLoad, LineLoad, SurfaceLoad or PressureLoad");
            }
        }

        /// <summary>
        /// Add Panel to Model.
        /// </summary>
        private void AddPanel(Shells.Panel obj, bool overwrite)
        {
            // in model?
            bool inModel = this.PanelInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.Panels.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add panel properties
            if (obj.Material != null)
            {
                this.AddMaterial(obj.Material, overwrite);
            }

            if (obj.Section != null)
            {
                this.AddSection(obj.Section, overwrite);
            }

            // Add timber application data
            if (obj.TimberPanelData != null)
            {
                // Add library types
                if (obj.TimberPanelData.PanelType != null)
                {
                    var panelType = obj.TimberPanelData.PanelType;
                    if (panelType.GetType() == typeof(FemDesign.Materials.CltPanelLibraryType))
                    {
                        this.AddCltPanelLibraryType((FemDesign.Materials.CltPanelLibraryType)panelType, overwrite);
                    }
                    else if (panelType.GetType() == typeof(FemDesign.Materials.OrthotropicPanelLibraryType))
                    {
                        this.AddTimberPanelLibraryType((FemDesign.Materials.OrthotropicPanelLibraryType)panelType, overwrite);
                    }
                    else if (panelType.GetType() == typeof(FemDesign.Materials.GlcPanelLibraryType))
                    {
                        this.AddGlcPanelLibraryType((FemDesign.Materials.GlcPanelLibraryType)panelType, overwrite);
                    }
                    else
                    {
                        throw new System.ArgumentException($"The type {panelType.GetType()} is a member of {typeof(Materials.IPanelLibraryType)} but don't have a method for adding library data to the model.");
                    }
                }
                else
                {
                    throw new System.ArgumentException($"Could not find the related library data with guid: {obj.TimberPanelData._panelTypeReference}. Failed to add panel library data.");
                }
            }
            // add line connection types from border
            foreach (Releases.RigidityDataLibType3 predef in obj.Region.GetPredefinedRigidities())
            {
                this.AddPredefinedRigidity(predef, overwrite);
            }

            // add line connection types of internal panels
            if (obj.InternalPanels != null)
            {
                foreach (InternalPanel intPanel in obj.InternalPanels.IntPanels)
                {
                    foreach (Releases.RigidityDataLibType3 predef in intPanel.Region.GetPredefinedRigidities())
                    {
                        this.AddPredefinedRigidity(predef, overwrite);
                    }
                }
            }

            // add panel
            this.Entities.Panels.Add(obj);
        }

        /// <summary>
        /// Check if Panel in Model.
        /// </summary>
        private bool PanelInModel(Shells.Panel obj)
        {
            foreach (Shells.Panel elem in this.Entities.Panels)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add PointLoad to Model.
        /// </summary>
        private void AddPointLoad(Loads.PointLoad obj, bool overwrite)
        {
            // in model?
            bool inModel = this.PointLoadInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.Loads.PointLoads.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add point load
            this.Entities.Loads.PointLoads.Add(obj);
        }

        /// <summary>
        /// Check if PointLoad in Model.
        /// </summary>
        private bool PointLoadInModel(Loads.PointLoad obj)
        {
            foreach (Loads.PointLoad elem in this.Entities.Loads.PointLoads)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add LineLoad to Model.
        /// </summary>
        private void AddLineLoad(Loads.LineLoad obj, bool overwrite)
        {
            // in model?
            bool inModel = this.LineLoadInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite == true)
            {
                this.Entities.Loads.LineLoads.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add line load
            this.Entities.Loads.LineLoads.Add(obj);
        }

        /// <summary>
        /// Check if LineLoad in Model.
        /// </summary>
        private bool LineLoadInModel(Loads.LineLoad obj)
        {
            foreach (Loads.LineLoad elem in this.Entities.Loads.LineLoads)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        private void AddLineStressLoad(Loads.LineStressLoad obj, bool overwrite)
        {
            // line stress loads null?
            if (this.Entities.Loads.LineStressLoads == null)
            {
                this.Entities.Loads.LineStressLoads = new List<Loads.LineStressLoad>();
            }

            // in model?
            bool inModel = this.Entities.Loads.LineStressLoads.Any(x => x.Guid == obj.Guid);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.LineStressLoads.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add line stress load
            this.Entities.Loads.LineStressLoads.Add(obj);
        }

        /// <summary>
        /// Add LineTemperatureLoad to Model.
        /// </summary>
        private void AddLineTemperatureLoad(Loads.LineTemperatureLoad obj, bool overwrite)
        {
            // in model?
            bool inModel = this.LineTemperatureLoadInModel(obj);

            // in model, don't overwrite
            if (inModel && overwrite == false)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }
            else if (inModel && overwrite == true)
            {
                this.Entities.Loads.LineTemperatureLoads.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add line temperature load
            this.Entities.Loads.LineTemperatureLoads.Add(obj);
        }

        /// <summary>
        /// Check if LineTemperatureLoad in Model.
        /// </summary>
        private bool LineTemperatureLoadInModel(Loads.LineTemperatureLoad obj)
        {
            foreach (Loads.LineTemperatureLoad elem in this.Entities.Loads.LineTemperatureLoads)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add PressureLoad to Model.
        /// </summary>
        private void AddPressureLoad(Loads.PressureLoad obj, bool overwrite)
        {
            bool inModel = this.PressureLoadInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.PressureLoads.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add pressure load
            this.Entities.Loads.PressureLoads.Add(obj);
        }

        /// <summary>
        /// Check if PressureLoad in Model.
        /// </summary>
        private bool PressureLoadInModel(Loads.PressureLoad obj)
        {
            foreach (Loads.PressureLoad elem in this.Entities.Loads.PressureLoads)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add SurfaceLoad to Model.
        /// </summary>
        private void AddSurfaceLoad(Loads.SurfaceLoad obj, bool overwrite)
        {
            // in model?
            bool inModel = this.SurfaceLoadInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.SurfaceLoads.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add surface load
            this.Entities.Loads.SurfaceLoads.Add(obj);
        }

        /// <summary>
        /// Check if SurfaceLoad in Model.
        /// </summary>
        private bool SurfaceLoadInModel(Loads.SurfaceLoad obj)
        {
            foreach (Loads.SurfaceLoad elem in this.Entities.Loads.SurfaceLoads)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add SurfaceTemperatureLoad to Model.
        /// </summary>
        private void AddSurfaceTemperatureLoad(Loads.SurfaceTemperatureLoad obj, bool overwrite)
        {
            // in model?
            bool inModel = this.SurfaceTemperatureLoadInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.SurfaceTemperatureLoads.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add surface temperature loads
            this.Entities.Loads.SurfaceTemperatureLoads.Add(obj);
        }

        /// <summary>
        /// Check if SurfaceLoad in Model.
        /// </summary>
        private bool SurfaceTemperatureLoadInModel(Loads.SurfaceTemperatureLoad obj)
        {
            foreach (Loads.SurfaceTemperatureLoad elem in this.Entities.Loads.SurfaceTemperatureLoads)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add MassConversionTable to Model.
        /// MassConversionTable is always overwritten.
        /// </summary>
        private void AddMassConversionTable(Loads.MassConversionTable obj)
        {
            this.Entities.Loads.LoadCaseMassConversionTable = obj;
        }

        /// <summary>
        /// Add Footfall to Model.
        /// </summary>
        private void AddFootfall(Loads.Footfall obj, bool overwrite)
        {
            // in model?
            bool inModel = this.FootfallInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.FootfallAnalysisData.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add footfall
            this.Entities.Loads.FootfallAnalysisData.Add(obj);
        }

        /// <summary>
        /// Check if Footfall in Model.
        /// </summary>
        private bool FootfallInModel(Loads.Footfall obj)
        {
            foreach (Loads.Footfall elem in this.Entities.Loads.FootfallAnalysisData)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddLoadCases(List<Loads.LoadCase> loadCases, bool overwrite = true)
        {
            // check if model contains entities, sections and materials
            if (this.Entities == null)
                this.Entities = new Entities();

            if (loadCases != null)
                foreach (Loads.LoadCase loadCase in loadCases)
                    this.AddLoadCase(loadCase, overwrite);
        }

        /// <summary>
        /// Add LoadCase to Model.
        /// </summary>
        private void AddLoadCase(Loads.LoadCase loadCase, bool overwrite)
        {
            if (loadCase is null)
                throw new ArgumentNullException("loadCase");

            // in model?
            bool inModel = this.LoadCaseInModel(loadCase);

            // in model, don't overwrite?
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{loadCase.GetType().FullName} with guid: {loadCase.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.LoadCases.RemoveAll(x => x.Guid == loadCase.Guid);
            }

            // add load case
            if (this.LoadCaseNameTaken(loadCase))
            {
                loadCase.Name = loadCase.Name + " (1)";
            }
            this.Entities.Loads.LoadCases.Add(loadCase);
        }

        /// <summary>
        /// Check if LoadCase in Model.
        /// </summary>
        private bool LoadCaseInModel(Loads.LoadCase obj)
        {
            foreach (Loads.LoadCase elem in this.Entities.Loads.LoadCases)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if LoadCase name is in use in Model.
        /// </summary>
        private bool LoadCaseNameTaken(Loads.LoadCase obj)
        {
            foreach (Loads.LoadCase elem in this.Entities.Loads.LoadCases)
            {
                if (elem.Name == obj.Name)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddLoadCombinations(List<Loads.LoadCombination> loadCombinations, bool overwrite = true)
        {
            // check if model contains entities, sections and materials
            if (this.Entities == null)
                this.Entities = new Entities();

            if (loadCombinations != null)
                foreach (Loads.LoadCombination loadCombination in loadCombinations)
                    this.AddLoadCombination(loadCombination, overwrite);

            this.CheckCombItems();
        }

        /// <summary>
        /// Check if any load combination has a combItem object.
        /// if it does throw exception if not all of them has a combItem.
        /// Either only combItems or no combItems :)
        /// </summary>
        private void CheckCombItems()
        {
            if (this.Entities.Loads.LoadCombinations.Any(x => x.CombItem != null) && this.Entities.Loads.LoadCombinations.Any(x => x.CombItem == null))
            {
                throw new System.ArgumentException("Some load combinations have calculation setup (combItem) while others do not.");
            }
        }

        /// <summary>
        /// Add LoadGroupTable to Model.
        /// </summary>
        public void AddLoadGroupTable(List<Loads.ModelGeneralLoadGroup> generalLoadGroups, bool overwrite)
        {
            // Null or no load groups
            if (generalLoadGroups == null || generalLoadGroups.Count == 0) return;

            // check if model contains entities, sections and materials
            if (this.Entities == null)
                this.Entities = new Entities();

            // Create load group table with the sequenced general_load_group_type
            Loads.LoadGroupTable loadGroupTable = new Loads.LoadGroupTable(generalLoadGroups);

            // in model?
            bool inModel = this.LoadGroupTableInModel();

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException("The model already contains a load group table");
            }

            // not in model, or overwrite
            this.Entities.Loads.LoadGroupTable = loadGroupTable;
        }

        /// <summary>
        /// Add LoadCombination to Model.
        /// </summary>
        private void AddLoadCombination(Loads.LoadCombination loadCombination, bool overwrite)
        {
            if (loadCombination is null)
                throw new ArgumentNullException("loadCombination");

            // in model?
            bool inModel = this.LoadCombinationInModel(loadCombination);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{loadCombination.GetType().FullName} with guid: {loadCombination.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.LoadCombinations.RemoveAll(x => x.Guid == loadCombination.Guid);
            }

            // add load combination
            if (this.LoadCombinationNameTaken(loadCombination))
            {
                loadCombination.Name = loadCombination.Name + " (1)";
            }
            this.Entities.Loads.LoadCombinations.Add(loadCombination);
        }

        /// <summary>
        /// Check if LoadCombination in Model.
        /// </summary>
        private bool LoadCombinationInModel(Loads.LoadCombination obj)
        {
            foreach (Loads.LoadCombination elem in this.Entities.Loads.LoadCombinations)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if the model already has a load group table
        /// </summary>
        private bool LoadGroupTableInModel()
        {
            if (this.Entities.Loads.LoadGroupTable == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Check if LoadCombination name is in use in Model.
        /// </summary>
        private bool LoadCombinationNameTaken(Loads.LoadCombination obj)
        {
            foreach (Loads.LoadCombination elem in this.Entities.Loads.LoadCombinations)
            {
                if (elem.Name == obj.Name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Slab to Model.
        /// </summary>
        private void AddSlab(Shells.Slab obj, bool overwrite)
        {
            // in model?
            bool inModel = this.SlabInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Slabs.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add shell properties
            this.AddMaterial(obj.Material, overwrite);
            this.AddSurfaceReinforcementParameters(obj, overwrite);

            // add SurfaceReinforcement
            this.AddSurfaceReinforcements(obj, overwrite);

            // add line connection types (predefined rigidity)
            foreach (Releases.RigidityDataLibType3 predef in obj.SlabPart.Region.GetPredefinedRigidities())
            {
                this.AddPredefinedRigidity(predef, overwrite);
            }

            // add shellC
            this.Entities.Slabs.Add(obj);
        }

        /// <summary>
        /// Check if Slab in Model.
        /// </summary>
        private bool SlabInModel(Shells.Slab obj)
        {
            foreach (Shells.Slab elem in this.Entities.Slabs)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Material (reinforcing) to Model.
        /// </summary>
        private void AddReinforcingMaterial(Materials.Material obj, bool overwrite)
        {
            // in model?
            bool inModel = this.ReinforcingMaterialInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.ReinforcingMaterials.Material.RemoveAll(x => x.Guid == obj.Guid);
                this.ReinforcingMaterials.Material.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.ReinforcingMaterials.Material.Add(obj);
            }
        }

        /// <summary>
        /// Check if Material (reinforcring) in Model.
        /// </summary>
        private bool ReinforcingMaterialInModel(Materials.Material obj)
        {
            foreach (Materials.Material elem in this.ReinforcingMaterials.Material)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Add predefined rigidity
        /// </summary>
        private void AddPredefinedRigidity(Releases.RigidityDataLibType3 obj, bool overwrite)
        {
            // in model?
            bool inModel = this.PredefRigidityInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.LineConnectionTypes.PredefinedTypes.RemoveAll(x => x.Guid == obj.Guid);
                this.LineConnectionTypes.PredefinedTypes.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.LineConnectionTypes.PredefinedTypes.Add(obj);
            }
        }

        /// <summary>
        /// Check if Material (reinforcring) in Model.
        /// </summary>
        private bool PredefRigidityInModel(Releases.RigidityDataLibType3 obj)
        {
            foreach (Releases.RigidityDataLibType3 elem in this.LineConnectionTypes.PredefinedTypes)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add StructureGrid (axis or storey) to model.
        /// </summary>
        /// <param name="obj">Axis, Storey</param>
        /// <param name="overwrite"></param>
        private void AddStructureGrid(object obj, bool overwrite)
        {
            if (obj == null)
            {
                throw new System.ArgumentException("Passed object is null");
            }
            else if (obj.GetType() == typeof(StructureGrid.Axis))
            {
                this.AddAxis((StructureGrid.Axis)obj, overwrite);
            }
            else if (obj.GetType() == typeof(StructureGrid.Storey))
            {
                this.AddStorey((StructureGrid.Storey)obj, overwrite);
            }
            else
            {
                throw new System.ArgumentException("Passed object must be Axis or Storey");
            }
        }

        /// <summary>
        /// Add axis to entities.
        /// </summary>
        /// <param name="obj">Axis.</param>
        /// <param name="overwrite"></param>
        private void AddAxis(StructureGrid.Axis obj, bool overwrite)
        {
            // check if axes in entities
            if (this.Entities.Axes == null)
            {
                this.Entities.Axes = new StructureGrid.Axes();
            }

            // in model?
            bool inModel = this.AxisInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Axes.Axis.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.Axes.Axis.Add(obj);
        }

        /// <summary>
        /// Check if axis in entities.
        /// </summary>
        /// <param name="obj">Axis.</param>
        /// <returns></returns>
        private bool AxisInModel(StructureGrid.Axis obj)
        {
            foreach (StructureGrid.Axis elem in this.Entities.Axes.Axis)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Storey to Model.
        /// </summary>
        /// <param name="obj">Storey.</param>
        /// <param name="overwrite"></param>
        private void AddStorey(StructureGrid.Storey obj, bool overwrite)
        {
            // check if storeys in entities
            if (this.Entities.Storeys == null)
            {
                this.Entities.Storeys = new StructureGrid.Storeys();
            }

            // in model?
            bool inModel = this.StoreyInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            else if (inModel && overwrite)
            {
                this.Entities.Storeys.Storey.RemoveAll(x => x.Guid == obj.Guid);
            }

            // check if geometry is consistent
            this.ConsistenStoreyGeometry(obj);

            // add to storeys
            this.Entities.Storeys.Storey.Add(obj);
        }

        /// <summary>
        /// Check if storey in entities.
        /// </summary>
        /// <param name="obj">Storey.</param>
        /// <returns></returns>
        private bool StoreyInModel(StructureGrid.Storey obj)
        {
            foreach (StructureGrid.Storey elem in this.Entities.Storeys.Storey)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Check if geometry of storey is consistent with geometry of storeys aldread added.
        /// Storey origo should share XY-coordinates. Z-coordinate should be unique.
        /// Storey direction should be identical.
        /// </summary>
        /// <param name="obj"></param>
        private void ConsistenStoreyGeometry(StructureGrid.Storey obj)
        {
            foreach (StructureGrid.Storey elem in this.Entities.Storeys.Storey)
            {
                if (elem.Origo.X != obj.Origo.X || elem.Origo.Y != obj.Origo.Y)
                {
                    throw new System.ArgumentException($"Storey does not share XY-coordinates with storeys in model (point x: {elem.Origo.X}, y: {elem.Origo.Y}). If model was empty make sure all storeys added to model share XY-coordinates.");
                }
                if (!elem.Direction.Equals(obj.Direction))
                {
                    throw new System.ArgumentException($"Storey does not share direction with storeys in model (vector i: {elem.Direction.X} , j: {elem.Direction.Y}). If model was empty make sure all storeys added to model share direction.");
                }
            }
        }

        /// <summary>
        /// Add BarReinforcement(s) from Bar to Model.
        /// </summary>
        private void AddBarReinforcements(Bars.Bar obj, bool overwrite)
        {
            foreach (Reinforcement.BarReinforcement barReinf in obj.Reinforcement)
            {
                this.AddReinforcingMaterial(barReinf.Wire.ReinforcingMaterial, overwrite);
                this.AddBarReinforcement(barReinf, overwrite);
            }
        }

        /// <summary>
        /// Add BarReinforcement to Model.
        /// </summary>
        private void AddBarReinforcement(Reinforcement.BarReinforcement obj, bool overwrite)
        {
            // in model?
            bool inModel = this.BarReinforcementInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Did you add the same {obj.GetType().FullName} to different Bars?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.BarReinforcements.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.BarReinforcements.Add(obj);
        }

        /// <summary>
        /// Check if BarReinforcement in Model.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool BarReinforcementInModel(Reinforcement.BarReinforcement obj)
        {
            foreach (Reinforcement.BarReinforcement elem in this.Entities.BarReinforcements)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add SurfaceReinforcement(s) from Slab to Model.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="overwrite"></param>
        private void AddSurfaceReinforcements(Shells.Slab obj, bool overwrite)
        {
            foreach (Reinforcement.SurfaceReinforcement surfaceReinforcement in obj.SurfaceReinforcement)
            {
                this.AddReinforcingMaterial(surfaceReinforcement.Wire.ReinforcingMaterial, overwrite);
                this.AddSurfaceReinforcement(surfaceReinforcement, overwrite);
            }
        }


        /// <summary>
        /// Add SurfaceReinforcement to Model.
        /// </summary>
        private void AddSurfaceReinforcement(Reinforcement.SurfaceReinforcement obj, bool overwrite)
        {
            // in model?
            bool inModel = this.SurfaceReinforcementInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Did you add the same {obj.GetType().FullName} to different Slabs?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.SurfaceReinforcements.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.SurfaceReinforcements.Add(obj);

        }

        /// <summary>
        /// Check if SurfaceReinforcement in Model.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool SurfaceReinforcementInModel(Reinforcement.SurfaceReinforcement obj)
        {
            foreach (Reinforcement.SurfaceReinforcement elem in this.Entities.SurfaceReinforcements)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add SurfaceReinforcementParameters to Model.
        /// </summary>
        private void AddSurfaceReinforcementParameters(Shells.Slab slab, bool overwrite)
        {
            if (slab.SurfaceReinforcementParameters != null)
            {
                // obj
                Reinforcement.SurfaceReinforcementParameters obj = slab.SurfaceReinforcementParameters;
                // in model?
                bool inModel = this.SurfaceReinforcementParametersInModel(obj);

                // in model, don't overwrite
                if (inModel && !overwrite)
                {
                    throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
                }

                // in model, overwrite
                else if (inModel && overwrite)
                {
                    this.Entities.SurfaceReinforcementParameters.RemoveAll(x => x.Guid == obj.Guid);
                }

                // add obj
                this.Entities.SurfaceReinforcementParameters.Add(obj);
            }
        }

        /// <summary>
        /// Check if SurfaceReinforcementParameters in Model.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool SurfaceReinforcementParametersInModel(Reinforcement.SurfaceReinforcementParameters obj)
        {
            foreach (Reinforcement.SurfaceReinforcementParameters elem in this.Entities.SurfaceReinforcementParameters)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Support to Model
        /// </summary>
        /// <param name="obj">PointSupport, LineSupport or SurfaceSupport</param>
        /// <param name="overwrite"></param>
        private void AddSupport(ISupportElement obj, bool overwrite)
        {
            if (obj == null)
            {
                throw new System.ArgumentException("Passed object is null");
            }
            else if (obj.GetType() == typeof(Supports.PointSupport))
            {
                this.AddPointSupport((Supports.PointSupport)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Supports.LineSupport))
            {
                this.AddLineSupport((Supports.LineSupport)obj, overwrite);
            }
            else if (obj.GetType() == typeof(Supports.SurfaceSupport))
            {
                this.AddSurfaceSupport((Supports.SurfaceSupport)obj, overwrite);
            }
            else
            {
                throw new System.ArgumentException("Passed object must be PointSupport, LineSupport or SurfaceSupport");
            }
        }

        /// <summary>
        /// Add PointSupport to Model.
        /// </summary>
        private void AddPointSupport(Supports.PointSupport obj, bool overwrite)
        {
            // in model?
            bool inModel = this.PointSupportInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Supports.PointSupport.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.Supports.PointSupport.Add(obj);

            // add predefined rigidity
            if (obj.Group?.PredefRigidity != null)
            {
                this.AddPointSupportGroupLibItem(obj.Group.PredefRigidity, overwrite);
            }
        }

        /// <summary>
        /// Add predefined point support rigidity to model
        /// </summary>
        private void AddPointSupportGroupLibItem(Releases.RigidityDataLibType2 obj, bool overwrite)
        {
            // if null create new element
            if (this.PointSupportGroupTypes == null)
            {
                this.PointSupportGroupTypes = new LibraryItems.PointSupportGroupTypes();
                this.PointSupportGroupTypes.PredefinedTypes = new List<Releases.RigidityDataLibType2>();
            }

            // in model?
            bool inModel = this.PointSupportGroupTypes.PredefinedTypes.Any(x => x.Guid == obj.Guid);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.PointSupportGroupTypes.PredefinedTypes.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add lib item
            this.PointSupportGroupTypes.PredefinedTypes.Add(obj);
        }

        /// <summary>
        /// Check if PointSupport in Model.
        /// </summary>
        private bool PointSupportInModel(Supports.PointSupport obj)
        {
            foreach (Supports.PointSupport elem in this.Entities.Supports.PointSupport)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// Add LineSupport to Model.
        /// </summary>
        private void AddLineSupport(Supports.LineSupport obj, bool overwrite)
        {
            // in model?
            bool inModel = this.LineSupportInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Supports.LineSupport.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.Supports.LineSupport.Add(obj);

            // add lib item
            if (obj.Group.PredefRigidity != null)
            {
                this.AddLineSupportGroupLibItem(obj.Group.PredefRigidity, overwrite);
            }
        }

        /// <summary>
        /// Check if LineSupport in Model.
        /// </summary>
        private bool LineSupportInModel(Supports.LineSupport obj)
        {
            foreach (Supports.LineSupport elem in this.Entities.Supports.LineSupport)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add predefined line support rigidity to model
        /// </summary>
        private void AddLineSupportGroupLibItem(Releases.RigidityDataLibType2 obj, bool overwrite)
        {
            // if null create new element
            if (this.LineSupportGroupTypes == null)
            {
                this.LineSupportGroupTypes = new LibraryItems.LineSupportGroupTypes();
                this.LineSupportGroupTypes.PredefinedTypes = new List<Releases.RigidityDataLibType2>();
            }

            // in model?
            bool inModel = this.LineSupportGroupTypes.PredefinedTypes.Any(x => x.Guid == obj.Guid);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.LineSupportGroupTypes.PredefinedTypes.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.LineSupportGroupTypes.PredefinedTypes.Add(obj);
        }

        /// <summary>
        /// Add SurfaceSupport to Model.
        /// </summary>
        private void AddSurfaceSupport(Supports.SurfaceSupport obj, bool overwrite)
        {
            // in model?
            bool inModel = this.SurfaceSupportInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Supports.SurfaceSupport.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.Supports.SurfaceSupport.Add(obj);

            // add lib item
            if (obj.PredefRigidity != null)
            {
                this.AddSurfaceSupportLibItem(obj.PredefRigidity, overwrite);
            }
        }

        /// <summary>
        /// Check if SurfaceSupport in Model.
        /// </summary>
        private bool SurfaceSupportInModel(Supports.SurfaceSupport obj)
        {
            foreach (Supports.SurfaceSupport elem in this.Entities.Supports.SurfaceSupport)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add predefined surface support rigidity to model
        /// </summary>
        private void AddSurfaceSupportLibItem(Releases.RigidityDataLibType1 obj, bool overwrite)
        {
            // if null create new element
            if (this.SurfaceSupportTypes == null)
            {
                this.SurfaceSupportTypes = new LibraryItems.SurfaceSupportTypes();
                this.SurfaceSupportTypes.PredefinedTypes = new List<Releases.RigidityDataLibType1>();
            }

            // in model?
            bool inModel = this.SurfaceSupportTypes.PredefinedTypes.Any(x => x.Guid == obj.Guid);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.SurfaceSupportTypes.PredefinedTypes.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.SurfaceSupportTypes.PredefinedTypes.Add(obj);
        }


        /// <summary>
        /// Add SurfaceSupport to Model.
        /// </summary>
        private void AddStiffnessPoint(Supports.StiffnessPoint obj, bool overwrite)
        {
            // in model?
            bool inModel = this.StiffnessPointInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Supports.StiffnessPoint.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.Supports.StiffnessPoint.Add(obj);
        }


        /// <summary>
        /// Check if StiffnessPoint in Model.
        /// </summary>
        private bool StiffnessPointInModel(Supports.StiffnessPoint obj)
        {
            foreach (Supports.StiffnessPoint elem in this.Entities.Supports.StiffnessPoint)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Add Material to Model.
        /// </summary>
        private void AddMaterial(Materials.Material obj, bool overwrite)
        {
            // in model?
            bool inModel = this.MaterialInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Materials.Material.RemoveAll(x => x.Guid == obj.Guid);
                this.Materials.Material.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.Materials.Material.Add(obj);
            }
        }

        /// <summary>
        /// Check if Material in Model.
        /// </summary>
        private bool MaterialInModel(Materials.Material obj)
        {
            foreach (Materials.Material elem in this.Materials.Material)
            {
                if (obj != null && elem != null)
                {
                    if (elem.Guid == obj.Guid)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void AddPtcStrandType(Reinforcement.PtcStrandLibType obj, bool overwrite)
        {
            bool inModel = this.PtcStrandTypeInModel(obj);
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }
            else if (inModel && overwrite)
            {
                this.PtcStrandTypes.PtcStrandLibTypes.RemoveAll(x => x.Guid == obj.Guid);
                this.PtcStrandTypes.PtcStrandLibTypes.Add(obj);
            }
            else if (!inModel)
                this.PtcStrandTypes.PtcStrandLibTypes.Add(obj);
        }

        private bool PtcStrandTypeInModel(Reinforcement.PtcStrandLibType obj)
        {
            foreach (Reinforcement.PtcStrandLibType elem in this.PtcStrandTypes.PtcStrandLibTypes)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Timber panel library type to Model.
        /// </summary>
        private void AddTimberPanelLibraryType(Materials.OrthotropicPanelLibraryType obj, bool overwrite)
        {
            // if null create new element
            if (this.OrthotropicPanelTypes == null)
            {
                this.OrthotropicPanelTypes = new Materials.OrthotropicPanelTypes();
                this.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes = new List<Materials.OrthotropicPanelLibraryType>();
            }

            // in model?
            bool inModel = this.TimberPanelLibraryTypeInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes.RemoveAll(x => x.Guid == obj.Guid);
                this.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes.Add(obj);
            }
        }

        /// <summary>
        /// Check if Timber panel library type in Model.
        /// </summary>
        private bool TimberPanelLibraryTypeInModel(Materials.OrthotropicPanelLibraryType obj)
        {
            foreach (Materials.OrthotropicPanelLibraryType elem in this.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Clt panel library type to Model.
        /// </summary>
        private void AddCltPanelLibraryType(Materials.CltPanelLibraryType obj, bool overwrite)
        {
            // if null create new element
            if (this.CltPanelTypes == null)
            {
                this.CltPanelTypes = new Materials.CltPanelTypes();
                this.CltPanelTypes.CltPanelLibraryTypes = new List<Materials.CltPanelLibraryType>();
            }

            // in model?
            bool inModel = this.CltPanelLibraryTypeInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.CltPanelTypes.CltPanelLibraryTypes.RemoveAll(x => x.Guid == obj.Guid);
                this.CltPanelTypes.CltPanelLibraryTypes.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.CltPanelTypes.CltPanelLibraryTypes.Add(obj);
            }
        }

        /// <summary>
        /// Check if Clt panel library type in Model.
        /// </summary>
        private bool CltPanelLibraryTypeInModel(Materials.CltPanelLibraryType obj)
        {
            foreach (Materials.CltPanelLibraryType elem in this.CltPanelTypes.CltPanelLibraryTypes)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Glc panel library type to Model.
        /// </summary>
        private void AddGlcPanelLibraryType(Materials.GlcPanelLibraryType obj, bool overwrite)
        {
            // if null create new element
            if (this.GlcPanelTypes == null)
            {
                this.GlcPanelTypes = new Materials.GlcPanelTypes();
                this.GlcPanelTypes.GlcPanelLibraryTypes = new List<Materials.GlcPanelLibraryType>();
            }

            // in model?
            bool inModel = this.GlcPanelLibraryTypeInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.GlcPanelTypes.GlcPanelLibraryTypes.RemoveAll(x => x.Guid == obj.Guid);
                this.GlcPanelTypes.GlcPanelLibraryTypes.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.GlcPanelTypes.GlcPanelLibraryTypes.Add(obj);
            }
        }

        /// <summary>
        /// Check if Glc panel library type in Model.
        /// </summary>
        private bool GlcPanelLibraryTypeInModel(Materials.GlcPanelLibraryType obj)
        {
            foreach (Materials.GlcPanelLibraryType elem in this.GlcPanelTypes.GlcPanelLibraryTypes)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Section to Model.
        /// </summary>
        private void AddSection(FemDesign.Sections.Section obj, bool overwrite)
        {
            // in model?
            bool inModel = this.SectionInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Sections.Section.RemoveAll(x => x.Guid == obj.Guid);
                this.Sections.Section.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.Sections.Section.Add(obj);
            }
        }

        /// <summary>
        /// Check if Section in Model.
        /// </summary>
        private bool SectionInModel(FemDesign.Sections.Section obj)
        {
            foreach (FemDesign.Sections.Section elem in this.Sections.Section)
            {
                if (obj != null && elem != null)
                {
                    if (elem.Guid == obj.Guid)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Add LabelledSection to Model
        /// </summary>
        private void AddLabelledSection(AuxiliaryResults.LabelledSection obj, bool overwrite)
        {
            if (this.Entities.LabelledSections == null)
            {
                this.Entities.LabelledSections = new AuxiliaryResults.LabelledSectionsGeometry();
            }

            // in model?
            bool inModel = this.LabelledSectionInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                // pass - note that this should not throw an exception.
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.LabelledSections.LabelledSections.RemoveAll(x => x.Guid == obj.Guid);
                this.Entities.LabelledSections.LabelledSections.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.Entities.LabelledSections.LabelledSections.Add(obj);
            }
        }

        /// <summary>
        /// Check if LabelledSection in Model
        /// </summary>
        private bool LabelledSectionInModel(AuxiliaryResults.LabelledSection obj)
        {
            foreach (AuxiliaryResults.LabelledSection elem in this.Entities.LabelledSections.LabelledSections)
            {
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }


        public void SetConstructionStages(List<Stage> stages, bool assignModifedElement = false, bool assignNewElement = false, bool ghostMethod = false)
        {
            var obj = new ConstructionStages(stages, assignModifedElement, assignNewElement, ghostMethod);
            SetConstructionStages(obj);
        }


        private void SetConstructionStages(ConstructionStages obj)
        {
            if (this.ConstructionStages == null)
            {
                this.ConstructionStages = new ConstructionStages();
            }
            this.ConstructionStages = obj;

            if (obj.Stages.Count == 1)
            {
                throw new Exception("Number of Stages must be greater than 2");
            }

            foreach (var stage in obj.Stages)
            {
                if (stage.Elements != null)
                {
                    foreach (var element in stage.Elements)
                    {
                        //var newElement = element.DeepClone();
                        element.StageId = stage.Id;
                    }
                }
            }
        }

        #endregion

        #region AddElements and AddLoads

        /// <summary>
        /// Add structural elements to Model. 
        /// </summary>
        /// <typeparam name="T">Structural elements (IStructureElement).</typeparam>
        /// <param name="elements">Structural elements to be added.</param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public Model AddElements<T>(IEnumerable<T> elements, bool overwrite = true) where T : IStructureElement
        {
            // check if model contains entities, sections and materials
            if (this.Entities == null)
                this.Entities = new Entities();

            foreach (var item in elements)
            {
                try
                {
                    AddEntity(item as dynamic, overwrite);
                }
                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException exeption)
                {
                    if (item == null)
                        throw new ArgumentNullException("Can not add null element to model.", exeption);
                    throw new System.NotImplementedException($"Class Model don't have a method AddEntity that accepts {item.GetType()}. ", exeption);
                }
            }

            return this;
        }

        /// <summary>
        /// Adds loads to the model.
        /// </summary>
        /// <typeparam name="T">ILoadElement is any load object in FEM-Design.</typeparam>
        /// <param name="elements">Load elements to be added.</param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public Model AddLoads<T>(IEnumerable<T> elements, bool overwrite = true) where T : ILoadElement
        {
            // check if model contains entities
            if (this.Entities == null)
                this.Entities = new Entities();

            foreach (var item in elements)
            {
                try
                {
                    AddEntity(item as dynamic, overwrite);
                }
                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException exeption)
                {
                    if (item == null)
                        throw new ArgumentNullException("Can not add null load to model.", exeption);
                    throw new System.NotImplementedException($"Class Model don't have a method AddEntity that accepts {item.GetType()}. ", exeption);
                }
            }

            return this;
        }

        /// <summary>
        /// Add supports to the model.
        /// </summary>
        /// <typeparam name="T">ISuppotElement is any support object.</typeparam>
        /// <param name="elements">Support elements to be added.</param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        public Model AddSupports<T>(IEnumerable<T> elements, bool overwrite = true) where T : ISupportElement
        {
            // check if model contains entities, sections and materials
            if (this.Entities == null)
            {
                this.Entities = new Entities();
            }

            foreach (var item in elements)
            {
                try
                {
                    AddEntity(item as dynamic, overwrite);
                }
                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException exeption)
                {
                    if (item == null)
                        throw new ArgumentNullException("Can not add null support to model.", exeption);
                    throw new System.NotImplementedException($"Class Model don't have a method AddEntity that accepts {item.GetType()}. ", exeption);
                }
            }

            return this;
        }

        private void AddEntity(Bars.Bar obj, bool overwrite) => AddBar(obj, overwrite);
        private void AddEntity(Shells.Slab obj, bool overwrite) => AddSlab(obj, overwrite);
        private void AddEntity(Shells.Panel obj, bool overwrite) => AddPanel(obj, overwrite);
        private void AddEntity(Reinforcement.Ptc obj, bool overwrite) => AddPtc(obj, overwrite);


        private void AddEntity(Cover obj, bool overwrite) => AddCover(obj, overwrite);

        private void AddEntity(ModellingTools.FictitiousShell obj, bool overwrite) => AddFictShell(obj, overwrite);
        private void AddEntity(ModellingTools.FictitiousBar obj, bool overwrite) => AddFictBar(obj, overwrite);
        private void AddEntity(ModellingTools.ConnectedPoints obj, bool overwrite) => AddConnectedPoints(obj, overwrite);
        private void AddEntity(ModellingTools.ConnectedLines obj, bool overwrite) => AddConnectedLine(obj, overwrite);
        //private void AddEntity(ModellingTools.SurfaceConnection obj, bool overwrite) => AddSurfaceConnection(obj, overwrite);
        private void AddEntity(ModellingTools.Diaphragm obj, bool overwrite) => AddDiaphragm(obj, overwrite);

        private void AddEntity(AuxiliaryResults.LabelledSection obj, bool overwrite) => AddLabelledSection(obj, overwrite);

        private void AddEntity(Supports.PointSupport obj, bool overwrite) => AddPointSupport(obj, overwrite);
        private void AddEntity(Supports.LineSupport obj, bool overwrite) => AddLineSupport(obj, overwrite);
        private void AddEntity(Supports.SurfaceSupport obj, bool overwrite) => AddSurfaceSupport(obj, overwrite);
        private void AddEntity(Supports.StiffnessPoint obj, bool overwrite) => AddStiffnessPoint(obj, overwrite);

        private void AddEntity(StructureGrid.Axis axis, bool overwrite) => AddAxis(axis, overwrite);
        private void AddEntity(StructureGrid.Storey storey, bool overwrite) => AddStorey(storey, overwrite);

        private void AddEntity(Loads.PointLoad obj, bool overwrite) => AddPointLoad(obj, overwrite);
        private void AddEntity(Loads.SurfaceTemperatureLoad obj, bool overwrite) => AddSurfaceTemperatureLoad(obj, overwrite);
        private void AddEntity(Loads.SurfaceLoad obj, bool overwrite) => AddSurfaceLoad(obj, overwrite);
        private void AddEntity(Loads.PressureLoad obj, bool overwrite) => AddPressureLoad(obj, overwrite);
        private void AddEntity(Loads.LineTemperatureLoad obj, bool overwrite) => AddLineTemperatureLoad(obj, overwrite);
        private void AddEntity(Loads.LineStressLoad obj, bool overwrite) => AddLineStressLoad(obj, overwrite);
        private void AddEntity(Loads.LineLoad obj, bool overwrite) => AddLineLoad(obj, overwrite);
        private void AddEntity(Loads.Footfall obj, bool overwrite) => AddFootfall(obj, overwrite);
        private void AddEntity(Loads.MassConversionTable obj, bool overwrite) => AddMassConversionTable(obj);

        private void AddEntity(Loads.LoadCase obj, bool overwrite) => AddLoadCase(obj, overwrite);
        private void AddEntity(Loads.LoadCombination obj, bool overwrite) => AddLoadCombination(obj, overwrite);


        #endregion

        #region deconstruct
        /// <summary>
        /// Get Bars from Model. 
        /// Bars will be reconstructed from Model incorporating all references: ComplexSection, Section, Material.
        /// </summary>
        internal void GetBars()
        {
            Dictionary<Guid, Sections.ComplexSection> complexSectionsMap = this.Sections.ComplexSection.ToDictionary(s => s.Guid, s => s.DeepClone());
            Dictionary<Guid, Materials.Material> materialMap = this.Materials.Material.ToDictionary(d => d.Guid, d => d.DeepClone());
            Dictionary<Guid, Sections.Section> sectionsMap = this.Sections.Section.ToDictionary(s => s.Guid, s => s.DeepClone());
            Dictionary<Guid, StruSoft.Interop.StruXml.Data.Complex_composite_type> complexCompositeMap = new Dictionary<Guid, StruSoft.Interop.StruXml.Data.Complex_composite_type>();
            Dictionary<Guid, StruSoft.Interop.StruXml.Data.Composite_data> compositeSectionMap = new Dictionary<Guid, StruSoft.Interop.StruXml.Data.Composite_data>();

            if (this.Composites != null)
            {
                complexCompositeMap = this.Composites.Complex_composite.ToDictionary(s => Guid.Parse(s.Guid), s => s.DeepClone());
                compositeSectionMap = this.Composites.Composite_section.ToDictionary(s => Guid.Parse(s.Guid), s => s.DeepClone());
            }

            foreach (Bars.Bar item in this.Entities.Bars)
            {
                // set type on barPart
                item.BarPart.Type = item.Type;

                // get section and material for truss
                if (item.BarPart.SectionType == Bars.SectionType.Truss)
                {
                    // section
                    try
                    {
                        item.BarPart.TrussUniformSectionObj = sectionsMap[new System.Guid(item.BarPart.ComplexSectionRef)];
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ArgumentException("No matching section found. Model.GetBars() failed.");
                    }
                    catch (ArgumentNullException)
                    {
                        throw new ArgumentNullException($"BarPart {item.BarPart.Name} BarPart.ComplexSectionRef is null");
                    }

                    // material
                    try
                    {
                        item.BarPart.ComplexMaterialObj = materialMap[item.BarPart.ComplexMaterialRef];
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ArgumentException("No matching material found. Model.GetBars() failed.");
                    }
                    catch (ArgumentNullException)
                    {
                        throw new ArgumentNullException($"BarPart {item.BarPart.Name} BarPart.ComplexMaterialRef is null");
                    }
                }
                // do nothing for beam or column with complex section (delta beam type)
                else if (item.BarPart.SectionType == Bars.SectionType.DeltaBeamColumn)
                {
                    // pass
                }
                // get section and material for beam or column with complex section (not delta beam type)
                else if (item.BarPart.SectionType == Bars.SectionType.RegularBeamColumn)
                {
                    // section
                    try
                    {
                        // complex section
                        item.BarPart.ComplexSectionObj = complexSectionsMap[new System.Guid(item.BarPart.ComplexSectionRef)];

                        // sections
                        try
                        {
                            foreach (Sections.ComplexSectionPart part in item.BarPart.ComplexSectionObj.Parts)
                            {
                                part.SectionObj = sectionsMap[part.SectionRef];
                            }
                        }
                        catch (KeyNotFoundException)
                        {
                            throw new ArgumentException("No matching section found. Model.GetBars() failed.");
                        }
                    }
                    catch (ArgumentNullException)
                    {
                        throw new ArgumentNullException($"BarPart {item.BarPart.Name} BarPart.ComplexSectionRef is null");
                    }

                    // material
                    try
                    {
                        item.BarPart.ComplexMaterialObj = materialMap[item.BarPart.ComplexMaterialRef];
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ArgumentException("No matching complex material found. Model.GetBars() failed.");
                    }
                }

                // get section and material for beam or column with complex composite section
                else if (item.BarPart.SectionType == Bars.SectionType.CompositeBeamColumn)
                {
                    try
                    {
                        // assign the Complex Composite Object to the bar part
                        item.BarPart.ComplexCompositeObj = complexCompositeMap[new System.Guid(item.BarPart.ComplexCompositeRef)];

                        // iterate over the composite section inside the complex composite and assign the object from the database Composite
                        foreach (StruSoft.Interop.StruXml.Data.Composite_section_type compositeSection in item.BarPart.ComplexCompositeObj.Composite_section)
                        {
                            compositeSection.CompositeSectionDataObj = compositeSectionMap[Guid.Parse(compositeSection.Guid)];
                        }

                        // assign the material object to the Composite_part_type
                        // it might be clever to move this method outside the loop and call it (add compositePart)
                        foreach (var compositeSection in item.BarPart.ComplexCompositeObj.Composite_section)
                        {
                            foreach (var compositePart in compositeSection.CompositeSectionDataObj.Part)
                            {
                                compositePart.MaterialObj = materialMap[Guid.Parse(compositePart.Material)];
                                compositePart.SectionObj = sectionsMap[Guid.Parse(compositePart.Section)];
                            }
                        }
                    }
                    catch (KeyNotFoundException)
                    {
                        throw new ArgumentException("No matching complex composite or composite section");
                    }
                }
                else
                {
                    throw new System.ArgumentException("Type of bar is not supported.");
                }

                // get bar reinforcement
                foreach (Reinforcement.BarReinforcement barReinf in this.Entities.BarReinforcements)
                {
                    if (barReinf.BaseBar.Guid == item.BarPart.Guid)
                    {
                        // get wire material
                        foreach (Materials.Material material in this.ReinforcingMaterials.Material)
                        {
                            if (barReinf.Wire.ReinforcingMaterialGuid == material.Guid)
                            {
                                barReinf.Wire.ReinforcingMaterial = material;
                            }
                        }

                        // check if material found
                        if (barReinf.Wire.ReinforcingMaterial == null)
                        {
                            throw new System.ArgumentException("No matching reinforcement wire material found. Model.GetBars() failed.");
                        }
                        else
                        {
                            // add bar reinforcement to bar
                            item.Reinforcement.Add(barReinf);
                        }

                    }
                }

                // get ptc
                foreach (Reinforcement.Ptc ptc in this.Entities.PostTensionedCables)
                {
                    if (ptc.BaseObject == item.BarPart.Guid)
                    {
                        // get strand material
                        foreach (Reinforcement.PtcStrandLibType material in this.PtcStrandTypes.PtcStrandLibTypes)
                        {
                            if (ptc.StrandTypeGuid == material.Guid)
                            {
                                ptc.StrandType = material;
                            }
                        }

                        // check if material found
                        if (ptc.StrandType == null)
                        {
                            throw new System.ArgumentException("No matching ptc strand found. Model.GetBars() failed.");
                        }
                        else
                        {
                            // add ptc to bar
                            item.Ptc.Add(ptc);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get FictitiousShells from Model.
        /// FicititiousShells will be reconstruted from Model incorporating predefined EdgeConnections
        /// </summary>
        /// <returns></returns>
        internal void GetFictitiousShells()
        {
            foreach (ModellingTools.FictitiousShell item in this.Entities.AdvancedFem.FictitiousShells)
            {
                // set line_connection_types (i.e predefined edge connections) on edge
                if (this.LineConnectionTypes != null)
                {
                    if (this.LineConnectionTypes.PredefinedTypes != null)
                    {
                        item.Region.SetPredefinedRigidities(this.LineConnectionTypes.PredefinedTypes);
                    }
                }
            }
        }

        /// <summary>
        /// Get Slabs from Model.
        /// Slabs will be reconstruted from Model incorporating all references: Material, Predefined EdgeConnections, SurfaceReinforcementParameters, SurfaceReinforcement.
        /// </summary>
        /// <returns></returns>
        internal void GetSlabs()
        {
            foreach (Shells.Slab item in this.Entities.Slabs)
            {
                // get material
                foreach (Materials.Material _material in this.Materials.Material)
                {
                    if (_material.Guid == item.SlabPart.ComplexMaterialGuid)
                    {
                        item.Material = _material;
                    }
                }

                // set line_connection_types (i.e predefined edge connections) on edge
                if (this.LineConnectionTypes != null)
                {
                    if (this.LineConnectionTypes.PredefinedTypes != null)
                    {
                        item.SlabPart.Region.SetPredefinedRigidities(this.LineConnectionTypes.PredefinedTypes);
                    }
                }

                // get surface reinforcement parameters
                foreach (Reinforcement.SurfaceReinforcementParameters surfaceReinforcementParameter in this.Entities.SurfaceReinforcementParameters)
                {
                    if (surfaceReinforcementParameter.BaseShell.Guid == item.SlabPart.Guid)
                    {
                        item.SurfaceReinforcementParameters = surfaceReinforcementParameter;
                    }
                }

                // get surface reinforcement
                foreach (Reinforcement.SurfaceReinforcement surfaceReinforcement in this.Entities.SurfaceReinforcements)
                {
                    if (surfaceReinforcement.BaseShell.Guid == item.SlabPart.Guid)
                    {

                        // get wire material
                        foreach (Materials.Material material in this.ReinforcingMaterials.Material)
                        {
                            if (surfaceReinforcement.Wire.ReinforcingMaterialGuid == material.Guid)
                            {
                                surfaceReinforcement.Wire.ReinforcingMaterial = material;
                            }
                        }

                        // add surface reinforcement to slab
                        item.SurfaceReinforcement.Add(surfaceReinforcement);
                    }
                }

                // check if material found
                if (item.Material == null)
                {
                    throw new System.ArgumentException("No matching material found. Model.GeSlabs() failed.");
                }
            }
        }

        internal void GetPanels()
        {
            foreach (Shells.Panel panel in this.Entities.Panels)
            {
                // get material
                foreach (Materials.Material material in this.Materials.Material)
                {
                    if (material.Guid == panel.ComplexMaterial)
                    {
                        panel.Material = material;
                    }
                }

                // get section
                foreach (Sections.Section section in this.Sections.Section)
                {
                    if (section.Guid == panel.ComplexSection)
                    {
                        panel.Section = section;
                    }
                }

                // get timber application data
                if (panel.TimberPanelData != null)
                {
                    // timber panel types
                    if (this.OrthotropicPanelTypes != null && this.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes != null)
                    {
                        foreach (FemDesign.Materials.OrthotropicPanelLibraryType libItem in this.OrthotropicPanelTypes.OrthotropicPanelLibraryTypes)
                        {
                            if (libItem.Guid == panel.TimberPanelData._panelTypeReference)
                            {
                                panel.TimberPanelData.PanelType = libItem;
                            }
                        }
                    }

                    // clt panel types
                    if (this.CltPanelTypes != null && this.CltPanelTypes.CltPanelLibraryTypes != null)
                    {
                        foreach (FemDesign.Materials.CltPanelLibraryType libItem in this.CltPanelTypes.CltPanelLibraryTypes)
                        {
                            if (libItem.Guid == panel.TimberPanelData._panelTypeReference)
                            {
                                panel.TimberPanelData.PanelType = libItem;
                            }
                        }
                    }

                    // glc panel types
                    if (this.GlcPanelTypes != null && this.GlcPanelTypes.GlcPanelLibraryTypes != null)
                    {
                        foreach (FemDesign.Materials.GlcPanelLibraryType libItem in this.GlcPanelTypes.GlcPanelLibraryTypes)
                        {
                            if (libItem.Guid == panel.TimberPanelData._panelTypeReference)
                            {
                                panel.TimberPanelData.PanelType = libItem;
                            }
                        }
                    }

                    // check if libItem found
                    if (panel.TimberPanelData.PanelType == null)
                    {
                        throw new System.ArgumentException("An orthotropic/clt/glc library item was expected but not found. Can't construct Panel. Model.GetPanels() failed.");
                    }
                }

                // predefined rigidity
                if (this.LineConnectionTypes != null)
                {
                    if (this.LineConnectionTypes.PredefinedTypes != null)
                    {
                        panel.Region.SetPredefinedRigidities(this.LineConnectionTypes.PredefinedTypes);
                        foreach (InternalPanel internalPanel in panel.InternalPanels.IntPanels)
                        {
                            internalPanel.Region.SetPredefinedRigidities(this.LineConnectionTypes.PredefinedTypes);
                        }
                    }
                }
            }
        }

        internal void GetPointSupports()
        {
            foreach (Supports.PointSupport pointSupport in this.Entities.Supports.PointSupport)
            {
                // predefined rigidity
                if (this.PointSupportGroupTypes != null && this.PointSupportGroupTypes.PredefinedTypes != null)
                {
                    foreach (Releases.RigidityDataLibType2 predefinedType in this.PointSupportGroupTypes.PredefinedTypes)
                    {
                        if (pointSupport.Group._predefRigidityRef != null && predefinedType.Guid == pointSupport.Group._predefRigidityRef.Guid)
                        {
                            pointSupport.Group.PredefRigidity = predefinedType;
                        }
                    }
                }
            }
        }

        internal void GetLineSupports()
        {
            foreach (Supports.LineSupport lineSupport in this.Entities.Supports.LineSupport)
            {
                // predefined rigidity
                if (this.LineSupportGroupTypes != null && this.LineSupportGroupTypes.PredefinedTypes != null)
                {
                    foreach (Releases.RigidityDataLibType2 predefinedType in this.LineSupportGroupTypes.PredefinedTypes)
                    {
                        if (lineSupport.Group._predefRigidityRef != null && predefinedType.Guid == lineSupport.Group._predefRigidityRef.Guid)
                        {
                            lineSupport.Group.PredefRigidity = predefinedType;
                        }
                    }
                }
            }
        }

        internal void GetSurfaceSupports()
        {
            foreach (Supports.SurfaceSupport surfaceSupport in this.Entities.Supports.SurfaceSupport)
            {
                // predefined rigidity
                if (this.SurfaceSupportTypes != null && this.SurfaceSupportTypes.PredefinedTypes != null)
                {
                    foreach (Releases.RigidityDataLibType1 predefinedType in this.SurfaceSupportTypes.PredefinedTypes)
                    {
                        if (surfaceSupport._predefRigidityRef != null && predefinedType.Guid == surfaceSupport._predefRigidityRef.Guid)
                        {
                            surfaceSupport.PredefRigidity = predefinedType;
                        }
                    }
                }
            }
        }

        internal void GetPointConnections()
        {
            foreach (ModellingTools.ConnectedPoints connectedPoint in this.Entities.AdvancedFem.ConnectedPoints)
            {
                // predefined rigidity
                if (this.PointConnectionTypes != null && this.PointConnectionTypes.PredefinedTypes != null)
                {
                    foreach (Releases.RigidityDataLibType2 predefinedType in this.PointConnectionTypes.PredefinedTypes)
                    {
                        if (connectedPoint._predefRigidityRef != null && predefinedType.Guid == connectedPoint._predefRigidityRef.Guid)
                        {
                            connectedPoint.PredefRigidity = predefinedType;
                        }
                    }
                }
            }
        }

        internal void GetLineConnections()
        {
            foreach (ModellingTools.ConnectedLines connectedLine in this.Entities.AdvancedFem.ConnectedLines)
            {
                // predefined rigidity
                if (this.LineConnectionTypes != null && this.LineConnectionTypes.PredefinedTypes != null)
                {
                    foreach (Releases.RigidityDataLibType3 predefinedType in this.LineConnectionTypes.PredefinedTypes)
                    {
                        if (connectedLine._predefRigidityRef != null && predefinedType.Guid == connectedLine._predefRigidityRef.Guid)
                        {
                            connectedLine.PredefRigidity = predefinedType;
                        }
                    }
                }
            }
        }

        internal void GetConstructionStages()
        {
            var loadCaseNames = this.Entities.Loads.LoadCases?.ToDictionary(lc => lc.Guid, lc => lc.Name);

            List<IStageElement> stageElements = new List<IStageElement>();
            stageElements.AddRange(this.Entities.Bars);
            stageElements.AddRange(this.Entities.Panels);
            stageElements.AddRange(this.Entities.Slabs);
            stageElements.AddRange(this.Entities.Supports.PointSupport);
            stageElements.AddRange(this.Entities.Supports.LineSupport);
            stageElements.AddRange(this.Entities.Supports.SurfaceSupport);
            stageElements.AddRange(this.Entities.AdvancedFem.Diaphragms);

            var elementsPerStage = stageElements.GroupBy(s => s.StageId).ToDictionary(g => g.Key, g => g.ToList());

            int i = 0;
            foreach (var stage in this.ConstructionStages.Stages)
            {
                i++; // Starts at 1
                stage.Id = i;

                if (elementsPerStage.ContainsKey(i))
                    stage.Elements = elementsPerStage[i];

                if (stage.ActivatedLoadCases != null)
                {
                    foreach (var activatedLoadCase in stage.ActivatedLoadCases)
                    {
                        if (activatedLoadCase.IsLoadCase)
                            activatedLoadCase.LoadCaseDisplayName = loadCaseNames[activatedLoadCase.LoadCaseGuid];
                        else if (activatedLoadCase.IsPTCLoadCase)
                            activatedLoadCase.LoadCaseDisplayName = "PTC " + activatedLoadCase.PTCLoadCase.ToString().ToUpper();
                        else if (activatedLoadCase.IsMovingLoad)
                            // TODO: Use the moving load name
                            activatedLoadCase.LoadCaseDisplayName = "<MovingLoad>";
                    }
                }
            }
        }
        internal void GetLoadCombinations()
        {
            var loadCasesMap = this.Entities.Loads.LoadCases?.ToDictionary(lc => lc.Guid);
            var stageMap = this.ConstructionStages?.Stages?.ToDictionary(s => s.Id);

            foreach (var lComb in this.Entities.Loads.LoadCombinations)
            {
                foreach (Loads.ModelLoadCase mLoadCase in lComb.ModelLoadCase)
                {
                    if (mLoadCase.IsMovingLoadLoadCase)
                        continue;

                    mLoadCase.LoadCase = loadCasesMap[mLoadCase.Guid].DeepClone();
                }

                if (lComb.StageLoadCase != null && lComb.StageLoadCase.IsFinalStage == false)
                {
                    lComb.StageLoadCase.Stage = stageMap[lComb.StageLoadCase.StageIndex].DeepClone();
                }
            }
        }
        #endregion
    }
}
