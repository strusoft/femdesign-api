
using System.Xml.Serialization;
using Autodesk.DesignScript.Runtime;
using FemDesign.Releases;


namespace FemDesign.Supports
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class PointSupport: NamedEntityBase
    {
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
            return PointSupport.Rigid(Geometry.Point3d.FromDynamo(point), identifier);
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
            return PointSupport.Hinged(Geometry.Point3d.FromDynamo(point), identifier);
        }

        /// <summary>
        /// Define a PointSupport
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="point"></param>
        /// <param name="motions">Motions. Translation releases.</param>
        /// <param name="motionsPlasticLimits">Motions plastic limits. Translation releases.</param>
        /// <param name="rotations">Rotations. Rotation releases.</param>
        /// <param name="rotationsPlasticLimits">Rotations plastic limits. Rotation releases.</param>
        /// <param name="identifier">Identifier. Optional, default value if undefined.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static PointSupport Define(Autodesk.DesignScript.Geometry.Point point, Motions motions, [DefaultArgument("MotionsPlasticLimits.Default()")] MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, [DefaultArgument("RotationsPlasticLimits.Default()")] RotationsPlasticLimits rotationsPlasticLimits, [DefaultArgument("S")] string identifier)
        {
            return new PointSupport(Geometry.Point3d.FromDynamo(point), motions, motionsPlasticLimits, rotations, rotationsPlasticLimits, identifier);
        }

        internal Autodesk.DesignScript.Geometry.Point GetDynamoGeometry()
        {
            return this.Position.ToDynamo();
        }
    }
}