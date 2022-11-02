// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    public partial class Bar : EntityBase, INamedEntity, IStructureElement, IStageElement
    {
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



        [XmlAttribute("type")]
        public BarType Type { get; set; }

        [XmlAttribute("stage")]
        public int StageId { get; set; } = 1;

        [XmlElement("bar_part", Order = 1)]
        public BarPart BarPart { get; set; } // bar_part_type

        [XmlElement("truss_behaviour", Order = 2)]
        public StruSoft.Interop.StruXml.Data.Truss_chr_type TrussBehaviour { get; set; }

        [XmlElement("end", Order = 3)]
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

        public string Name => this.BarPart.Name.Substring(0, this.BarPart.Name.Length - 2); // Remove trailing ".1" from barpart name
        public int Instance => this.BarPart.Instance;

        [XmlIgnore]
        public string Identifier
        {
            get => this.BarPart.Identifier;
            set => this.BarPart.Identifier = value;
        }
        [XmlIgnore]
        public bool LockedIdentifier
        {
            get => this.BarPart.LockedIdentifier;
            set => this.BarPart.LockedIdentifier = value;
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        protected Bar()
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
        public Bar(Geometry.Edge edge, Materials.Material material, Sections.Section section, BarType type, Eccentricity eccentricity = null, Connectivity connectivity = null, string identifier = "B")
        {
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;
            //this.Identifier = identifier;

            if (eccentricity == null) { eccentricity = Eccentricity.Default; }
            if (connectivity == null) { connectivity = Connectivity.Default; }
            this.BarPart = new BarPart(edge, this.Type, material, section, eccentricity, connectivity, identifier);
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
        public Bar(FemDesign.Geometry.Point3d startPoint, FemDesign.Geometry.Point3d endPoint, Materials.Material material, Sections.Section section, BarType type, Geometry.Vector3d localY = null, Eccentricity startEccentricity = null, Eccentricity endEccentricity = null, Connectivity startConnectivity = null, Connectivity endConnectivity = null, string identifier = "B")
        {
            var orientY = localY ?? (endPoint - startPoint).Cross(Geometry.Vector3d.UnitZ);
            Geometry.Edge edge = new Geometry.Edge(startPoint, endPoint, orientY);
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;
            //this.Identifier = identifier;

            if (startEccentricity == null) { startEccentricity = Eccentricity.Default; }
            if (endEccentricity == null) { endEccentricity = Eccentricity.Default; }
            if (startConnectivity == null) { startConnectivity = Connectivity.Default; }
            if (endConnectivity == null) { endConnectivity = Connectivity.Default; }

            this.BarPart = new BarPart(edge, this.Type, material, section, startEccentricity, endEccentricity, startConnectivity, endConnectivity, identifier);
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
            //this.Identifier = identifier;


            this.BarPart = new BarPart(edge, this.Type, material, section, startEccentricity, endEccentricity, startConnectivity, endConnectivity, identifier);
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
            //this.Identifier = identifier;
            this.BarPart = new BarPart(edge, this.Type, material, startSection, endSection, startEccentricity, endEccentricity, startConnectivity, endConnectivity, identifier);
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
            //this.Identifier = identifier;
            this.BarPart = new BarPart(edge, this.Type, material, sections, eccentricities, connectivities, identifier);
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
            //this.Identifier = identifier;
            this.BarPart = new BarPart(edge, this.Type, material, sections, positions, eccentricities, startConnectivity, endConnectivity, identifier);
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
            //this.Identifier = identifier;
            this.BarPart = new BarPart(edge, this.Type, material, section, identifier);
        }



        /// <summary>
        /// Construct a truss element.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="material"></param>
        /// <param name="section"></param>
        /// <param name="identifier"></param>
        /// <exception cref="System.Exception"></exception>
        public static Bar Truss(Geometry.Edge edge, Materials.Material material, Sections.Section section, string identifier)
        {
            var truss = new Bar();

            truss.EntityCreated();
            truss.Type = BarType.Truss;
            //truss.Identifier = identifier;
            truss.BarPart = new BarPart(edge, truss.Type, material, section, identifier);
            return truss;
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
            if (this.Type == BarType.Beam || this.Type == BarType.Column)
                return $"{this.Type} Start: {this.BarPart.Edge.Points.First()}, End: {this.BarPart.Edge.Points.Last()}, Length: {this.BarPart.Edge.Length} m, Sections: ({this.BarPart.ComplexSectionObj.Sections.First()._sectionName}, {this.BarPart.ComplexSectionObj.Sections.Last()._sectionName}), Material: {this.BarPart.ComplexMaterialObj}";

            else if (this.Type == BarType.Truss)
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