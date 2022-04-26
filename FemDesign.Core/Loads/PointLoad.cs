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
        }

        
    }
}