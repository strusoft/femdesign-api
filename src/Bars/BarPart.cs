// https://strusoft.com/
using System;
using System.Collections.Generic;
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
        [XmlElement("curve", Order = 1)]
        public Geometry.Edge Edge { get; set; } // edge_type
        
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

        [XmlAttribute("name")]
        public string Name { get; set; } // identifier

        [XmlAttribute("complex_material")]
        public System.Guid ComplexMaterial { get; set; } // guidtype

        [XmlAttribute("complex_section")]
        public System.Guid ComplexSection { get; set; } // guidtype

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
        public ModelEccentricity Eccentricity { get; set; } // eccentricity_type
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
                barPart.ComplexSection = complexSection.Guid;
                barPart.Connectivity = new List<Connectivity>{connectivity, connectivity}; // start and end eccentricity
                barPart.Eccentricity = new ModelEccentricity(eccentricity);
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
            barPart.ComplexSection = section.Guid;
            return barPart;
        }
    }
}