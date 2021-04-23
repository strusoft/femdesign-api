using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FemDesign.GH
{
    internal static partial class Convert
    {
        internal static FemDesign.Geometry.FdPoint3d FromRhino(this Rhino.Geometry.Point3d point)
        {
            return new FemDesign.Geometry.FdPoint3d(point.X, point.Y, point.Z);
        }

        internal static FemDesign.Geometry.FdVector3d FromRhino(this Rhino.Geometry.Vector3d vector)
        {
            return new FemDesign.Geometry.FdVector3d(vector.X, vector.Y, vector.Z);
        }
    }
}
