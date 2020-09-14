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
        public Geometry.FdVector3d LocalX { get; set; }
        [XmlElement("local_y", Order = 2)]
        public Geometry.FdVector3d LocalY { get; set; }
        [XmlElement("rigidity", Order = 3)]
        public Releases.RigidityDataType3 Rigidity { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Group()
        {
            
        }

        /// <summary>
        /// Constructor by edge and rigidity.
        /// </summary>
        internal Group(Geometry.Edge edge, Releases.Motions motions, Releases.Rotations rotations)
        {
            this.LocalX = edge.CoordinateSystem.LocalZ;
            this.LocalY = edge.CoordinateSystem.LocalY;
            this.Rigidity = Releases.RigidityDataType3.Define(motions, rotations);
        }

        /// <summary>
        /// Constructor by vectors and rigidity.
        /// </summary>
        public Group(Geometry.FdVector3d localX, Geometry.FdVector3d localY, Releases.Motions motions, Releases.Rotations rotations)
        {
            this.LocalX = localX;
            this.LocalY = localY;
            this.Rigidity = Releases.RigidityDataType3.Define(motions, rotations);
        }
    }
}