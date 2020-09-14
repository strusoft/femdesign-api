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
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class LineSupport: EntityBase
    {

        // serialization properties
        [XmlAttribute("name")]
        public string Name { get; set; } // identifier. Default = S
        [XmlAttribute("moving_local")]
        public bool MovingLocal { get; set; } // bool
        [XmlElement("group", Order = 1)]
        public Group Group { get; set; } // support_rigidity_data_type
        [XmlElement("edge", Order = 2)]
        public Geometry.Edge Edge { get; set; } // edge_type
        [XmlElement("normal", Order = 3)]
        public Geometry.FdVector3d Normal { get; set; } // point_type_3d
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private LineSupport()
        {
            
        }

        /// <summary>
        /// LineSupport along edge with rigidity (motions, rotations). Group LCS aligned with edge LCS.
        /// </summary>
        internal LineSupport(Geometry.Edge edge, Releases.Motions motions, Releases.Rotations rotations, bool movingLocal, string identifier)
        {
            PointSupport._instance++; // PointSupport and LineSupport share the same instance counter.
            this.EntityCreated();
            this.Name =  identifier + "." + PointSupport._instance.ToString();
            this.MovingLocal = movingLocal;

            // orient edge
            edge.OrientCoordinateSystemToGCS();

            // set edge specific properties
            this.Group = new Group(edge, motions, rotations);
            this.Edge = edge;
            this.Normal = edge.CoordinateSystem.LocalZ;
        }

        /// <summary>
        /// Rigid LineSupport along edge.
        /// </summary>
        internal static LineSupport Rigid(Geometry.Edge edge, bool movingLocal, string identifier)
        {
            Releases.Motions motions = Releases.Motions.RigidLine();
            Releases.Rotations rotations = Releases.Rotations.RigidLine();
            return new LineSupport(edge, motions, rotations, movingLocal, identifier);
        }

        /// <summary>
        /// Hinged LineSupport along edge.
        /// </summary>
        internal static LineSupport Hinged(Geometry.Edge edge, bool movingLocal, string identifier)
        {
            Releases.Motions motions = Releases.Motions.RigidLine();
            Releases.Rotations rotations = Releases.Rotations.Free();
            return new LineSupport(edge, motions, rotations, movingLocal, identifier);
        }

        #region dynamo
        /// <summary>
        /// Create a rigid line support element.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve"></param>
        /// <param name="movingLocal">LCS changes direction along line?</param>
        /// <param name="identifier">Identifier. Optional, default value if undefined.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static LineSupport Rigid(Autodesk.DesignScript.Geometry.Curve curve, [DefaultArgument("false")] bool movingLocal, [DefaultArgument("S")] string identifier)
        {
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc1(curve);
            return LineSupport.Rigid(edge, movingLocal, identifier);
        }

        /// <summary>
        /// Create a hinged line support element.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve"></param>
        /// <param name="movingLocal">LCS changes direction along line?</param>
        /// <param name="identifier">Identifier. Optional, default value if undefined.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static LineSupport Hinged(Autodesk.DesignScript.Geometry.Curve curve, [DefaultArgument("false")] bool movingLocal, [DefaultArgument("S")] string identifier)
        {
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc1(curve);
            return LineSupport.Hinged(edge, movingLocal, identifier);
        }

        /// <summary>
        /// Define a LineSupport.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="curve"></param>
        /// <param name="motions">Motions. Translation releases.</param>
        /// <param name="rotations">Rotations. Rotation releases.</param>
        /// <param name="movingLocal">LCS changes direction along line?</param>
        /// <param name="identifier">Identifier. Optional, default value if undefined.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static LineSupport Define(Autodesk.DesignScript.Geometry.Curve curve, Releases.Motions motions, Releases.Rotations rotations, [DefaultArgument("false")] bool movingLocal, [DefaultArgument("S")] string identifier)
        {
            Geometry.Edge edge = Geometry.Edge.FromDynamoLineOrArc1(curve);
            return new LineSupport(edge, motions, rotations, movingLocal, identifier);
        }

        internal Autodesk.DesignScript.Geometry.Curve GetDynamoGeometry()
        {
            return this.Edge.ToDynamo();
        }
        
        #endregion
    }
}