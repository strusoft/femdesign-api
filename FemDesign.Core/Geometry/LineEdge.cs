using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    [System.Serializable]
    public partial class LineEdge : Edge
    {
        public LineEdge()
        {

        }

        public LineEdge(FdPoint3d startPoint, FdPoint3d endPoint, FdCoordinateSystem coordinateSystem) : base(startPoint, endPoint, coordinateSystem)
        {
        }

        public LineEdge(FdPoint3d startPoint, FdPoint3d endPoint, FdVector3d localY) : base(startPoint, endPoint, localY)
        {
        }
    }
}
