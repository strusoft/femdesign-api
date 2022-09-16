
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Loads
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class PointLoad: ForceLoadBase
    {
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
            var p0 = Geometry.Point3d.FromDynamo(point);
            var v0 = Geometry.Vector3d.FromDynamo(force);
            PointLoad pointLoad = new PointLoad(p0, v0, loadCase, comment, ForceLoadType.Force);
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
            var p0 = Geometry.Point3d.FromDynamo(point);
            var v0 = Geometry.Vector3d.FromDynamo(moment);
            PointLoad pointLoad = new PointLoad(p0, v0, loadCase, comment, ForceLoadType.Moment);
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