// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    [System.Serializable]
    public class FdVector2d
    {
        [XmlAttribute("x")]
        public double X { get; set;}
        [XmlAttribute("y")]
        public double Y { get; set; }

        /// <summary>
        /// Parameterless constructor for
        /// </summary>
        private FdVector2d()
        {
            // pass
        }

        /// <summary>
        /// Create new vector.
        /// </summary>
        /// <param name="x">i-component.</param>
        /// <param name="y">j-component.</param>
        public FdVector2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Convert to 3d vector.
        /// </summary>
        /// <returns></returns>
        public FdVector3d To3d()
        {
            return new FdVector3d(this.X, this.Y, 0);
        }

        /// <summary>
        /// Check if vector is zero.
        /// </summary>
        /// <returns></returns>
        public bool IsZero()
        {
            if (this.X == 0 && this.Y == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}