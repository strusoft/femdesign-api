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
    [XmlInclude(typeof(LineEdge))]
    [XmlInclude(typeof(ArcEdge))]
    [XmlInclude(typeof(CircleEdge))]
    [XmlRoot("database", Namespace = "urn:strusoft")]
    [System.Serializable]
    public partial class Edge
    {
        [XmlIgnore]
        private CoordinateSystem _coordinateSystem;
        /// <summary>
        /// Get/Set LCS
        /// 
        /// If no LCS exists on Edge (i.e. when an Edge was deserialized from path) an LCS will be reconstructed.
        /// </summary>
        [XmlIgnore]
        public CoordinateSystem CoordinateSystem
        {
            get
            {
                if (this._coordinateSystem != null)
                {
                    return this._coordinateSystem;
                }
                else
                {
                    Point3d origin;
                    Vector3d localX, localY, localZ;

                    // arc1
                    if (this.Type == "arc" && this.Points.Count == 1)
                    {
                        // sweep angle
                        double sweepAngle = this.EndAngle - this.StartAngle;

                        // find p0, p1 and p2
                        Point3d p0, p1, p2;
                        Vector3d v = this.XAxis.Scale(this.Radius);
                        p0 = this.Points[0].Translate(v);
                        p1 = this.Points[0].Translate(v.RotateAroundAxis(sweepAngle / 2, this.Normal));
                        p2 = this.Points[0].Translate(v.RotateAroundAxis(sweepAngle, this.Normal));

                        origin = p1;
                        localX = new Vector3d(p0, p2).Normalize();
                        localZ = this.Normal;
                        localY = localZ.Cross(localX);
                        return new CoordinateSystem(origin, localX, localY, localZ);
                    }

                    // arc2
                    else if (this.Type == "arc" && this.Points.Count == 3)
                    {
                        origin = this.Points[1];
                        localX = new Vector3d(this.Points[0], this.Points[2]).Normalize();
                        Vector3d v = new Vector3d(this.Points[0], this.Points[1]).Normalize();
                        localZ = v.Cross(localX);
                        localY = localZ.Cross(localX);
                        return new CoordinateSystem(origin, localX, localY, localZ);
                    }

                    // line
                    else if (this.Type == "line" && this.Points.Count == 2)
                    {
                        Vector3d v = new Vector3d(this.Points[0], this.Points[1]);
                        origin = this.Points[0].Translate(v.Scale(0.5));
                        localX = v.Normalize();
                        localY = this.Normal;
                        localZ = localX.Cross(localY);
                        return new CoordinateSystem(origin, localX, localY, localZ);
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
        public List<Geometry.Point3d> Points = new List<Geometry.Point3d>(); // sequence: point_type_3d // ordered internal points, or the center of the circle/arc
        [XmlElement("normal", Order = 2)]
        public Vector3d Normal { get; set; } // point_type_3d // normal of the curve; it must be used if the curve is arc or circle.
        [XmlElement("x_axis", Order = 3)]
        public Geometry.Vector3d XAxis { get; set; } // point_type_3d // axis of base line (the value default is the x axis {1, 0, 0}) angles are measured from this direction.
        [XmlElement("edge_connection", Order = 4)]
        public Shells.EdgeConnection EdgeConnection { get; set; } // optional. ec_type.
        [XmlAttribute("type")]
        public string _type; // edgetype
        [XmlIgnore]
        public string Type
        {
            get { return this._type; }
            set { this._type = RestrictedString.EdgeType(value); }
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
                    return (new Vector3d(this.Points[0], this.Points[1]).Length());
                }
                else if (this.Type == "arc")
                {
                    if (this.Points.Count == 3)
                    {
                        // https://github.com/strusoft/femdesign-api/issues/123#issuecomment-1205322334
                        var v1 = Points[0] - Points[1];
                        var v2 = Points[2] - Points[1];
                        var normal = v1.Cross(v2);
                        var m1 = Points[1] + 0.5 * v1;
                        var m2 = Points[1] + 0.5 * v2;
                        var N1 = normal.Cross(v1);
                        var N2 = normal.Cross(v2);

                        var (t1, t2) = Proximity.LineLineProximity(m1, N1, m2, N2);

                        var centre = m1 + (t1 * N1);

                        var radius = (Points[0] - centre).Length();
                        var r1 = (Points[0] - centre).Normalize();
                        var r2 = (Points[2] - centre).Normalize();
                        var angle = Math.Acos(r1.Dot(r2));
                        return angle * radius;
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
        public Edge(double radius, double startAngle, double endAngle, Geometry.Point3d centerPoint, Geometry.Vector3d xAxis, Geometry.CoordinateSystem coordinateSystem)
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
        public Edge(Geometry.Point3d _startPoint, Geometry.Point3d _midPoint, Geometry.Point3d _endPoint, Geometry.CoordinateSystem _coordinateSystem)
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
        public Edge(double _radius, Geometry.Point3d _centerPoint, Geometry.CoordinateSystem _coordinateSystem)
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
        public Edge(Geometry.Point3d _startPoint, Geometry.Point3d _endPoint, Geometry.CoordinateSystem _coordinateSystem)
        {
            this.Type = "line";
            this.Points.Add(_startPoint);
            this.Points.Add(_endPoint);
            this.Normal = _coordinateSystem.LocalY;
            this.XAxis = _coordinateSystem.LocalX;
            this.CoordinateSystem = _coordinateSystem;
        }

        /// <summary>
        /// Construct Edge of line type by points and normal (localY).
        /// </summary>
        public Edge(Point3d startPoint, Point3d endPoint, Vector3d localY = null)
        {
            this.Type = "line";
            this.Points.Add(startPoint);
            this.Points.Add(endPoint);
			if (!this.IsLineVertical())
			{
                this.Normal = localY ?? -(endPoint - startPoint).Cross(Vector3d.UnitZ);
			}
			else
			{
                this.Normal = localY ?? Vector3d.UnitX;
            }
            this.XAxis = this.CoordinateSystem.LocalX;
        }

        /// <summary>
        /// Check if line
        /// </summary>
        public bool IsLine()
        {
            return (this.Type == "line");
        }

        /// <summary>
        /// Check if line is vertical (i.e. parallel to global Z)
        /// </summary>
        public bool IsLineVertical()
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
        public bool IsLineTangentEqualToGlobalZ()
        {
            if (this.IsLine())
            {
                return (this.CoordinateSystem.LocalX.Equals(Geometry.Vector3d.UnitZ, Tolerance.Vector3d));
            }
            else
            {
                throw new System.ArgumentException($"Edge type: {this.Type}, is not line.");
            }

        }

        /// <summary>
        /// Reverse this edge
        /// </summary>
        public void Reverse()
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

        /// <summary>
        /// Convert an Edge to a LineSegment
        /// </summary>
        /// <param name="edge"></param>
        public static explicit operator LineSegment(Edge edge)
        {
            if (!edge.IsLine())
                throw new ArgumentException("Edge is not a line");

            return new LineSegment(edge.Points[0], edge.Points[1]);
        }
    }
}