using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.DesignScript.Runtime;
using AG = Autodesk.DesignScript.Geometry;

namespace FemDesign.DynamoZeroTouch
{
    [IsVisibleInDynamoLibrary(false)]
    public static class Convert
    {
        #region Point
        
        public static FemDesign.Geometry.FdPoint3d ToFemDesign(this AG.Point point)
        {
            return new Geometry.FdPoint3d(point.X, point.Y, point.Z);
        }
        
        #endregion
    }
}
