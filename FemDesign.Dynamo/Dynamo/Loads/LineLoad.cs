
using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Loads
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class LineLoad: ForceLoadBase
    {
        /// <summary>
        /// Create a uniform force line load.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve">Curve defining the line load.</param>
        /// <param name="force">Force at start and end.</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="constLoadDir">Bool. Constant load direction? If true direction of load will be constant along action line. If false direction will vary along action line - characteristic direction is in the middle point of line. Optional.</param>
        /// <param name="comment">Comment.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static LineLoad ForceUniform(Autodesk.DesignScript.Geometry.Curve curve, Autodesk.DesignScript.Geometry.Vector force, LoadCase loadCase, [DefaultArgument("true")] bool constLoadDir, string comment = "")
        {
            // convert geometry
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc1(curve);
            Geometry.Vector3d _startForce = Geometry.Vector3d.FromDynamo(force);
            Geometry.Vector3d _endForce = _startForce;

            // check zero vector
            if (_startForce.IsZero())
            {
                throw new System.ArgumentException($"Force is zero.");
            }

            //
            return new LineLoad(edge, _startForce, _endForce, loadCase, ForceLoadType.Force, comment, constLoadDir, loadProjection: false);
        }

        /// <summary>
        /// Create a uniform or non-uniform force line load.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve">Curve defining the line load.</param>
        /// <param name="startForce">Force at start. The start force will define the direction of the line load.</param>
        /// <param name="endForce">Force at end.</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="constLoadDir">Bool. Constant load direction? If true direction of load will be constant along action line. If false direction will vary along action line - characteristic direction is in the middle point of line. Optional.</param>
        /// <param name="comment">Comment.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static LineLoad Force(Autodesk.DesignScript.Geometry.Curve curve, Autodesk.DesignScript.Geometry.Vector startForce, Autodesk.DesignScript.Geometry.Vector endForce, LoadCase loadCase, [DefaultArgument("true")] bool constLoadDir, string comment = "")
        {
            // convert geometry
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc1(curve);
            Geometry.Vector3d _startForce = Geometry.Vector3d.FromDynamo(startForce);
            Geometry.Vector3d _endForce = Geometry.Vector3d.FromDynamo(endForce);

            //
            return new LineLoad(edge, _startForce,  _endForce, loadCase, ForceLoadType.Force, comment, constLoadDir, loadProjection: false);
        }

        /// <summary>
        /// Create a uniform moment line load.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve">Curve defining the line load.</param>
        /// <param name="force">Force (moment) at start and end</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="constLoadDir">Bool. Constant load direction? If true direction of load will be constant along action line. If false direction will vary along action line - characteristic direction is in the middle point of line. Optional.</param>
        /// <param name="comment">Comment.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static LineLoad MomentUniform(Autodesk.DesignScript.Geometry.Curve curve, Autodesk.DesignScript.Geometry.Vector force, LoadCase loadCase, [DefaultArgument("true")] bool constLoadDir, string comment = "")
        {
            // convert geometry
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc1(curve);
            Geometry.Vector3d _startForce = Geometry.Vector3d.FromDynamo(force);
            Geometry.Vector3d _endForce = _startForce;

            // check zero vector
            if (_startForce.IsZero())
            {
                throw new System.ArgumentException($"Force is zero.");
            }

            return new LineLoad(edge, _startForce, _endForce, loadCase, ForceLoadType.Moment, comment, constLoadDir, loadProjection: false);
        }

        /// <summary>
        /// Create a uniform or non-uniform moment line load.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve">Curve defining the line load.</param>
        /// <param name="startForce">Force (moment) at start. The start force will define the direction of the line load.</param>
        /// <param name="endForce">Force (moment) at end.</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="constLoadDir">Bool. Constant load direction? If true direction of load will be constant along action line. If false direction will vary along action line - characteristic direction is in the middle point of line. Optional.</param>
        /// <param name="comment">Comment.</param>
        [IsVisibleInDynamoLibrary(true)]
        public static LineLoad Moment(Autodesk.DesignScript.Geometry.Curve curve, Autodesk.DesignScript.Geometry.Vector startForce, Autodesk.DesignScript.Geometry.Vector endForce, LoadCase loadCase, [DefaultArgument("true")] bool constLoadDir, string comment = "")
        {
            // convert geometry
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc1(curve);
            Geometry.Vector3d _startForce = Geometry.Vector3d.FromDynamo(startForce);
            Geometry.Vector3d _endForce = Geometry.Vector3d.FromDynamo(endForce);

            return new LineLoad(edge, _startForce, _endForce, loadCase, ForceLoadType.Moment, comment, constLoadDir, loadProjection: false);
        }

        /// <summary>
        /// Convert LineLoad edge to Dynamo curve.
        /// </summary>
        internal Autodesk.DesignScript.Geometry.Curve GetDynamoGeometry()
        {
            return this.Edge.ToDynamo();
        }
    }
}