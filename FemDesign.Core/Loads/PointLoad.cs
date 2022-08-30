// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// point_load_type
    /// </summary>
    [System.Serializable]
    public partial class PointLoad: ForceLoadBase
    {
        [XmlElement("direction")]
        public Geometry.FdVector3d Direction { get; set; } // point_type_3d
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
        public PointLoad(Geometry.FdPoint3d point, Geometry.FdVector3d force, LoadCase loadCase, string comment, ForceLoadType type)
        {
            this.EntityCreated();
            this.LoadCase = loadCase.Guid;
            this.Comment = comment;
            this.LoadType = type;
            this.Direction = force.Normalize();
            this.Load = new LoadLocationValue(point, force.Length());
            this.LoadCaseObj = loadCase;
        }

        public override string ToString()
        {
            var units = this.LoadType == ForceLoadType.Force ? "kN" : "kNm";
            return $"{this.GetType().Name} Pos: ({this.Load.X}, {this.Load.Y}, {this.Load.Z}), {this.LoadType}: {this.Direction * this.Load.Value} {units}, LoadCase: {this.LoadCaseObj.Identifier}";
        }
    }
}