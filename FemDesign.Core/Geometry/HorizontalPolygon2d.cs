using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    [System.Serializable]
    public partial class HorizontalPolygon2d
    {
        [XmlElement("point")]
        public List<Point2d> Points { get; set; }
        private HorizontalPolygon2d() { }
        public HorizontalPolygon2d(List<Point2d> point2d)
        {
            this.Points = point2d;
        }
    }
}
