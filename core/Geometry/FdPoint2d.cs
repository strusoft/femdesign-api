// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    [System.Serializable]
    public class FdPoint2d
    {
        [XmlAttribute("x")]
        public double X { get; set;}
        [XmlAttribute("y")]
        public double Y { get; set; }

        /// <summary>
        /// Parameterless constructor for
        /// </summary>
        private FdPoint2d()
        {
            // pass
        }

        /// <summary>
        /// Create new point.
        /// </summary>
        /// <param name="x">x-coordinate.</param>
        /// <param name="y">y-coordinate.</param>
        public FdPoint2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Convert to 3d point.
        /// </summary>
        /// <returns></returns>
        public FdPoint3d To3d()
        {
            return new FdPoint3d(this.X, this.Y, 0);
        }
    }
}