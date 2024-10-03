// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using StruSoft.Interop.StruXml.Data;

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
    public partial class Bar : NamedEntityBase, INamedEntity, IStructureElement, IStageElement, IBar
    {
        [XmlIgnore]
        private static int _barInstance = 0; // Number of bar/beam instances created
        [XmlIgnore]
        private static int _columnInstance = 0; // Number of column instances created
        [XmlIgnore]
        private static int _trussInstance = 0; // Number of truss instances created
        protected override int GetUniqueInstanceCount() // This method body must be the same as the method of the BarPart class
        {
            switch (this.Type)
            {
                case BarType.Beam:
                    return ++_barInstance;
                case BarType.Column:
                    return ++_columnInstance;
                case BarType.Truss:
                    return ++_trussInstance;
                default:
                    throw new System.ArgumentException($"Incorrect type of bar: {this.Type}");
            }
        }

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

        [XmlIgnore]
        public Geometry.Edge Edge
        {
            get
            {
                return this.BarPart.Edge;
            }
        }

        [XmlAttribute("type")]
        public BarType Type { get; set; }

        [XmlAttribute("stage")]
        public int StageId { get; set; } = 1;

        [XmlElement("bar_part", Order = 1)]
        public BarPart _barPart; // bar_part_type

        [XmlIgnore]
        public BarPart BarPart
        { 
            get
            {
                this._barPart._name = this._name + ".1";    // BarPart name must be updated with the Bar's name.
                return this._barPart;
            }
            set
            {
                this._barPart = value;
                this._name = this._barPart._name.Remove(this._barPart._name.Length - 2);    // The BarPart class is public, so the user can create a new BarPart obj. and assign it to this property. Bar name must be updated with the BarPart's name.
            }
        }

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


        public bool IsSteel()
        {
            return this.BarPart.ComplexMaterialObj.Family == Materials.Family.Steel;

        }

        public bool IsConcrete()
        {
            return this.BarPart.ComplexMaterialObj.Family == Materials.Family.Concrete;

        }

        public bool IsTimber()
        {
            return this.BarPart.ComplexMaterialObj.Family == Materials.Family.Timber;
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
        /// <param name="section">Section, same at start/end.</param>
        /// <param name="eccentricity">Analytical eccentricity, same at start and end. If null, eccentricity set to 0.</param>
        /// <param name="connectivity">Connectivity, same at start/end. Connectivity set to Rigid if null.</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, Materials.Material material, Sections.Section section, BarType type, Eccentricity eccentricity = null, Connectivity connectivity = null, string identifier = "B")
        {
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;
            this.Identifier = identifier;

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
        /// <param name="section">Section, same at start/end.</param>
        /// <param name="localY">Vector to orient the cross section. If null, localY will be set as a cross product between Z-Axis and the local X Axis./end</param>
        /// <param name="startEccentricity">Analytical eccentricity at start. If null, eccentricity set to 0.</param>
        /// <param name="endEccentricity">Analytical eccentricity at end. If null, eccentricity set to 0.</param>
        /// <param name="startConnectivity">Connectivity. Connectivity set to Rigid if null.</param>
        /// <param name="endConnectivity">Connectivity. Connectivity set to Rigid if null.</param>
        /// <param name="identifier">Identifier</param>
        public Bar(FemDesign.Geometry.Point3d startPoint, FemDesign.Geometry.Point3d endPoint, Materials.Material material, Sections.Section section, BarType type, Geometry.Vector3d localY = null, Eccentricity startEccentricity = null, Eccentricity endEccentricity = null, Connectivity startConnectivity = null, Connectivity endConnectivity = null, string identifier = "B")
        {
            var orientY = localY ?? (endPoint - startPoint).Cross(Geometry.Vector3d.UnitZ);
            Geometry.Edge edge = new Geometry.Edge(startPoint, endPoint, orientY);
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;
            this.Identifier = identifier;

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
        /// <param name="section">Section, same at start/end.</param>
        /// <param name="startEccentricity">Analytical eccentricity at start. If null, eccentricity set to 0.</param>
        /// <param name="endEccentricity">Analytical eccentricity at end. If null, eccentricity set to 0.</param>
        /// <param name="startConnectivity">Start connectivity.</param>
        /// <param name="endConnectivity">End connectivity.</param>
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
            this.Identifier = identifier;


            this.BarPart = new BarPart(edge, this.Type, material, section, startEccentricity, endEccentricity, startConnectivity, endConnectivity, identifier);
        }

        /// <summary>
        /// Construct beam or column with start/end section and different start/end conditions
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="material"></param>
        /// <param name="startSection">Start section.</param>
        /// <param name="endSection">End section.</param>
        /// <param name="startEccentricity">Analytical eccentricity at start. If null, eccentricity set to 0.</param>
        /// <param name="endEccentricity">Analytical eccentricity at end. If null, eccentricity set to 0.</param>
        /// <param name="startConnectivity">Start connectivity.</param>
        /// <param name="endConnectivity">End connectivity.</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section startSection, Sections.Section endSection, Eccentricity startEccentricity, Eccentricity endEccentricity, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;
            this.Identifier = identifier;
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
            this.Identifier = identifier;
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
        /// <param name="startConnectivity">Start connectivity.</param>
        /// <param name="endConnectivity">End connectivity.</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section[] sections, double[] positions, Eccentricity[] eccentricities, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;
            this.Identifier = identifier;
            this.BarPart = new BarPart(edge, this.Type, material, sections, positions, eccentricities, startConnectivity, endConnectivity, identifier);
        }


        /// <summary>
        /// Construct a truss element.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="material"></param>
        /// <param name="section"></param>
        /// <param name="trussBehaviour"></param>
        /// <param name="identifier"></param>
        /// <exception cref="System.Exception"></exception>
        public Bar(Geometry.Edge edge, Materials.Material material, Sections.Section section, string identifier = "B", Truss_chr_type trussBehaviour = null)
        {
            this.EntityCreated();
            this.Type = BarType.Truss;
            this.Identifier = identifier;
            if(trussBehaviour != null)
            {
                this.TrussBehaviour = trussBehaviour;
            }
            else
            {
                this.TrussBehaviour = Truss_chr_type.Elastic();
            }
            this.BarPart = new BarPart(edge, this.Type, material, section, identifier);
        }


        /// <summary>
        /// Construct a composite beam or composite column with uniform section and uniform start/end conditions.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="compositeSection"></param>
        /// <param name="eccentricity">Analytical eccentricity, same at start/end. Eccentricity set to 0,0 if null.</param>
        /// <param name="connectivity">Connectivity, same at start/end. Connectivity set to Rigid if null.</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Composites.CompositeSection compositeSection, Eccentricity eccentricity = null, Connectivity connectivity = null, string identifier = "B")
        {
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;

            if (eccentricity == null) { eccentricity = Eccentricity.Default; }
            if (connectivity == null) { connectivity = Connectivity.Default; }
            this.BarPart = new BarPart(edge, this.Type, compositeSection, eccentricity, connectivity, identifier);
        }

        /// <summary>
        /// Construct composite beam or composite column with uniform section and different start/end conditions.
        /// </summary>
        /// <param name="edge"></param>
        /// <param name="type"></param>
        /// <param name="compositeSection"></param>
        /// <param name="startEccentricity">Analytical start eccentricity</param>
        /// <param name="endEccentricity">Analytical end eccentricity</param>
        /// <param name="startConnectivity">Start connectivity</param>
        /// <param name="endConnectivity">End connectivity</param>
        /// <param name="identifier">Identifier</param>
        public Bar(Geometry.Edge edge, BarType type, Composites.CompositeSection compositeSection, Eccentricity startEccentricity = null, Eccentricity endEccentricity = null, Connectivity startConnectivity = null, Connectivity endConnectivity = null, string identifier = "B")
        {
            if (type == BarType.Truss) { throw new System.Exception("Truss is not a valid type"); }

            this.EntityCreated();
            this.Type = type;

            if (startEccentricity == null) { startEccentricity = Eccentricity.Default; }
            if (endEccentricity == null) { endEccentricity = Eccentricity.Default; }
            if (startConnectivity == null) { startConnectivity = Connectivity.Default; }
            if (endConnectivity == null) { endConnectivity = Connectivity.Default; }

            this.BarPart = new BarPart(edge, this.Type, compositeSection, startEccentricity, endEccentricity, startConnectivity, endConnectivity, identifier);
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
            truss.Identifier = identifier;
            truss.BarPart = new BarPart(edge, truss.Type, material, section, identifier);
            return truss;
        }

        public void UpdateSection(Sections.Section section)
        {
            if(this.Type == BarType.Truss)
            {
                this.UpdateTrussSection(section);
            }
            else if(!this.BarPart.HasComplexCompositeRef || !this.BarPart.HasDeltaBeamComplexSectionRef)
            {
                var sections = new Sections.Section[2] { section, section };
                this.UpdateSection(sections);
            }
            else
            {
                throw new NotImplementedException("Bar with Composite Section can not be updated yet! Send a request.");
            }
        }

        /// <summary>
        /// Update Sections for a Bar Element. Composite Section and Delta Beam are not implemented yet. Send us a request.
        /// </summary>
        /// <param name="sections"></param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public void UpdateSection(Sections.Section[] sections)
        {
            if(!this.BarPart.HasComplexCompositeRef || !this.BarPart.HasDeltaBeamComplexSectionRef)
            {
                if (this.Type != BarType.Truss)
                {
                    if (sections.Length < 2)
                    {
                        throw new System.ArgumentException($"Number of sections: {sections.Length}, must be 2 or more");
                    }

                    for (int i = 0; i < sections.Length; i++)
                    {
                        this.BarPart.ComplexSectionObj.Parts[i].SectionObj = sections[i];
                        this.BarPart.ComplexSectionObj.Parts[i].SectionRef = sections[i].Guid;
                    }
                }
                else if (this.Type == BarType.Truss)
                {
                    throw new System.ArgumentException($"Number of sections: {sections.Length}, must be 1.");
                }
            }
            else
            {
                throw new NotImplementedException("Bar with Composite or Delta Section can not be updated yet! Send a request.");
            }
        }
        private void UpdateTrussSection(Sections.Section section)
        {
            this.BarPart.TrussUniformSectionObj = section;
        }

        /// <summary>
        /// Update Sections for a Bar Element. Material for Composite Section and Delta Beam are not implemented yet. Send us a request.
        /// </summary>
        /// <param name="material"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void UpdateMaterial(Materials.Material material)
        {
            if (!this.BarPart.HasComplexCompositeRef || !this.BarPart.HasDeltaBeamComplexSectionRef)
            {
                this.BarPart.ComplexMaterialObj = material;
                this.BarPart.ComplexMaterialRef = material.Guid;
            }
            else
            {
                throw new NotImplementedException("Bar with Composite Section or Delta Beam can not be updated yet! Send a request.");
            }
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
            {
                if (this.BarPart.HasComplexCompositeRef)
                {
                    var compositeSection = this.BarPart.ComplexCompositeObj.CompositeSections[0];
                    List<string> compositeMaterials = compositeSection.Materials.Select(m => m.Name).ToList();

                    return $"{this.Type} Start: {this.BarPart.Edge.Points.First()}, End: {this.BarPart.Edge.Points.Last()}, Length: {this.BarPart.Edge.Length} m, Section: Composite - {compositeSection.Type}, Materials: {string.Join(", ", compositeMaterials)}";
                }
                if (this.BarPart.HasDeltaBeamComplexSectionRef)
                {
                    return $"{this.Type} Start: {this.BarPart.Edge.Points.First()}, End: {this.BarPart.Edge.Points.Last()}, Length: {this.BarPart.Edge.Length} m, Sections: Delta section type, Material: Delta beam";
                }
                return $"{this.Type} Start: {this.BarPart.Edge.Points.First()}, End: {this.BarPart.Edge.Points.Last()}, Length: {this.BarPart.Edge.Length} m, Sections: ({this.BarPart.ComplexSectionObj.Sections.First()._sectionName}, {this.BarPart.ComplexSectionObj.Sections.Last()._sectionName}), Material: {this.BarPart.ComplexMaterialObj}";
            }
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