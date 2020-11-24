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
        /// Bar edge
        /// </summary>
        [XmlElement("curve", Order = 1)]
        public Geometry.Edge Edge { get; set; }
        
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
        public Eccentricity[] _eccentricity = new Eccentricity[2];

        /// <summary>
        /// Get/set start eccentricity of bar
        /// </summary>
        [XmlIgnore]
        public Eccentricity StartEccentricity
        {
            get
            {
                return this._eccentricity[0];
            }
            set
            {
                // set value
                this._eccentricity[0] = value;

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
                return this._eccentricity[1];
            }
            set
            {
                // set value
                this._eccentricity[1] = value;

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
        private Sections.Section[] _section = new Sections.Section[2];

        /// <summary>
        /// Get/set start section of bar
        /// </summary>
        [XmlIgnore]
        public Sections.Section StartSection
        {
            get
            {
                return this._section[0];
            }
            set
            {
                // set value
                this._section[0] = value;

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
                return this._section[1];
            }
            set
            {
                // set value
                this._section[1] = value;

                // update complex section
                this._complexSection.Section = this.ModelSection.ToList();                
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
        /// Complex section property.
        /// </summary>
        [XmlIgnore]
        private Sections.ComplexSection _complexSection;

        /// <summary>
        /// Complex section property.
        /// </summary>
        [XmlIgnore]
        public Sections.ComplexSection ComplexSection
        {
            get
            {
                return this._complexSection;
            }
            set
            {
                this._complexSection = value;
                this.ComplexSectionRef = value.Guid;
            }
        }

        [XmlAttribute("name")]
        public string Name { get; set; } // identifier

        [XmlAttribute("complex_material")]
        public System.Guid ComplexMaterial { get; set; } // guidtype

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
        public List<Connectivity> Connectivity = new List<Connectivity>(); // connectivity_type
        [XmlElement("eccentricity", Order = 4)]
        public ModelEccentricity _modelEccentricity { get; set; } // eccentricity_type

        [XmlElement("buckling_data", Order = 5)]
        public Buckling.BucklingData BucklingData { get; set; } // buckling_data_type
        [XmlElement("end", Order = 6)]
        public string End { get; set; } // empty_type

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private BarPart()
        {
            
        }

        private BarPart(string _name, Geometry.Edge _edge, Materials.Material _material)
        {
            this.EntityCreated();
            this.Name = _name + ".1";
            this.ComplexMaterial = _material.Guid;
            this.EccentricityCalc = true; // default should be false, but is always true since FD15? should be activated if eccentricity is defined
            this.Edge = _edge;
            this.LocalY = _edge.CoordinateSystem.LocalY;
            this.End = "";
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
                barPart.Connectivity = new List<Connectivity>{connectivity, connectivity}; // start and end eccentricity
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