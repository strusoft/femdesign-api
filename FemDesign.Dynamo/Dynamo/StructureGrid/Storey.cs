using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.StructureGrid
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Storey: EntityBase
    {
        #region dynamo
   
        /// <summary>
        /// Create a storey.
        /// </summary>
        /// <param name="origo">Origo of storey. Storeys can only have unique Z-coordinates. If several storeys are placed in a model their origos should share XY-coordinates.</param>
        /// <param name="direction">Direction of storey x'-axis in the XY-plane. If several storeys are placed in a model their direction should be identical. Optional, default value is GCS x-axis.</param>
        /// <param name="dimensionX">Dimension in x'-direction.</param>
        /// <param name="dimensionY">Dimension in y'-direction.</param>
        /// <param name="name">Name of storey.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Storey Define(string name, Autodesk.DesignScript.Geometry.Point origo, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.XAxis()")] Autodesk.DesignScript.Geometry.Vector direction, double dimensionX = 50, double dimensionY = 30)
        {
            // convert geometry
            Geometry.Point3d p = Geometry.Point3d.FromDynamo(origo);
            Geometry.Vector3d v = Geometry.Vector3d.FromDynamo(direction);

            // return
            return new Storey(p, v, dimensionX, dimensionY, name);
        }
        
        #endregion
    }
}