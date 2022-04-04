// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;


namespace FemDesign.Bars
{
    /// <summary>
    /// bar_part_type
    /// 
    /// Underlying representation of a Bar-element.
    /// </summary>
    [System.Serializable]
    public partial class BarPart : EntityBase
    {
        /// <summary>
        /// Edge field
        /// </summary>
        [XmlElement("curve", Order = 1)]
        public Geometry.Edge _edge;

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
                        throw new System.ArgumentException($"Edge type: {value.Type}, is not line or arc.");
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
        private Geometry.FdCoordinateSystem _coordinateSystem;

        [XmlIgnore]
        private Geometry.FdCoordinateSystem CoordinateSystem
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
        public Geometry.FdPoint3d LocalOrigin
        {
            get
            {
                return this.CoordinateSystem.Origin;
            }
        }

        [XmlIgnore]
        public Geometry.FdVector3d LocalX
        {
            get
            {
                return this.CoordinateSystem.LocalX;
            }
        }

        [XmlElement("local-y", Order = 2)]
        public Geometry.FdVector3d _localY;

        [XmlIgnore]
        public Geometry.FdVector3d LocalY
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
        public Geometry.FdVector3d LocalZ
        {
            get
            {
                return this.CoordinateSystem.LocalZ;
            }
        }

        // /// <summary>
        // /// Private field for bar with start and end eccentricity
        // /// </summary>
        // [XmlIgnore]
        // public Eccentricity[] _eccentricities = new Eccentricity[2];

        // [XmlIgnore]
        // public Eccentricity[] Eccentricities
        // {
        //     get
        //     {
        //         // truss has no eccentricity
        //         if (this.Type == BarType.Truss)
        //         {
        //             return null;
        //         }

        //         // get eccentricities from complex section
        //         else if (this.AnyEccentricityIsNull)
        //         {
        //             if(this.HasComplexCompositeRef)
        //             {
        //                 this._eccentricities = this._modelEccentricity.Analytical;
        //             }
        //             else
        //             {
        //                 this._eccentricities = this.ComplexSection.Parts.Select(x => x.Eccentricity).ToArray();
        //             }
        //         }

        //         // return
        //         return this._eccentricities;
        //     }
        //     set
        //     {
        //         // truss has no eccentricity
        //         if (this.Type == BarType.Truss)
        //         {
        //             // pass
        //         }

        //         else if (value.Length == 1)
        //         {
        //             this._eccentricities[0] = value[0];
        //             this._eccentricities[1] = value[0];
        //         }

        //         else if (value.Length == 2)
        //         {
        //             this._eccentricities[0] = value[0];
        //             this._eccentricities[1] = value[1];
        //         }

        //         else
        //         {
        //             throw new System.ArgumentException($"Incorrect length of Sections: {value.Length}. Length should be 1 or 2");
        //         }
        //     }
        // }

        // /// <summary>
        // /// Check if one or both eccentricities are null
        // /// </summary>
        // public bool AnyEccentricityIsNull
        // {
        //     get
        //     {
        //         return (this._eccentricities[0] == null || this._eccentricities[1] == null);
        //     }
        // }

        // /// <summary>
        // /// Get/set start eccentricity of bar
        // /// </summary>
        // [XmlIgnore]
        // public Eccentricity StartEccentricity
        // {
        //     get
        //     {
        //         return this._eccentricities[0];
        //     }
        //     set
        //     {
        //         this._eccentricities[0] = value;
        //     }
        // }

        // /// <summary>
        // /// Get/set end eccentricity of bar
        // /// </summary>
        // [XmlIgnore]
        // public Eccentricity EndEccentricity
        // {
        //     get
        //     {
        //         return this._eccentricities[1];
        //     }
        //     set
        //     {
        //         this._eccentricities[1] = value;
        //     }
        // }

        // /// <summary>
        // /// Private field for bar with start and end section
        // /// </summary>
        // [XmlIgnore]
        // private Sections.Section[] _sections = new Sections.Section[2];

