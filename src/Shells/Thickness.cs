// https://strusoft.com/

namespace FemDesign.Shells
{
    /// <summary>
    /// location_value_type
    /// </summary>
    [System.Serializable]
    public class Thickness: LocationValue
    {
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal Thickness()
        {

        }

        /// <summary>
        /// Construct Thickness object at point with value.
        /// </summary>
        internal Thickness(Geometry.FdPoint3d point, double val)
        {
            this.x = point.x;
            this.y = point.y;
            this.z = point.z;
            this.val = val;            
        }

    }
}