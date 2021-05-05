using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign;
using FemDesign.Geometry;
using FemDesign.Supports;
using Autodesk.DesignScript.Runtime;

namespace FemDesign.DynamoZeroTouch
{
    public class PointSupport
    {
        public static FemDesign.Supports.PointSupport Hinged(Autodesk.DesignScript.Geometry.Point point, [DefaultArgument("S")] string identifier)
        {
            return FemDesign.Supports.PointSupport.Hinged(point.ToFemDesign(), identifier);
        }
    }
}
