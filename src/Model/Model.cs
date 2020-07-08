// https://strusoft.com/

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace FemDesign
{
    /// <summary>
    /// Model. Represents a complete struxml model.
    /// </summary>
    [XmlRoot("database", Namespace = "urn:strusoft")]
    public class Model
    {
        [XmlIgnore]
        private bool fromStruxml { get; set; } // was model constructed from a .struxml file.
        [XmlIgnore]
        internal Calculate.Application fdApp = new Calculate.Application(); // start a new FdApp to get process information.
        /// <summary>
        /// The actual struXML version;  should be equal to the schema version the xml file is conformed to.
        /// </summary>
        [XmlAttribute("struxml_version")]
        public string struxmlVersion { get; set; } // versiontype
        /// <summary>
        /// Name of the StruSoft or 3rd party product what generated this XML file.
        /// </summary>
        [XmlAttribute("source_software")]
        public string sourceSoftware { get; set; } // string
        /// <summary>
        /// The data is partial data, so the oldest entity latest modification date and time is the
        /// value in UTC. If the current XML contains the whole database, the start_time value is
        /// "1970-01-01T00:00:00Z". The date and time always in UTC!
        /// </summary>
        [XmlAttribute("start_time")]
        public string startTime { get; set; } // dateTime
        /// <summary>
        /// The data is partial data, so the newest entity latest modification date and time is this
        /// value in UTC. This date and time always in UTC!
        /// </summary>
        [XmlAttribute("end_time")]
        public string endTime { get; set; } // dateTime
        [XmlAttribute("guid")]
        public System.Guid guid { get; set; } // guidtype
        [XmlAttribute("convertid")]
        public string convertid { get; set; } // guidtype
        /// <summary>Calculation code</summary>
        [XmlAttribute("standard")]
        public string standard { get; set; } // standardtype
        /// <summary>National annex of calculation code</summary>
        [XmlAttribute("country")]
        public string country { get; set; } // eurocodetype
        [XmlAttribute("xmlns")]
        public string xmlns { get; set; }
        [XmlElement("entities", Order = 1)]
        public Entities entities { get; set; } 
        [XmlElement("sections", Order = 2)]
        public Sections.ModelSections sections { get; set; }
        [XmlElement("materials", Order = 3)]
        public Materials.Materials materials { get; set; }
        [XmlElement("reinforcing_materials", Order = 4)]
        public Materials.ReinforcingMaterials reinforcingMaterials { get; set; }
        [XmlElement("composites", Order = 5)]
        public List<DummyXmlObject> composites {get {return null;} set {value = null;}}
        [XmlElement("point_connection_types", Order = 6)]
        public List<DummyXmlObject> pointConnectionTypes {get {return null;} set {value = null;}}
        [XmlElement("point_support_group_types", Order = 7)]
        public List<DummyXmlObject> pointSupportGroupTypes {get {return null;} set {value = null;}}
        [XmlElement("line_connection_types", Order = 8)]
        public LineConnectionTypes.LineConnectionTypes lineConnectionTypes {get; set; }

        // line_connection_types
        // line_support_group_types
        // surface_connection_types
        // surface_support_types
        // timber_panel_types
        // glc_panel_types
        // clt_panel_types
        // ptc_strand_types
        // vehicle_types
        // bolt_types
        // geometry

        [XmlElement("end", Order = 9)]
        public string end { get; set; }

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
            this.fromStruxml = false;
            this.struxmlVersion = "01.00.000";
            this.sourceSoftware = "FEM-Design 18.00.004";
            this.startTime = "1970-01-01T00:00:00.000";
            this.endTime = System.DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture);
            this.guid = System.Guid.NewGuid();
            this.convertid = "00000000-0000-0000-0000-000000000000";
            this.standard = "EC";
            this.country = country;
            this.end = "";
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
            object obj = deserializer.Deserialize(reader);
            Model fdModel = (Model)obj;
            fdModel.fromStruxml = true;
            reader.Close();
            return fdModel;
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
        internal Model AddEntities(List<Bars.Bar> bars, List<Shells.Slab> shells, List<Cover> covers, List<object> loads, List<Loads.LoadCase> loadCases, List<Loads.LoadCombination> loadCombinations, List<object> supports, List<StructureGrid.Storey> storeys, List<StructureGrid.Axis> axes) 
        {
            if (this.fromStruxml)
            {
                // check if model contains entities, sections and materials
                if (this.entities == null)
                {
                    this.entities = new Entities();
                }
                if (this.sections == null)
                {
                    this.sections = new Sections.ModelSections();
                }
                if (this.materials == null)
                {
                    this.materials = new Materials.Materials();
                }
                if (this.reinforcingMaterials == null)
                {
                    this.reinforcingMaterials = new Materials.ReinforcingMaterials();
                }

                // if model was imported from struxml: do not reset entities, sections or materials.
                // reset to false
                this.fromStruxml = false;
            }

            else
            {
                // if model was created in runtime: reset entities, sections and materials.
                this.entities = new Entities();
                this.sections = new Sections.ModelSections();
                this.materials = new Materials.Materials();
                this.reinforcingMaterials = new Materials.ReinforcingMaterials();
            }

            if (bars != null)
            {
                foreach (Bars.Bar bar in bars)
                {
                    this.AddBar(bar);
                }
            }

            if (shells != null)
            {
                foreach (Shells.Slab shell in shells)
                {
                    this.AddSlab(shell);
                }
            }

            if (covers != null)
            {
                foreach (Cover cover in covers)
                {
                    this.AddCover(cover);
                }
            }

            if (loads != null)
            {
                foreach (object load in loads)
                {
                    this.AddLoad(load);
                }
            }
            
            if (loadCases != null)
            {
                foreach (Loads.LoadCase loadCase in loadCases)
                {
                    this.AddLoadCase(loadCase);
                }
            }

            if (loadCombinations != null)
            {
                foreach (Loads.LoadCombination loadCombination in loadCombinations)
                {
                    this.AddLoadCombination(loadCombination);
                }
            }

            if (supports != null)
            {
                foreach (object support in supports)
                {
                    this.AddSupport(support);
                }
            }

            if (storeys != null)
            {
                foreach (StructureGrid.Storey storey in storeys)
                {
                    this.AddStorey(storey);
                }
            }

            if (axes != null)
            {
                foreach (StructureGrid.Axis axis in axes)
                {
                    this.AddAxis(axis);
                }
            }

            return this;
        }

        /// <summary>
        /// Add Bar to Model.
        /// </summary>
        private void AddBar(Bars.Bar obj)
        {
            if (this.BarInModel(obj))
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.guid} has already been added to model. Are you adding the same element twice?");
            }
            else
            {
                // add bar properties
                this.AddComplexSection(obj);
                this.AddMaterial(obj.material);
                this.AddSection(obj.section);

                // add bar
                this.entities.bar.Add(obj);  
            }
        }

        /// <summary>
        /// Check if Bar in Model.
        /// </summary>
        private bool BarInModel(Bars.Bar obj)
        {
            foreach (Bars.Bar elem in this.entities.bar)
            {
                if (elem.guid == obj.guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add ComplexSection (from Bar) to Model.
        /// </summary>
        private void AddComplexSection(Bars.Bar obj)
        {   
            if (obj.complexSection == null)
            {
                // pass
            }
            else
            {
                this.sections.complexSection.Add(obj.complexSection);
            }     
        }

        /// <summary>
        /// Check if ComplexSection in Model.
        /// </summary>
        private string ComplexSectionInModel(FemDesign.Sections.ComplexSection obj)
        {
            foreach (FemDesign.Sections.ComplexSection elem in this.sections.complexSection)
            {
                if (elem.section[0].guid == obj.section[0].guid &&
                    elem.section[0].ecc.Equals(obj.section[0].ecc) &&
                    elem.section[1].guid == obj.section[1].guid &&
                    elem.section[1].ecc.Equals(obj.section[1].ecc))
                {
                    return elem.guid.ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// Add Cover to Model.
        /// </summary>
        private void AddCover(Cover obj)
        {
            if (this.CoverInModel(obj))
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.guid} has already been added to model. Are you adding the same element twice?");
            }
            else
            {
                // add cover
                this.entities.advancedFem.cover.Add(obj);  
            }
        }

        /// <summary>
        /// Check if Cover in Model.
        /// </summary>
        private bool CoverInModel(Cover obj)
        {
            foreach (Cover elem in this.entities.advancedFem.cover)
            {
                if (elem.guid == obj.guid)
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
        private void AddLoad(object obj)
        {
            if (obj == null)
            {
                throw new System.ArgumentException("Passed object is null");
            }
            else if (obj.GetType() == typeof(Loads.PointLoad))
            {
                this.AddPointLoad((Loads.PointLoad)obj);
            }
            else if (obj.GetType() == typeof(Loads.LineLoad))
            {
                this.AddLineLoad((Loads.LineLoad)obj);
            }
            else if (obj.GetType() == typeof(Loads.PressureLoad))
            {
                this.AddPressureLoad((Loads.PressureLoad)obj);
            }
            else if (obj.GetType() == typeof(Loads.SurfaceLoad))
            {
                this.AddSurfaceLoad((Loads.SurfaceLoad)obj);
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
        /// Add PointLoad to Model.
        /// </summary>
        private void AddPointLoad(Loads.PointLoad obj)
        {
            if (this.PointLoadInModel(obj))
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.guid} has already been added to model. Are you adding the same element twice?");
            }
            else
            {
                this.entities.loads.pointLoad.Add(obj);
            }    
        }

        /// <summary>
        /// Check if PointLoad in Model.
        /// </summary>
        private bool PointLoadInModel(Loads.PointLoad obj)
        {
            foreach (Loads.PointLoad elem in this.entities.loads.pointLoad)
            {
                if (elem.guid == obj.guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add LineLoad to Model.
        /// </summary>
        private void AddLineLoad(Loads.LineLoad obj)
        {
            if (this.LineLoadInModel(obj))
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.guid} has already been added to model. Are you adding the same element twice?");
            }
            else
            {
                this.entities.loads.lineLoad.Add(obj);
            }   
        }

        /// <summary>
        /// Check if LineLoad in Model.
        /// </summary>
        private bool LineLoadInModel(Loads.LineLoad obj)
        {
            foreach (Loads.LineLoad elem in this.entities.loads.lineLoad)
            {
                if (elem.guid == obj.guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add PressureLoad to Model.
        /// </summary>
        private void AddPressureLoad(Loads.PressureLoad obj)
        {
            if (this.PressureLoadInModel(obj))
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.guid} has already been added to model. Are you adding the same element twice?");
            }
            else
            {
                this.entities.loads.pressureLoad.Add(obj);
            }   
        }

        /// <summary>
        /// Check if PressureLoad in Model.
        /// </summary>
        private bool PressureLoadInModel(Loads.PressureLoad obj)
        {
            foreach (Loads.PressureLoad elem in this.entities.loads.pressureLoad)
            {
                if (elem.guid == obj.guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add SurfaceLoad to Model.
        /// </summary>
        private void AddSurfaceLoad(Loads.SurfaceLoad obj)
        {
            if (this.SurfaceLoadInModel(obj))
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.guid} has already been added to model. Are you adding the same element twice?");
            }
            else
            {
                this.entities.loads.surfaceLoad.Add(obj);
            }   
        }

        /// <summary>
        /// Check if SurfaceLoad in Model.
        /// </summary>
        private bool SurfaceLoadInModel(Loads.SurfaceLoad obj)
        {
            foreach (Loads.SurfaceLoad elem in this.entities.loads.surfaceLoad)
            {
                if (elem.guid == obj.guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add MassConversionTable to Model.
        /// </summary>
        private void AddMassConversionTable(Loads.MassConversionTable obj)
        {
            this.entities.loads.loadCaseMassConversionTable = obj;  
        }

        /// <summary>
        /// Add LoadCase to Model.
        /// </summary>
        private void AddLoadCase(Loads.LoadCase obj)
        {
            if (this.LoadCaseInModel(obj))
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.guid} has already been added to model. Are you adding the same element twice?");
            }
            else
            {
                if (this.LoadCaseNameTaken(obj))
                {
                    obj.name = obj.name + " (1)";
                }
                this.entities.loads.loadCase.Add(obj);
            } 
        }

        /// <summary>
        /// Check if LoadCase in Model.
        /// </summary>
        private bool LoadCaseInModel(Loads.LoadCase obj)
        {
            foreach (Loads.LoadCase elem in this.entities.loads.loadCase)
            {
                if (elem.guid == obj.guid)
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
            foreach (Loads.LoadCase elem in this.entities.loads.loadCase)
            {
                if (elem.name == obj.name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add LoadCombination to Model.
        /// </summary>
        private void AddLoadCombination(Loads.LoadCombination obj)
        {
            if (this.LoadCombinationInModel(obj))
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.guid} has already been added to model. Are you adding the same element twice?");
            }
            else
            {
                if (this.LoadCombinationNameTaken(obj))
                {
                    obj.name = obj.name + " (1)";
                }
                this.entities.loads.loadCombination.Add(obj);
            }
        }

        /// <summary>
        /// Check if LoadCombination in Model.
        /// </summary>
        private bool LoadCombinationInModel(Loads.LoadCombination obj)
        {
            foreach (Loads.LoadCombination elem in this.entities.loads.loadCombination)
            {
                if (elem.guid == obj.guid)
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
            foreach (Loads.LoadCombination elem in this.entities.loads.loadCombination)
            {
                if (elem.name == obj.name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Slab to Model.
        /// </summary>
        private void AddSlab(Shells.Slab obj)
        {
            if (this.SlabInModel(obj))
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.guid} has already been added to model. Are you adding the same element twice?");
            }
            else
            {
                // add shell properties
                this.AddMaterial(obj.material);
                this.AddSurfaceReinforcementParameters(obj);

                // add SurfaceReinforcement
                this.AddSurfaceReinforcements(obj);
                
                // add shell
                this.entities.slab.Add(obj); 
            }
        }

        /// <summary>
        /// Check if Slab in Model.
        /// </summary>
        private bool SlabInModel(Shells.Slab obj)
        {
            foreach (Shells.Slab elem in this.entities.slab)
            {
                if (elem.guid == obj.guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Material (reinforcing) to Model.
        /// </summary>
        private void AddReinforcingMaterial(Materials.Material obj)
        {
            if (this.ReinforcingMaterialInModel(obj))
            {
                // pass - note that this should not throw an exception.
            }
            else
            {
                this.reinforcingMaterials.material.Add(obj);
            }  
        }

        /// <summary>
        /// Check if Material (reinforcring) in Model.
        /// </summary>
        private bool ReinforcingMaterialInModel(Materials.Material obj)
        {
            foreach (Materials.Material elem in this.reinforcingMaterials.material)
            {
                if (elem.guid == obj.guid)
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
        private void AddStructureGrid(object obj)
        {
            if (obj == null)
            {
                throw new System.ArgumentException("Passed object is null");
            }
            else if (obj.GetType() == typeof(StructureGrid.Axis))
            {
                this.AddAxis((StructureGrid.Axis)obj);
            }
            else if (obj.GetType() == typeof(StructureGrid.Storey))
            {
                this.AddStorey((StructureGrid.Storey)obj);
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
        private void AddAxis(StructureGrid.Axis obj)
        {
            // check if axes in entities
            if (this.entities.axes == null)
            {
                this.entities.axes = new StructureGrid.Axes();
            }

            // add axis to entities
            if (this.AxisInModel(obj))
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.guid} has already been added to model. Are you adding the same element twice?");
            }
            else
            {
                this.entities.axes.axis.Add(obj);
            }
        }

        /// <summary>
        /// Check if axis in entities.
        /// </summary>
        /// <param name="obj">Axis.</param>
        /// <returns></returns>
        private bool AxisInModel(StructureGrid.Axis obj)
        {
            foreach (StructureGrid.Axis elem in this.entities.axes.axis)
            {
                if (elem.guid == obj.guid)
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
        private void AddStorey(StructureGrid.Storey obj)
        {
            // check if storeys in entities
            if (this.entities.storeys == null)
            {
                this.entities.storeys = new StructureGrid.Storeys();
            }

            // add storey to entities
            if (this.StoreyInModel(obj))
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.guid} has already been added to model. Are you adding the same element twice?");
            }
            else
            {
                // check if geometry is consistent
                this.ConsistenStoreyGeometry(obj);

                // add to storeys
                this.entities.storeys.storey.Add(obj);
            }
        }

        /// <summary>
        /// Check if storey in entities.
        /// </summary>
        /// <param name="obj">Storey.</param>
        /// <returns></returns>
        private bool StoreyInModel(StructureGrid.Storey obj)
        {
            foreach (StructureGrid.Storey elem in this.entities.storeys.storey)
            {
                if (elem.guid == obj.guid)
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
            foreach (StructureGrid.Storey elem in this.entities.storeys.storey)
            {
                if (elem.origo.x != obj.origo.x || elem.origo.y != obj.origo.y)
                {
                    throw new System.ArgumentException($"Storey does not share XY-coordinates with storeys in model (point x: {elem.origo.x}, y: {elem.origo.y}). If model was empty make sure all storeys added to model share XY-coordinates.");
                }
                if (!elem.direction.Equals(obj.direction))
                {
                    throw new System.ArgumentException($"Storey does not share direction with storeys in model (vector i: {elem.direction.x} , j: {elem.direction.y}). If model was empty make sure all storeys added to model share direction.");
                }
            }
        }

        /// <summary>
        /// Add SurfaceReinforcement(s) from Slab to Model.
        /// </summary>
        /// <param name="obj"></param>
        private void AddSurfaceReinforcements(Shells.Slab obj)
        {
            foreach (Reinforcement.SurfaceReinforcement surfaceReinforcement in obj.surfaceReinforcement)
            {
                this.AddReinforcingMaterial(surfaceReinforcement.wire.reinforcingMaterial);
                this.AddSurfaceReinforcement(surfaceReinforcement);
            }
        }

        /// <summary>
        /// Add SurfaceReinforcement to Model.
        /// </summary>
        private void AddSurfaceReinforcement(Reinforcement.SurfaceReinforcement obj)
        {
            if (this.SurfaceReinforcementInModel(obj))
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.guid} has already been added to model. Did you add the same {obj.GetType().FullName} to different Slabs?");
            }
            else
            {
                this.entities.surfaceReinforcement.Add(obj);
            }  
        }

        /// <summary>
        /// Check if SurfaceReinforcement in Model.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool SurfaceReinforcementInModel(Reinforcement.SurfaceReinforcement obj)
        {
            foreach (Reinforcement.SurfaceReinforcement elem in this.entities.surfaceReinforcement)
            {
                if (elem.guid == obj.guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add SurfaceReinforcementParameters to Model.
        /// </summary>
        private void AddSurfaceReinforcementParameters(Shells.Slab obj)
        {
            if (obj.surfaceReinforcementParameters != null)
            {
                if (this.SurfaceReinforcementParametersInModel(obj.surfaceReinforcementParameters))
                {
                    throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.guid} has already been added to model. Are you adding the same element twice?");
                }
                else
                {
                    this.entities.surfaceReinforcementParameters.Add(obj.surfaceReinforcementParameters);
                }
            } 
        }

        /// <summary>
        /// Check if SurfaceReinforcementParameters in Model.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool SurfaceReinforcementParametersInModel(Reinforcement.SurfaceReinforcementParameters obj)
        {
            foreach (Reinforcement.SurfaceReinforcementParameters elem in this.entities.surfaceReinforcementParameters)
            {
                if (elem.guid == obj.guid)
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
        private void AddSupport(object obj)
        {
            if (obj == null)
            {
                throw new System.ArgumentException("Passed object is null");
            }
            else if (obj.GetType() == typeof(Supports.PointSupport))
            {
                this.AddPointSupport((Supports.PointSupport)obj);
            }
            else if (obj.GetType() == typeof(Supports.LineSupport))
            {
                this.AddLineSupport((Supports.LineSupport)obj);
            }
            else
            {
                throw new System.ArgumentException("Passed object must be PointSupport or LineSupport");
            }
        }

        /// <summary>
        /// Add PointSupport to Model.
        /// </summary>
        private void AddPointSupport(Supports.PointSupport obj)
        {
            if (this.PointSupportInModel(obj))
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.guid} has already been added to model. Are you adding the same element twice?");
            }
            else
            {
                this.entities.supports.pointSupport.Add(obj);
            }
        }

        /// <summary>
        /// Check if PointSupport in Model.
        /// </summary>
        private bool PointSupportInModel(Supports.PointSupport obj)
        {
            foreach (Supports.PointSupport elem in this.entities.supports.pointSupport)
            {
                if (elem.guid == obj.guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add LineSupport to Model.
        /// </summary>
        private void AddLineSupport(Supports.LineSupport obj)
        {
            if (this.LineSupportInModel(obj))
            {
                throw new System.ArgumentException($"{obj.GetType().FullName} with guid: {obj.guid} has already been added to model. Are you adding the same element twice?");
            }
            else
            {
                this.entities.supports.lineSupport.Add(obj);
            }
        }

        /// <summary>
        /// Check if LineSupport in Model.
        /// </summary>
        private bool LineSupportInModel(Supports.LineSupport obj)
        {
            foreach (Supports.LineSupport elem in this.entities.supports.lineSupport)
            {
                if (elem.guid == obj.guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Material to Model.
        /// </summary>
        private void AddMaterial(Materials.Material obj)
        {
            if (MaterialInModel(obj))
            {
                // pass - note that this should not throw an exception.
            }
            else
            {
                this.materials.material.Add(obj);
            }
        }

        /// <summary>
        /// Check if Material in Model.
        /// </summary>
        private bool MaterialInModel(Materials.Material obj)
        {
            foreach (Materials.Material elem in this.materials.material)
            {
                if (elem.guid == obj.guid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Add Section to Model.
        /// </summary>
        private void AddSection(FemDesign.Sections.Section obj)
        {
            if (SectionInModel(obj))
            {
                // pass - note that this should not throw an exception.
            }
            else
            {
                this.sections.section.Add(obj);
            }
        }

        /// <summary>
        /// Check if Section in Model.
        /// </summary>
        private bool SectionInModel(FemDesign.Sections.Section obj)
        {
            foreach (FemDesign.Sections.Section elem in this.sections.section)
            {
                if (elem.guid == obj.guid)
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
        internal List<Bars.Bar> GetBars()
        {
            List<Bars.Bar> _bars = new List<Bars.Bar>();
            foreach (Bars.Bar item in this.entities.bar)
            {
                // get complex section
                if (item.type != "truss")
                {
                    foreach (FemDesign.Sections.ComplexSection _complexSection in this.sections.complexSection)
                    {
                        if (_complexSection.guid == item.barPart.complexSection)
                        {
                            item.complexSection = _complexSection;
                        }
                    }

                    // check if complex section found
                    if (item.complexSection == null)
                    {
                        throw new System.ArgumentException("No matching complex section found. Model.GetBars() failed.");
                    }
                }


                // get material
                foreach (Materials.Material _material in this.materials.material)
                {
                    if (_material.guid == item.barPart.complexMaterial)
                    {
                        item.material = _material;
                    }
                }

                // check if material found
                if (item.material == null)
                {
                    throw new System.ArgumentException("No matching material found. Model.GetBars() failed.");
                }

                // get section
                foreach (FemDesign.Sections.Section _section in this.sections.section)
                {
                    if (item.complexSection == null)
                    {
                        if (_section.guid == item.barPart.complexSection)
                        {
                            item.section = _section;
                        }
                    }
                    else
                    {
                        if (_section.guid == item.complexSection.section[0].guid)
                        {
                            item.section = _section;
                        }
                    }  
                }
                
                // check if section found
                if (item.section == null)
                {
                    throw new System.ArgumentException("No matching section found. Model.GetBars() failed");
                }

                // add to return object
                _bars.Add(item);
            }
            return _bars;
        }

        /// <summary>
        /// Get Slabs from Model.
        /// Slabs will be reconstruted from Model incorporating all references: Material, Predefined EdgeConnections, SurfaceReinforcementParameters, SurfaceReinforcement.
        /// </summary>
        /// <returns></returns>
        internal List<Shells.Slab> GetSlabs()
        {
            List<Shells.Slab> _slabs = new List<Shells.Slab>();
            foreach (Shells.Slab item in this.entities.slab)
            {
                // get material
                foreach (Materials.Material _material in this.materials.material)
                {
                    if (_material.guid == item.slabPart.complexMaterial)
                    {
                        item.material = _material;
                    }
                }

                // get line_connection_type     
                if (this.lineConnectionTypes != null)
                {         
                    foreach (Geometry.Contour _contour in item.slabPart.region.contours)
                    {
                        foreach (Geometry.Edge _edge in _contour.edges)
                        {
                            if (_edge.edgeConnection != null)
                            {
                                if (_edge.edgeConnection.predefinedRigidity != null)
                                {
                                    foreach (LineConnectionTypes.PredefinedType _predefinedType in this.lineConnectionTypes.predefinedType)
                                    {
                                        // replace referenceType with a rigidity
                                        if (_edge.edgeConnection.predefinedRigidity.guid == _predefinedType.guid)
                                        {
                                            _edge.edgeConnection.rigidity = _predefinedType.rigidity;
                                            // remove referenceType
                                            _edge.edgeConnection.predefinedRigidity = null;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // get surface reinforcement parameters
                foreach (Reinforcement.SurfaceReinforcementParameters _surfaceReinforcementParameter in this.entities.surfaceReinforcementParameters)
                {
                    if (_surfaceReinforcementParameter.baseShell.guid == item.slabPart.guid)
                    {
                        item.surfaceReinforcementParameters = _surfaceReinforcementParameter;
                    }
                }

                // get surface reinforcement
                foreach (Reinforcement.SurfaceReinforcement _surfaceReinforcement in this.entities.surfaceReinforcement)
                {
                    if (_surfaceReinforcement.baseShell.guid == item.slabPart.guid)
                    {

                        // get wire material
                        foreach (Materials.Material _material in this.reinforcingMaterials.material)
                        {
                            if (_surfaceReinforcement.wire.reinforcingMaterialGuid == _material.guid)
                            {
                                _surfaceReinforcement.wire.reinforcingMaterial = _material;
                            }
                        }

                        // add surface reinforcement to slab
                        item.surfaceReinforcement.Add(_surfaceReinforcement);
                    }
                }

                // check if material found
                if (item.material == null)
                {
                    throw new System.ArgumentException("No matching material found. Model.GetBars() failed.");
                }

                // add to return object
                _slabs.Add(item);
            }
            return _slabs;
        }
        #endregion
    }
}