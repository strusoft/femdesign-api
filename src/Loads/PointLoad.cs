// https://strusoft.com/

using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Loads
{
    /// <summary>
    /// point_load_type
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
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

        #region dynamo
        /// <summary>
        /// Create force point load.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="point">Point.</param>
        /// <param name="force">Force as Vector. Force x/y/z-components in kN.</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="comment">Comment.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static PointLoad Force(Autodesk.DesignScript.Geometry.Point point, Autodesk.DesignScript.Geometry.Vector force, LoadCase loadCase, string comment = "")
        {
            var p0 = Geometry.FdPoint3d.FromDynamo(point);
            var v0 = Geometry.FdVector3d.FromDynamo(force);
            PointLoad pointLoad = new PointLoad(p0, v0, loadCase, comment, "force");
            return pointLoad;
        }
        /// <summary>
        /// Create moment point load.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="point">Point.</param>
        /// <param name="moment">Moment as Vector. Moment x/y/z components in kN/m.</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="comment">Comment.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static PointLoad Moment(Autodesk.DesignScript.Geometry.Point point, Autodesk.DesignScript.Geometry.Vector moment, LoadCase loadCase, string comment = "")
        {
            var p0 = Geometry.FdPoint3d.FromDynamo(point);
            var v0 = Geometry.FdVector3d.FromDynamo(moment);
            PointLoad pointLoad = new PointLoad(p0, v0, loadCase, comment, "moment");
            return pointLoad;
        }

        /// <summary>
        /// Convert PointLoad point to Dynamo point.
        /// </summary>
        internal Autodesk.DesignScript.Geometry.Point GetDynamoGeometry()
        {
            return this.Load.GetFdPoint().ToDynamo();
        }
        #endregion
        
    }
}