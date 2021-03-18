// https://strusoft.com/

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
    /// <summary>
    /// Model. Represents a complete struxml model.
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    [XmlRoot("database", Namespace = "urn:strusoft")]
    public class Model
    {
        [XmlIgnore]
        internal Calculate.Application FdApp = new Calculate.Application(); // start a new FdApp to get process information.
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
        [IsVisibleInDynamoLibrary(true)]
        [XmlAttribute("standard")]
        public string Standard { get; set; } // standardtype
        /// <summary>National annex of calculation code</summary>
        [IsVisibleInDynamoLibrary(true)]
        [XmlAttribute("country")]
        public string Country { get; set; } // eurocodetype
        [XmlAttribute("xmlns")]
        public string Xmlns { get; set; }
        [XmlElement("entities", Order = 1)]
        public Entities Entities { get; set; } 
        [XmlElement("sections", Order = 2)]
        public Sections.ModelSections Sections { get; set; }
        [XmlElement("materials", Order = 3)]
        public Materials.Materials Materials { get; set; }
        [XmlElement("reinforcing_materials", Order = 4)]
        public Materials.ReinforcingMaterials ReinforcingMaterials { get; set; }
        [XmlElement("composites", Order = 5)]
        public List<DummyXmlObject> Composites {get {return null;} set {value = null;}}
        [XmlElement("point_connection_types", Order = 6)]
        public LibraryItems.PointConnectionTypes PointConnectionTypes { get; set;}
        [XmlElement("point_support_group_types", Order = 7)]
        public LibraryItems.PointSupportGroupTypes PointSupportGroupTypes { get; set;}
        [XmlElement("line_connection_types", Order = 8)]
        public LibraryItems.LineConnectionTypes LineConnectionTypes {get; set; }
        [XmlElement("line_support_group_types", Order = 9)]
        public LibraryItems.LineSupportGroupTypes LineSupportGroupTypes { get; set;}
        [XmlElement("surface_connection_types", Order = 10)]
        public LibraryItems.SurfaceConnectionTypes SurfaceConnectionTypes { get; set;}
        [XmlElement("surface_support_types", Order = 11)]
        public LibraryItems.SurfaceSupportTypes SurfaceSupportTypes { get; set;}
        [XmlElement("timber_panel_types", Order = 12)]
        public Materials.TimberPanelTypes TimberPanelTypes { get; set; }
        [XmlElement("glc_panel_types", Order = 13)]
        public Materials.GlcPanelTypes GlcPanelTypes { get; set; }
        [XmlElement("clt_panel_types", Order = 14)]
        public Materials.CltPanelTypes CltPanelTypes { get; set; }
        [XmlElement("ptc_strand_types", Order = 15)]
        public Reinforcement.PtcStrandType PtcStrandTypes { get; set; }
        // vehicle_types
        // bolt_types
        // geometry

        [XmlElement("end", Order = 16)]
        public string End { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Model()
        {

        }

        /// <summary>
        /// Internal constructor used by GH components and Dynamo nodes to initialize a model.
        /// </summary>
        internal Model(string country)
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
        }

        #region serialization
        /// <summary>
        /// Deserialize model from file (.struxml).
        /// </summary>
        internal static Model DeserializeFromFilePath(string filePath)
        {
            // check file extension
            if (Path.GetExtension(filePath) != ".struxml")
            {
                throw new System.ArgumentException("File extension must be .struxml! Model.DeserializeModel failed.");
            }

            //
            XmlSerializer deserializer = new XmlSerializer(typeof(Model));
            TextReader reader = new StreamReader(filePath);

            // catch inner exception
            object obj;
            try
            {    
                obj = deserializer.Deserialize(reader);
            }
            catch (System.InvalidOperationException ex)
            {
                throw ex.InnerException.InnerException;
            }

            // close reader
            reader.Close();

            // cast type
            Model model = (Model)obj;

            // prepare elements with library references
            model.GetBars();
            model.GetFictitiousShells();
            model.GetLineSupports();
            model.GetPanels();
            model.GetPointSupports();
            model.GetSlabs();
            model.GetSurfaceSupports();

            // return
            return model;
            
        }

        /// <summary>
        /// Serialize Model to file (.struxml).
        /// </summary>
        /// <param name="filePath"></param>
        internal void SerializeModel(string filePath)
        {
            // check file extension
            if (Path.GetExtension(filePath) != ".struxml")
            {
                throw new System.ArgumentException("File extension must be .struxml! Model.SerializeModel failed.");
            }

            // serialize
            XmlSerializer serializer = new XmlSerializer(typeof(Model));
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, this);
            }
        }
        #endregion

        #region addEntities
        /// <summary>
        /// Add entities to Model. Internal method used by GH components and Dynamo nodes.
        /// </summary>
        internal Model AddEntities(List<Bars.Bar> bars, List<ModellingTools.FictitiousBar> fictitiousBars, List<ModellingTools.ConnectedLines> connectedLines, List<Shells.Slab> shells, List<ModellingTools.FictitiousShell> fictitiousShells, List<Shells.Panel> panels ,List<Cover> covers, List<object> loads, List<Loads.LoadCase> loadCases, List<Loads.LoadCombination> loadCombinations, List<object> supports, List<StructureGrid.Storey> storeys, List<StructureGrid.Axis> axes, bool overwrite) 
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

            if (connectedLines != null) 
            {
                foreach(ModellingTools.ConnectedLines connLine in connectedLines) 
                {
                    this.AddConnectedLine(connLine, overwrite);
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

            if (loadCombinations != null)
            {
                foreach (Loads.LoadCombination loadCombination in loadCombinations)
                {
                    this.AddLoadCombination(loadCombination, overwrite);
                }
            }

            if (supports != null)
            {
                foreach (object support in supports)
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

            // add material
            this.AddMaterial(obj.BarPart.Material, overwrite);

            // add sections
            this.AddComplexSection(obj, overwrite);
            this.AddSection(obj.BarPart.StartSection, overwrite);
            this.AddSection(obj.BarPart.EndSection, overwrite);

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
        private void AddComplexSection(Bars.Bar bar, bool overwrite)
        {   
            if (bar.BarPart.ComplexSectionIsNull)
            {
                // pass
            }
            else
            {
                // obj
                Sections.ComplexSection obj = bar.BarPart.ComplexSection;

                // in model?
                bool inModel = this.ComplexSectionInModel(obj);

                // in model, don't overwrite
                if (inModel && overwrite == false)
                {
                    throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
                }

                // in model, overwrite
                else if (inModel && overwrite == true)
                {
                    this.Sections.ComplexSection.RemoveAll(x => x.Guid == obj.Guid);
                }

                // add complex section
                this.Sections.ComplexSection.Add(obj);
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
            if ( this.Entities.AdvancedFem.ConnectedLines == null)
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
        }


        /// <summary>
        /// Add Load to Model.
        /// </summary>
        /// <param name="obj">PointLoad, LineLoad, PressureLoad, SurfaceLoad</param>
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

            // add timber application data
            if (obj.TimberApplicationData != null)
            {
                // add library types
                if (obj.TimberPanelLibraryData != null && obj.TimberPanelLibraryData.Guid == obj.TimberApplicationData.PanelType)
                {
                    this.AddTimberPanelLibraryType(obj.TimberPanelLibraryData, overwrite); 
                }
                else if (obj.CltPanelLibraryData != null && obj.CltPanelLibraryData.Guid == obj.TimberApplicationData.PanelType)
                {
                    this.AddCltPanelLibraryType(obj.CltPanelLibraryData, overwrite);
                }
                else if (obj.GlcPanelLibraryData != null && obj.GlcPanelLibraryData.Guid == obj.TimberApplicationData.PanelType)
                {
                    this.AddGlcPanelLibraryType(obj.GlcPanelLibraryData, overwrite);
                }
                else
                {
                    throw new System.ArgumentException($"Could not find the related lirbary data with guid: {obj.TimberApplicationData.PanelType}. Failed to add panel library data.");
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
            else if(inModel && overwrite)
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
        /// Add LoadCase to Model.
        /// </summary>
        private void AddLoadCase(Loads.LoadCase obj, bool overwrite)
        {
            // in model?
            bool inModel = this.LoadCaseInModel(obj);

            // in model, don't overwrite?
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.LoadCases.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add load case
            if (this.LoadCaseNameTaken(obj))
            {
                obj.Name = obj.Name + " (1)";
            }
            this.Entities.Loads.LoadCases.Add(obj);
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

        /// <summary>
        /// Add LoadCombination to Model.
        /// </summary>
        private void AddLoadCombination(Loads.LoadCombination obj, bool overwrite)
        {
            // in model?
            bool inModel = this.LoadCombinationInModel(obj);

            // in model, don't overwrite
            if (inModel && !overwrite)
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.Guid} has already been added to model. Are you adding the same element twice?");
            }

            // in model, overwrite
            else if (inModel && overwrite)
            {
                this.Entities.Loads.LoadCombinations.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add load combination
            if (this.LoadCombinationNameTaken(obj))
            {
                obj.Name = obj.Name + " (1)";
            }
            this.Entities.Loads.LoadCombinations.Add(obj);
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
            foreach(Releases.RigidityDataLibType3 predef in obj.SlabPart.Region.GetPredefinedRigidities())
            {   
                this.AddPredefinedRigidity(predef, overwrite);
            }
            
            // add shell
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
        /// Add SurfaceReinforcement(s) from Slab to Model.
        /// </summary>
        /// <param name="obj"></param>
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
                this.Entities.SurfaceReinforcement.RemoveAll(x => x.Guid == obj.Guid);
            }

            // add obj
            this.Entities.SurfaceReinforcement.Add(obj);
  
        }

        /// <summary>
        /// Check if SurfaceReinforcement in Model.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool SurfaceReinforcementInModel(Reinforcement.SurfaceReinforcement obj)
        {
            foreach (Reinforcement.SurfaceReinforcement elem in this.Entities.SurfaceReinforcement)
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
        /// <param name="obj">PointSupport, LineSupport</param>
        private void AddSupport(object obj, bool overwrite)
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
                throw new System.ArgumentException("Passed object must be PointSupport or LineSupport");
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
            if (obj.Group.PredefRigidity != null)
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
        private void AddTimberPanelLibraryType(Materials.TimberPanelLibraryType obj, bool overwrite)
        {
            // if null create new element
            if (this.TimberPanelTypes == null)
            {
                this.TimberPanelTypes = new Materials.TimberPanelTypes();
                this.TimberPanelTypes.TimberPanelLibraryTypes = new List<Materials.TimberPanelLibraryType>();
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
                this.TimberPanelTypes.TimberPanelLibraryTypes.RemoveAll(x => x.Guid == obj.Guid);
                this.TimberPanelTypes.TimberPanelLibraryTypes.Add(obj);
            }

            // not in model
            else if (!inModel)
            {
                this.TimberPanelTypes.TimberPanelLibraryTypes.Add(obj);
            }
        }

        /// <summary>
        /// Check if Timber panel library type in Model.
        /// </summary>
        private bool TimberPanelLibraryTypeInModel(Materials.TimberPanelLibraryType obj)
        {
            foreach (Materials.TimberPanelLibraryType elem in this.TimberPanelTypes.TimberPanelLibraryTypes)
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
                if (elem.Guid == obj.Guid)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
        
        #region deconstruct
        /// <summary>
        /// Get Bars from Model. 
        /// Bars will be reconstructed from Model incorporating all references: ComplexSection, Section, Material.
        /// </summary>
        internal void GetBars()
        {
            foreach (Bars.Bar item in this.Entities.Bars)
            {
                // set type on barPart
                item.BarPart.Type = item.Type;

                // get complex section
                if (item.Type != Bars.BarType.Truss)
                {
                    foreach (FemDesign.Sections.ComplexSection complexSection in this.Sections.ComplexSection)
                    {
                        if (complexSection.Guid == item.BarPart.ComplexSectionRef)
                        {
                            item.BarPart.ComplexSection = complexSection;
                        }
                    }

                    // check if complex section found
                    if (item.BarPart.ComplexSectionIsNull)
                    {
                        throw new System.ArgumentException("No matching complex section found. Model.GetBars() failed.");
                    }
                }


                // get material
                foreach (Materials.Material material in this.Materials.Material)
                {
                    if (material.Guid == item.BarPart.ComplexMaterialRef)
                    {
                        item.BarPart.Material = material;
                    }
                }

                // check if material found
                if (item.BarPart.Material == null)
                {
                    throw new System.ArgumentException("No matching material found. Model.GetBars() failed.");
                }

                // get section
                foreach (Sections.Section section in this.Sections.Section)
                {
                    if (item.BarPart.Type == Bars.BarType.Truss)
                    {
                        if (section.Guid == item.BarPart.ComplexSectionRef)
                        {
                            item.BarPart.StartSection = section;
                            item.BarPart.EndSection = section;
                        }
                    }
                    else
                    {
                        if (section.Guid == item.BarPart.ComplexSection.Section[0].SectionRef)
                        {
                            item.BarPart.StartSection = section;
                        }
                        
                        if (section.Guid == item.BarPart.ComplexSection.Section.Last().SectionRef)
                        {
                            item.BarPart.EndSection = section;
                        }
                    }  
                }
                
                // check if section found
                if (item.BarPart.StartSection == null || item.BarPart.EndSection == null)
                {
                    throw new System.ArgumentException("No matching section found. Model.GetBars() failed");
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
                    if (_material.Guid == item.SlabPart.ComplexMaterial)
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
                foreach (Reinforcement.SurfaceReinforcement surfaceReinforcement in this.Entities.SurfaceReinforcement)
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
                    throw new System.ArgumentException("No matching material found. Model.GetBars() failed.");
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
                if (panel.TimberApplicationData != null)
                {
                    // timber panel types
                    if (this.TimberPanelTypes != null && this.TimberPanelTypes.TimberPanelLibraryTypes != null)
                    {
                        foreach (FemDesign.Materials.TimberPanelLibraryType libItem in this.TimberPanelTypes.TimberPanelLibraryTypes)
                        {
                            if (libItem.Guid == panel.TimberApplicationData.PanelType)
                            {
                                panel.TimberPanelLibraryData = libItem;
                            }
                        }
                    }
                    

                    // clt panel types
                    if (this.CltPanelTypes != null && this.CltPanelTypes.CltPanelLibraryTypes != null)
                    {
                        foreach (FemDesign.Materials.CltPanelLibraryType libItem in this.CltPanelTypes.CltPanelLibraryTypes)
                        {
                            if (libItem.Guid == panel.TimberApplicationData.PanelType)
                            {
                                panel.CltPanelLibraryData = libItem;
                            }
                        }
                    }
                    
                    
                    // glc panel types
                    if (this.GlcPanelTypes != null && this.GlcPanelTypes.GlcPanelLibraryTypes != null)
                    {
                        foreach (FemDesign.Materials.GlcPanelLibraryType libItem in this.GlcPanelTypes.GlcPanelLibraryTypes)
                        {
                            if (libItem.Guid == panel.TimberApplicationData.PanelType)
                            {
                                panel.GlcPanelLibraryData = libItem;
                            }
                        }
                    }
                    

                    // check if libItem found
                    if (panel.TimberPanelLibraryData == null && panel.CltPanelLibraryData == null && panel.GlcPanelLibraryData == null)
                    {
                        throw new System.ArgumentException("A timber/clt/glc library item was expected but not found. Can't construct Panel. Model.GetPanels() failed.");
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

        #endregion
        
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
        public static Model AddElements(Model fdModel, [DefaultArgument("[]")] List<Bars.Bar> bars, [DefaultArgument("[]")] List<ModellingTools.FictitiousBar> fictitiousBars, [DefaultArgument("[]")] List<ModellingTools.ConnectedLines> connectedLines, [DefaultArgument("[]")] List<Shells.Slab> shells, [DefaultArgument("[]")] List<ModellingTools.FictitiousShell> fictitiousShells, [DefaultArgument("[]")] List<Shells.Panel> panels, [DefaultArgument("[]")] List<Cover> covers, [DefaultArgument("[]")] List<object> loads, [DefaultArgument("[]")] List<Loads.LoadCase> loadCases, [DefaultArgument("[]")] List<Loads.LoadCombination> loadCombinations, [DefaultArgument("[]")] List<object> supports, [DefaultArgument("[]")] List<StructureGrid.Storey> storeys, [DefaultArgument("[]")] List<StructureGrid.Axis> axes, bool overwrite = false)
        {
            // deep clone model
            Model model = fdModel.DeepClone();

            // add entities
            model.AddEntities(bars, fictitiousBars, connectedLines, shells, fictitiousShells, panels, covers, loads, loadCases, loadCombinations, supports, storeys, axes, overwrite);
            return model;
        }

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
        public static Model CreateNewModel([DefaultArgument("S")] string countryCode, [DefaultArgument("[]")] List<Bars.Bar> bars, [DefaultArgument("[]")] List<ModellingTools.FictitiousBar> fictitiousBars, [DefaultArgument("[]")] List<ModellingTools.ConnectedLines> connectedLines, [DefaultArgument("[]")] List<Shells.Slab> shells, [DefaultArgument("[]")] List<ModellingTools.FictitiousShell> fictitiousShells, [DefaultArgument("[]")] List<Shells.Panel> panels, [DefaultArgument("[]")] List<Cover> covers, [DefaultArgument("[]")] List<object> loads, [DefaultArgument("[]")] List<Loads.LoadCase> loadCases, [DefaultArgument("[]")] List<Loads.LoadCombination> loadCombinations, [DefaultArgument("[]")] List<object> supports, [DefaultArgument("[]")] List<StructureGrid.Storey> storeys, [DefaultArgument("[]")] List<StructureGrid.Axis> axes)
        {
            //
            if (countryCode == null)
            {
                countryCode = "S";
            }

            // create model
            Model _model = new Model(countryCode);
            _model.AddEntities(bars, fictitiousBars, connectedLines, shells, fictitiousShells, panels, covers, loads, loadCases, loadCombinations, supports, storeys, axes, false);
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
        public static Dictionary<string, object> ReadStr(string strPath,[DefaultArgument("[]")] List<string> bscPath)
        {
            Calculate.FdScript fdScript = Calculate.FdScript.ReadStr(strPath, bscPath);
            Calculate.Application fdApp = new Calculate.Application();
            bool hasExited =  fdApp.RunFdScript(fdScript, false, true);
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