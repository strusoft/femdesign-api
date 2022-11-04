using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.StructureGrid
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class Axis: EntityBase
    {
        #region dynamo
        /// <summary>
        /// Create an axis.
        /// </summary>
        /// <param name="line">Line of axis. Line will be projected onto XY-plane.</param>
        /// <param name="prefix">Prefix of axis identifier.</param>
        /// <param name="id">Number of axis identifier. Number can be converted to letter.</param>
        /// <param name="idIsLetter">Is identifier a letter?</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Axis Define(Autodesk.DesignScript.Geometry.Line line, [DefaultArgument("")] string prefix, int id, [DefaultArgument("false")] bool idIsLetter)
        {
            // convert geometry
            Geometry.Point3d p0 = Geometry.Point3d.FromDynamo(line.StartPoint);
            Geometry.Point3d p1 = Geometry.Point3d.FromDynamo(line.EndPoint);

            //
            return new Axis(p0, p1, prefix, id, idIsLetter);

        }
        #endregion
    }
}