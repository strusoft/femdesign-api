// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    [System.Serializable]
    public class FdPoint2d
    {
        [XmlAttribute("x")]
        public double x { get; set;}
        [XmlAttribute("y")]
        public double y { get; set; }

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
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Convert to 3d point.
        /// </summary>
        /// <returns></returns>
        public FdPoint3d To3d()
        {
            return new FdPoint3d(this.x, this.y, 0);
        }
    }
}