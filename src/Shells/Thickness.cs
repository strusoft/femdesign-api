// https://strusoft.com/
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Shells
{
    /// <summary>
    /// location_value_type
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
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
            this.X = point.X;
            this.Y = point.Y;
            this.Z = point.Z;
            this.Value = val;            
        }

        #region dynamo
        /// <summary>
        /// Create a Thickness object.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="point">Position of thickness value.</param>
        /// <param name="val">Value of thickness.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Thickness Define(Autodesk.DesignScript.Geometry.Point point, double val)
        {
            return new Thickness(Geometry.FdPoint3d.FromDynamo(point), val);
        }
        #endregion
    }
}