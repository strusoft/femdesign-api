// https://strusoft.com/

using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Supports
{
    /// <summary>
    /// point_support_type
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class PointSupport: EntityBase
    {
        [XmlIgnore]
        public static int instance = 0; // used for PointSupports and LineSupports
        [XmlAttribute("name")]
        public string name { get; set; } // identifier
        [XmlElement("group", Order = 1)]
        public Group group { get; set; }  // support_rigidity_data_type
        [XmlElement("position", Order = 2)]
        public Geometry.FdPoint3d position { get; set; } // point_type_3d
        public PointSupport()
        {
            // parameterless constructor for serialization
        }

        /// <summary>
        /// PointSupport at point with rigidity (motions, rotations). Group aligned with GCS.
        /// </summary>
        public PointSupport(Geometry.FdPoint3d point, Releases.Motions motions, Releases.Rotations rotations, string identifier)
        {
            instance++;
            this.EntityCreated();
            this.name = identifier + "." + instance.ToString();
            this.group = new Group(new Geometry.FdVector3d(1,0,0), new Geometry.FdVector3d(0,1,0), motions, rotations); // aligned with GCS
            this.position = point;
        }

        /// <summary>
        /// Rigid PointSupport at point.
        /// </summary>
        public static PointSupport Rigid(Geometry.FdPoint3d point, string identifier)
        {
            Releases.Motions motions = Releases.Motions.RigidPoint();
            Releases.Rotations rotations = Releases.Rotations.RigidPoint();
            return new PointSupport(point, motions, rotations, identifier);
        }

        /// <summary>
        /// Hinged PointSupport at point.
        /// </summary>
        public static PointSupport Hinged(Geometry.FdPoint3d point, string identifier)
        {
            Releases.Motions motions = Releases.Motions.RigidPoint();
            Releases.Rotations rotations = Releases.Rotations.Free();
            return new PointSupport(point, motions, rotations, identifier);
        }

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
            return this.position.ToDynamo();
        }

        #endregion
        #region grasshopper
        internal Rhino.Geometry.Point3d GetRhinoGeometry()
        {
            return this.position.ToRhino();
        }

        #endregion
    }
}