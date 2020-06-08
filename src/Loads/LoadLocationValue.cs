// https://strusoft.com/

namespace FemDesign.Loads
{
    /// <summary>
    /// location_value_type
    /// </summary>
    public class LoadLocationValue: LocationValue
    {
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LoadLocationValue()
        {

        }

        /// <summary>
        /// Internal constructor accessed by GH components and Dynamo nodes.
        /// </summary>
        internal LoadLocationValue(Geometry.FdPoint3d loadPosition, double val)
        {
            this.x = loadPosition.x;
            this.y = loadPosition.y;
            this.z = loadPosition.z;
            this.val = val;
        }

    }
}