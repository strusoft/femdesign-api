using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    [System.Serializable]
    public partial class ArcEdge : Edge
    {
        public ArcEdge()
        {

        }

        public ArcEdge(FdPoint3d start, FdPoint3d middle, FdPoint3d end, FdCoordinateSystem coordinateSystem) : base(start, middle, end, coordinateSystem)
        {
        }

        public ArcEdge(double radius, double startAngle, double endAngle,  FdPoint3d center, FdVector3d xAxis, FdCoordinateSystem coordinateSystem) : base(radius, startAngle, endAngle, center, xAxis, coordinateSystem)
        {
        }

    }
}
