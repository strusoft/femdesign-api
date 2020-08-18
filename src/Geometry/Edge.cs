// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Geometry
{
    /// <summary>
    /// edge_type
    /// 
    /// Curves in FEM-Design are expressed as edges. This extended edge also contains a LCS to keep track of directions.
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
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
        public FdCoordinateSystem coordinateSystem
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
                    if (this.type == "arc" && this.points.Count == 1)
                    {
                            // not implemented. only use for bars is intended at this point.
                            throw new System.ArgumentException("Could not reconstruct FdCoordinateSystem from Edge of type Arc1.");
                    }

                    // arc2
                    if (this.type == "arc" && this.points.Count == 3)
                    {
                        origin = this.points[1];
                        localX = new FdVector3d(this.points[0], this.points[2]).Normalize();
                        localZ = this.normal;
                        localY = localX.Cross(localZ);
                        return new FdCoordinateSystem(origin, localX, localY, localZ);
                    }

                    // line
                    else if (this.type == "line" && this.points.Count == 2)
                    {
                        FdVector3d v = new FdVector3d(this.points[0], this.points[1]);
                        origin = new FdPoint3d(v.x/2, v.y/2, v.z/2);
                        localX = v.Normalize();
                        localY = this.normal;
                        localZ = localX.Cross(localY);
                        return new FdCoordinateSystem(origin, localX, localY, localZ);
                    }

                    // else
                    else
                    {
                        throw new System.ArgumentException($"Could not reconstruct FdCoordinateSystem from Edge of type: {this.type}");
                    }
                }
            }
            set { this._coordinateSystem = value; }
        }
        [XmlElement("point", Order = 1)]
        public List<Geometry.FdPoint3d> points = new List<Geometry.FdPoint3d>(); // sequence: point_type_3d // ordered internal points, or the center of the circle/arc
        [XmlElement("normal", Order = 2)]
        public FdVector3d normal { get; set; } // point_type_3d // normal of the curve; it must be used if the curve is arc or circle.
        [XmlElement("x_axis", Order = 3)]
        public Geometry.FdVector3d xAxis { get; set; } // point_type_3d // axis of base line (the value default is the x axis {1, 0, 0}) angles are measured from this direction.
        [XmlElement("edge_connection", Order = 4)]
        public Shells.ShellEdgeConnection edgeConnection { get; set; } // optional. ec_type.
        [XmlAttribute("type")]
        public string _type; // edgetype
        [XmlIgnore]
        public string type
        {
            get {return this._type;}
            set {this._type = RestrictedString.EdgeType(value);}
        }
        [XmlAttribute("radius")]
        public double radius { get; set; }   // optional. double
        [XmlAttribute("start_angle")]
        public double startAngle { get; set; } // optional. double
        [XmlAttribute("end_angle")]
        public double endAngle { get; set; } // optional. double

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal Edge()
        {

        }

        /// <summary>
        /// Construct Edge of arc1 type.
        /// </summary>
        internal Edge(double _radius, double _startAngle, double _endAngle, Geometry.FdPoint3d _centerPoint, Geometry.FdVector3d _xAxis, Geometry.FdCoordinateSystem _coordinateSystem)
        {
            this.type = "arc";
            this.radius = _radius;
            this.startAngle = _startAngle;
            this.endAngle = _endAngle;
            this.points.Add(_centerPoint);
            this.normal = _coordinateSystem.localZ;
            this.xAxis = _xAxis;
            this.coordinateSystem = _coordinateSystem;
        }

        /// <summary>
        /// Construct Edge of arc2 type.
        /// </summary>
        internal Edge(Geometry.FdPoint3d _startPoint, Geometry.FdPoint3d _midPoint, Geometry.FdPoint3d _endPoint, Geometry.FdCoordinateSystem _coordinateSystem)
        {
            this.type = "arc";
            this.points.Add(_startPoint);
            this.points.Add(_midPoint);
            this.points.Add(_endPoint);
            this.coordinateSystem = _coordinateSystem;
        }

        /// <summary>
        /// Construct Edge of circle type.
        /// </summary>
        internal Edge(double _radius, Geometry.FdPoint3d _centerPoint, Geometry.FdCoordinateSystem _coordinateSystem)
        {
            this.type = "circle";
            this.radius = _radius;
            this.points.Add(_centerPoint);
            this.normal = _coordinateSystem.localZ;
            this.coordinateSystem = _coordinateSystem;
        }
        /// <summary>
        /// Construct Edge of line type by points and coordinate system.
        /// </summary>
        internal Edge(Geometry.FdPoint3d _startPoint, Geometry.FdPoint3d _endPoint, Geometry.FdCoordinateSystem _coordinateSystem)
        {
            this.type = "line";
            this.points.Add(_startPoint);
            this.points.Add(_endPoint);
            this.normal = _coordinateSystem.localY;
            this.coordinateSystem = _coordinateSystem;
        }

        /// <summary>
        /// Construct Edge of line type by points and normal (localY).
        /// </summary>
        internal Edge(FdPoint3d startPoint, FdPoint3d endPoint, FdVector3d localY)
        {
            this.type = "line";
            this.points.Add(startPoint);
            this.points.Add(endPoint);
            this.normal = localY;
        }

        internal bool IsLine()
        {
            return (this.type == "line");
        }

        internal bool IsLineVertical()
        {
            if (this.IsLine())
            {
                return (this.points[0].x == this.points[1].x && this.points[0].y == this.points[1].y);
            }
            else
            {
                throw new System.ArgumentException($"Edge type: {this.type}, is not line.");
            }
        }

        #region dynamo
        /// <summary>
        /// Convert a Dynamo Curve to Edge.
        /// </summary>
        /// <param name="obj"></param>
        public static Geometry.Edge FromDynamo(Autodesk.DesignScript.Geometry.Curve obj)
        {
            // if polyline or similar.
            Autodesk.DesignScript.Geometry.Geometry[] items = obj.Explode();
            if (items.Length != 1)
            {
               throw new System.ArgumentException("Exploded Curve should only have one item."); 
            }
            Autodesk.DesignScript.Geometry.Geometry item = items[0];

            // if Arc
            if (item.GetType() == typeof(Autodesk.DesignScript.Geometry.Arc))
            {
                // output is a general purpose Edge
                return Edge.FromDynamoArc1((Autodesk.DesignScript.Geometry.Arc)item);
            }

            // if Circle
            else if (item.GetType() == typeof(Autodesk.DesignScript.Geometry.Circle))
            {
                return Edge.FromDynamoCircle((Autodesk.DesignScript.Geometry.Circle)item);
            }

            // if Line
            else if (item.GetType() == typeof(Autodesk.DesignScript.Geometry.Line))
            {
                return Edge.FromDynamoLine((Autodesk.DesignScript.Geometry.Line)item);
            }

            // if NurbsCurve
            else if (item.GetType() == typeof(Autodesk.DesignScript.Geometry.NurbsCurve))
            {
                return Edge.FromDynamoNurbsCurve((Autodesk.DesignScript.Geometry.NurbsCurve)item);
            }

            // else
            else
            {
                throw new System.ArgumentException($"Curve type: {obj.GetType()}, is not supported for conversion to an Edge.");
            }
        }

        /// <summary>
        /// Create Edge (Line or Arc1) from Dynamo Line or Arc.
        /// </summary>
        public static Geometry.Edge FromDynamoLineOrArc1(Autodesk.DesignScript.Geometry.Curve obj)
        {
            // if polyline or similar.
            Autodesk.DesignScript.Geometry.Geometry[] items = obj.Explode();
            if (items.Length != 1)
            {
               throw new System.ArgumentException("Exploded Curve should only have one item."); 
            }
            Autodesk.DesignScript.Geometry.Geometry item = items[0];

            // if Arc
            if (item.GetType() == typeof(Autodesk.DesignScript.Geometry.Arc))
            {           
                return Edge.FromDynamoArc1((Autodesk.DesignScript.Geometry.Arc)item);
            }

            // if Line
            else if (item.GetType() == typeof(Autodesk.DesignScript.Geometry.Line))
            {
                return Edge.FromDynamoLine((Autodesk.DesignScript.Geometry.Line)item);
            }

            else
            {
                throw new System.ArgumentException($"Curve type: {obj.GetType()}, is not Line or Arc.");
            }
        }

        /// <summary>
        /// Create Edge (Line or Arc2) from Dynamo Line or Arc.
        /// </summary>
        public static Geometry.Edge FromDynamoLineOrArc2(Autodesk.DesignScript.Geometry.Curve obj)
        {
            // if polyline or similar.
            Autodesk.DesignScript.Geometry.Geometry[] items = obj.Explode();
            if (items.Length != 1)
            {
               throw new System.ArgumentException("Exploded Curve should only have one item."); 
            }
            Autodesk.DesignScript.Geometry.Geometry item = items[0];

            // if Arc
            if (item.GetType() == typeof(Autodesk.DesignScript.Geometry.Arc))
            {           
                return Edge.FromDynamoArc2((Autodesk.DesignScript.Geometry.Arc)item);
            }

            // if Line
            else if (item.GetType() == typeof(Autodesk.DesignScript.Geometry.Line))
            {
                return Edge.FromDynamoLine((Autodesk.DesignScript.Geometry.Line)item);
            }

            else
            {
                throw new System.ArgumentException($"Curve type: {obj.GetType()}, is not Line or Arc.");
            }
        }

        /// <summary>
        /// Create Edge (Arc1) from Dynamo Arc.
        /// </summary>
        public static Geometry.Edge FromDynamoArc1(Autodesk.DesignScript.Geometry.Arc obj)
        {
            double radius = obj.Radius;
            double startAngle = 0;
            double endAngle = startAngle + Degree.ToRadians(obj.SweepAngle);
            FdPoint3d centerPoint = FdPoint3d.FromDynamo(obj.CenterPoint);
            FdVector3d xAxis = new FdVector3d(centerPoint, FdPoint3d.FromDynamo(obj.StartPoint)).Normalize();

            // lcs
            FdCoordinateSystem cs = FdCoordinateSystem.FromDynamoCurve(obj);

            // return
            return new Geometry.Edge(radius, startAngle, endAngle, centerPoint, xAxis, cs);
        }

        /// <summary>
        /// Create Edge (Arc2) from Dynamo Arc.
        /// </summary>
        public static Geometry.Edge FromDynamoArc2(Autodesk.DesignScript.Geometry.Arc obj)
        {
           FdPoint3d p0 = FdPoint3d.FromDynamo(obj.StartPoint);
           FdPoint3d p1 = FdPoint3d.FromDynamo(obj.PointAtParameter(0.5));
           FdPoint3d p2 = FdPoint3d.FromDynamo(obj.EndPoint);

            // lcs
            FdCoordinateSystem cs = FdCoordinateSystem.FromDynamoCurve(obj); 

           // return
           return new Geometry.Edge(p0, p1, p2, cs);
        }

        /// <summary>
        /// Create Edge (Circle) from Dynamo Circle.
        /// </summary>
        public static Geometry.Edge FromDynamoCircle(Autodesk.DesignScript.Geometry.Circle obj)
        {
            double radius = obj.Radius;
            FdPoint3d centerPoint = FdPoint3d.FromDynamo(obj.CenterPoint);

            // lcs
            FdCoordinateSystem cs = FdCoordinateSystem.FromDynamoCurve(obj);

            // return
            return new Geometry.Edge(radius, centerPoint, cs);
        }

        /// <summary>
        /// Create Edge (Line) from Dynamo Line.
        /// </summary>
        public static Geometry.Edge FromDynamoLine(Autodesk.DesignScript.Geometry.Line obj)
        {
            FdPoint3d startPoint = FdPoint3d.FromDynamo(obj.StartPoint);
            FdPoint3d endPoint = FdPoint3d.FromDynamo(obj.EndPoint);

            // lcs
            FdCoordinateSystem cs = FdCoordinateSystem.FromDynamoCurve(obj);

            // return
            return new Geometry.Edge(startPoint, endPoint, cs);
        }

        /// <summary>
        /// Create Edge (Line) from Dynamo NurbsCurve.
        /// </summary>
        public static Geometry.Edge FromDynamoLinearNurbsCurve(Autodesk.DesignScript.Geometry.NurbsCurve obj)
        {
            FdPoint3d startPoint = FdPoint3d.FromDynamo(obj.StartPoint);
            FdPoint3d endPoint = FdPoint3d.FromDynamo(obj.EndPoint);

            // lcs
            FdCoordinateSystem cs = FdCoordinateSystem.FromDynamoCurve(obj);

            // return
            return new Geometry.Edge(startPoint, endPoint, cs);
        }

        /// <summary>
        /// Create Edge (Line or Circle or Arc) from Dynamo NurbsCurve.
        /// </summary>
        public static Geometry.Edge FromDynamoNurbsCurve(Autodesk.DesignScript.Geometry.NurbsCurve obj)
        {
            // points on curve
            Autodesk.DesignScript.Geometry.Point startPoint, midPoint, endPoint;
            startPoint = obj.StartPoint;
            midPoint = obj.PointAtParameter(0.5);
            endPoint = obj.EndPoint;

            // distances to compare with curve length.
            double dist0 = Autodesk.DesignScript.Geometry.Vector.ByTwoPoints(startPoint, endPoint).Length;
            double dist1 = Autodesk.DesignScript.Geometry.Vector.ByTwoPoints(startPoint, midPoint).Length;

            // check if NurbsCurve is a Line
            if (Math.Abs(dist0 - obj.Length) < Tolerance.lengthComparison)
            {
                return Edge.FromDynamoLinearNurbsCurve(obj);
            }

            // check if NurbsCurve is a Circle
            else if (obj.IsClosed && Math.Abs(dist1 * Math.PI - obj.Length) < Tolerance.lengthComparison)
            {
                Autodesk.DesignScript.Geometry.Point p0, p1, p2;
                p0 = obj.PointAtParameter(0);
                p1 = obj.PointAtParameter(0.333);
                p2 = obj.PointAtParameter(0.667);
                Autodesk.DesignScript.Geometry.Circle circle = Autodesk.DesignScript.Geometry.Circle.ByThreePoints(p0, p1, p2);
                return  Edge.FromDynamoCircle(circle);
            }

            // if NurbsCurve is not a Line or a Circle.
            else
            {
                // See if it can be cast to an Arc by three points
                try
                {
                    Autodesk.DesignScript.Geometry.Arc arc =  Autodesk.DesignScript.Geometry.Arc.ByThreePoints(startPoint, midPoint, endPoint);
                    return Edge.FromDynamoArc1(arc);
                }

                // If casting was not successful the NurbsCurve is not a Line, a Circle or an Arc
                catch
                {      
                    throw new System.ArgumentException("NurbsCurve is not a Line, a Circle or an Arc. Unable to convert NurbsCurve to an Edge.");
                }
            }
        }

        /// <summary>
        /// Create Dynamo Curve from Edge (any).
        /// </summary>
        public Autodesk.DesignScript.Geometry.Curve ToDynamo()
        {
            if (this.type == "arc")
            {
                return Edge.ToDynamoArc(this);
            }
            else if (this.type == "circle")
            {
                return Edge.ToDynamoCircle(this);
            }
            else if (this.type == "line")
            {
                return Edge.ToDynamoLine(this);
            }
            else
            {
                throw new System.ArgumentException($"Edge type: {this.type}, is not supported for conversion to a Dynamo Curve");
            }
        }

        /// <summary>
        /// Create Dynamo Arc from Edge (Arc).
        /// </summary>
        public static Autodesk.DesignScript.Geometry.Arc ToDynamoArc(Geometry.Edge obj)
        {
            if (obj.points.Count == 3)
            {
                return Autodesk.DesignScript.Geometry.Arc.ByThreePoints(obj.points[0].ToDynamo(), obj.points[1].ToDynamo(), obj.points[2].ToDynamo());
            }
            else
            {
                Autodesk.DesignScript.Geometry.Point p0 = obj.points[0].Translate(obj.xAxis.Scale(obj.radius)).ToDynamo();
                return Autodesk.DesignScript.Geometry.Arc.ByCenterPointStartPointSweepAngle(obj.points[0].ToDynamo(), p0, Degree.FromRadians(obj.endAngle - obj.startAngle), obj.normal.ToDynamo());
            }
        }

        /// <summary>
        /// Create Dynamo Circle from Edge (Circle).
        /// </summary>
        public static Autodesk.DesignScript.Geometry.Circle ToDynamoCircle(Geometry.Edge obj)
        {
            return Autodesk.DesignScript.Geometry.Circle.ByCenterPointRadiusNormal(obj.points[0].ToDynamo(), obj.radius, obj.normal.ToDynamo());
        }

        /// <summary>
        /// Create Dynamo Line from Edge (Line).
        /// </summary>
        public static Autodesk.DesignScript.Geometry.Line ToDynamoLine(Geometry.Edge obj)
        {
            return Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(obj.points[0].ToDynamo(), obj.points[1].ToDynamo());
        }      

        #endregion

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
            if (obj.GetLength() < FemDesign.Tolerance.point3d)
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
            if (obj.GetLength() < FemDesign.Tolerance.point3d)
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
        /// </summary>
        public static Geometry.Edge FromRhinoLineOrArc2(Rhino.Geometry.Curve obj)
        {
            // check length
            if (obj.GetLength() < FemDesign.Tolerance.point3d)
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
            if (this.type == "arc")
            {
                return Edge.ToRhinoArcCurve(this);
            }
            else if (this.type == "circle")
            {
                return Edge.ToRhinoArcCurveFromCircle(this);
            }
            else if (this.type == "line")
            {
                return Edge.ToRhinoLineCurve(this);
            }
            else
            {
                throw new System.ArgumentException($"Edge type: {this.type}, is not supported for conversion to a Rhino Curve");
            }
        }

        /// <summary>
        /// Create Rhino open ArcCurve from Edge(Arc).
        /// </summary>
        public static Rhino.Geometry.ArcCurve ToRhinoArcCurve(Geometry.Edge obj)
        {
            if (obj.points.Count == 3)
            {
                Rhino.Geometry.Arc arc = new Rhino.Geometry.Arc(obj.points[0].ToRhino(), obj.points[1].ToRhino(), obj.points[2].ToRhino());
                return new Rhino.Geometry.ArcCurve(arc);
            }

            else
            {
                Rhino.Geometry.Interval interval = new Rhino.Geometry.Interval(obj.startAngle, obj.endAngle);
                Rhino.Geometry.Plane plane = new Rhino.Geometry.Plane(obj.points[0].ToRhino(), obj.normal.ToRhino());
                Rhino.Geometry.Circle circle = new Rhino.Geometry.Circle(plane, obj.radius);
                Rhino.Geometry.Arc arc = new Rhino.Geometry.Arc(circle, interval);
                return new Rhino.Geometry.ArcCurve(arc);
            }
        }

        /// <summary>
        /// Create Rhino closed ArcCurve from Edge(Circle).
        /// </summary>
        public static Rhino.Geometry.ArcCurve ToRhinoArcCurveFromCircle(Geometry.Edge obj)
        {
            Rhino.Geometry.Plane plane = new Rhino.Geometry.Plane(obj.points[0].ToRhino(), obj.normal.ToRhino());
            Rhino.Geometry.Circle circle = new Rhino.Geometry.Circle(plane, obj.radius);
            return new Rhino.Geometry.ArcCurve(circle);
        }

        /// <summary>
        /// Create Rhino LineCurve from Edge(Line).
        /// </summary>
        public static Rhino.Geometry.LineCurve ToRhinoLineCurve(Geometry.Edge obj)
        {
            return new Rhino.Geometry.LineCurve(obj.points[0].ToRhino(), obj.points[1].ToRhino());
        }

        #endregion
    }
}