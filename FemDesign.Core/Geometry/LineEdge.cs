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
    public partial class LineEdge : Edge
    {
        public LineEdge()
        {

        }

        public LineEdge(Point3d startPoint, Point3d endPoint, CoordinateSystem coordinateSystem) : base(startPoint, endPoint, coordinateSystem)
        {
        }

        public LineEdge(Point3d startPoint, Point3d endPoint, Vector3d localY) : base(startPoint, endPoint, localY)
        {
        }
    }
}
