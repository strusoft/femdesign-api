// https://strusoft.com/

namespace FemDesign.Shells
{
    /// <summary>
    /// location_value_type
    /// </summary>
    [System.Serializable]
    public partial class Thickness: LocationValue
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
        public Thickness(Geometry.Point3d point, double val)
        {
            this.X = point.X;
            this.Y = point.Y;
            this.Z = point.Z;
            this.Value = val;            
        }

        public override string ToString()
        {
            return $"{this.GetType().Name} Pos: {this.GetFdPoint()}, Value: {this.Value} m";
        }
    }
}