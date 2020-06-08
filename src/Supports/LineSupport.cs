// https://strusoft.com/

using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Supports
{
    /// <summary>
    /// line_support_type
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class LineSupport: EntityBase
    {

        // serialization properties
        [XmlAttribute("name")]
        public string name { get; set; } // identifier. Default = S
        [XmlAttribute("moving_local")]
        public bool movingLocal { get; set; } // bool
        [XmlElement("group", Order = 1)]
        public Group group { get; set; } // support_rigidity_data_type
        [XmlElement("edge", Order = 2)]
        public Geometry.Edge edge { get; set; } // edge_type
        [XmlElement("normal", Order = 3)]
        public Geometry.FdVector3d normal { get; set; } // point_type_3d
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LineSupport()
        {
            
        }

        /// <summary>
        /// LineSupport along edge with rigidity (motions, rotations). Group LCS aligned with edge LCS.
        /// </summary>
        internal LineSupport(Geometry.Edge _edge, Releases.Motions motions, Releases.Rotations rotations, bool movingLocal)
        {
            PointSupport.instance++; // PointSupport and LineSupport share the same instance counter.
            this.EntityCreated();
            this.name = "S." + PointSupport.instance.ToString();
            this.movingLocal = movingLocal;
            this.group = new Group(_edge, motions, rotations);
            this.edge = _edge;
            this.normal = _edge.coordinateSystem.localZ;
        }

        /// <summary>
        /// Rigid LineSupport along edge.
        /// </summary>
        internal static LineSupport Rigid(Geometry.Edge edge, bool movingLocal = false)
        {
            Releases.Motions motions = Releases.Motions.RigidLine();
            Releases.Rotations rotations = Releases.Rotations.RigidLine();
            return new LineSupport(edge, motions, rotations, movingLocal);
        }

        /// <summary>
        /// Hinged LineSupport along edge.
        /// </summary>
        internal static LineSupport Hinged(Geometry.Edge edge, bool movingLocal = false)
        {
            Releases.Motions motions = Releases.Motions.RigidLine();
            Releases.Rotations rotations = Releases.Rotations.Free();
            return new LineSupport(edge, motions, rotations, movingLocal);
        }

        #region dynamo
        /// <summary>
        /// Create a rigid line support element.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve"></param>
        /// <param name="movingLocal">LCS changes direction along line?</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static LineSupport Rigid(Autodesk.DesignScript.Geometry.Curve curve, bool movingLocal = false)
        {
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc1(curve);
            return LineSupport.Rigid(edge, movingLocal);
        }

        /// <summary>
        /// Create a hinged line support element.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve"></param>
        /// <param name="movingLocal">LCS changes direction along line?</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static LineSupport Hinged(Autodesk.DesignScript.Geometry.Curve curve, bool movingLocal = false)
        {
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc1(curve);
            return LineSupport.Hinged(edge, movingLocal);
        }

        /// <summary>
        /// Define a LineSupport.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve"></param>
        /// <param name="motions">Motions. Translation releases.</param>
        /// <param name="rotations">Rotations. Rotation releases.</param>
        /// <param name="movingLocal">LCS changes direction along line?</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static LineSupport Define(Autodesk.DesignScript.Geometry.Curve curve, Releases.Motions motions, Releases.Rotations rotations, bool movingLocal = false)
        {
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc1(curve);
            return new LineSupport(edge, motions, rotations, movingLocal);
        }

        internal Autodesk.DesignScript.Geometry.Curve GetDynamoGeometry()
        {
            return this.edge.ToDynamo();
        }
        
        #endregion
        #region grasshopper
        internal Rhino.Geometry.Curve GetRhinoGeometry()
        {
            return this.edge.ToRhino();
        }
        #endregion
    }
}