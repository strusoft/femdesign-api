// https://strusoft.com/

using System;
using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    [System.Serializable]
    public partial class Face
    {
        [XmlIgnore]
        public double Node1;
        [XmlIgnore]
        public double Node2;
        [XmlIgnore]
        public double Node3;
        [XmlIgnore]
        public double Node4;

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        public Face()
        {

        }

        /// <summary>
        /// Construct Face from index node1 node2 node3 node4.
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <param name="node3"></param>
        /// <param name="node4"></param>
        public Face(int node1, int node2, int node3, int node4)
        {
            this.Node1 = node1;
            this.Node2 = node2;
            this.Node3 = node3;
            this.Node4 = node4;
        }

        /// <summary>
        /// Construct Face from index node1 node2 node3 node4.
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <param name="node3"></param>
        /// <param name="node4"></param>
        public Face(int node1, int node2, int node3)
        {
            this.Node1 = node1;
            this.Node2 = node2;
            this.Node3 = node3;
        }
    }
}