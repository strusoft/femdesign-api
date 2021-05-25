
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Supports
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class PointSupport: EntityBase
    {
        #region dynamo
        /// <summary>
        /// Create a rigid point support element.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="point"></param>
        /// <param name="identifier">Identifier. Optional, default value if undefined.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static PointSupport Rigid(Autodesk.DesignScript.Geometry.Point point, [DefaultArgument("S")] string identifier)
        {
            return PointSupport.Rigid(Geometry.FdPoint3d.FromDynamo(point), identifier);
        }

        /// <summary>
        /// Create a hinged point support element.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="point"></param>
        /// <param name="identifier">Identifier. Optional, default value if undefined.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static PointSupport Hinged(Autodesk.DesignScript.Geometry.Point point, [DefaultArgument("S")] string identifier)
        {
            return PointSupport.Hinged(Geometry.FdPoint3d.FromDynamo(point), identifier);
        }

        /// <summary>
        /// Define a PointSupport
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="point"></param>
        /// <param name="motions">Motions. Translation releases.</param>
        /// <param name="rotations">Rotations. Rotation releases.</param>
        /// <param name="identifier">Identifier. Optional, default value if undefined.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static PointSupport Define(Autodesk.DesignScript.Geometry.Point point, Releases.Motions motions, Releases.Rotations rotations, [DefaultArgument("S")] string identifier)
        {
            return new PointSupport(Geometry.FdPoint3d.FromDynamo(point), motions, rotations, identifier);
        }

        internal Autodesk.DesignScript.Geometry.Point GetDynamoGeometry()
        {
            return this.Position.ToDynamo();
        }

        #endregion
    }
}