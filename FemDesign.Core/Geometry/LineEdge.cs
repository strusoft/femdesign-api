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
        [XmlIgnore]
        public Point3d StartPoint => base.Points[0];

        [XmlIgnore]
        public Point3d EndPoint => base.Points[1];

        public LineEdge()
        {
        }

        public LineEdge(Point3d startPoint, Point3d endPoint, CoordinateSystem coordinateSystem) : base(startPoint, endPoint, coordinateSystem)
        {
        }

        public LineEdge(Point3d startPoint, Point3d endPoint, Vector3d localY = null) : base(startPoint, endPoint, localY)
        {
        }

        /// <summary>
        /// Evaluate a curve at a certain factor along its length.
        /// </summary>
        /// <param name="lengthFactor"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Point3d PointAtLength(double lengthFactor)
		{
            var curveLength = this.Length;

            if (lengthFactor > curveLength) throw new ArgumentException("Length factor cannot be larger than the length of the curve.");
            if (lengthFactor < 0.00) throw new ArgumentException("Length factor cannot be smaller than zero.");

            var percentage = lengthFactor / curveLength;

            var domainX = this.EndPoint.X - this.StartPoint.X;
            var domainY = this.EndPoint.Y - this.StartPoint.Y;
            var domainZ = this.EndPoint.Z - this.StartPoint.Z;

            return new Point3d(domainX * percentage + this.StartPoint.X, domainY * percentage + this.StartPoint.Y, domainZ * percentage + this.StartPoint.Z);
        }

    }
}
