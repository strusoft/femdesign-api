// https://strusoft.com/

using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    /// <summary>
    /// contour_type
    /// </summary>
    [System.Serializable]
    public class Contour
    {
        // [XmlIgnore]
        // public Geometry.FdVector3d LocalZ
        // {
        //     get
        //     {
        //         if (this.Edges.Count == 1)
        //         {
        //             return this.Edges[0].Normal;
        //         }
        //         else if (this.Edges.Count > 1)
        //         {
        //             // select points
        //             var points = this.Edges.Select(x => x.Points[x.Points.Count - 1]);
                    
        //             // find max value
        //             double max = points.Max(x => x.X);

        //         }
        //     }
        // }

        [XmlElement("edge")]
        public List<Edge> Edges = new List<Edge>(); // sequence: edge_type

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
            this.Edges = edges;
        }
    }
}