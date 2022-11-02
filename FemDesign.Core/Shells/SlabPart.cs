// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace FemDesign.Shells
{
    /// <summary>
    /// slab_part_type
    /// </summary>
    [System.Serializable]
    public partial class SlabPart: NamedEntityPartBase
    {
        private static int _plateInstance = 0;
        private static int _wallInstance = 0;
        protected override int GetUniqueInstanceCount()
        {
            switch (this.SlabType)
            {
                case SlabType.Plate:
                    return ++_plateInstance;
                case SlabType.Wall:
                    return ++_wallInstance;
                default:
                    throw new ArgumentException($"Incorrect type of slab: {this.SlabType}");
            }
        }

        [XmlIgnore]
        public SlabType SlabType;

        /// <summary>
        /// Get ShellEccentricity
        /// </summary>
        [XmlIgnore]
        public ShellEccentricity ShellEccentricity
        {
            get
            {
               return new ShellEccentricity(this.Alignment, this.AlignOffset, this.EccentricityCalculation, this.EccentricityByCracking); 
            }
        }

        /// <summary>
        /// Get ShellOrthotropy
        /// </summary>
        [XmlIgnore]
        public ShellOrthotropy ShellOrthotropy
        {
            get
            {
                return new ShellOrthotropy(this.OrthoAlfa, this.OrthoRatio);
            }
        }

        [XmlAttribute("complex_material")]
        public System.Guid ComplexMaterialGuid {get; set;} // guidtype

        [XmlIgnore]
        public Materials.Material ComplexMaterial { get; set; } // guidtype

        [XmlAttribute("alignment")]
        public GenericClasses.VerticalAlignment Alignment { get; set; }
        [XmlAttribute("align_offset")]
        public double _alignOffset; // abs_max_1e20
        [XmlIgnore]
        public double AlignOffset
        {
            get {return this._alignOffset;}
            set {this._alignOffset = RestrictedDouble.AbsMax_1e20(value);}
        }
        [XmlAttribute("ortho_alfa")]
        public double _orthoAlfa; // two_quadrants
        [XmlIgnore]
        public double OrthoAlfa
        {
            get {return this._orthoAlfa;}
            set {this._orthoAlfa = RestrictedDouble.TwoQuadrantsRadians(value);}
        }
        [XmlAttribute("ortho_ratio")]
        public double _orthoRatio; // orthotropy_type
        [XmlIgnore]
        public double OrthoRatio
        {
            get {return this._orthoRatio;}
            set {this._orthoRatio = RestrictedDouble.NonNegMax_1(value);}
        }
        [XmlAttribute("ecc_calc")]
        public bool EccentricityCalculation { get; set; } // bool
        [XmlAttribute("ecc_crack")]
        public bool EccentricityByCracking { get; set; } // bool
        [XmlAttribute("mesh_size")]
        public double _meshSize; // non_neg_max_1e20
        [XmlIgnore]
        public double MeshSize
        {
            get {return this._meshSize;}
            set {this._meshSize = RestrictedDouble.NonNegMax_1e20(value);}
        }
        [XmlElement("contour", Order = 1)]
        public List<Geometry.Contour> _region;
        [XmlIgnore]
        public Geometry.Region Region
        {
            get { return new Geometry.Region(this._region); }
            set { this._region = value.Contours; }
        }
        [XmlElement("thickness", Order = 2)]
        public List<Thickness> _thickness; // sequence: location_value
        [XmlIgnore]
        public List<Thickness> Thickness
        {
            get { return this._thickness; }
            set
            {
                if ( value.Count == 1 || value.Count == 2 || value.Count == 3)
                {
                    this._thickness = value;
                }
                else
                {
                    throw new System.ArgumentException("List of thickness objects must contain either 1, 2 or 3 elements");
                }
            }
        }
        [XmlElement("local_pos", Order = 3)]
        public Geometry.Point3d LocalPos {get; set;} // point_type_3d
        [XmlElement("local_x", Order = 4)]
        public Geometry.Vector3d _localX; // point_type_3d
        [XmlIgnore]
        public Geometry.Vector3d LocalX
        {
            get
            {
                return this._localX;
            }
            set
            {
                Geometry.Vector3d val = value.Normalize();
                Geometry.Vector3d z = this.LocalZ;

                double dot = z.Dot(val);
                if (Math.Abs(dot) < Tolerance.DotProduct)
                {
                    this._localX = val;
                    this._localY = z.Cross(val); // follows right-hand-rule
                }
                
                else
                {
                    throw new System.ArgumentException($"X-axis is not perpendicular to Z-axis. The dot-product is {dot}, but should be 0");
                }
            }
        }
        [XmlElement("local_y", Order = 5)]
        public Geometry.Vector3d _localY; // point_type_3d
        [XmlIgnore]
        public Geometry.Vector3d LocalY
        {
            get
            {
                return this._localY;
            }
        }
        [XmlIgnore]
        private Geometry.Vector3d _localZ;
        [XmlIgnore]
        public Geometry.Vector3d LocalZ
        {
            get
            {
                if (this.LocalX == null || this.LocalY == null)
                {
                    throw new System.ArgumentException("Impossible to construct z-axis as either this.localX or this.localY is null.");
                }
                else
                {     
                    return this.LocalX.Cross(LocalY).Normalize();
                }
            }
            set
            {
                Geometry.Vector3d val = value.Normalize();
                Geometry.Vector3d x = this.LocalX;

                double dot = x.Dot(val);
                if (Math.Abs(dot) < Tolerance.DotProduct)
                {
                    this._localZ = val;
                    this._localY = val.Cross(x); // follows right-hand-rule
                }
                
                else
                {
                    throw new System.ArgumentException($"Z-axis is not perpendicular to X-axis. The dot-product is {dot}, but should be 0");
                }
            }
        }

        [XmlElement(ElementName = "stiffness_modifiers", Order = 6)]
        public SlabStiffnessFactors SlabStiffnessFactors { get; set; }

        [XmlElement("end", Order = 7)]
        public string End {get; set;} // empty_type

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private SlabPart()
        {
            
        }

        /// <summary>
        /// Construct SlabPart.
        /// </summary>
        public SlabPart(SlabType type, string identifier, Geometry.Region region, List<Thickness> thickness, Materials.Material complexMaterial, ShellEccentricity alignment, ShellOrthotropy orthotropy)
        {
            this.EntityCreated();
            this.SlabType = type;
            this.Identifier = identifier;
            this.Region = region;
            this.ComplexMaterialGuid = complexMaterial.Guid;
            this.ComplexMaterial = complexMaterial;
            this.Alignment = alignment.Alignment;
            this.AlignOffset = alignment.Eccentricity;
            this.OrthoAlfa = orthotropy.OrthoAlfa;
            this.OrthoRatio = orthotropy.OrthoRatio;
            this.EccentricityCalculation = alignment.EccentricityCalculation;
            this.EccentricityByCracking = alignment.EccentricityByCracking;
            this.Thickness = thickness;
            this.LocalPos = region.CoordinateSystem.Origin;
            this._localX = region.CoordinateSystem.LocalX;
            this._localY = region.CoordinateSystem.LocalY;
            this.End = "";
        }

        /// <summary>
        /// Construct SlabPart with EdgeConnections.
        /// </summary>
        public static SlabPart Define(SlabType type, string identifier, Geometry.Region region, List<Thickness> thickness, Materials.Material material, EdgeConnection shellEdgeConnection = null, ShellEccentricity eccentricity = null, ShellOrthotropy orthotropy = null)
        {
            shellEdgeConnection = shellEdgeConnection ?? EdgeConnection.Default;
            eccentricity = eccentricity ?? ShellEccentricity.Default;
            orthotropy = orthotropy ?? ShellOrthotropy.Default;

            // add edgeConnections to region
            region.SetEdgeConnections(shellEdgeConnection);
            
            // construct new slabPart
            SlabPart slabPart = new SlabPart(type, identifier, region, thickness, material, eccentricity, orthotropy);

            // return
            return slabPart;
        }

        /// <summary>
        /// Get EdgeConnections. Used for Deconstruct methods and when redefining EdgeConnections on existing slab. 
        /// </summary>
        public List<Shells.EdgeConnection> GetEdgeConnections()
        {
            return this.Region.GetEdgeConnections();
        }


    }
}