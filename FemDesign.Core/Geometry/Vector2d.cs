// https://strusoft.com/

using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    [System.Serializable]
    public partial class Vector2d
    {
        [XmlAttribute("x")]
        public double X { get; set;}
        [XmlAttribute("y")]
        public double Y { get; set; }

        public static Vector2d Zero => new Vector2d(0,0);
        public static Vector2d UnitX => new Vector2d(1,0);
        public static Vector2d UnitY => new Vector2d(0,1);

        /// <summary>
        /// Parameterless constructor for
        /// </summary>
        private Vector2d()
        {
            // pass
        }

        /// <summary>
        /// Create new vector.
        /// </summary>
        /// <param name="x">i-component.</param>
        /// <param name="y">j-component.</param>
        public Vector2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Convert to 3d vector.
        /// </summary>
        /// <returns></returns>
        public Vector3d To3d()
        {
            return new Vector3d(this.X, this.Y, 0);
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