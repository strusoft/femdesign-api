// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// point_load_type
    /// </summary>
    [System.Serializable]
    public class PointLoad: ForceLoadBase
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
        internal PointLoad(Geometry.FdPoint3d point, Geometry.FdVector3d force, LoadCase loadCase, string comment, string type)
        {
            this.EntityCreated();
            this.LoadCase = loadCase.Guid;
            this.Comment = comment;
            this.LoadType = type;
            this.Direction = force.Normalize();
            this.Load = new LoadLocationValue(point, force.Length());
        }

        
        #region grasshopper

        /// <summary>
        /// Convert PointLoad point to Rhino point.
        /// </summary>
        internal Rhino.Geometry.Point3d GetRhinoGeometry()
        {
            return this.Load.GetFdPoint().ToRhino();
        }
        #endregion
    }
}