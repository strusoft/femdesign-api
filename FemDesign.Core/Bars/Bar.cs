// https://strusoft.com/
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign.Bars
{
    /// <summary>
    /// bar_type
    /// 
    /// Bar-element
    /// </summary>
    [XmlInclude(typeof(Beam))]
    [XmlInclude(typeof(Column))]
    [XmlInclude(typeof(Truss))]
    [XmlRoot("database", Namespace = "urn:strusoft")]
    [System.Serializable]
    public partial class Bar : EntityBase, IStructureElement, IStageElement
    {
        [XmlIgnore]
        private static int _barInstance = 0; // used for counter of name)
        [XmlIgnore]
        private static int _columnInstance = 0; // used for counter of name
        [XmlIgnore]
        private static int _trussInstance = 0; // used for counter of name

        /// <summary>
        /// Truss only.
        /// </summary>enum
        [XmlAttribute("maxforce")]
        public double _maxCompression; // non_neg_max_1e30
        [XmlIgnore]
        public double MaxCompression
        {
            get { return this._maxCompression; }
            set { this._maxCompression = RestrictedDouble.NonNegMax_1e30(value); }
        }

        /// <summary>
        /// Truss only.
        /// </summary>
        [XmlAttribute("compressions_plasticity")]
        public bool CompressionPlasticity { get; set; } // bool

        /// <summary>
        /// Truss only.
        /// </summary>
        [XmlAttribute("tension")]
        public double _maxTension; // non_neg_max_1e30

        [XmlIgnore]
        public double MaxTension
        {
            get { return this._maxTension; }
            set { this._maxTension = RestrictedDouble.NonNegMax_1e30(value); }
        }

        /// <summary>
        /// Truss only.
        /// </summary>
        [XmlAttribute("tensions_plasticity")]
        public bool TensionPlasticity { get; set; } // bool

        [XmlAttribute("name")]
        public string _name { get; set; } // identifier

        [XmlIgnore]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (this.Type == BarType.Beam)
                {
                    Bar._barInstance++;
                    this._name = value + "." + Bar._barInstance.ToString();

                    // update barpart identifier
                    if (this.BarPart != null)
                    {
                        this.BarPart.Name = this._name;
                    }
                }
                else if (this.Type == BarType.Column)
                {
                    Bar._columnInstance++;
                    this._name = value + "." + Bar._columnInstance.ToString();

                    // update barpart identifier
                    if (this.BarPart != null)
                    {
                        this.BarPart.Name = this._name;
                    }
                }
                else if (this.Type == BarType.Truss)
                {
                    Bar._trussInstance++;
                    this._name = value + "." + Bar._trussInstance.ToString();

                    // update barpart identifier
                    if (this.BarPart != null)
                    {
                        this.BarPart.Name = this._name;
                    }
                }
                else
                {
                    throw new System.ArgumentException($"Incorrect type of bar: {this.Type}");
                }
            }
        }

        [XmlIgnore]
        public string Instance
		{
			get
			{
                var found = this._name.IndexOf(".");
                return this._name.Substring(found + 1);
			}
		}
        public string Identifier => this.Name.Split('.')[0];

        [XmlAttribute("type")]
        public BarType _type; // beamtype

        [XmlIgnore]
        public BarType Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
            // get {return this._type;}
            // set {this._type = RestrictedString.BeamType(value);}
        }

        [XmlAttribute("stage")]
        public int StageId { get; set; } = 1;

        [XmlElement("bar_part", Order = 1)]
        public BarPart BarPart { get; set; } // bar_part_type

        [XmlElement("end", Order = 2)]
        public string End = "";

        [XmlIgnore]
        public List<Reinforcement.Ptc> Ptc = new List<Reinforcement.Ptc>();

        [XmlIgnore]
        public List<Reinforcement.BarReinforcement> Reinforcement = new List<Reinforcement.BarReinforcement>();

        [XmlIgnore]
        public List<Reinforcement.BarReinforcement> Stirrups
        {
            get
            {
                return this.Reinforcement.Where(x => x.Stirrups != null).ToList();
            }
        }
        [XmlIgnore]
        public List<Reinforcement.BarReinforcement> LongitudinalBars
        {
            get
            {
                return this.Reinforcement.Where(x => x.LongitudinalBar != null).ToList();
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal Bar()
        {

        }

        /// <summary>
        /// Construct beam or column with uniform section and uniform start/end conditions
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="section">Section, same at start/end</param>
        /// <param name="eccentricity">Analytical eccentricity, same at start. Eccentricity set to 0,0 if null/end</param>
        /// <param name="connectivity">Connectivity, same at start/end. Connectivity set to Rigid if null</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, Materials.Material material, Sections.Section section, BarType type = BarType.Beam, Eccentricity eccentricity = null, Connectivity connectivity = null, string identifier = "B")
        {
            if(type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }
            
            this.EntityCreated();
            this.Type = type;
            this.Name = identifier;

            if(eccentricity == null) { eccentricity = Eccentricity.Default; }
            if(connectivity == null) { connectivity = Connectivity.Default; }
            this.BarPart = new BarPart(edge, this.Type, material, section, eccentricity, connectivity, this.Name);
        }

        /// <summary>
        /// Construct beam or column with uniform section and uniform start/end conditions
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="section">Section, same at start/end</param>
        /// <param name="localY">Vector to orient the cross section. If null, localY will be set as a cross product between Z-Axis and the local X Axis./end</param>
        /// <param name="startEccentricity">Analytical eccentricity. Eccentricity set to 0,0 if null/end</param>
        /// <param name="endEccentricity">Analytical eccentricity. Eccentricity set to 0,0 if null/end</param>
        /// <param name="startConnectivity">Connectivity. Connectivity set to Rigid if null/end</param>
        /// <param name="endConnectivity">Connectivity. Connectivity set to Rigid if null</param>
        /// <param name="identifier">Identifier</param>
        public Bar(FemDesign.Geometry.Point3d startPoint, FemDesign.Geometry.Point3d endPoint, Materials.Material material, Sections.Section section, BarType type = BarType.Beam, Geometry.Vector3d localY = null, Eccentricity startEccentricity = null, Eccentricity endEccentricity = null, Connectivity startConnectivity = null, Connectivity endConnectivity = null, string identifier = "B")
        {
            var orientY = localY ?? (endPoint - startPoint).Cross(Geometry.Vector3d.UnitZ);
            Geometry.Edge edge = new Geometry.Edge(startPoint, endPoint, orientY);
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;
            this.Name = identifier;

            if (startEccentricity == null) { startEccentricity = Eccentricity.Default; }
            if (endEccentricity == null) { endEccentricity = Eccentricity.Default; }
            if (startConnectivity == null) { startConnectivity = Connectivity.Default; }
            if (endConnectivity == null) { endConnectivity = Connectivity.Default; }

            this.BarPart = new BarPart(edge, this.Type, material, section, startEccentricity, endEccentricity, startConnectivity, endConnectivity, this.Name);
        }



        /// <summary>
        /// Construct beam or column with uniform section and different start/end conditions
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="section">Section, same at start/end</param>
        /// <param name="startEccentricity">Analytical start eccentricity</param>
        /// <param name="endEccentricity">Analytical end eccentricity</param>
        /// <param name="startConnectivity">Start connectivity</param>
        /// <param name="endConnectivity">End connectivity</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section section, Eccentricity startEccentricity = null, Eccentricity endEccentricity = null, Connectivity startConnectivity = null, Connectivity endConnectivity = null, string identifier = "B")
        {
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            if (startEccentricity == null) { startEccentricity = Eccentricity.Default; }
            if (endEccentricity == null) { endEccentricity = Eccentricity.Default; }
            if (startConnectivity == null) { startConnectivity = Connectivity.Default; }
            if (endConnectivity == null) { endConnectivity = Connectivity.Default; }

            this.EntityCreated();
            this.Type = type;
            this.Name = identifier;


            this.BarPart = new BarPart(edge, this.Type, material, section, startEccentricity, endEccentricity, startConnectivity, endConnectivity, this.Name);
        }

        /// <summary>
        /// Construct beam or column with start/end section and different start/end conditions
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="startSection">Start section</param>
        /// <param name="endSection">End section</param>
        /// <param name="startEccentricity">Analytical start eccentricity</param>
        /// <param name="endEccentricity">Analytical end eccentricity</param>
        /// <param name="startConnectivity">Start connectivity</param>
        /// <param name="endConnectivity">End connectivity</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section startSection, Sections.Section endSection, Eccentricity startEccentricity, Eccentricity endEccentricity, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;
            this.Name = identifier;
            this.BarPart = new BarPart(edge, this.Type, material, startSection, endSection, startEccentricity, endEccentricity, startConnectivity, endConnectivity, this.Name);
        }


        /// <summary>
        /// Construct beam or column with start/end section and different start/end conditions
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="sections">List of sections, 1 (uniform) or 2 (start/end) items.</param>
        /// <param name="eccentricities">List of analytical eccentricities, 1 (uniform) or 2 (start/end) items.</param>
        /// <param name="connectivities">List of connectivities, 1 (uniform) or 2 (start/end) items.</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section[] sections, Eccentricity[] eccentricities, Connectivity[] connectivities, string identifier)
        {
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;
            this.Name = identifier;
            this.BarPart = new BarPart(edge, this.Type, material, sections, eccentricities, connectivities, this.Name);
        }

        /// <summary>
        /// Construct beam or column with non-uniform section and different start/end conditions
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="sections">List of sections, 2 or more items.</param>
        /// <param name="positions">List of parametric (0-1) section positions, 2 or more items.</param>
        /// <param name="eccentricities">List of analytical eccentricities, 2 or more items.</param>
        /// <param name="startConnectivity">Start connectivity</param>
        /// <param name="endConnectivity">End connectivity</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section[] sections, double[] positions, Eccentricity[] eccentricities, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;
            this.Name = identifier;
            this.BarPart = new BarPart(edge, this.Type, material, sections, positions, eccentricities, startConnectivity, endConnectivity, this.Name);
        }


        /// <summary>
        /// Construct a truss element.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="material"></param>
        /// <param name="section"></param>
        /// <param name="identifier"></param>
        /// <exception cref="System.Exception"></exception>
        public Bar(Geometry.Edge edge, Materials.Material material, Sections.Section section, string identifier)
        {
            this.EntityCreated();
            this.Type = BarType.Truss;
            this.Name = identifier;
            this.BarPart = new BarPart(edge, this.Type, material, section, identifier);
        }

        /// Update entities if this bar should be "reconstructed"
        public void UpdateEntities()
        {
            if (this.Type == BarType.Truss)
            {
                this.EntityCreated();
                this.BarPart.EntityCreated();
            }
            else
            {
                this.EntityCreated();
                this.BarPart.EntityCreated();
                this.BarPart.ComplexSectionObj.EntityCreated();
            }
        }

        public override string ToString()
        {
            if(this.Type == BarType.Beam || this.Type == BarType.Column)
                return $"{this.Type} Start: {this.BarPart.Edge.Points.First()}, End: {this.BarPart.Edge.Points.Last()}, Length: {this.BarPart.Edge.Length} m, Sections: ({this.BarPart.ComplexSectionObj.Sections.First()._sectionName}, {this.BarPart.ComplexSectionObj.Sections.Last()._sectionName}), Material: {this.BarPart.ComplexMaterialObj}";

            else if(this.Type == BarType.Truss)
            {
                return $"{this.Type} Start: {this.BarPart.Edge.Points.First()}, End: {this.BarPart.Edge.Points.Last()}, Length: {this.BarPart.Edge.Length} m, Section: {this.BarPart.TrussUniformSectionObj._sectionName}, Material: {this.BarPart.ComplexMaterialObj}";
            }
            else
            {
                return base.ToString();
            }
        }
    }
}