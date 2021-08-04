// https://strusoft.com/

using System.Xml.Serialization;
using FemDesign.GenericClasses;
using FemDesign.Releases;

namespace FemDesign.Supports
{
    /// <summary>
    /// point_support_type
    /// </summary>
    [System.Serializable]
    public partial class PointSupport: EntityBase, IStructureElement, ISupportElement
    {
        [XmlIgnore]
        public static int _instance = 0; // used for PointSupports and LineSupports
        [XmlAttribute("name")]
        public string Name { get; set; } // identifier
        [XmlElement("group", Order = 1)]
        public Group Group { get; set; }  // support_rigidity_data_type
        [XmlElement("position", Order = 2)]
        public Geometry.FdPoint3d Position { get; set; } // point_type_3d
        public Motions Motions { get { return Group?.Rigidity?.Motions; } }
        public MotionsPlasticLimits MotionsPlasticityLimits { get { return Group?.Rigidity?.PlasticLimitForces; } }
        public Rotations Rotations { get { return Group?.Rigidity?.Rotations; } }
        public RotationsPlasticLimits RotationsPlasticityLimits { get { return Group?.Rigidity?.PlasticLimitMoments; } }
        
        public PointSupport()
        {
            // parameterless constructor for serialization
        }

        /// <summary>
        /// PointSupport at point with rigidity (motions, rotations). Group aligned with GCS.
        /// </summary>
        public PointSupport(Geometry.FdPoint3d point, Motions motions, Rotations rotations, string identifier = "S")
        {
            var group = new Group(Geometry.FdVector3d.UnitX(), Geometry.FdVector3d.UnitY(), motions, rotations);
            Initialize(point, group, identifier);
        }

        /// <summary>
        /// PointSupport at point with rigidity (motions, rotations) and plastic limits (forces, moments). Group aligned with GCS.
        /// </summary>
        public PointSupport(Geometry.FdPoint3d point, Motions motions, MotionsPlasticLimits motionsPlasticLimits, Rotations rotations, RotationsPlasticLimits rotationsPlasticLimits, string identifier = "S")
        {
            var group = new Group(Geometry.FdVector3d.UnitX(), Geometry.FdVector3d.UnitY(), motions, motionsPlasticLimits, rotations, rotationsPlasticLimits);
            Initialize(point, group, identifier);
        }

        private void Initialize(Geometry.FdPoint3d point, Group group, string identifier)
        {
            PointSupport._instance++;
            this.EntityCreated();
            this.Name = identifier + "." + PointSupport._instance.ToString();
            this.Group = group;
            this.Position = point;
        }

        /// <summary>
        /// Rigid PointSupport at point.
        /// </summary>
        public static PointSupport Rigid(Geometry.FdPoint3d point, string identifier = "S")
        {
            Motions motions = Motions.RigidPoint();
            Rotations rotations = Rotations.RigidPoint();
            return new PointSupport(point, motions, rotations, identifier);
        }

        /// <summary>
        /// Hinged PointSupport at point.
        /// </summary>
        public static PointSupport Hinged(Geometry.FdPoint3d point, string identifier = "S")
        {
            Releases.Motions motions = Releases.Motions.RigidPoint();
            Releases.Rotations rotations = Releases.Rotations.Free();
            return new PointSupport(point, motions, rotations, identifier);
        }

    }
}