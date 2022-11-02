using System;
using System.Xml.Serialization;
using Autodesk.DesignScript.Runtime;
using FemDesign.Releases;


namespace FemDesign.Supports
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class LineSupport: NamedEntityBase
    {
        /// <summary>
        /// Create a rigid line support element.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve"></param>
        /// <param name="movingLocal">LCS changes direction along line?</param>
        /// <param name="localY">Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS</param>
        /// <param name="orientLCS">Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.</param>
        /// <param name="identifier">Identifier. Optional, default value if undefined.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static LineSupport Rigid(Autodesk.DesignScript.Geometry.Curve curve, [DefaultArgument("false")] bool movingLocal, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localY, [DefaultArgument("true")] bool orientLCS,  [DefaultArgument("S")] string identifier)
        {
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc1(curve);
            FemDesign.Supports.LineSupport obj = LineSupport.Rigid(edge, movingLocal, identifier);

            // set local y-axis
            if (!localY.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                obj.Group.LocalY = FemDesign.Geometry.Vector3d.FromDynamo(localY);
            }

            // else orient coordinate system to GCS
            else
            {
                if (orientLCS)
                {  
                    obj.Group.OrientCoordinateSystemToGCS();
                }
            }

            return obj;
        }

        /// <summary>
        /// Create a hinged line support element.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve"></param>
        /// <param name="movingLocal">LCS changes direction along line?</param>
        /// <param name="localY">Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS</param>
        /// <param name="orientLCS">Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.</param>
        /// <param name="identifier">Identifier. Optional, default value if undefined.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static LineSupport Hinged(Autodesk.DesignScript.Geometry.Curve curve, [DefaultArgument("false")] bool movingLocal, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localY, [DefaultArgument("true")] bool orientLCS, [DefaultArgument("S")] string identifier)
        {
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc1(curve);
            FemDesign.Supports.LineSupport obj = LineSupport.Hinged(edge, movingLocal, identifier);

            // set local y-axis
            if (!localY.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                obj.Group.LocalY = FemDesign.Geometry.Vector3d.FromDynamo(localY);
            }

            // else orient coordinate system to GCS
            else
            {
                if (orientLCS)
                {  
                    obj.Group.OrientCoordinateSystemToGCS();
                }
            }

            return obj;
        }

        /// <summary>
        /// Define a LineSupport.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve"></param>
        /// <param name="motions">Motions. Translation releases.</param>
        /// <param name="rotations">Rotations. Rotation releases.</param>
        /// <param name="movingLocal">LCS changes direction along line?</param>
        /// <param name="localY">Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS</param>
        /// <param name="orientLCS">Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.</param>
        /// <param name="identifier">Identifier. Optional, default value if undefined.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static LineSupport Define(Autodesk.DesignScript.Geometry.Curve curve, Motions motions, [DefaultArgument("MotionsPlasticLimits.Default()")] MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, [DefaultArgument("RotationsPlasticLimits.Default()")] RotationsPlasticLimits rotationsPlasticLimits, [DefaultArgument("false")] bool movingLocal, [DefaultArgument("Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)")] Autodesk.DesignScript.Geometry.Vector localY, [DefaultArgument("true")] bool orientLCS, [DefaultArgument("S")] string identifier)
        {
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc1(curve);
            FemDesign.Supports.LineSupport obj = new LineSupport(edge, motions, motionsPlasticLimits, rotations, rotationsPlasticLimits, movingLocal, identifier);

            // set local y-axis
            if (!localY.Equals(Autodesk.DesignScript.Geometry.Vector.ByCoordinates(0,0,0)))
            {
                obj.Group.LocalY = FemDesign.Geometry.Vector3d.FromDynamo(localY);
            }

            // else orient coordinate system to GCS
            else
            {
                if (orientLCS)
                {  
                    obj.Group.OrientCoordinateSystemToGCS();
                }
            }

            return obj;
        }

        internal Autodesk.DesignScript.Geometry.Curve GetDynamoGeometry()
        {
            return this.Edge.ToDynamo();
        }
    }
}