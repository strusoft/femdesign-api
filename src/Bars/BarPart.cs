// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Bars
{
    /// <summary>
    /// bar_part_type
    /// 
    /// Underlying representation of a Bar-element.
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class BarPart: EntityBase
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
                return this._edge;
            }
            set
            {
                this._edge = value;

                if (this.Type == "beam")
                {
                    if (value.Type == "line" || value.Type == "arc")
                    {
                        this._edge = value;
                    }
                    else
                    {
                        throw new System.ArgumentException($"Edge type: {value.Type}, is not line or arc.");
                    }   
                }
                else if (this.Type == "column")
                {
                    // check if line
                    if (!value.IsLine())
                    {
                        throw new System.ArgumentException($"Edge type: {value.Type}, is not line.");
                    }

                    // check if line is vertical
                    if (!value.IsLineVertical())
                    {
                        throw new System.ArgumentException("Edge (Line) must be vertial for column definition");
                    }

                    // set value
                    this._edge = value;
                }
                else if (this.Type == "truss")
                {
                    // check if line
                    if (!value.IsLine())
                    {
                        throw new System.ArgumentException($"Edge type: {value.Type}, is not line.");
                    }

                    // set value
                    this._edge = value;
                }
                else
                {
                    throw new System.ArgumentException($"Incorrect type of bar: {this.Type}");
                }
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

        /// <summary>
        /// Private field for bar with start and end eccentricity
        /// </summary>
        [XmlIgnore]
        public Eccentricity[] _eccentricities = new Eccentricity[2];

        [XmlIgnore]
        public Eccentricity[] Eccentricities
        {
            get
            {
                return this._eccentricities;
            }
            set
            {
                if (value.Length == 1)
                {
                    this._eccentricities[0] = value[0];
                    this._eccentricities[1] = value[0];
                }
                else if (value.Length == 2)
                {
                    if (this.Type == "truss")
                    {
                        throw new System.ArgumentException("Truss can only have 1 section");
                    }

                    this._eccentricities[0] = value[0];
                    this._eccentricities[1] = value[1];
                }
                else
                {
                    throw new System.ArgumentException($"Incorrect length of Sections: {value.Length}. Length should be 1 or 2");
                }

                // update model eccentricity
                this.ModelEccentricity.StartAnalytical = value[0];
                this.ModelEccentricity.EndAnalytical = value[value.Length - 1];
                this.ModelEccentricity.StartPhysical = value[0];
                this.ModelEccentricity.EndPhysical = value[value.Length - 1];
            }
        }

        /// <summary>
        /// Get/set start eccentricity of bar
        /// </summary>
        [XmlIgnore]
        public Eccentricity StartEccentricity
        {
            get
            {
                return this._eccentricities[0];
            }
            set
            {
                // set value
                this._eccentricities[0] = value;

                // update complex section
                this._complexSection.Section = this.ModelSection.ToList();

                // update model eccentricity
                this._modelEccentricity.StartAnalytical = value;
                this._modelEccentricity.StartPhysical = value;
            }
        }

        /// <summary>
        /// Get/set end eccentricity of bar
        /// </summary>
        [XmlIgnore]
        public Eccentricity EndEccentricity
        {
            get
            {
                return this._eccentricities[1];
            }
            set
            {
                // set value
                this._eccentricities[1] = value;

                // update complex section                
                this._complexSection.Section = this.ModelSection.ToList();

                // update model eccentricity
                this._modelEccentricity.EndAnalytical = value;
                this._modelEccentricity.EndPhysical = value;
            }
        }

        /// <summary>
        /// Private field for bar with start and end section
        /// </summary>
        [XmlIgnore]
        private Sections.Section[] _sections = new Sections.Section[2];

        [XmlIgnore]
        public Sections.Section[] Sections
        {
            get
            {
                return this._sections;
            }
            set
            {
                if (value.Length == 1)
                {
                    this._sections[0] = value[0];
                    this._sections[1] = value[0];
                }
                else if (value.Length == 2)
                {
                    if (this.Type == "truss")
                    {
                        throw new System.ArgumentException("Truss can only have 1 section");
                    }

                    this._sections[0] = value[0];
                    this._sections[1] = value[1];
                }
                else
                {
                    throw new System.ArgumentException($"Incorrect length of Sections: {value.Length}. Length should be 1 or 2");
                }
            }
        }

        /// <summary>
        /// Try to get a uniform section. Compares section based on guid.
        public Sections.Section UniformSection
        {
            get
            {
                if (this.Sections.Length != 2)
                {
                    throw new System.ArgumentException($"Sections should contain 2 items but contains {this.Sections.Length}.");
                }

                if (this.Sections[0].Guid == this.Sections[1].Guid)
                {
                    return this.Sections[0];
                }
                else
                {
                    throw new System.ArgumentException("Sections contain two different sections. Impossible to get a uniform section.");
                }
            }
        }

        /// <summary>
        /// Get/set start section of bar
        /// </summary>
        [XmlIgnore]
        public Sections.Section StartSection
        {
            get
            {
                return this._sections[0];
            }
            set
            {
                // set value
                this._sections[0] = value;

                // update complex section
                this._complexSection.Section = this.ModelSection.ToList();
            }
        }

        /// <summary>
        /// Get/set end section of bar
        /// </summary>
        [XmlIgnore]
        public Sections.Section EndSection
        {
            get
            {
                return this._sections[1];
            }
            set
            {
                // set value
                this._sections[1] = value;

                // update complex section
                this._complexSection.Section = this.ModelSection.ToList();                
            }
        }

        /// <summary>
        /// Section position field
        /// </summary>
        [XmlIgnore]
        public double[] _sectionPos;

        /// <summary>
        /// Section position property. Set position of sections for complex section by defining the parammetric position 0-1.
        /// </summary>
        [XmlIgnore]
        public double[] SectionPos
        {
            get
            {
                if (this._sectionPos == null)
                {
                    double[] val = new double[this.Sections.Length];

                    // set start and end pos
                    val[0] = 0;
                    val[val.Length - 1] = 1;

                    if (val.Length > 2)
                    {
                        // set intermediate pos
                        for (int idx = 1; idx < val.Length - 1; idx++)
                        {
                            val[idx] = 1/(val.Length - 1)*idx;
                        }
                    }

                    // set
                    this._sectionPos = val;

                    // return
                    return this._sectionPos;
                }

                else
                {
                    // return
                    return this._sectionPos;
                }
            }
            set
            {
                if (value.Length != this.Sections.Length)
                {
                    throw new System.ArgumentException($"Length of value: {value.Length} must be equal to length of Sections: {this.Sections.Length}");
                }

                if (value[0] != 0)
                {
                    throw new System.ArgumentException("First item of value must be 0");
                }

                if (value[value.Length - 1] != 1)
                {
                    throw new System.ArgumentException("Last item of value must be 1");
                }

                this._sectionPos = value;
            }
        }

        /// <summary>
        /// Private property used for complex section updates
        /// </summary>
        [XmlIgnore]
        private Sections.ModelSection[] ModelSection
        {
            get
            {
                return new Sections.ModelSection[]
                {
                    new Sections.ModelSection(0, this.StartSection, this.StartEccentricity),
                    new Sections.ModelSection(1, this.EndSection, this.EndEccentricity)
                };
            }
        }

        /// <summary>
        /// Complex section field.
        /// </summary>
        [XmlIgnore]
        private Sections.ComplexSection _complexSection;

        /// <summary>
        /// Complex section property getter. For model deserialization use other property.
        /// </summary>
        [XmlIgnore]
        public Sections.ComplexSection ComplexSection
        {
            get
            {
                if (this._complexSection == null)
                {
                    // construct new complex section
                    this._complexSection = new Sections.ComplexSection(this.ModelSection.ToList());

                    // set guid ref
                    this.ComplexSectionRef = this._complexSection.Guid;

                    // return
                    return this._complexSection;
                }
                else
                {
                    // update _complexSection with BarPart sections and eccentricities
                    this._complexSection.Section = ModelSection.ToList();

                    // no need to set guid ref

                    // return
                    return this._complexSection;
                }
            }
        }

        /// <summary>
        /// Property to get and set _complexSection during model deserialization
        /// </summary>
        [XmlIgnore]
        internal Sections.ComplexSection ComplexSectionDerserialization
        {
            get
            {
                return this._complexSection;
            }
            set
            {
                this._complexSection = value;
                this.ComplexSectionRef = this._complexSection.Guid;
            }
        }

        [XmlIgnore]
        public bool ComplexSectionIsNull
        {
            get
            {
                if (this._complexSection == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Material field
        /// </summary>
        [XmlIgnore]
        public Materials.Material _material;

        /// <summary>
        /// Material property. When a new material is set the ComplexMaterialRef is updated.
        /// </summary>
        [XmlIgnore]
        public Materials.Material Material
        {
            get
            {
                return this._material;
            }
            set
            {
                this._material = value;
                this.ComplexMaterialRef = this._material.Guid;
            }
        }

        [XmlAttribute("name")]
        public string _identifier;

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
        private string _type;
        
        [XmlIgnore]
        public string Type
        {
            get {return this._type;}
            set {this._type = RestrictedString.BeamType(value);}
        }

        [XmlAttribute("complex_material")]
        public System.Guid ComplexMaterialRef { get; set; } // guidtype

        [XmlAttribute("complex_section")]
        public System.Guid ComplexSectionRef { get; set; } // guidtype

        [XmlAttribute("made")]
        public string _made; // steelmadetype

        [XmlIgnore]
        public string Made
        {
            get {return this._made;}
            set
            {
                if (value == null)
                {
                    this._made = value;
                }
                else
                {
                    this._made = RestrictedString.SteelMadeType(value);
                }
            }
        }
        [XmlAttribute("ecc_calc")]
        public bool EccentricityCalc { get; set; } // bool

        [XmlElement("connectivity", Order = 3)]
        public Connectivity[] _connectivities = new Connectivity[2]; // connectivity_type
        
        [XmlIgnore]
        public Connectivity[] Connectivities
        {
            get
            {
                return this._connectivities;
            }
            set
            {
                if (value.Length == 1)
                {
                    this._connectivities[0] = value[0];
                    this._connectivities[1] = value[0];
                }
                else if (value.Length == 2)
                {
                    this._connectivities[0] = value[0];
                    this._connectivities[1] = value[1];
                }
                else
                {
                    throw new System.ArgumentException($"Incorrect length of Connectivities: {value.Length}. Length should be 1 or 2");
                }
            }
        }
           
        
        [XmlElement("eccentricity", Order = 4)]
        public ModelEccentricity _modelEccentricity;

        [XmlIgnore]
        public ModelEccentricity ModelEccentricity
        {
            get
            {
                if (this._modelEccentricity == null)
                {
                    this._modelEccentricity = new ModelEccentricity(this.Eccentricities[0], this.Eccentricities[this.Eccentricities.Length - 1], true);
                }

                return this._modelEccentricity;
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

        private BarPart(string name, Geometry.Edge _edge, Materials.Material _material)
        {
            this.EntityCreated();
            this.Identifier = name;
            this.ComplexMaterialRef = _material.Guid;
            this.EccentricityCalc = true; // default should be false, but is always true since FD15? should be activated if eccentricity is defined
            this._edge = _edge;
            this.LocalY = _edge.CoordinateSystem.LocalY;
        }

        /// <summary>
        /// Construct BarPart (beam or column)
        /// </summary>
        public BarPart(Geometry.Edge edge, string type, Materials.Material material, Sections.Section[] sections, Connectivity[] connectivities, Eccentricity[] eccentricities, string identifier)
        {
            this.EntityCreated();
            this.Edge = edge;
            this.Type = type;
            this.Material = material;
            this.Sections = sections;
            this.Connectivities = connectivities;
            this.Eccentricities = eccentricities;
            this.EccentricityCalc = true;
            this.Identifier = identifier;
        }

        /// <summary>
        /// Construct BarPart (truss)
        /// <summary>
        public BarPart(Geometry.Edge edge, string type, Materials.Material material, Sections.Section section, string identifier)
        {
            this.EntityCreated();
            this.Edge = edge;
            this.Type = type;
            this.Material = material;
            this.Sections = new Sections.Section[1]{section};
            this.Identifier = identifier;
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

        /// <summary>
        /// Create a beam BarPart. Beam BarPart can be constructed on both Edge (Line and Arc).
        /// </summary>
        internal static BarPart Beam(string name, Geometry.Edge edge, Connectivity connectivity, Eccentricity eccentricity, Materials.Material material, Sections.ComplexSection complexSection)
        {
            if (edge.Type == "line" || edge.Type == "arc")
            {
                BarPart barPart = new BarPart(name, edge, material);
                barPart.ComplexSectionRef = complexSection.Guid;
                barPart._connectivities = new Connectivity[]{connectivity, connectivity}; // start and end eccentricity
                barPart._modelEccentricity = new ModelEccentricity(eccentricity);
                return barPart;
            }
            else
            {
                throw new System.ArgumentException($"Edge type: {edge.Type}, is not line or arc.");
            }   
        }

        /// <summary>
        /// Create a column BarPart. Column BarPart can only be constructed on an Edge (Line).
        /// </summary>
        internal static BarPart Column(string name, Geometry.Edge edge, Connectivity connectivity, Eccentricity eccentricity, Materials.Material material, Sections.ComplexSection complexSection)
        {
            // check if line
            if (!edge.IsLine())
            {
                throw new System.ArgumentException($"Edge type: {edge.Type}, is not line.");
            }

            // check if line is vertical
            if (!edge.IsLineVertical())
            {
                throw new System.ArgumentException("Edge (Line) must be vertial for column definition");
            }

            BarPart barPart = BarPart.Beam(name, edge, connectivity, eccentricity, material, complexSection);
            return barPart; 
        }

        /// <summary>
        /// Create a Truss BarPart. Truss BarPart can only be constructed on an Edge (Line).
        /// 
        /// Truss has no ComplexSection.
        /// </summary>
        internal static BarPart Truss(string name, Geometry.Edge edge, Materials.Material material, Sections.Section section)
        {
            // check if line
            if (!edge.IsLine())
            {
                throw new System.ArgumentException($"Edge type: {edge.Type}, is not line.");
            }
            
            BarPart barPart = new BarPart(name, edge, material);
            barPart.ComplexSectionRef = section.Guid;
            return barPart;
        }
    }
}