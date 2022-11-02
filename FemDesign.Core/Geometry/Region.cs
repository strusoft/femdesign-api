// https://strusoft.com/
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Geometry
{
    /// <summary>
    /// region_type
    /// 
    /// Surfaces in FEM-Design are expressed as regions of contours (outlines). This extended region also contains a LCS to keep track of directions.
    /// </summary>
    [System.Serializable]
    public partial class Region
    {
        [XmlIgnore]
        public Geometry.CoordinateSystem CoordinateSystem { get; set; }

        /// <summary>
        /// Used for panels and sections
        /// </summary>
        [XmlIgnore]
        public Vector3d LocalZ
        {
            get
            {
                return this.Contours[0].LocalZ;
            }
            
            set
            {
                int par = value.Parallel(this.LocalZ);
                if (par == 1)
                {
                    // pass
                }
                
                else if (par == -1)
                {
                    this.Reverse();
                }

                else
                {
                    Vector3d v = this.LocalZ;
                    throw new System.ArgumentException($"Value: ({value.X}, {value.Y}, {value.Z}) is not parallell to LocalZ ({v.X}, {v.Y}, {v.Z}) ");
                }
            }
        }

        [XmlElement("contour")]
        public List<Contour> Contours = new List<Contour>(); // sequence: contour_type

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        internal Region()
        {
            
        }

        public Region(List<Contour> contours)
        {
            this.Contours = contours;
        }

        public Region(List<Contour> contours, CoordinateSystem coordinateSystem)
        {
            this.Contours = contours;
            this.CoordinateSystem = coordinateSystem;
        }

        /// <summary>
        /// Create region by points and coordinate system.
        /// </summary>
        /// <param name="points">List of sorted points defining the outer perimeter of the region.</param>
        /// <param name="coordinateSystem">Coordinate system of the region</param>
        public Region(List<Point3d> points, CoordinateSystem coordinateSystem)
        {
            // edge normal
            Vector3d edgeLocalY = coordinateSystem.LocalZ;

            List<Edge> edges = new List<Edge>();
            for (int idx = 0 ; idx < points.Count; idx++)
            {
                // startPoint
                Point3d p0 = p0 = points[idx];

                // endPoint
                Point3d p1;
                if (idx != points.Count - 1)
                {
                    p1 = points[idx + 1];
                }

                else
                {
                    p1 = points[0];
                }

                // create edge
                edges.Add(new Edge(p0, p1, edgeLocalY));
            }
            
            // create contours
            Contour contour = new Contour(edges);

            // set properties
            this.Contours = new List<Contour>{contour};
            this.CoordinateSystem = coordinateSystem;
        }

        public static Region RectangleXZ(double width, double height)
        {
            var points0 = new Point3d(0,0,0);
            var points1 = new Point3d(width,0,0);
            var points2 = new Point3d(width,0,height);
            var points3 = new Point3d(0,0,height);

            var points = new List<Point3d>() { points0, points1, points2, points3 };

            var fdCoordinate = new CoordinateSystem(points0, points1, points3);

            // set properties
            var region = new Region(points, fdCoordinate);

            return region;
        }

        public static Region RectangleXY(Point3d corner, double widthX, double widthY)
        {
            var points0 = corner + new Vector3d(0, 0, 0);
            var points1 = corner + new Vector3d(widthX, 0, 0);
            var points2 = corner + new Vector3d(widthX, widthY, 0);
            var points3 = corner + new Vector3d(0, widthY, 0);

            var points = new List<Point3d>() { points0, points1, points2, points3 };

            var fdCoordinate = new CoordinateSystem(points0, points1, points3);

            // set properties
            var region = new Region(points, fdCoordinate);

            return region;
        }


        /// <summary>
        /// Reverse the contours in this region
        /// </summary>
        public void Reverse()
        {
            foreach (Contour contour in this.Contours)
            {
                contour.Reverse();
            }
        }
        
        /// <summary>
        /// Get region from a Slab.
        /// </summary>
        public static Region FromSlab(Shells.Slab slab)
        {
            return slab.SlabPart.Region;
        }

        /// <summary>
        /// Set EdgeConnection on Edge in region by index.
        /// </summary>
        public void SetEdgeConnection(Shells.EdgeConnection edgeConnection, int index)
        {
            int edgeIdx = 0;
            foreach (Contour contour in this.Contours)
            {
                if (contour.Edges != null)
                {
                    int cInstance = 0;
                    foreach (Edge edge in contour.Edges)
                    {
                        cInstance++;
                        if (index == edgeIdx)
                        {
                            if (!edgeConnection.Release)
                            {
                                edge.EdgeConnection = null;
                            }
                            else
                            {
                                string name = edgeConnection.Name == null ? "CE." + cInstance.ToString() : edgeConnection.Name;

                                Shells.EdgeConnection ec = Shells.EdgeConnection.CopyExisting(edgeConnection, name);
                                edge.EdgeConnection = ec;
                            }
                            return;
                        }
                        edgeIdx++;
                    }
                }
                else
                {
                    throw new System.ArgumentException("No edges in contour!");
                }
            }

            // edge not found
            throw new System.ArgumentException("Edge not found.");
        }

        /// <summary>
        /// Set EdgeConnection on all Edges in Region.
        /// </summary>
        public void SetEdgeConnections(Shells.EdgeConnection edgeConnection)
        {
            if (edgeConnection.Release)
            {
                foreach (Contour contour in this.Contours)
                {
                    if (contour.Edges != null)
                    {
                        int cInstance = 0;
                        foreach (Edge edge in contour.Edges)
                        {
                            cInstance++;
                            string name = "CE." + cInstance.ToString();
                            Shells.EdgeConnection ec = Shells.EdgeConnection.CopyExisting(edgeConnection, name);
                            edge.EdgeConnection = ec;
                        }
                    }
                    else
                    {
                        throw new System.ArgumentException("No edges in contour!");
                    }
                }
            }
            else
            {
                // don't modify edges if no releases on edgeConnection.
            }
        }

        /// <summary>
        /// Get all EdgeConnection from all Edges in Region.
        /// </summary>
        public List<Shells.EdgeConnection> GetEdgeConnections()
        {
            var edgeConnections = new List<Shells.EdgeConnection>();
            foreach (Contour contour in this.Contours)
            {
                foreach (Edge edge in contour.Edges)
                {
                    edgeConnections.Add(edge.EdgeConnection);
                }
            }
            return edgeConnections;
        }

        /// <summary>
        /// Get all PredefinedRigidities from all Edges in Region
        /// </summary>
        /// <returns></returns>
        public List<Releases.RigidityDataLibType3> GetPredefinedRigidities()
        {
            List<Releases.RigidityDataLibType3> predefRigidities = new List<Releases.RigidityDataLibType3>();
            foreach (Contour contour in this.Contours)
            {
                foreach(Edge edge in contour.Edges)
                {
                    if (edge.EdgeConnection != null)
                    {
                        if (edge.EdgeConnection.PredefRigidity != null)
                        {
                            predefRigidities.Add(edge.EdgeConnection.PredefRigidity);
                        }
                    }
                    
                }
            }
            return predefRigidities;
        }

        /// <summary>
        /// Set line connection types (i.e predefined line connection type) on region edges
        /// </summary>
        public void SetPredefinedRigidities(List<Releases.RigidityDataLibType3> predefinedTypes)
        {
            foreach (Geometry.Contour contour in this.Contours)
            {
                foreach (Geometry.Edge edge in contour.Edges)
                {
                    if (edge.EdgeConnection != null)
                    {
                        if (edge.EdgeConnection._predefRigidityRef != null)
                        {
                            foreach (Releases.RigidityDataLibType3 predefinedType in predefinedTypes)
                            {
                                // add predefined type to edge connection if edge connection predef rigidity reference matches guid of predefine type
                                if (edge.EdgeConnection._predefRigidityRef.Guid == predefinedType.Guid)
                                {
                                    edge.EdgeConnection.PredefRigidity = predefinedType;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns a new instance of region, without any EdgeConnections.
        /// </summary>
        /// <returns></returns>
        public Region RemoveEdgeConnections()
        {
            Region newRegion = this.DeepClone();
            foreach (Contour newContour in newRegion.Contours)
            {
                foreach (Edge newEdge in newContour.Edges)
                {
                    if (newEdge.EdgeConnection != null)
                    {
                        newEdge.EdgeConnection = null;
                    }
                }
            }
            return newRegion;
        }

        public static double Area(List<Point3d> points)
        {
            var area = System.Math.Abs(points.Take(points.Count - 1).Select((p, i) => (points[i + 1].X - p.X) * (points[i + 1].Y + p.Y)).Sum() / 2);
            return area;
        }

    }
}