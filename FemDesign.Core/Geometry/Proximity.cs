using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Geometry
{
    public static class Proximity
    {
        /// <summary>
        /// Find the closest position between two infinite lines.
        /// Closest point on line 1 will thus be at
        /// <code>var cp1 = p1 + (t1 * v1)</code>
        /// </summary>
        /// <param name="p1">Reference point on line 1</param>
        /// <param name="v1">Direction of line 1</param>
        /// <param name="p2">Reference point on line 2</param>
        /// <param name="v2">Direction of line 2</param>
        /// <returns>Parameters t1, t2, where the two lines are the closest to each other</returns>
        public static (double t1, double t2) LineLineProximity(Point3d p1, Vector3d v1, Point3d p2, Vector3d v2)
        {
            var cp = v1.Cross(v2);
            var n1 = v1.Cross(-cp);
            var n2 = v2.Cross(cp);

            double t1 = (p2 - p1).Dot(n2) / (v1.Dot(n2));
            double t2 = (p1 - p2).Dot(n1) / (v2.Dot(n1));

            return (t1, t2);
        }

        /// <summary>
        /// Find the closest position between two infinite lines.
        /// Closest point on line 1 will thus be at
        /// <code>var cp1 = start1 + (t1 * (end1 - start1))</code>
        /// </summary>
        /// <param name="start1">First reference point for infinite line 1</param>
        /// <param name="end1">Second reference point for infinite line 1</param>
        /// <param name="start2">First reference point for infinite line 2</param>
        /// <param name="end2">Second reference point for infinite line 2</param>
        /// <inheritdoc cref="LineLineProximity(Point3d, Vector3d, Point3d, Vector3d)"/>
        public static (double t1, double t2) LineLineProximity(Point3d start1, Point3d end1, Point3d start2, Point3d end2)
        {
            return LineLineProximity(start1, end1 - start1, start2, end2 - start2);
        }

    }
}
