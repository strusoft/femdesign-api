// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Supports
{
    /// <summary>
    /// point_support_type
    /// </summary>
    [System.Serializable]
    public class PointSupport: EntityBase
    {
        [XmlIgnore]
        public static int _instance = 0; // used for PointSupports and LineSupports
        [XmlAttribute("name")]
        public string Name { get; set; } // identifier
        [XmlElement("group", Order = 1)]
        public Group Group { get; set; }  // support_rigidity_data_type
        [XmlElement("position", Order = 2)]
        public Geometry.FdPoint3d Position { get; set; } // point_type_3d
        public PointSupport()
        {
            // parameterless constructor for serialization
        }

        /// <summary>
        /// PointSupport at point with rigidity (motions, rotations). Group aligned with GCS.
        /// </summary>
        public PointSupport(Geometry.FdPoint3d point, Releases.Motions motions, Releases.Rotations rotations, string identifier)
        {
            PointSupport._instance++;
            this.EntityCreated();
            this.Name = identifier + "." + PointSupport._instance.ToString();
            this.Group = new Group(new Geometry.FdVector3d(1,0,0), new Geometry.FdVector3d(0,1,0), motions, rotations); // aligned with GCS
            this.Position = point;
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

        #region grasshopper
        internal Rhino.Geometry.Point3d GetRhinoGeometry()
        {
            return this.Position.ToRhino();
        }

        #endregion
    }
}