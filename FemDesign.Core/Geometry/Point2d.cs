// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    [System.Serializable]
    public partial class Point2d
    {
        [XmlAttribute("x")]
        public double X { get; set;}
        [XmlAttribute("y")]
        public double Y { get; set; }

        public static Point2d Origin => new Point2d(0, 0);

        /// <summary>
        /// Parameterless constructor for
        /// </summary>
        private Point2d()
        {
            // pass
        }

        /// <summary>
        /// Create new point.
        /// </summary>
        /// <param name="x">x-coordinate.</param>
        /// <param name="y">y-coordinate.</param>
        public Point2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Convert to 3d point.
        /// </summary>
        /// <returns></returns>
        public Point3d To3d()
        {
            return new Point3d(this.X, this.Y, 0);
        }
    }
}