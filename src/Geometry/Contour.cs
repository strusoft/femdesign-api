// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    /// <summary>
    /// contour_type
    /// </summary>
    [System.Serializable]
    public class Contour
    {
        [XmlElement("edge")]
        public List<Edge> edges = new List<Edge>(); // sequence: edge_type

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal Contour()
        {

        }

        /// <summary>
        /// Construct Contour from Edges.
        /// 
        /// Edges should form a closed contour.
        /// </summary>
        internal Contour(List<Edge> edges)
        {
            this.edges = edges;
        }
    }
}