        // [XmlIgnore]
        // public Sections.Section[] Sections
        // {
        //     get
        //     {
        //         return this._sections;
        //     }
        //     set
        //     {
        //         if (value.Length == 1)
        //         {
        //             this._sections[0] = value[0];
        //             this._sections[1] = value[0];
        //         }
        //         else if (value.Length == 2)
        //         {
        //             if (this.Type == BarType.Truss)
        //             {
        //                 throw new System.ArgumentException("Truss can only have 1 section");
        //             }

        //             this._sections[0] = value[0];
        //             this._sections[1] = value[1];
        //         }
        //         else
        //         {
        //             throw new System.ArgumentException($"Incorrect length of Sections: {value.Length}. Length should be 1 or 2");
        //         }
        //     }
        // }

        // /// <summary>
        // /// Check if one or both sections are null
        // /// </summary>
        // public bool AnySectionIsNull
        // {
        //     get
        //     {
        //         return (this.Sections[0] == null || this.Sections[1] == null);
        //     }
        // }

        // /// <summary>
        // /// Try to get a uniform section. Compares section based on guid.
        // /// </summary>
        // public Sections.Section UniformSection
        // {
        //     get
        //     {
        //         if (this.Sections.Length != 2)
        //         {
        //             throw new System.ArgumentException($"Sections should contain 2 items but contains {this.Sections.Length}.");
        //         }

        //         if (this.Sections[0].Guid == this.Sections[1].Guid)
        //         {
        //             return this.Sections[0];
        //         }
        //         else
        //         {
        //             throw new System.ArgumentException("Sections contain two different sections. Impossible to get a uniform section.");
        //         }
        //     }
        // }

        // /// <summary>
        // /// Get/set start section of bar
        // /// </summary>
        // [XmlIgnore]
        // public Sections.Section StartSection
        // {
        //     get
        //     {
        //         return this._sections[0];
        //     }
        //     set
        //     {
        //         // set value
        //         this._sections[0] = value;
        //     }
        // }

        // /// <summary>
        // /// Get/set end section of bar
        // /// </summary>
        // [XmlIgnore]
        // public Sections.Section EndSection
        // {
        //     get
        //     {
        //         return this._sections[1];
        //     }
        //     set
        //     {
        //         // set value
        //         this._sections[1] = value;
        //     }
        // }

        // /// <summary>
        // /// Section position field
        // /// </summary>
        // [XmlIgnore]
        // public double[] _sectionPos;

        // /// <summary>
        // /// Section position property. Set position of sections for complex section by defining the parammetric position 0-1.
        // /// </summary>
        // [XmlIgnore]
        // public double[] SectionPos
        // {
        //     get
        //     {
        //         if (this._sectionPos == null)
        //         {
        //             double[] val = new double[this.Sections.Length];

        //             // set start and end pos
        //             val[0] = 0;
        //             val[val.Length - 1] = 1;

        //             if (val.Length > 2)
        //             {
        //                 // set intermediate pos
        //                 for (int idx = 1; idx < val.Length - 1; idx++)
        //                 {
        //                     val[idx] = 1 / (val.Length - 1) * idx;
        //                 }
        //             }

        //             // set
        //             this._sectionPos = val;

        //             // return
        //             return this._sectionPos;
        //         }

        //         else
        //         {
        //             // return
        //             return this._sectionPos;
        //         }
        //     }
        //     set
        //     {
        //         if (value.Length != this.Sections.Length)
        //         {
        //             throw new System.ArgumentException($"Length of value: {value.Length} must be equal to length of Sections: {this.Sections.Length}");
        //         }

        //         if (value[0] != 0)
        //         {
        //             throw new System.ArgumentException("First item of value must be 0");
        //         }

        //         if (value[value.Length - 1] != 1)
        //         {
        //             throw new System.ArgumentException("Last item of value must be 1");
        //         }

