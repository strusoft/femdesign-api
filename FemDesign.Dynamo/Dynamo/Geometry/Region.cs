
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Geometry
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Region
    {
        #region dynamo
        /// <summary>
        /// Create Region from Dynamo surface.
        /// </summary>
        public static Geometry.Region FromDynamo(Autodesk.DesignScript.Geometry.Surface obj)
        {
            // get all perimeter curves
            // curves[] is ordered by the loops of the surface.
            // the assumption here is that the loop with the largest circumference (i.e. the outline) is placed first in the array
            // for fd definition it is neccessary that the outline is the first contour, the subsequent loops can have any order.
            if (obj.Faces.Count() != 1)
                throw new System.Exception("Surface has more than one face");
            var face = obj.Faces.First();

            // create contours
            List<Geometry.Edge> edges = new List<Geometry.Edge>();
            List<Geometry.Contour> contours = new List<Geometry.Contour>();


            foreach (var loop in face.Loops)
            {
                foreach(var coEdge in loop.CoEdges)
                {
                    var crvGeometry = coEdge.Edge.CurveGeometry;
                    edges.Add(Geometry.Edge.FromDynamo(crvGeometry));
                }
                contours.Add(new Geometry.Contour(new List<Edge>(edges)));
                edges.Clear();
            }

            contours.Reverse();
            // get LCS
            CoordinateSystem cs = CoordinateSystem.FromDynamoSurface(obj);
            // return
            return new Geometry.Region(contours, cs);
        }

        /// <summary>
        /// Convert Region to Dynamo surface.
        /// </summary>
        public Autodesk.DesignScript.Geometry.Surface ToDynamoSurface()
        {
            // get closed curves
            List<Autodesk.DesignScript.Geometry.PolyCurve> closedCurves = new List<Autodesk.DesignScript.Geometry.PolyCurve>();
            foreach (Geometry.Contour contour in this.Contours)
            {
                List<Autodesk.DesignScript.Geometry.Curve> curves = new List<Autodesk.DesignScript.Geometry.Curve>();
                foreach (Geometry.Edge edge in contour.Edges)
                {
                    curves.Add(edge.ToDynamo());
                }
                closedCurves.Add(Autodesk.DesignScript.Geometry.PolyCurve.ByJoinedCurves(curves));
                curves.Clear();
            }

            // get surface
            List<Autodesk.DesignScript.Geometry.Surface> surfaces = new List<Autodesk.DesignScript.Geometry.Surface>();

            // make sure that curves are order from longer to shorter
            var orderedCurves = closedCurves.OrderBy(c => c.Length).Reverse().ToList();

            foreach (Autodesk.DesignScript.Geometry.PolyCurve closedCurve in orderedCurves)
            {
                surfaces.Add(Autodesk.DesignScript.Geometry.Surface.ByPatch(closedCurve));
            }
            Autodesk.DesignScript.Geometry.Surface primarySurface = surfaces[0];
            surfaces.RemoveAt(0);
            foreach (Autodesk.DesignScript.Geometry.Surface secondarySurface in surfaces)
            {
                primarySurface = (Autodesk.DesignScript.Geometry.Surface)primarySurface.Split(secondarySurface)[0];
            }

            // return
            return primarySurface;
        }

        /// <summary>
        /// Convert Edges in Region to Dynamo curves.
        /// </summary>
        public List<List<Autodesk.DesignScript.Geometry.Curve>> ToDynamoCurves()
        {
            List<List<Autodesk.DesignScript.Geometry.Curve>> outlines = new List<List<Autodesk.DesignScript.Geometry.Curve>>();
            foreach (Geometry.Contour contour in this.Contours)
            {
                List<Autodesk.DesignScript.Geometry.Curve> curves = new List<Autodesk.DesignScript.Geometry.Curve>();
                foreach (Geometry.Edge edge in contour.Edges)
                {
                    curves.Add(edge.ToDynamo());
                }
                outlines.Add(new List<Autodesk.DesignScript.Geometry.Curve>(curves));
                curves.Clear();
            }
            return outlines;
        }

        #endregion
    }
}