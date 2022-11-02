// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.ComponentModel;

using FemDesign.GenericClasses;

namespace FemDesign.Bars
{
    /// <summary>
    /// bar_part_type
    /// 
    /// Underlying representation of a Bar-element.
    /// </summary>
    [System.Serializable]
    public partial class BarPart : NamedEntityPartBase, IStageElement
    {
        [XmlIgnore]
        private static int _barInstance = 0; // Number of bar/beam instances created
        [XmlIgnore]
        private static int _columnInstance = 0; // Number of column instances created
        [XmlIgnore]
        private static int _trussInstance = 0; // Number of truss instances created
        protected override int GetUniqueInstanceCount()
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
        /// Edge field
        /// </summary>
        [XmlElement("curve", Order = 1)]
        public Geometry.Edge _edge;

        [XmlAttribute("stage")]
        public int StageId { get; set; } = 1;

        /// <summary>
        /// Edge property
        /// </summary>
        [XmlIgnore]
        public Geometry.Edge Edge
        {
            get
            {
                if (this._edge.Type == "line")
                {
                    this._edge.Normal = this.LocalY;
                }

                return this._edge;
            }
            set
            {
                if (this.Type == BarType.Beam)
                {
                    // check if line or arc
                    if (value.Type == "line" || value.Type == "arc")
                    {
                        // pass
                    }
                    else
                    {
                        throw new System.ArgumentException($"Edge type: {value.Type}, is not line or arc. Circle is not supported. Consider splitting the Circle in two arches.");
                    }
                }
                else if (this.Type == BarType.Column)
                {
                    // check if line
                    if (!value.IsLine())
                    {
                        throw new System.ArgumentException($"Edge type: {value.Type}, is not line.");
                    }

                    // check if line local x equals positive global Z
                    if (!value.IsLineTangentEqualToGlobalZ())
                    {
                        throw new System.ArgumentException("Edge (Line) must be vertial with its tangent equal to positive global Z for column definition");
                    }
                }
                else if (this.Type == BarType.Truss)
                {
                    // check if line
                    if (!value.IsLine())
                    {
                        throw new System.ArgumentException($"Edge type: {value.Type}, is not line.");
                    }
                }
                else
                {
                    throw new System.ArgumentException($"Incorrect type of bar: {this.Type}");
                }

                // set value
                this._edge = value;

                // update cooridnate system to fit new edge
                this.CoordinateSystem = value.CoordinateSystem;
            }
        }

        [XmlIgnore]
        private Geometry.CoordinateSystem _coordinateSystem;

        [XmlIgnore]
        private Geometry.CoordinateSystem CoordinateSystem
        {
            get
            {
                if (this._coordinateSystem == null)
                {
                    this._coordinateSystem = this.Edge.CoordinateSystem;
                    return this._coordinateSystem;
                }
                else
                {
                    return this._coordinateSystem;
                }
            }
            set
            {
                this._coordinateSystem = value;
                this._localY = value.LocalY;
            }
        }

        [XmlIgnore]
        public Geometry.Point3d LocalOrigin
        {
            get
            {
                return this.CoordinateSystem.Origin;
            }
        }

        [XmlIgnore]
        public Geometry.Vector3d LocalX
        {
            get
            {
                return this.CoordinateSystem.LocalX;
            }
        }

        [XmlElement("local-y", Order = 2)]
        public Geometry.Vector3d _localY;

        [XmlIgnore]
        public Geometry.Vector3d LocalY
        {
            get
            {
                return this._localY;
            }
            set
            {
                this.CoordinateSystem.SetYAroundX(value);
                this._localY = this.CoordinateSystem.LocalY;
            }
        }

        [XmlIgnore]
        public Geometry.Vector3d LocalZ
        {
            get
            {
                return this.CoordinateSystem.LocalZ;
            }
        }

        [XmlIgnore]
        public BarType Type;

        [XmlIgnore]
        public SectionType SectionType
        {
            get
            {
                if (this.Type == BarType.Truss)
                {
                    return SectionType.Truss;
                }
                else if (this.Type != BarType.Truss && this.HasComplexSectionRef && !this.HasDeltaBeamComplexSectionRef)
                {
                    return SectionType.RegularBeamColumn;
                }
                else if (this.Type != BarType.Truss && this.HasComplexCompositeRef)
                {
                    return SectionType.CompositeBeamColumn;
                }
                else if (this.Type != BarType.Truss && this.HasComplexSectionRef && this.HasDeltaBeamComplexSectionRef)
                {
                    return SectionType.DeltaBeamColumn;
                }
                else
                {
                    throw new System.ArgumentException("Type of bar is not supported.");
                }
            }
        }