        //         this._sectionPos = value;
        //     }
        // }

        // /// <summary>
        // /// Private property used for complex section updates
        // /// </summary>
        // [XmlIgnore]
        // private Sections.ComplexSectionPart[] ModelSection
        // {
        //     get
        //     {
        //         return new Sections.ComplexSectionPart[]
        //         {
        //             new Sections.ComplexSectionPart(0, this.StartSection, this.StartEccentricity),
        //             new Sections.ComplexSectionPart(1, this.EndSection, this.EndEccentricity)
        //         };
        //     }
        // }

        // /// <summary>
        // /// Complex section field.
        // /// </summary>
        // [XmlIgnore]
        // private Sections.ComplexSection _complexSection;

        // /// <summary>
        // /// Complex section property getter. For model deserialization use other property.
        // /// </summary>
        // [XmlIgnore]
        // public Sections.ComplexSection ComplexSection
        // {
        //     get
        //     {
        //         // truss has no ComplexSection
        //         if (this.Type == BarType.Truss)
        //         {
        //             return null;
        //         }

        //         // update _complexSection with BarPart sections and eccentricities
        //         else if (!this.AnySectionIsNull)
        //         {
        //             this._complexSection.Parts = this.ModelSection.ToList();
        //         }

        //         else
        //         {
        //             // pass
        //         }

        //         // return
        //         return this._complexSection;
        //     }
        //     set
        //     {
        //         this._complexSection = value;
        //         this.ComplexSectionRef = value.Guid;
        //         this.Eccentricities = value.Parts.Select(x => x.Eccentricity).ToArray();
        //     }
        // }

        // /// <summary>
        // /// Check if ComplexSection is null without updating ComplexSection (check if private field _complexSection is null)
        // [XmlIgnore]
        // public bool ComplexSectionIsNull
        // {
        //     get
        //     {
        //         if (this._complexSection == null)
        //         {
        //             return true;
        //         }
        //         else
        //         {
        //             return false;
        //         }
        //     }
        // }

        /// <summary>
        /// Identifier field
        /// </summary>
        [XmlAttribute("name")]
        public string _identifier;

        /// <summary>
        /// Identifier property
        /// </summary>
        [XmlIgnore]
        public string Identifier
        {
            get
            {
                return this._identifier;
            }
            set
            {
                this._identifier = value + ".1";
            }
        }

        [XmlIgnore]
        public BarType Type { get; set; }

        [XmlAttribute("complex_composite")]
        public System.Guid ComplexCompositeRef { get; set; } // guidtype
        [XmlIgnore]
        public bool HasComplexCompositeRef { get => this.ComplexCompositeRef != System.Guid.Empty; }
        [XmlIgnore]
        public StruSoft.Interop.StruXml.Data.Complex_composite_type ComplexCompositeObj { get; set; }

        [XmlAttribute("composite_section")]
        public System.Guid CompositeSectionRef { get; set; } // guidtype
        [XmlIgnore]
        public bool HasComplexSectionRef { get => this.ComplexCompositeRef != System.Guid.Empty; }

        [XmlIgnore]
        public List<StruSoft.Interop.StruXml.Data.Composite_section_type> CompositeSection { get; set; }

        [XmlAttribute("complex_material")]
        public string _complexMaterialRef;
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
                        throw new System.Exception("No trussUniformSectionObj available.");
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
                        throw new System.Exception("No complexSectionObj available.");
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
                else if (this.HasComplexSectionRef)
                {
                    var sectionEcc = this.ComplexSectionObj.Eccentricities;
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
        [XmlElement("end", Order = 6)]
        public string End = "";

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
        /// <summary>
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
        /// <summary>
        public void OrientCoordinateSystemToGCS()
        {
            var cs = this.CoordinateSystem;
            cs.OrientEdgeTypeLcsToGcs();
            this.CoordinateSystem = cs;
        }
    }
}