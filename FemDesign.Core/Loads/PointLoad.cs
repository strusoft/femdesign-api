// https://strusoft.com/

using System.Xml.Serialization;
using FemDesign.Geometry;

namespace FemDesign.Loads
{
    /// <summary>
    /// point_load_type
    /// </summary>
    [System.Serializable]
    public partial class PointLoad: ForceLoadBase
    {
        [XmlElement("direction")]
        public Geometry.Vector3d Direction { get; set; } // point_type_3d
        [XmlElement("load")]
        public LoadLocationValue Load { get; set; } // location_value

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private PointLoad()
        {

        }

        /// <summary>
        /// Internal constructor accessed by static methods.
        /// </summary>
        public PointLoad(Geometry.Point3d point, Geometry.Vector3d force, LoadCase loadCase, string comment, ForceLoadType type)
        {
            this.EntityCreated();
            this.LoadCaseGuid = loadCase.Guid;
            this.LoadCase = loadCase;
            this.Comment = comment;
            this.LoadType = type;
            this.Direction = force.Normalize();
            this.Load = new LoadLocationValue(point, force.Length());
        }

        public static PointLoad Force(Geometry.Point3d point, Geometry.Vector3d force, LoadCase loadCase, string comment = "")
		{
            return new PointLoad(point, force, loadCase, comment, ForceLoadType.Force);
        }

        public static PointLoad Moment(Geometry.Point3d point, Geometry.Vector3d force, LoadCase loadCase, string comment = "")
        {
            return new PointLoad(point, force, loadCase, comment, ForceLoadType.Moment);
        }

        public override string ToString()
        {
            var units = this.LoadType == ForceLoadType.Force ? "kN" : "kNm";
            return $"{this.GetType().Name} Pos: ({this.Load.X.ToString("0.00")}, {this.Load.Y.ToString("0.00")}, {this.Load.Z.ToString("0.00")}), {this.LoadType}: {this.Direction * this.Load.Value} {units}, LoadCase: {this.LoadCase.Name}";
        }
    }
}