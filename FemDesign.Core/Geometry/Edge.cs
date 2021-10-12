// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    /// <summary>
    /// edge_type
    /// 
    /// Curves in FEM-Design are expressed as edges. This extended edge also contains a LCS to keep track of directions.
    /// </summary>
    [System.Serializable]
    public partial class Edge
    {         
        [XmlIgnore]
        private FdCoordinateSystem _coordinateSystem;
        /// <summary>
        /// Get/Set LCS
        /// 
        /// If no LCS exists on Edge (i.e. when an Edge was deserialized from path) an LCS will be reconstructed.
        /// </summary>
        [XmlIgnore]
        public FdCoordinateSystem CoordinateSystem
        {
            get
            {
                if (this._coordinateSystem != null)
                {
                    return this._coordinateSystem;
                }
                else
                {
                    FdPoint3d origin;
                    FdVector3d localX, localY, localZ;
                    
                    // arc1
                    if (this.Type == "arc" && this.Points.Count == 1)
                    {
                        // sweep angle
                        double sweepAngle = this.EndAngle - this.StartAngle;

                        // find p0, p1 and p2
                        FdPoint3d p0, p1, p2;
                        FdVector3d v = this.XAxis.Scale(this.Radius);
                        p0 = this.Points[0].Translate(v);
                        p1 = this.Points[0].Translate(v.RotateAroundAxis(sweepAngle/2, this.Normal));
                        p2 = this.Points[0].Translate(v.RotateAroundAxis(sweepAngle, this.Normal));

                        origin = p1;
                        localX = new FdVector3d(p0, p2).Normalize();
                        localZ = this.Normal;
                        localY = localZ.Cross(localX);
                        return new FdCoordinateSystem(origin, localX, localY, localZ);
                    }

                    // arc2
                    else if (this.Type == "arc" && this.Points.Count == 3)
                    {
                        origin = this.Points[1];
                        localX = new FdVector3d(this.Points[0], this.Points[2]).Normalize();
                        FdVector3d v = new FdVector3d(this.Points[0], this.Points[1]).Normalize();
                        localZ = v.Cross(localX);
                        localY = localZ.Cross(localX);
                        return new FdCoordinateSystem(origin, localX, localY, localZ);
                    }

                    // line
                    else if (this.Type == "line" && this.Points.Count == 2)
                    {
                        FdVector3d v = new FdVector3d(this.Points[0], this.Points[1]);
                        origin = this.Points[0].Translate(v.Scale(0.5));
                        localX = v.Normalize();
                        localY = this.Normal;
                        localZ = localX.Cross(localY);
                        return new FdCoordinateSystem(origin, localX, localY, localZ);
                    }

                    // else
                    else
                    {
                        throw new System.ArgumentException($"Could not reconstruct FdCoordinateSystem from Edge of type: {this.Type}");
                    }
                }
            }
            set { this._coordinateSystem = value; }
        }
        [XmlElement("point", Order = 1)]
        public List<Geometry.FdPoint3d> Points = new List<Geometry.FdPoint3d>(); // sequence: point_type_3d // ordered internal points, or the center of the circle/arc
        [XmlElement("normal", Order = 2)]
        public FdVector3d Normal { get; set; } // point_type_3d // normal of the curve; it must be used if the curve is arc or circle.
        [XmlElement("x_axis", Order = 3)]
        public Geometry.FdVector3d XAxis { get; set; } // point_type_3d // axis of base line (the value default is the x axis {1, 0, 0}) angles are measured from this direction.
        [XmlElement("edge_connection", Order = 4)]
        public Shells.ShellEdgeConnection EdgeConnection { get; set; } // optional. ec_type.
        [XmlAttribute("type")]
        public string _type; // edgetype
        [XmlIgnore]
        public string Type
        {
            get {return this._type;}
            set {this._type = RestrictedString.EdgeType(value);}
        }
        [XmlAttribute("radius")]
        public double Radius { get; set; }   // optional. double
        [XmlAttribute("start_angle")]
        public double StartAngle { get; set; } // optional. double
        [XmlAttribute("end_angle")]
        public double EndAngle { get; set; } // optional. double
        [XmlIgnore]
        public double Length
        {
            get
            {
                if (this.Type == "line")
                {
                    return (new FdVector3d(this.Points[0], this.Points[1]).Length());
                }
                else if (this.Type == "arc")
                {
                    if (this.Points.Count == 3)
                    {
                        throw new System.ArgumentException("Can't calculate length of edge for arc2 type. Calculation of sweep angle is not implemented.");
                    }
                    else
                    {
                        return (this.EndAngle - this.StartAngle) * this.Radius;
                    }
                }
                else if (this.Type == "circle")
                {
                    return 2 * Math.PI * this.Radius;
                }
                else
                {
                    throw new System.ArgumentException($"Edge type {this.Type} is not supported for length evaluation.");
                }
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal Edge()
        {

        }

        /// <summary>
        /// Construct Edge of arc1 type.
        /// </summary>
        public Edge(double radius, double startAngle, double endAngle, Geometry.FdPoint3d centerPoint, Geometry.FdVector3d xAxis, Geometry.FdCoordinateSystem coordinateSystem)
        {
            this.Type = "arc";
            this.Radius = radius;
            this.StartAngle = startAngle;
            this.EndAngle = endAngle;
            this.Points.Add(centerPoint);
            this.Normal = coordinateSystem.LocalZ;
            this.XAxis = xAxis;
            this.CoordinateSystem = coordinateSystem;
        }

        /// <summary>
        /// Construct Edge of arc2 type.
        /// </summary>
        public Edge(Geometry.FdPoint3d _startPoint, Geometry.FdPoint3d _midPoint, Geometry.FdPoint3d _endPoint, Geometry.FdCoordinateSystem _coordinateSystem)
        {
            this.Type = "arc";
            this.Points.Add(_startPoint);
            this.Points.Add(_midPoint);
            this.Points.Add(_endPoint);
            this.CoordinateSystem = _coordinateSystem;
        }

        /// <summary>
        /// Construct Edge of circle type.
        /// </summary>
        public Edge(double _radius, Geometry.FdPoint3d _centerPoint, Geometry.FdCoordinateSystem _coordinateSystem)
        {
            this.Type = "circle";
            this.Radius = _radius;
            this.Points.Add(_centerPoint);
            this.Normal = _coordinateSystem.LocalZ;
            this.CoordinateSystem = _coordinateSystem;
        }

        /// <summary>
        /// Construct Edge of line type by points and coordinate system.
        /// </summary>
        public Edge(Geometry.FdPoint3d _startPoint, Geometry.FdPoint3d _endPoint, Geometry.FdCoordinateSystem _coordinateSystem)
        {
            this.Type = "line";
            this.Points.Add(_startPoint);
            this.Points.Add(_endPoint);
            this.Normal = _coordinateSystem.LocalY;
            this.CoordinateSystem = _coordinateSystem;
        }

        /// <summary>
        /// Construct Edge of line type by points and normal (localY).
        /// </summary>
        public Edge(FdPoint3d startPoint, FdPoint3d endPoint, FdVector3d localY)
        {
            this.Type = "line";
            this.Points.Add(startPoint);
            this.Points.Add(endPoint);
            this.Normal = localY;
        }

        /// <summary>
        /// Check if line
        /// </summary>
        internal bool IsLine()
        {
            return (this.Type == "line");
        }

        /// <summary>
        /// Check if line is vertical (i.e. parallel to global Z)
        /// </summary>
        internal bool IsLineVertical()
        {
            if (this.IsLine())
            {
                return (this.Points[0].X == this.Points[1].X && this.Points[0].Y == this.Points[1].Y);
            }
            else
            {
                throw new System.ArgumentException($"Edge type: {this.Type}, is not line.");
            }
        }

        /// <summary>
        /// Check if line local x is equal to positive global Z
        /// </summary>
        internal bool IsLineTangentEqualToGlobalZ()
        {
            if (this.IsLine())
            {
                return (this.CoordinateSystem.LocalX.Equals(Geometry.FdVector3d.UnitZ(), Tolerance.Vector3d));
            }
            else
            {
                throw new System.ArgumentException($"Edge type: {this.Type}, is not line.");
            }

        }

        /// <summary>
        /// Reverse this edge
        /// </summary>
        internal void Reverse()
        {
            // reset coordinate system
            this.CoordinateSystem = null;

            if (this.Type == "line" && this.Points.Count == 2)
            {
                this.Points.Reverse();
            }

            else if (this.Type == "arc" && this.Points.Count == 3)
            {
                this.Points.Reverse();
            }

            else if (this.Type == "arc" && this.Points.Count == 1)
            {
                // get sweep angle
                double sweepAngle = this.EndAngle - this.StartAngle;
                
                // set new properties
                this.XAxis = this.XAxis.RotateAroundAxis(sweepAngle, this.Normal);
                this.Normal = this.Normal.Reverse();
                this.StartAngle = 0;
                this.EndAngle = sweepAngle;
            }

            else if (this.Type == "circle")
            {
                this.Normal = this.Normal.Reverse();
            }
            else
            {
                throw new System.ArgumentException($"Could not reverse Edge of type: {this.Type}");
            }
        }


    }
}