        [XmlAttribute("complex_composite")]
        public string ComplexCompositeRef { get; set; } // guidtype

        [XmlIgnore]
        public bool HasComplexCompositeRef { get => this.ComplexCompositeRef != null; }

        [XmlIgnore]
        public StruSoft.Interop.StruXml.Data.Complex_composite_type ComplexCompositeObj { get; set; }

        [XmlAttribute("complex_material")]
        public string _complexMaterialRef;

        [XmlIgnore]
        public System.Guid ComplexMaterialRef
        {
            get
            {
                return System.Guid.Parse(this._complexMaterialRef);
            }
            set
            {
                this._complexMaterialRef = value.ToString();
            }
        }
        [XmlIgnore]
        public bool HasComplexMaterialRef { get => this.ComplexMaterialRef != System.Guid.Empty; }

        /// <summary>
        /// Material field
        /// </summary>
        [XmlIgnore]
        public Materials.Material _complexMaterialObj;

        /// <summary>
        /// Material property. When a new material is set the ComplexMaterialRef is updated.
        /// </summary>
        [XmlIgnore]
        public Materials.Material ComplexMaterialObj
        {
            get
            {
                return this._complexMaterialObj;
            }
            set
            {
                this._complexMaterialObj = value;
                this.ComplexMaterialRef = this._complexMaterialObj.Guid;
            }
        }

        [XmlIgnore]
        private string _complexSectionRef;

        [XmlAttribute("complex_section")]
        public string ComplexSectionRef
        {
            get
            {
                // if truss --> guid from TrussUniformSectionObj
                if (this.Type == BarType.Truss)
                {
                    if (this.TrussUniformSectionObj == null)
                    {
                        return this._complexSectionRef;
                    }
                    else
                    {
                        var r = this.TrussUniformSectionObj.Guid.ToString();
                        this._complexSectionRef = r;
                        return r;
                    }
                }
                // else --> guid from ComplexSectionObj
                else
                {
                    if (this.ComplexSectionObj == null)
                    {
                        return this._complexSectionRef;
                    }
                    else
                    {
                        var r = this.ComplexSectionObj.Guid.ToString();
                        this._complexSectionRef = r;
                        return r;
                    }

                }
            }
            set
            {
                this._complexSectionRef = value;
            }
        }

        [XmlIgnore]
        public bool HasComplexSectionRef { get => this.ComplexSectionRef != null; }

        [XmlIgnore]
        public bool HasDeltaBeamComplexSectionRef { get => !System.Guid.TryParse(this.ComplexSectionRef, out System.Guid result); }

        [XmlIgnore]
        public Sections.ComplexSection ComplexSectionObj;

        [XmlIgnore]
        public Sections.Section TrussUniformSectionObj;

        [XmlIgnore]
        public SteelMadeType? SteelMadeType;

        [XmlAttribute("made")]
        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public SteelMadeType steelMadeType
        {
            get { return SteelMadeType.Value; }
            set { SteelMadeType = value; }
        }

