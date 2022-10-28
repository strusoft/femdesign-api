#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Shells
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Thickness: LocationValue
    {
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
            return new Thickness(Geometry.Point3d.FromDynamo(point), val);
        }
        
        #endregion
    }
}