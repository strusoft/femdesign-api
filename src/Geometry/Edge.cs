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
    public class Edge
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

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal Edge()
        {

        }

        /// <summary>
        /// Construct Edge of arc1 type.
        /// </summary>
        internal Edge(double radius, double startAngle, double endAngle, Geometry.FdPoint3d centerPoint, Geometry.FdVector3d xAxis, Geometry.FdCoordinateSystem coordinateSystem)
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
        internal Edge(Geometry.FdPoint3d _startPoint, Geometry.FdPoint3d _midPoint, Geometry.FdPoint3d _endPoint, Geometry.FdCoordinateSystem _coordinateSystem)
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
        internal Edge(double _radius, Geometry.FdPoint3d _centerPoint, Geometry.FdCoordinateSystem _coordinateSystem)
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
        internal Edge(Geometry.FdPoint3d _startPoint, Geometry.FdPoint3d _endPoint, Geometry.FdCoordinateSystem _coordinateSystem)
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
        internal Edge(FdPoint3d startPoint, FdPoint3d endPoint, FdVector3d localY)
        {
            this.Type = "line";
            this.Points.Add(startPoint);
            this.Points.Add(endPoint);
            this.Normal = localY;
        }

        internal bool IsLine()
        {
            return (this.Type == "line");
        }

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


        #region grasshopper
        /// <summary>
        /// Convert a Rhino Curve to one or several Edges.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<Geometry.Edge> FromRhinoBrep(Rhino.Geometry.Curve obj)
        {
            // initate list
            List<Geometry.Edge> edges = new List<Edge>();

            // if curve must be represented by a collection of curves
            // if polyline
            if (obj.IsPolyline())
            {
                obj.TryGetPolyline(out Rhino.Geometry.Polyline poly);
                foreach (Rhino.Geometry.Line line in poly.GetSegments())
                {
                    edges.Add(Geometry.Edge.FromRhinoLineCurve(new Rhino.Geometry.LineCurve(line)));
                }
                return edges;
            }

            // if nurbscurve of degree > 2
            // else if (obj.GetType() == typeof(Rhino.Geometry.NurbsCurve) && obj.Degree > 2)
            // {
            //     if (obj.IsLinear() || obj.IsArc() || obj.IsCircle() || !obj.IsPlanar())
            //     {
            //         // pass
            //     }
            //     else
            //     {
            //         // pass
            //         // add method to translate nurbscurve to arcs and lines.
            //     }
            // }
            
            // else if curve can be represented by a single curve
            else
            {
                edges.Add(Geometry.Edge.FromRhino(obj));
                return edges;
            }
        }


        /// <summary>
        /// Convert a Rhino Curve to Edge
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="arcType1">True if output is general purpose Edge. False if output is BarPart Edge.</param>
        /// <returns></returns>
        public static Geometry.Edge FromRhino(Rhino.Geometry.Curve obj)
        {
            // check length
            if (obj.GetLength() < FemDesign.Tolerance.Point3d)
            {
                throw new System.ArgumentException("Curve has no length.");
            }

            // if ArcCurve
            if (obj.GetType() == typeof(Rhino.Geometry.ArcCurve))
            {
                Rhino.Geometry.ArcCurve arcCurve = (Rhino.Geometry.ArcCurve)obj;

                // if Arc
                if (!obj.IsClosed)
                {
                    return Edge.FromRhinoArc1(arcCurve);
                }

                // if Circle
                else
                {
                    return Edge.FromRhinoCircle(arcCurve);
                }
            }

            // if LineCurve
            else if (obj.GetType() == typeof(Rhino.Geometry.LineCurve))
            {
                return Edge.FromRhinoLineCurve((Rhino.Geometry.LineCurve)obj);
            }

            // if NurbsCurve
            else if (obj.GetType() == typeof(Rhino.Geometry.NurbsCurve))
            {
                return Edge.FromRhinoNurbsCurve((Rhino.Geometry.NurbsCurve)obj);
            }

            // else
            else
            {
                throw new System.ArgumentException($"Curve type: {obj.GetType()}, is not supported for conversion to an Edge.");
            }
        }

        /// <summary>
        /// Create Edge (Line or Arc1) from Rhino LineCurve or open ArcCurve.
        /// </summary>
        public static Geometry.Edge FromRhinoLineOrArc1(Rhino.Geometry.Curve obj)
        {
            // check length
            if (obj.GetLength() < FemDesign.Tolerance.Point3d)
            {
                throw new System.ArgumentException("Curve has no length.");
            }

            // if ArcCurve
            if (obj.GetType() == typeof(Rhino.Geometry.ArcCurve))
            {
                Rhino.Geometry.ArcCurve arcCurve = (Rhino.Geometry.ArcCurve)obj;

                // if Arc
                if (!obj.IsClosed)
                {
                    return Edge.FromRhinoArc1(arcCurve);
                }

                else
                {
                    throw new System.ArgumentException($"Curve type: {obj.GetType()}, is not Line or Arc.");
                }
            }

            // if LineCurve
            else if (obj.GetType() == typeof(Rhino.Geometry.LineCurve))
            {
                return Edge.FromRhinoLineCurve((Rhino.Geometry.LineCurve)obj);
            }

            // if PolylineCurve
            else if (obj.GetType() == typeof(Rhino.Geometry.PolylineCurve))
            {
                if (obj.SpanCount == 1)
                {
                    Rhino.Geometry.LineCurve lnCrv = new Rhino.Geometry.LineCurve(obj.PointAtStart, obj.PointAtEnd);
                    return Edge.FromRhinoLineCurve(lnCrv);
                }
                else
                {
                    throw new System.ArgumentException($"PolylineCurve with SpanCount: {obj.SpanCount}, is not supported for conversion to an Edge.");
                }
            }

            else
            {
                throw new System.ArgumentException($"Curve type: {obj.GetType()}, is not Line or Arc.");
            }
        }

        /// <summary>
        /// Create Edge (Line or Arc2) from Rhino LineCurve or open ArcCurve.
        /// Used for bar definition.
        /// </summary>
        public static Geometry.Edge FromRhinoLineOrArc2(Rhino.Geometry.Curve obj)
        {
            // check length
            if (obj.GetLength() < FemDesign.Tolerance.Point3d)
            {
                throw new System.ArgumentException("Curve has no length.");
            }

            // if ArcCurve
            if (obj.GetType() == typeof(Rhino.Geometry.ArcCurve))
            {
                Rhino.Geometry.ArcCurve arcCurve = (Rhino.Geometry.ArcCurve)obj;

                // if Arc
                if (!obj.IsClosed)
                {
                    return Edge.FromRhinoArc2(arcCurve);
                }

                else
                {
                    throw new System.ArgumentException($"Curve type: {obj.GetType()}, is not Line or Arc.");
                }
            }

            // if LineCurve
            else if (obj.GetType() == typeof(Rhino.Geometry.LineCurve))
            {
                return Edge.FromRhinoLineCurve((Rhino.Geometry.LineCurve)obj);
            }

            // if PolylineCurve
            else if (obj.GetType() == typeof(Rhino.Geometry.PolylineCurve))
            {
                if (obj.SpanCount == 1)
                {
                    Rhino.Geometry.LineCurve lnCrv = new Rhino.Geometry.LineCurve(obj.PointAtStart, obj.PointAtEnd);
                    return Edge.FromRhinoLineCurve(lnCrv);
                }
                else
                {
                    throw new System.ArgumentException($"PolylineCurve with SpanCount: {obj.SpanCount}, is not supported for conversion to an Edge.");
                }
            }

            else
            {
                throw new System.ArgumentException($"Curve type: {obj.GetType()}, is not Line or Arc.");
            }
        }

        /// <summary>
        /// Create Edge (Arc1) from Rhino open ArcCurve.
        /// </summary>
        public static Geometry.Edge FromRhinoArc1(Rhino.Geometry.ArcCurve obj)
        {
            double radius = obj.Arc.Radius;
            double startAngle = 0;
            double endAngle = obj.Arc.EndAngle - obj.Arc.StartAngle;
            FdPoint3d centerPoint = FdPoint3d.FromRhino(obj.Arc.Center);
            FdVector3d xAxis = new FdVector3d(centerPoint, FdPoint3d.FromRhino(obj.Arc.StartPoint));

            // lcs
            FdCoordinateSystem cs = FdCoordinateSystem.FromRhinoCurve(obj);

            // return
            return new Geometry.Edge(radius, startAngle, endAngle, centerPoint, xAxis, cs);
        }

        /// <summary>
        /// Create Edge (Arc2) from Rhino open ArcCurve.
        /// </summary>
        public static Geometry.Edge FromRhinoArc2(Rhino.Geometry.ArcCurve obj)
        {
            FdPoint3d startPoint = FdPoint3d.FromRhino(obj.Arc.StartPoint);
            FdPoint3d midPoint = FdPoint3d.FromRhino(obj.Arc.MidPoint);
            FdPoint3d endPoint = FdPoint3d.FromRhino(obj.Arc.EndPoint);

            // lcs
            FdCoordinateSystem cs = FdCoordinateSystem.FromRhinoCurve(obj);

            // return
            return new Geometry.Edge(startPoint, midPoint, endPoint, cs);
        }

        /// <summary>
        /// Create Edge (Circle) from Rhino closed ArcCurve.
        /// </summary>
        public static Geometry.Edge FromRhinoCircle(Rhino.Geometry.ArcCurve obj)
        {
            double radius = obj.Radius;
            FdPoint3d centerPoint = FdPoint3d.FromRhino(obj.Arc.Center);

            // lcs
            FdCoordinateSystem cs = FdCoordinateSystem.FromRhinoCurve(obj);

            // return
            return new Geometry.Edge(radius, centerPoint, cs);
        }

        /// <summary>
        /// Create Edge (Line) from Rhino LineCurve.
        /// </summary>
        public static Geometry.Edge FromRhinoLineCurve(Rhino.Geometry.LineCurve obj)
        {
            FdPoint3d startPoint = FdPoint3d.FromRhino(obj.PointAtStart);
            FdPoint3d endPoint = FdPoint3d.FromRhino(obj.PointAtEnd);

            // lcs
            FdCoordinateSystem cs = FdCoordinateSystem.FromRhinoCurve(obj);

            // return
            return new Geometry.Edge(startPoint, endPoint, cs);
        }

        /// <summary>
        /// Create Edge (Line) from Rhino linear NurbsCurve.
        /// </summary>
        public static Geometry.Edge FromRhinoLinearNurbsCurve(Rhino.Geometry.NurbsCurve obj)
        {
            FdPoint3d startPoint = FdPoint3d.FromRhino(obj.PointAtStart);
            FdPoint3d endPoint = FdPoint3d.FromRhino(obj.PointAtEnd);

            // lcs
            FdCoordinateSystem cs = FdCoordinateSystem.FromRhinoCurve(obj);

            // return
            return new Geometry.Edge(startPoint, endPoint, cs);
        }


        /// <summary>
        /// Create Edge (Line or Circle or Arc) from Rhino  NurbsCurve.
        /// </summary>
        public static Geometry.Edge FromRhinoNurbsCurve(Rhino.Geometry.NurbsCurve obj)
        {
            // check if NurbsCurve is a line
            if (obj.IsLinear())
            {
                return Edge.FromRhinoLinearNurbsCurve(obj);
            }

            // check if NurbsCurve is a circle
            else if (obj.IsArc() && obj.IsClosed)
            {
                obj.TryGetArc(out Rhino.Geometry.Arc _obj);
                return Edge.FromRhinoCircle(new Rhino.Geometry.ArcCurve(_obj));
            }

            // check if NurbsCurve is an arc
            else if ((obj.IsArc() || obj.Degree == 2) && !obj.IsClosed)
            {
                obj.TryGetArc(out Rhino.Geometry.Arc _obj);
                return Edge.FromRhinoArc1(new Rhino.Geometry.ArcCurve(_obj));
            }

            else
            {
                throw new System.ArgumentException($"Unsupported Curve. Degree of NurbsCurve is likely to high. Degree is: {obj.Degree}, versus a supported degree of 2.");
            }
        }

        /// <summary>
        /// Convert an Edge to a Rhino Curve.
        /// </summary>
        public Rhino.Geometry.Curve ToRhino()
        {
            if (this.Type == "arc")
            {
                return Edge.ToRhinoArcCurve(this);
            }
            else if (this.Type == "circle")
            {
                return Edge.ToRhinoArcCurveFromCircle(this);
            }
            else if (this.Type == "line")
            {
                return Edge.ToRhinoLineCurve(this);
            }
            else
            {
                throw new System.ArgumentException($"Edge type: {this.Type}, is not supported for conversion to a Rhino Curve");
            }
        }

        /// <summary>
        /// Create Rhino open ArcCurve from Edge(Arc).
        /// </summary>
        public static Rhino.Geometry.ArcCurve ToRhinoArcCurve(Geometry.Edge obj)
        {
            if (obj.Points.Count == 3)
            {
                Rhino.Geometry.Arc arc = new Rhino.Geometry.Arc(obj.Points[0].ToRhino(), obj.Points[1].ToRhino(), obj.Points[2].ToRhino());
                return new Rhino.Geometry.ArcCurve(arc);
            }

            else
            {
                Rhino.Geometry.Interval interval = new Rhino.Geometry.Interval(obj.StartAngle, obj.EndAngle);
                Rhino.Geometry.Plane plane = new Rhino.Geometry.Plane(obj.Points[0].ToRhino(), obj.XAxis.ToRhino(), obj.Normal.Cross(obj.XAxis).Normalize().ToRhino());
                Rhino.Geometry.Circle circle = new Rhino.Geometry.Circle(plane, obj.Radius);
                Rhino.Geometry.Arc arc = new Rhino.Geometry.Arc(circle, interval);
                return new Rhino.Geometry.ArcCurve(arc);
            }
        }

        /// <summary>
        /// Create Rhino closed ArcCurve from Edge(Circle).
        /// </summary>
        public static Rhino.Geometry.ArcCurve ToRhinoArcCurveFromCircle(Geometry.Edge obj)
        {
            Rhino.Geometry.Plane plane = new Rhino.Geometry.Plane(obj.Points[0].ToRhino(), obj.Normal.ToRhino());
            Rhino.Geometry.Circle circle = new Rhino.Geometry.Circle(plane, obj.Radius);
            return new Rhino.Geometry.ArcCurve(circle);
        }

        /// <summary>
        /// Create Rhino LineCurve from Edge(Line).
        /// </summary>
        public static Rhino.Geometry.LineCurve ToRhinoLineCurve(Geometry.Edge obj)
        {
            return new Rhino.Geometry.LineCurve(obj.Points[0].ToRhino(), obj.Points[1].ToRhino());
        }

        #endregion
    }
}