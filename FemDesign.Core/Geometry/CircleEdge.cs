using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    [System.Serializable]
    public partial class CircleEdge : Edge
    {
        public CircleEdge()
        {

        }

        public CircleEdge(double radius, FdPoint3d centerPoint, FdCoordinateSystem coordinateSystem) : base(radius, centerPoint, coordinateSystem)
        {
        }

    }
}
