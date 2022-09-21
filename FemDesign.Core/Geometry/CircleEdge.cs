using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    [XmlRoot("database", Namespace = "urn:strusoft")]
    [System.Serializable]
    public partial class CircleEdge : Edge
    {
        public CircleEdge()
        {

        }

        public CircleEdge(double radius, Point3d centerPoint, CoordinateSystem coordinateSystem) : base(radius, centerPoint, coordinateSystem)
        {
        }

    }
}
