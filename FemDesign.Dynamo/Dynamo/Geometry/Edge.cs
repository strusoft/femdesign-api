
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Geometry
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Edge
    {
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
        /// Used for bar definition.
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
            Point3d centerPoint = Point3d.FromDynamo(obj.CenterPoint);
            Vector3d xAxis = new Vector3d(centerPoint, Point3d.FromDynamo(obj.StartPoint)).Normalize();

            // lcs
            CoordinateSystem cs = CoordinateSystem.FromDynamoCurve(obj);

            // return
            return new Geometry.Edge(radius, startAngle, endAngle, centerPoint, xAxis, cs);
        }

        /// <summary>
        /// Create Edge (Arc2) from Dynamo Arc.
        /// Used for bar definition.
        /// </summary>
        public static Geometry.Edge FromDynamoArc2(Autodesk.DesignScript.Geometry.Arc obj)
        {
           Point3d p0 = Point3d.FromDynamo(obj.StartPoint);
           Point3d p1 = Point3d.FromDynamo(obj.PointAtParameter(0.5));
           Point3d p2 = Point3d.FromDynamo(obj.EndPoint);

            // lcs
            CoordinateSystem cs = CoordinateSystem.FromDynamoCurve(obj); 

           // return
           return new Geometry.Edge(p0, p1, p2, cs);
        }

        /// <summary>
        /// Create Edge (Circle) from Dynamo Circle.
        /// </summary>
        public static Geometry.Edge FromDynamoCircle(Autodesk.DesignScript.Geometry.Circle obj)
        {
            double radius = obj.Radius;
            Point3d centerPoint = Point3d.FromDynamo(obj.CenterPoint);

            // lcs
            CoordinateSystem cs = CoordinateSystem.FromDynamoCurve(obj);

            // return
            return new Geometry.Edge(radius, centerPoint, cs);
        }

        /// <summary>
        /// Create Edge (Line) from Dynamo Line.
        /// </summary>
        public static Geometry.Edge FromDynamoLine(Autodesk.DesignScript.Geometry.Line obj)
        {
            Point3d startPoint = Point3d.FromDynamo(obj.StartPoint);
            Point3d endPoint = Point3d.FromDynamo(obj.EndPoint);

            // lcs
            CoordinateSystem cs = CoordinateSystem.FromDynamoCurve(obj);

            // return
            return new Geometry.Edge(startPoint, endPoint, cs);
        }

        /// <summary>
        /// Create Edge (Line) from Dynamo NurbsCurve.
        /// </summary>
        public static Geometry.Edge FromDynamoLinearNurbsCurve(Autodesk.DesignScript.Geometry.NurbsCurve obj)
        {
            Point3d startPoint = Point3d.FromDynamo(obj.StartPoint);
            Point3d endPoint = Point3d.FromDynamo(obj.EndPoint);

            // lcs
            CoordinateSystem cs = CoordinateSystem.FromDynamoCurve(obj);

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
            if (Math.Abs(dist0 - obj.Length) < Tolerance.LengthComparison)
            {
                return Edge.FromDynamoLinearNurbsCurve(obj);
            }

            // check if NurbsCurve is a Circle
            else if (obj.IsClosed && Math.Abs(dist1 * Math.PI - obj.Length) < Tolerance.LengthComparison)
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
            if (this.Type == "arc")
            {
                return Edge.ToDynamoArc(this);
            }
            else if (this.Type == "circle")
            {
                return Edge.ToDynamoCircle(this);
            }
            else if (this.Type == "line")
            {
                return Edge.ToDynamoLine(this);
            }
            else
            {
                throw new System.ArgumentException($"Edge type: {this.Type}, is not supported for conversion to a Dynamo Curve");
            }
        }

        /// <summary>
        /// Create Dynamo Arc from Edge (Arc).
        /// </summary>
        public static Autodesk.DesignScript.Geometry.Arc ToDynamoArc(Geometry.Edge obj)
        {
            if (obj.Points.Count == 3)
            {
                return Autodesk.DesignScript.Geometry.Arc.ByThreePoints(obj.Points[0].ToDynamo(), obj.Points[1].ToDynamo(), obj.Points[2].ToDynamo());
            }
            else
            {
                Autodesk.DesignScript.Geometry.Point p0 = obj.Points[0].Translate(obj.XAxis.Scale(obj.Radius)).ToDynamo();
                return Autodesk.DesignScript.Geometry.Arc.ByCenterPointStartPointSweepAngle(obj.Points[0].ToDynamo(), p0, Degree.FromRadians(obj.EndAngle - obj.StartAngle), obj.Normal.ToDynamo());
            }
        }

        /// <summary>
        /// Create Dynamo Circle from Edge (Circle).
        /// </summary>
        public static Autodesk.DesignScript.Geometry.Circle ToDynamoCircle(Geometry.Edge obj)
        {
            return Autodesk.DesignScript.Geometry.Circle.ByCenterPointRadiusNormal(obj.Points[0].ToDynamo(), obj.Radius, obj.Normal.ToDynamo());
        }

        /// <summary>
        /// Create Dynamo Line from Edge (Line).
        /// </summary>
        public static Autodesk.DesignScript.Geometry.Line ToDynamoLine(Geometry.Edge obj)
        {
            return Autodesk.DesignScript.Geometry.Line.ByStartPointEndPoint(obj.Points[0].ToDynamo(), obj.Points[1].ToDynamo());
        }      

        #endregion
    }
}