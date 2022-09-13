// https://strusoft.com/

namespace FemDesign.Loads
{
    /// <summary>
    /// location_value_type
    /// </summary>
    [System.Serializable]
    public partial class LoadLocationValue: LocationValue
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
        public LoadLocationValue(Geometry.Point3d loadPosition, double val)
        {
            this.X = loadPosition.X;
            this.Y = loadPosition.Y;
            this.Z = loadPosition.Z;
            this.Value = val;
        }



    }
}