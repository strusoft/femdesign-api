// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    /// <summary>
    /// contour_type
    /// </summary>
    [System.Serializable]
    public partial class Contour
    {

        /// <summary>
        /// Get LocalZ of Contour by analysing the direction of the contour.
        /// </summary>
        [XmlIgnore]
        internal Geometry.Vector3d LocalZ
        {
            get
            {
                // contour is circle
                if (this.Edges.Count == 0)
                {
                    throw new System.ArgumentException("Contour contains no edges.");
                }

                else if (this.Edges.Count == 1)
                {
                    // return normal
                    return this.Edges[0].Normal;
                }

                else
                {
                    // any arcs in contour?
                    Edge firstArc = this.Edges.Find(item => item.Type == "arc");
                    if (firstArc != null)
                    {
                        // return normal
                        return firstArc.Normal;
                    }

                    // no arcs in contour?
                    else
                    {
                        // select points
                        var points = this.Edges.Select(x => x.Points.Last());
                        
                        // get ranges
                        double rangeX = points.Max(item => item.X) - points.Min(item => item.X);
                        double rangeY = points.Max(item => item.Y) - points.Min(item => item.Y);
                        double rangeZ = points.Max(item => item.Y) - points.Min(item => item.Z);

                        
                        // get index of edge with outermost point
                        int idx;
                        if (rangeX != 0)
                        {
                            idx = this.Edges.IndexOf(this.Edges.OrderBy(item => item.Points.Last().X).Last());
                        }
                        else if (rangeY != 0)
                        {
                            idx = this.Edges.IndexOf(this.Edges.OrderBy(item => item.Points.Last().Y).Last());
                        }
                        else if (rangeZ != 0)
                        {
                            idx = this.Edges.IndexOf(this.Edges.OrderBy(item => item.Points.Last().Z).Last());
                        }
                        else
                        {
                            throw new System.ArgumentException("Could not get index of edge with outermost point.");
                        }

                        // get edges
                        Edge[] edges = new Edge[2];
                        if (idx == 0)
                        {
                            edges[0] = this.Edges[0];
                            edges[1] = this.Edges[1];
                        }
                        else if (idx == this.Edges.Count - 1)
                        {
                            edges[0] = this.Edges.Last();
                            edges[1] = this.Edges[0];
                        }
                        else
                        {
                            edges[0] = this.Edges[idx];
                            edges[1] = this.Edges[idx + 1];
                        }

                        // construct vector
                        Vector3d v0 = new Vector3d(edges[0].Points[0], edges[0].Points[1]);
                        Vector3d v1 = new Vector3d(edges[1].Points[0], edges[1].Points[1]);

                        // return normal
                        return v0.Cross(v1).Normalize();
                    }   
                }
            }
        }

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
        public Contour(List<Edge> edges)
        {
            this.Edges = edges;
        }

        /// <summary>
        /// Reverse direction of edges in this contour
        /// </summary>
        internal void Reverse()
        {
            // reverse every edge
            foreach (Edge edge in this.Edges)
            {
                edge.Reverse();
            }

            // reverse list of edges
            this.Edges.Reverse();
        }
    }
}