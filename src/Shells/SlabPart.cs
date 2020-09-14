// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Shells
{
    /// <summary>
    /// slab_part_type
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class SlabPart: EntityBase
    {
        [XmlAttribute("name")]
        public string Name {get; set;} // identifier
        [XmlAttribute("complex_material")]
        public System.Guid ComplexMaterial {get; set;} // guidtype
        [XmlAttribute("alignment")]
        public string _alignment; // ver_align
        [XmlIgnore]
        public string Alignment
        {
            get {return this._alignment;}
            set {this._alignment = RestrictedString.VerticalAlign(value);}
        }
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
        public Geometry.FdPoint3d LocalPos {get; set;} // point_type_3d
        [XmlElement("local_x", Order = 4)]
        public Geometry.FdVector3d _localX; // point_type_3d
        [XmlIgnore]
        public Geometry.FdVector3d LocalX
        {
            get
            {
                return this._localX;
            }
            set
            {
                Geometry.FdVector3d val = value.Normalize();
                Geometry.FdVector3d z = this.LocalZ;

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
        public Geometry.FdVector3d _localY; // point_type_3d
        [XmlIgnore]
        public Geometry.FdVector3d LocalY
        {
            get
            {
                return this._localY;
            }
        }
        [XmlIgnore]
        private Geometry.FdVector3d _localZ;
        [XmlIgnore]
        public Geometry.FdVector3d LocalZ
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
                Geometry.FdVector3d val = value.Normalize();
                Geometry.FdVector3d x = this.LocalX;

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
        [XmlElement("end", Order = 6)]
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
        internal SlabPart(string name, Geometry.Region region, List<Thickness> thickness, Materials.Material complexMaterial, ShellEccentricity alignment, ShellOrthotropy orthotropy)
        {
            this.EntityCreated();
            this.Name = name;
            this.Region = region;
            this.ComplexMaterial = complexMaterial.Guid;
            this.Alignment = alignment.Alignment;
            this.AlignOffset = alignment.Eccentricity;
            this.OrthoAlfa = orthotropy.OrthoAlfa;
            this.OrthoRatio = orthotropy.OrthoRatio;
            this.EccentricityCalculation = alignment.EccentricityCalculation;
            this.EccentricityByCracking = alignment.EccentricityByCracking;
            this.Thickness = thickness;
            this.LocalPos = region.CoordinateSystem.origin;
            this._localX = region.CoordinateSystem.LocalX;
            this._localY = region.CoordinateSystem.LocalY;
            this.End = "";
        }

        /// <summary>
        /// Construct SlabPart with EdgeConnections.
        /// </summary>
        internal static SlabPart Define(string name, Geometry.Region region, List<Thickness> thickness, Materials.Material material, ShellEdgeConnection shellEdgeConnection, ShellEccentricity eccentricity, ShellOrthotropy orthotropy)
        {
            // add edgeConnections to region
            region.SetEdgeConnections(shellEdgeConnection);
            
            // construct new slabPart
            SlabPart slabPart = new SlabPart(name, region, thickness, material, eccentricity, orthotropy);

            // return
            return slabPart;
        }

        /// <summary>
        /// Get EdgeConnections. Used for Deconstruct methods and when redefining EdgeConnections on existing slab. 
        /// </summary>
        internal List<Shells.ShellEdgeConnection> GetEdgeConnections()
        {
            return this.Region.GetEdgeConnections();
        }

        #region dynamo

        /// <summary>
        /// Get Dynamo Surface from SlabPart Contours (Region).
        /// </summary>
        internal Autodesk.DesignScript.Geometry.Surface GetDynamoSurface()
        {
            return this.Region.ToDynamoSurface();
        }

        /// <summary>
        /// Get Dynamo Curves from SlabPart Contours (Region). Nested list.
        /// </summary>
        internal List<List<Autodesk.DesignScript.Geometry.Curve>> GetDynamoCurvesNested()
        {
            return this.Region.ToDynamoCurves();
        }

        /// <summary>
        /// Get Dynamo Curves from SlabPart Contours (Region).
        /// </summary>
        internal List<Autodesk.DesignScript.Geometry.Curve> GetDynamoCurves()
        {
            var rtn = new List<Autodesk.DesignScript.Geometry.Curve>();
            foreach (List<Autodesk.DesignScript.Geometry.Curve> container in this.GetDynamoCurvesNested())
            {
                foreach (Autodesk.DesignScript.Geometry.Curve obj in container)
                {
                    rtn.Add(obj);
                }
            }
            return rtn;
        }
        #endregion

        #region grasshopper

        /// <summary>
        /// Get Rhino Surface from SlabPart Contours (Region).
        /// </summary>
        public Rhino.Geometry.Brep GetRhinoSurface()
        {
            return this.Region.ToRhinoBrep();
        }

        /// <summary>
        /// Get Rhino Curves from SlabPart Contours (Region).
        /// </summary>
        public List<Rhino.Geometry.Curve> GetRhinoCurves()
        {
            return this.Region.ToRhinoCurves();
        }

        #endregion
    }
}