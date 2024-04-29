// https://strusoft.com/
using System;
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
        public Plane Plane { get; set; }

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
                int par = value.IsParallel(this.LocalZ);
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

        [XmlIgnore]
        public List<Contour> Contours
        {
            get
            {
                if (this._contours == null)
                {
                    var contours = new List<Contour>();
                    return contours;
                }
                return this._contours;
            }
            set { this._contours = value; }
        }

        [XmlElement("contour")]
        public List<Contour> _contours { get; set; }

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

        public Region(List<Contour> contours, Plane plane)
        {
            this.Contours = contours;
            this.Plane = plane;
        }

        public bool IsPlanar
        {
            get
            {
                var points = new List<Geometry.Point3d>();

                foreach(var contour in this.Contours)
                {
                    foreach(var edge in contour.Edges)
                    {
                        points.AddRange(edge.Points);
                    }
                }

                bool isPlanar = Point3d.ArePointsOnPlane(points);
                return isPlanar;
            }
        }

        /// <summary>
        /// Create region by points and coordinate system.
        /// </summary>
        /// <param name="points">List of sorted points defining the outer perimeter of the region.</param>
        /// <param name="plane">Coordinate system of the region</param>
        public Region(List<Point3d> points, Plane plane)
        {
            // edge normal
            Vector3d edgeLocalY = plane.LocalZ;

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
            this.Plane = plane;
        }

        public static Region RectangleXZ(double width, double height)
        {
            var points0 = new Point3d(0,0,0);
            var points1 = new Point3d(width,0,0);
            var points2 = new Point3d(width,0,height);
            var points3 = new Point3d(0,0,height);

            var points = new List<Point3d>() { points0, points1, points2, points3 };

            var fdCoordinate = new Plane(points0, points1, points3);

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

            var plane = new Plane(points0, points1, points3);

            // set properties
            var region = new Region(points, plane);

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
                            // edge connection Normal are opposite to the normal of the contour
                            edge.EdgeConnection.Normal = this.LocalZ.Reverse();
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
                    var edgeConnection = edge.EdgeConnection;
                    if(edgeConnection != null)
                    {
                        edgeConnection.Edge = edge;
                        edgeConnection.Normal = this.LocalZ.Reverse();
                    }
                    edgeConnections.Add(edgeConnection);
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

        public static implicit operator Region(StruSoft.Interop.StruXml.Data.Region_type obj)
        {
            var edges = obj.Contour[0].Edge.Select( x => (Edge)x).ToList();
            var contour = new Contour(edges);
            var contours = new List<Contour>() { contour };

            var region = new Region(contours);

            return region;
        }

        public static implicit operator StruSoft.Interop.StruXml.Data.Region_type(Region obj)
        {
            var region = new StruSoft.Interop.StruXml.Data.Region_type();
            var contours = new List<StruSoft.Interop.StruXml.Data.Contour_type>();

            foreach(var contour in obj.Contours)
                contours.Add(contour);

            region.Contour  = contours;

            return region;
        }
    }
}