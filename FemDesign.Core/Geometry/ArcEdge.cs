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
    public partial class ArcEdge : Edge
    {
        public ArcEdge()
        {

        }

        public ArcEdge(Point3d start, Point3d middle, Point3d end, CoordinateSystem coordinateSystem) : base(start, middle, end, coordinateSystem)
        {
        }

        public ArcEdge(double radius, double startAngle, double endAngle,  Point3d center, Vector3d xAxis, CoordinateSystem coordinateSystem) : base(radius, startAngle, endAngle, center, xAxis, coordinateSystem)
        {
        }

    }
}
