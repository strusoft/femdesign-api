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

        public ArcEdge(Point3d start, Point3d middle, Point3d end, Plane plane) : base(start, middle, end, plane)
        {
        }

        public ArcEdge(double radius, double startAngle, double endAngle,  Point3d center, Vector3d xAxis, Plane plane) : base(radius, startAngle, endAngle, center, xAxis, plane)
        {
        }

    }
}
