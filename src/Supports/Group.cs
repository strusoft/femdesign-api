// https://strusoft.com/

using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Supports
{
    /// <summary>
    /// support_rigidity_data_type --> group
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class Group
    {
        /// <summary>
        /// Local x is actual local x for points. For lines local x is local z??
        /// </summary>
        [XmlElement("local_x", Order = 1)]
        public Geometry.FdVector3d localX { get; set; }
        [XmlElement("local_y", Order = 2)]
        public Geometry.FdVector3d localY { get; set; }
        [XmlElement("rigidity", Order = 3)]
        public Releases.RigidityDataType3 rigidity { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Group()
        {
            
        }

        /// <summary>
        /// Constructor by edge and rigidity.
        /// </summary>
        internal Group(Geometry.Edge _edge, Releases.Motions motions, Releases.Rotations rotations)
        {
            this.localX = _edge.coordinateSystem.localZ;
            this.localY = _edge.coordinateSystem.localY;
            this.rigidity = Releases.RigidityDataType3.Define(motions, rotations);
        }

        /// <summary>
        /// Constructor by vectors and rigidity.
        /// </summary>
        public Group(Geometry.FdVector3d _localX, Geometry.FdVector3d _localY, Releases.Motions motions, Releases.Rotations rotations)
        {
            this.localX = _localX;
            this.localY = _localY;
            this.rigidity = Releases.RigidityDataType3.Define(motions, rotations);
        }
    }
}