        [System.ComponentModel.Browsable(false), System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ShouldSerializesteelMadeType() => SteelMadeType.HasValue;

        [XmlAttribute("ecc_calc")]
        public bool EccentricityCalc { get; set; } // bool


        [XmlElement("connectivity", Order = 3)]
        public Connectivity[] _connectivity = new Connectivity[2]; // connectivity_type

        [XmlIgnore]
        public Connectivity[] Connectivity
        {
            get
            {
                return this._connectivity;
            }
            set
            {
                if (this.Type == BarType.Truss)
                {
                    throw new System.Exception("Can't set connectivity to a truss");
                }
                else if (value.Length == 1)
                {
                    this._connectivity[0] = value[0];
                    this._connectivity[1] = value[0];
                }
                else if (value.Length == 2)
                {
                    this._connectivity[0] = value[0];
                    this._connectivity[1] = value[1];
                }
                else
                {
                    throw new System.ArgumentException($"Incorrect length of Connectivities: {value.Length}. Length should be 1 or 2");
                }
            }
        }

        [XmlIgnore]
        public ModelEccentricity _eccentricityTypeField;
        [XmlElement("eccentricity", Order = 4)]
        public ModelEccentricity _eccentricityTypeProperty
        {
            get
            {
                // truss has no eccentricity
                if (this.Type == BarType.Truss)
                {
                    return null;
                }
                // beam and column have eccentricity and analytical eccentricity
                // must equal complexSectionObj eccentricity
                else if (this.HasComplexSectionRef && !this.HasDeltaBeamComplexSectionRef)
                {
                    var sectionEcc = this.ComplexSectionObj.Eccentricities;

                    if (this._eccentricityTypeField == null)
                    {
                        Eccentricity eccentricity = new Eccentricity();
                        this._eccentricityTypeField = new ModelEccentricity(eccentricity);
                    }

                    this._eccentricityTypeField.StartAnalytical = sectionEcc.First();
                    this._eccentricityTypeField.EndAnalytical = sectionEcc.Last();
                    return this._eccentricityTypeField;
                }
                else
                {
                    return this._eccentricityTypeField;
                }
            }
            set
            {
                this._eccentricityTypeField = value;
            }
        }
        [XmlIgnore]
        public Eccentricity[] AnalyticalEccentricity
        {
            get
            {
                if (this.Type == BarType.Truss)
                {
                    throw new System.Exception("Truss has no eccentricity");
                }
                else if (this.HasComplexSectionRef)
                {
                    return this.ComplexSectionObj.Eccentricities;
                }
                else
                {
                    return this._eccentricityTypeProperty.Analytical;
                }
            }
            set
            {
                if (this.Type == BarType.Truss)
                {
                    throw new System.Exception("Truss has no eccentricity");
                }
                else if (this.HasComplexSectionRef)
                {
                    // create new complex section obj
                    var sections = this.ComplexSectionObj.Sections;
                    var pos = this.ComplexSectionObj.Positions;
                    this.ComplexSectionObj = new Sections.ComplexSection(sections, pos, value);
                }
                else if (this.HasComplexCompositeRef)
                {
                    this._eccentricityTypeProperty.Analytical = value;
                }
            }
        }
        [XmlElement("buckling_data", Order = 5)]
        public Buckling.BucklingData BucklingData { get; set; } // buckling_data_type

        [XmlElement("camber_type_2d", Order = 6)]
        public StruSoft.Interop.StruXml.Data.Camber_type_2d CamberType2d { get; set; }

        [XmlElement("stiffness_modifiers", Order = 7)]
        public BarStiffnessFactors BarStiffnessFactors { get; set; }

        [XmlElement("end", Order = 8)]
        public string End = "";

        [XmlAttribute("ecc_crack")]
        [DefaultValue(false)]
        public bool EccCrack { get; set; }

        [XmlAttribute("first_order_analysis_U")]
        [DefaultValue(false)]
        public bool FirstOrderAnalysisU { get; set; }

        [XmlAttribute("first_order_analysis_Sq")]
        [DefaultValue(false)]
        public bool FirstOrderAnalysisSq { get; set; }

        [XmlAttribute("first_order_analysis_Sf")]
        [DefaultValue(false)]
        public bool FirstOrderAnalysisSf { get; set; }

        [XmlAttribute("first_order_analysis_Sc")]
        [DefaultValue(false)]
        public bool FirstOrderAnalysisSc { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private BarPart()
        {

        }

        /// <summary>
        /// Construct beam or column with uniform section and uniform start/end conditions.
        /// </summary>
        public BarPart(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section section, Eccentricity eccentricity, Connectivity connectivity, string identifier)
        {
            if (type == BarType.Truss)
            {
                throw new System.ArgumentException($"Type: {type.ToString()}, is not of type {BarType.Beam.ToString()} or {BarType.Column.ToString()}");
            }
            else
            {
                this.EntityCreated();
                this.Type = type;
                this.Edge = edge;
                this.ComplexMaterialObj = material;
                this.ComplexSectionObj = new Sections.ComplexSection(section, eccentricity);
                this.Connectivity = new Connectivity[1] { connectivity };
                this.EccentricityCalc = true;
                this.Identifier = identifier;
            }
        }

        /// <summary>
        /// Construct beam or column with uniform section and different start/end conditions.
        /// </summary>
        public BarPart(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section section, Eccentricity startEccentricity, Eccentricity endEccentricity, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
            if (type == BarType.Truss)
            {
                throw new System.ArgumentException($"Type: {type.ToString()}, is not of type {BarType.Beam.ToString()} or {BarType.Column.ToString()}");
            }
            else
            {
                this.EntityCreated();
                this.Type = type;
                this.Edge = edge;
                this.ComplexMaterialObj = material;
                this.ComplexSectionObj = new Sections.ComplexSection(section, section, startEccentricity, endEccentricity);
                this.Connectivity = new Connectivity[2] { startConnectivity, endConnectivity };
                this.EccentricityCalc = true;
                this.Identifier = identifier;
            }
        }

        /// <summary>
        /// Construct beam or column with start/end section and different start/end conditions
        /// </summary>
        public BarPart(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section startSection, Sections.Section endSection, Eccentricity startEccentricity, Eccentricity endEccentricity, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
            if (type == BarType.Truss)
            {
                throw new System.ArgumentException($"Type: {type.ToString()}, is not of type {BarType.Beam.ToString()} or {BarType.Column.ToString()}");
            }
            else
            {
                this.EntityCreated();
                this.Type = type;
                this.Edge = edge;
                this.ComplexMaterialObj = material;
                this.ComplexSectionObj = new Sections.ComplexSection(startSection, endSection, startEccentricity, endEccentricity);
                this.Connectivity = new Connectivity[2] { startConnectivity, endConnectivity };
                this.EccentricityCalc = true;
                this.Identifier = identifier;
            }
        }

        /// <summary>
        /// Construct beam or column with non-uniform section and start/end conditions
        /// </summary>
        public BarPart(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section[] sections, Eccentricity[] eccentricities, Connectivity[] connectivities, string identifier)
        {
            if (type == BarType.Truss)
            {
                throw new System.ArgumentException($"Type: {type.ToString()}, is not of type {BarType.Beam.ToString()} or {BarType.Column.ToString()}");
            }
            else
            {
                this.EntityCreated();
                this.Type = type;
                this.Edge = edge;
                this.ComplexMaterialObj = material;
                this.ComplexSectionObj = new Sections.ComplexSection(sections, eccentricities);
                this.Connectivity = connectivities;
                this.EccentricityCalc = true;
                this.Identifier = identifier;
            }
        }

        /// <summary>
        /// Construct beam or column with non-uniform section and start/end conditions
        /// </summary>
        public BarPart(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section[] sections, double[] positions, Eccentricity[] eccentricities, Connectivity startConnectivity, Connectivity endConnectivity, string identifier)
        {
            if (type == BarType.Truss)
            {
                throw new System.ArgumentException($"Type: {type.ToString()}, is not of type {BarType.Beam.ToString()} or {BarType.Column.ToString()}");
            }
            else
            {
                this.EntityCreated();
                this.Type = type;
                this.Edge = edge;
                this.ComplexMaterialObj = material;
                this.ComplexSectionObj = new Sections.ComplexSection(sections, positions, eccentricities);
                this.Connectivity = new Connectivity[2] { startConnectivity, endConnectivity };
                this.EccentricityCalc = true;
                this.Identifier = identifier;
            }
        }

        /// <summary>
        /// Construct BarPart (truss)
        /// </summary>
        public BarPart(Geometry.Edge edge, BarType type, Materials.Material material, Sections.Section section, string identifier)
        {
            if (type != BarType.Truss)
            {
                throw new System.ArgumentException($"Type: {type.ToString()}, is not of type {BarType.Truss.ToString()}");
            }
            else
            {
                this.EntityCreated();
                this.Type = type;
                this.Edge = edge;
                this.ComplexMaterialObj = material;
                this.TrussUniformSectionObj = section;
                this.Identifier = identifier;
            }
        }

        /// <summary>
        /// Orient this object's coordinate system to GCS
        /// </summary>
        public void OrientCoordinateSystemToGCS()
        {
            var cs = this.CoordinateSystem;
            cs.OrientEdgeTypeLcsToGcs();
            this.CoordinateSystem = cs;
        }
    }
}