// https://strusoft.com/

using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Geometry
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class FdVector2d
    {
        [XmlAttribute("x")]
        public double x { get; set;}
        [XmlAttribute("y")]
        public double y { get; set; }

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
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Convert to 3d vector.
        /// </summary>
        /// <returns></returns>
        public FdVector3d To3d()
        {
            return new FdVector3d(this.x, this.y, 0);
        }

        /// <summary>
        /// Check if vector is zero.
        /// </summary>
        /// <returns></returns>
        public bool IsZero()
        {
            if (this.x == 0 && this.y == 0)
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