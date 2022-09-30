using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FemDesign.Geometry;
using FemDesign.Loads;
using FemDesign.Supports;

using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public static class Convert
    {

        #region Vector
        internal static FemDesign.Geometry.Point2d FromRhino(this Rhino.Geometry.Point2d point)
        {
            return new FemDesign.Geometry.Point2d(point.X, point.Y);
        }
        internal static Rhino.Geometry.Point2d ToRhino(this FemDesign.Geometry.Point2d point)
        {
            return new Rhino.Geometry.Point2d(point.X, point.Y);
        }
        internal static FemDesign.Geometry.Point3d FromRhino(this Rhino.Geometry.Point3d point)
        {
            return new FemDesign.Geometry.Point3d(point.X, point.Y, point.Z);
        }
        internal static Rhino.Geometry.Point3d ToRhino(this FemDesign.Geometry.Point3d point)
        {
            return new Rhino.Geometry.Point3d(point.X, point.Y, point.Z);
        } 
        internal static FemDesign.Geometry.Vector3d FromRhino(this Rhino.Geometry.Vector3d vector)
        {
            return new FemDesign.Geometry.Vector3d(vector.X, vector.Y, vector.Z);
        }

        internal static Rhino.Geometry.Vector3d ToRhino(this FemDesign.Geometry.Vector3d vector)
        {
            return new Rhino.Geometry.Vector3d(vector.X, vector.Y, vector.Z);
        }

        #endregion

        #region Bar
        /// <summary>
        /// Create Rhino curve from underlying Edge (Line or Arc) of Bar.
        /// </summary>
        internal static Rhino.Geometry.Curve GetRhinoCurve(this Bars.Bar bar)
        {
            return bar.BarPart._edge.ToRhino();
        }
        #endregion

        #region Cover
        /// <summary>
        /// Create Rhino brep from underlying Region of Cover.
        /// </summary>
        internal static Rhino.Geometry.Brep GetRhinoSurface(this Cover cover)
        {
            return cover.Region.ToRhinoBrep();
        }

        /// <summary>
        /// Create Rhino curves from underlying Edges in Region of Cover.
        /// </summary>
        internal static List<Rhino.Geometry.Curve> GetRhinoCurves(this Cover cover)
        {
            return cover.Region.ToRhinoCurves();
        }

        #endregion

        #region Edge
        /// <summary>
        /// Convert a Rhino Curve to one or several Edges.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static List<Geometry.Edge> FromRhinoBrep(this Rhino.Geometry.Curve obj)
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
                    edges.Add(FromRhinoLineCurve(new Rhino.Geometry.LineCurve(line)));
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
                edges.Add(obj.FromRhino());
                return edges;
            }
        }


        /// <summary>
        /// Convert a Rhino Curve to Edge
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="arcType1">True if output is general purpose Edge. False if output is BarPart Edge.</param>
        /// <returns></returns>
        public static Geometry.Edge FromRhino(this Rhino.Geometry.Curve obj)
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
                    return arcCurve.FromRhinoArc1();
                }

                // if Circle
                else
                {
                    return arcCurve.FromRhinoCircle();
                }
            }

            // if LineCurve
            else if (obj.GetType() == typeof(Rhino.Geometry.LineCurve))
            {
                return ((Rhino.Geometry.LineCurve) obj).FromRhinoLineCurve();
            }

            // if NurbsCurve
            else if (obj.GetType() == typeof(Rhino.Geometry.NurbsCurve))
            {
                return ((Rhino.Geometry.NurbsCurve) obj).FromRhinoNurbsCurve();
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
        public static Geometry.Edge FromRhinoLineOrArc1(this Rhino.Geometry.Curve obj)
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
                    return arcCurve.FromRhinoArc1();
                }
                else
                {
                    return arcCurve.FromRhinoCircle();
                }
            }

            // if LineCurve
            else if (obj.GetType() == typeof(Rhino.Geometry.LineCurve))
            {
                return ((Rhino.Geometry.LineCurve) obj).FromRhinoLineCurve();
            }

            // if PolylineCurve
            else if (obj.GetType() == typeof(Rhino.Geometry.PolylineCurve))
            {
                if (obj.SpanCount == 1)
                {
                    Rhino.Geometry.LineCurve lnCrv = new Rhino.Geometry.LineCurve(obj.PointAtStart, obj.PointAtEnd);
                    return lnCrv.FromRhinoLineCurve();
                }
                else
                {
                    throw new System.ArgumentException($"PolylineCurve with SpanCount: {obj.SpanCount}, is not supported for conversion to an Edge.");
                }
            }

            // If Nurbs Curve
            else if(obj.GetType() == typeof(Rhino.Geometry.NurbsCurve))
            {
                if (obj.Degree == 1)
                {
                    Rhino.Geometry.LineCurve lnCrv = new Rhino.Geometry.LineCurve(obj.PointAtStart, obj.PointAtEnd);
                    return lnCrv.FromRhinoLineCurve();
                }
                else if (obj.Degree == 2)
                {
                    bool isArc = obj.TryGetArc(out Rhino.Geometry.Arc arc);
                    if (isArc == false)
                    {
                        throw new Exception("NurbsCurve with degree equal 2 can not be converted to Arc.");
                    }
                    else
                    {
                        var arcCurve = new Rhino.Geometry.ArcCurve(arc);
                        // if Arc
                        if (!obj.IsClosed)
                        {
                            return arcCurve.FromRhinoArc1();
                        }
                        else
                        {
                            return arcCurve.FromRhinoCircle();
                        }

                    }
                }
                else
                    throw new System.ArgumentException("NurbsCurve Degree greater than 2. Line or Arc cannot be created.");
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
        public static Geometry.Edge FromRhinoLineOrArc2(this Rhino.Geometry.Curve obj)
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
                    return arcCurve.FromRhinoArc2();
                }

                else
                {
                    return arcCurve.FromRhinoCircle();
                }
            }

            // if LineCurve
            else if (obj.GetType() == typeof(Rhino.Geometry.LineCurve))
            {
                return ((Rhino.Geometry.LineCurve) obj).FromRhinoLineCurve();
            }

            // if PolylineCurve
            else if (obj.GetType() == typeof(Rhino.Geometry.PolylineCurve))
            {
                if (obj.SpanCount == 1)
                {
                    Rhino.Geometry.LineCurve lnCrv = new Rhino.Geometry.LineCurve(obj.PointAtStart, obj.PointAtEnd);
                    return lnCrv.FromRhinoLineCurve();
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
        public static Geometry.Edge FromRhinoArc1(this Rhino.Geometry.ArcCurve obj)
        {
            double radius = obj.Arc.Radius;
            double startAngle = 0;
            double endAngle = obj.Arc.EndAngle - obj.Arc.StartAngle;
            Geometry.Point3d centerPoint = obj.Arc.Center.FromRhino();
            Geometry.Vector3d xAxis = new Geometry.Vector3d(centerPoint, obj.Arc.StartPoint.FromRhino());

            // lcs
            CoordinateSystem cs = obj.FromRhinoCurve();

            // return
            return new Geometry.Edge(radius, startAngle, endAngle, centerPoint, xAxis, cs);
        }

        /// <summary>
        /// Create Edge (Arc2) from Rhino open ArcCurve.
        /// </summary>
        public static Geometry.Edge FromRhinoArc2(this Rhino.Geometry.ArcCurve obj)
        {
            Geometry.Point3d startPoint = obj.Arc.StartPoint.FromRhino();
            Geometry.Point3d midPoint = obj.Arc.MidPoint.FromRhino();
            Geometry.Point3d endPoint = obj.Arc.EndPoint.FromRhino();

            // lcs
            CoordinateSystem cs = obj.FromRhinoCurve();

            // return
            return new Geometry.Edge(startPoint, midPoint, endPoint, cs);
        }

        /// <summary>
        /// Create Edge (Circle) from Rhino closed ArcCurve.
        /// </summary>
        public static Geometry.Edge FromRhinoCircle(this Rhino.Geometry.ArcCurve obj)
        {
            double radius = obj.Radius;
            Geometry.Point3d centerPoint = obj.Arc.Center.FromRhino();

            // lcs
            CoordinateSystem cs = obj.FromRhinoCurve();

            // return
            return new Geometry.Edge(radius, centerPoint, cs);
        }

        /// <summary>
        /// Create Edge (Line) from Rhino LineCurve.
        /// </summary>
        public static Geometry.Edge FromRhinoLineCurve(this Rhino.Geometry.LineCurve obj)
        {
            Geometry.Point3d startPoint = obj.PointAtStart.FromRhino();
            Geometry.Point3d endPoint = obj.PointAtEnd.FromRhino();

            // lcs
            CoordinateSystem cs = obj.FromRhinoCurve();

            // return
            return new Geometry.Edge(startPoint, endPoint, cs);
        }

        /// <summary>
        /// Create Edge (Line) from Rhino linear NurbsCurve.
        /// </summary>
        public static Geometry.Edge FromRhinoLinearNurbsCurve(this Rhino.Geometry.NurbsCurve obj)
        {
            Geometry.Point3d startPoint = obj.PointAtStart.FromRhino();
            Geometry.Point3d endPoint = obj.PointAtEnd.FromRhino();

            // lcs
            CoordinateSystem cs = obj.FromRhinoCurve();

            // return
            return new Geometry.Edge(startPoint, endPoint, cs);
        }


        /// <summary>
        /// Create Edge (Line or Circle or Arc) from Rhino  NurbsCurve.
        /// </summary>
        public static Geometry.Edge FromRhinoNurbsCurve(this Rhino.Geometry.NurbsCurve obj)
        {
            // check if NurbsCurve is a line
            if (obj.IsLinear())
            {
                return FromRhinoLinearNurbsCurve(obj);
            }

            // check if NurbsCurve is a circle
            else if (obj.IsArc() && obj.IsClosed)
            {
                obj.TryGetArc(out Rhino.Geometry.Arc _obj);
                return FromRhinoCircle(new Rhino.Geometry.ArcCurve(_obj));
            }

            // check if NurbsCurve is an arc
            else if ((obj.IsArc() || obj.Degree == 2) && !obj.IsClosed)
            {
                obj.TryGetArc(out Rhino.Geometry.Arc _obj);
                return FromRhinoArc1(new Rhino.Geometry.ArcCurve(_obj));
            }

            else
            {
                throw new System.ArgumentException($"Unsupported Curve. Degree of NurbsCurve is likely to high. Degree is: {obj.Degree}, versus a supported degree of 2.");
            }
        }

        /// <summary>
        /// Convert an Edge to a Rhino Curve.
        /// </summary>
        public static Rhino.Geometry.Curve ToRhino(this Edge edge)
        {
            if (edge.Type == "arc")
            {
                return ToRhinoArcCurve(edge);
            }
            else if (edge.Type == "circle")
            {
                return ToRhinoArcCurveFromCircle(edge);
            }
            else if (edge.Type == "line")
            {
                return ToRhinoLineCurve(edge);
            }
            else
            {
                throw new System.ArgumentException($"Edge type: {edge.Type}, is not supported for conversion to a Rhino Curve");
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

        #region CoorinateSystem

        /// <summary>
        /// Create FdCoordinateSystem from Rhino plane.
        /// </summary>
        internal static CoordinateSystem FromRhinoPlane(this Rhino.Geometry.Plane obj)
        {
            Geometry.Point3d origin = obj.Origin.FromRhino();
            Geometry.Vector3d localX = obj.XAxis.FromRhino();
            Geometry.Vector3d localY = obj.YAxis.FromRhino();
            Geometry.Vector3d localZ = obj.ZAxis.FromRhino();
            return new CoordinateSystem(origin, localX, localY, localZ);
        }

        /// <summary>
        /// Create FdCoordinateSystem from Rhino plane on curve mid u-point.
        /// </summary>
        internal static CoordinateSystem FromRhinoCurve(this Rhino.Geometry.Curve obj)
        {
            // reparameterize if neccessary
            if (obj.Domain.T0 == 0 && obj.Domain.T1 == 1)
            {
                // pass
            }
            else
            {
                obj.Domain = new Rhino.Geometry.Interval(0, 1);
            }

            // get frame @ mid-point
            Rhino.Geometry.Plane plane;
            obj.FrameAt(0.5, out plane);
            return plane.FromRhinoPlane();
        }

        /// <summary>
        /// Create FdCoordinateSystem from Rhino plane on surface mid u/v-point.
        /// </summary>
        internal static CoordinateSystem FromRhinoSurface(this Rhino.Geometry.Surface obj)
        {
            // reparameterize if necessary
            if (obj.Domain(0).T0 == 0 && obj.Domain(1).T0 == 0 && obj.Domain(0).T1 == 1 && obj.Domain(1).T1 == 1)
            {
                // pass
            }
            else
            {
                obj.SetDomain(0, new Rhino.Geometry.Interval(0, 1));
                obj.SetDomain(1, new Rhino.Geometry.Interval(0, 1));
            }

            Rhino.Geometry.Plane plane;
            obj.FrameAt(0.5, 0.5, out plane);
            return plane.FromRhinoPlane();
        }
        #endregion

        #region Region
        /// <summary>
        /// Create Region from Rhino brep.
        /// </summary>
        public static Geometry.Region FromRhino(this Rhino.Geometry.Brep obj)
        {
            // check if brep contains more than 1 surface
            if (obj.Surfaces.Count != 1)
            {
                throw new System.ArgumentException("Brep contains more than 1 surface.");
            }

            // check if brep surface is planar
            if (!obj.Surfaces[0].IsPlanar())
            {
                throw new System.ArgumentException("Brep surface is not planar. This problem might occur due to tolerance error - if your model space is in millimeters try to change to meters.");
            }

            // Reconstruct Brep to make sure that boundaries are Atomic.
            var innerNakedCurves = obj.DuplicateNakedEdgeCurves(true, false);
            var outerNakedCurves = obj.DuplicateNakedEdgeCurves(false, true);
            var nakedCurves = innerNakedCurves.Concat(outerNakedCurves);
            var outerCurves = new List<Rhino.Geometry.Curve>();
            foreach (var curve in nakedCurves)
            {
                var curves = Grasshopper.GeomUtility.Explode(curve);
                outerCurves.AddRange(curves);
            }
            obj = Rhino.Geometry.Brep.CreatePlanarBreps(outerCurves, Tolerance.Brep)[0];

            // get outline curves
            var container = new List<List<Rhino.Geometry.Curve>>();
            var loopCurves = new List<Rhino.Geometry.Curve>();

            foreach (Rhino.Geometry.BrepLoop loop in obj.Loops)
            {
                foreach (Rhino.Geometry.BrepTrim trim in loop.Trims)
                {
                    loopCurves.Add(trim.Edge.EdgeCurve);
                }
                container.Add(new List<Rhino.Geometry.Curve>(loopCurves));
                loopCurves.Clear();
            }

            // Change direction of EdgeCurves if necessary.
            // Circular Edge in Contour (id est the only Edge in that Contour) should have a normal in the opposite direction from any other type of Contour in Region (id est the direction of the Contour should be opposite).
            // EndPoint of EdgeCurve[idx] should be equal to StartPoint of EdgeCurve[idx + 1] in a Contour.
            foreach (List<Rhino.Geometry.Curve> items in container)
            {
                // if Contour consists of one curve
                if (items.Count == 1)
                {
                    // check if curve is a Circle
                    Rhino.Geometry.Curve curve = items[0];
                    if (curve.IsArc() && curve.IsClosed)
                    {
                        // check if Circle is planar
                        if (curve.IsPlanar())
                        {
                            curve.TryGetPlane(out Rhino.Geometry.Plane plane);
                            // compare Contour and Surface normals
                            if (obj.Surfaces[0].NormalAt(0, 0).IsParallelTo(plane.Normal, Tolerance.Point3d) == 1)
                            {
                                // reverse direction of Circle
                                curve.Reverse();
                            }
                        }
                        else
                        {
                            throw new System.ArgumentException("Curve is not planar");
                        }
                    }

                    // if curve for some reason is not a Circle
                    else
                    {
                        // if the curve can not be represented by a circle then direction is irrelevant.
                        // pass
                    }
                }

                // if Contour consists of more than one curve (i.e. is not a Circle)
                else
                {
                    Rhino.Geometry.Point3d pA0, pA1, pB0, pB1;
                    for (int idx = 0; idx < items.Count - 1; idx++)
                    {
                        // curve a = items[idx]
                        // curve b = items[idx + 1]
                        pA0 = items[idx].PointAtStart;
                        pA1 = items[idx].PointAtEnd;
                        pB0 = items[idx + 1].PointAtStart;
                        pB1 = items[idx + 1].PointAtEnd;

                        if (pA0.EpsilonEquals(pB0, Tolerance.Point3d))
                        {
                            if (idx == 0)
                            {
                                items[idx].Reverse();
                            }
                            else
                            {
                                throw new System.ArgumentException("pA0 == pB0 even though idx != 0. Bad outline.");
                            }
                        }

                        else if (pA0.EpsilonEquals(pB1, Tolerance.Point3d))
                        {
                            if (idx == 0)
                            {
                                items[idx].Reverse();
                                items[idx + 1].Reverse();
                            }
                            else
                            {
                                throw new System.ArgumentException("pA0 == pB1 even though idx != 0. Bad outline.");
                            }
                        }

                        else if (pA1.EpsilonEquals(pB0, Tolerance.Point3d))
                        {
                            // pass
                        }

                        else if (pA1.EpsilonEquals(pB1, Tolerance.Point3d))
                        {
                            items[idx + 1].Reverse();
                        }

                        else
                        {
                            throw new System.ArgumentException("Can't close outline. Bad outline.");
                        }
                    }

                    // check if outline is closed.
                    pA1 = items[items.Count - 1].PointAtEnd;
                    pB0 = items[0].PointAtStart;
                    if (pA1.EpsilonEquals(pB0, Tolerance.Point3d))
                    {

                    }

                    else
                    {
                        throw new System.ArgumentException("Can't close outline. Bad outline. Boundary Edge Directions should perform a close loop.");
                    }
                }
            }

            // Create contours
            List<Geometry.Edge> edges = new List<Geometry.Edge>();
            List<Geometry.Contour> contours = new List<Geometry.Contour>();

            foreach (List<Rhino.Geometry.Curve> items in container)
            {
                foreach (Rhino.Geometry.Curve curve in items)
                {
                    foreach (Geometry.Edge edge in curve.FromRhinoBrep())
                    {
                        edges.Add(edge);
                    }
                }
                contours.Add(new Geometry.Contour(new List<Edge>(edges)));
                edges.Clear();
            }

            // Get LCS
            CoordinateSystem cs = obj.Surfaces[0].FromRhinoSurface();

            // return
            return new Geometry.Region(contours, cs);
        }

        /// <summary>
        /// Convert Region to Rhino brep.
        /// </summary>
        internal static Rhino.Geometry.Brep ToRhinoBrep(this Region region)
        {
            List<Rhino.Geometry.Curve> curves = new List<Rhino.Geometry.Curve>();
            foreach (Geometry.Contour contour in region.Contours)
            {
                foreach (Geometry.Edge edge in contour.Edges)
                {
                    curves.Add(edge.ToRhino());
                }
            }
            Rhino.Geometry.Brep[] breps = Rhino.Geometry.Brep.CreatePlanarBreps(curves, Tolerance.Brep);

            if (breps.Length == 1)
            {
                return breps[0];
            }

            else
            {
                throw new System.ArgumentException("More than one Brep was generated from Region.");
            }
        }

        /// <summary>
        /// Convert Edges in Region to Rhino curves.
        /// </summary>
        internal static List<Rhino.Geometry.Curve> ToRhinoCurves(this Region region)
        {
            List<Rhino.Geometry.Curve> curves = new List<Rhino.Geometry.Curve>();
            foreach (Geometry.Contour contour in region.Contours)
            {
                foreach (Geometry.Edge edge in contour.Edges)
                {
                    curves.Add(edge.ToRhino());
                }
            }
            return curves;
        }

#endregion

#region RegionGroup
/// <summary>
/// Get rhino breps of underlying regions
/// </summary>
internal static List<Rhino.Geometry.Brep> ToRhino(this RegionGroup regionGroup)
        {
            List<Rhino.Geometry.Brep> breps = new List<Rhino.Geometry.Brep>();
            foreach (Region region in regionGroup.Regions)
            {
                breps.Add(region.ToRhinoBrep());
            }
            return breps;
        }
        #endregion

        #region Face
        internal static Rhino.Geometry.MeshFace ToRhino(this FemDesign.Geometry.Face face)
        {
            if(face.IsQuad())
            {
               return new Rhino.Geometry.MeshFace(face.Node1, face.Node2, face.Node3, face.Node4);
            }
            else
            // it is Triangular Mesh
            {
                return new Rhino.Geometry.MeshFace(face.Node1, face.Node2, face.Node3);
            }
        }
        #endregion

        #region LineLoad
        /// <summary>
        /// Convert LineLoad edge to Rhino curve.
        /// </summary>
        internal static Rhino.Geometry.Curve GetRhinoGeometry(this Loads.LineLoad lineLoad)
        {
            return lineLoad.Edge.ToRhino();
        }

        #endregion

        #region PointLoad

        /// <summary>
        /// Convert PointLoad point to Rhino point.
        /// </summary>
        internal static Rhino.Geometry.Point3d GetRhinoGeometry(this PointLoad pointLoad)
        {
            return pointLoad.Load.GetFdPoint().ToRhino();
        }
        #endregion

        #region PressureLoad

        /// <summary>
        /// Convert surface of PressureLoad to a Rhino brep.
        /// </summary>
        internal static Rhino.Geometry.Brep GetRhinoGeometry(this PressureLoad pressureLoad)
        {
            return pressureLoad.Region.ToRhinoBrep();
        }
        #endregion

        #region SurfaceLoad

        /// <summary>
        /// Convert surface of SurfaceLoad to a Rhino brep.
        /// </summary>
        internal static Rhino.Geometry.Brep GetRhinoGeometry(this SurfaceLoad surfaceLoad)
        {
            return surfaceLoad.Region.ToRhinoBrep();
        }
        #endregion

        #region SlabPart

        /// <summary>
        /// Get Rhino Surface from SlabPart Contours (Region).
        /// </summary>
        internal static Rhino.Geometry.Brep GetRhinoSurface(this Shells.SlabPart slabPart)
        {
            return slabPart.Region.ToRhinoBrep();
        }

        /// <summary>
        /// Get Rhino Curves from SlabPart Contours (Region).
        /// </summary>
        internal static List<Rhino.Geometry.Curve> GetRhinoCurves(this Shells.SlabPart slabPart)
        {
            return slabPart.Region.ToRhinoCurves();
        }

        #endregion

        #region LineSupport
        
        /// <summary>
        /// Get Rhino Curve from LineSupport.
        /// </summary>
        internal static Rhino.Geometry.Curve GetRhinoGeometry(this Supports.LineSupport lineSupport)
        {
            return lineSupport.Edge.ToRhino();
        }

        #endregion

        #region PointSupport
        
        /// <summary>
        /// Get Rhino Point from PointSupport.
        /// </summary>
        internal static Rhino.Geometry.Point3d GetRhinoGeometry(this Supports.PointSupport pointSupport)
        {
            return pointSupport.Position.ToRhino();
        }

        #endregion
    }
}
