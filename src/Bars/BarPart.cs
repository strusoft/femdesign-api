// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Bars
{
    /// <summary>
    /// bar_part_type
    /// 
    /// Underlying representation of a Bar-element.
    /// </summary>
    [System.Serializable]
    public class BarPart: EntityBase
    {
        [XmlAttribute("name")]
        public string name { get; set; } // identifier
        [XmlAttribute("complex_material")]
        public System.Guid complexMaterial { get; set; } // guidtype
        [XmlAttribute("complex_section")]
        public System.Guid complexSection { get; set; } // guidtype
        [XmlAttribute("made")]
        public string _made; // steelmadetype
        [XmlIgnore]
        public string made
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
        public bool ecc_calc { get; set; } // bool
        [XmlElement("curve", Order = 1)]
        public Geometry.Edge edge { get; set; } // edge_type
        [XmlElement("local-y", Order = 2)]
        public Geometry.FdVector3d _localY;//  point_type_3d
        [XmlIgnore]
        public Geometry.FdVector3d localY
        {
            get { return this._localY; }
            set
            {
                Geometry.FdVector3d val = value.Normalize();
                double dot = this.edge.coordinateSystem.localX.Dot(val);
                if (Math.Abs(dot) < Tolerance.dotProduct)
                {
                    this._localY = val;
                }

                else
                {
                    throw new System.ArgumentException($"X-axis is not perpendicular to y-axis: {value}. The dot-product is {dot}, but should be 0");
                }
            }
        }
        [XmlElement("connectivity", Order = 3)]
        public List<Connectivity> connectivity = new List<Connectivity>(); // connectivity_type
        [XmlElement("eccentricity", Order = 4)]
        public ModelEccentricity eccentricity { get; set; } // eccentricity_type
        [XmlElement("buckling_data", Order = 5)]
        public Buckling.BucklingData bucklingData { get; set; } // buckling_data_type
        [XmlElement("end", Order = 6)]
        public string end { get; set; } // empty_type

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private BarPart()
        {
            
        }

        private BarPart(string _name, Geometry.Edge _edge, Materials.Material _material)
        {
            this.EntityCreated();
            this.name = _name + ".1";
            this.complexMaterial = _material.guid;
            this.ecc_calc = true; // default should be false, but is always true since FD15? should be activated if eccentricity is defined
            
            // orient edge coordinate system
            _edge.OrientCoordinateSystemToGCS();
            
            this.edge = _edge;
            this.localY = _edge.coordinateSystem.localY;
            this.end = "";
        }

        /// <summary>
        /// Create a beam BarPart. Beam BarPart can be constructed on both Edge (Line and Arc).
        /// </summary>
        internal static BarPart Beam(string name, Geometry.Edge edge, Connectivity connectivity, Eccentricity eccentricity, Materials.Material material, Sections.ComplexSection complexSection)
        {
            if (edge.type == "line" || edge.type == "arc")
            {
                BarPart barPart = new BarPart(name, edge, material);
                barPart.complexSection = complexSection.guid;
                barPart.connectivity = new List<Connectivity>{connectivity, connectivity}; // start and end eccentricity
                barPart.eccentricity = new ModelEccentricity(eccentricity);
                return barPart;
            }
            else
            {
                throw new System.ArgumentException($"Edge type: {edge.type}, is not line or arc.");
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
                throw new System.ArgumentException($"Edge type: {edge.type}, is not line.");
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
                throw new System.ArgumentException($"Edge type: {edge.type}, is not line.");
            }
            
            BarPart barPart = new BarPart(name, edge, material);
            barPart.complexSection = section.guid;
            return barPart;
        }
    }
}