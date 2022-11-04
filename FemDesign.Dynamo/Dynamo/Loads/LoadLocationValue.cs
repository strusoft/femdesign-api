#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Loads
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class LoadLocationValue: LocationValue
    {
        #region dynamo
        /// <summary>
        /// Create a LoadLocationValue object.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="point">Position of Load.</param>
        /// <param name="intensity">Intensity of load.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static LoadLocationValue Define(Autodesk.DesignScript.Geometry.Point point, double intensity)
        {
            return new LoadLocationValue(Geometry.Point3d.FromDynamo(point), intensity);
        }

        #endregion
    }
}