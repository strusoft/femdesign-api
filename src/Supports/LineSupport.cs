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
        internal LineSupport(Geometry.Edge _edge, Releases.Motions motions, Releases.Rotations rotations, bool movingLocal, string identifier)
        {
            PointSupport.instance++; // PointSupport and LineSupport share the same instance counter.
            this.EntityCreated();
            this.name =  identifier + "." + PointSupport.instance.ToString();
            this.movingLocal = movingLocal;

            // orient edge
            _edge.OrientCoordinateSystemToGCS();

            // set edge specific properties
            this.group = new Group(_edge, motions, rotations);
            this.edge = _edge;
            this.normal = _edge.coordinateSystem.localZ;
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