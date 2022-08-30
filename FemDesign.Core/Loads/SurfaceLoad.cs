// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// surface_load_type
    /// </summary>
    [System.Serializable]
    public partial class SurfaceLoad: ForceLoadBase
    {
        // attributes
        [XmlAttribute("load_projection")]
        public bool LoadProjection { get; set; } // bool

        // elements        
        [XmlElement("region", Order = 1)]
        public Geometry.Region Region { get; set; } // region_type
        [XmlElement("direction", Order = 2)]
        public Geometry.FdVector3d Direction { get; set; } // point_type_3d
        [XmlElement("load", Order = 3)]
        public List<LoadLocationValue> Loads = new List<LoadLocationValue>(); // location_value

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private SurfaceLoad()
        {

        }

        /// <summary>
        /// Uniform surface load
        /// </summary>
        /// <param name="region"></param>
        /// <param name="load"></param>
        /// <param name="loadCase"></param>
        /// <param name="loadProjection">False: Intensity meant along action line (eg. dead load). True: Intensity meant perpendicular to direction of load (eg. snow load).</param>
        /// <param name="comment"></param>
        public SurfaceLoad(Geometry.Region region, Geometry.FdVector3d load, LoadCase loadCase, bool loadProjection = false, string comment = "") : this(region, new List<LoadLocationValue> { new LoadLocationValue(region.Contours[0].Edges[0].Points[0], load.Length()) }, load.Normalize(), loadCase, loadProjection, comment)
        {

        }

        /// <summary>
        /// Variable surface load
        /// </summary>
        public SurfaceLoad(Geometry.Region region, List<LoadLocationValue> loads, Geometry.FdVector3d loadDirection, LoadCase loadCase, bool loadProjection = false, string comment = "")
        {
            this.EntityCreated();
            this.LoadCase = loadCase.Guid;
            this.Comment = comment;
            this.LoadProjection = loadProjection;
            this.LoadType = ForceLoadType.Force;
            this.Region = region;
            this.Direction = loadDirection;
            foreach (LoadLocationValue _load in loads)
            {
                this.Loads.Add(_load);
            }
        }

        /// <summary>
        /// Create uniform SurfaceLoad
        /// </summary>
        /// <param name="region"></param>
        /// <param name="force"></param>
        /// <param name="loadCase"></param>
        /// <param name="loadProjection">False: Intensity meant along action line (eg. dead load). True: Intensity meant perpendicular to direction of load (eg. snow load).</param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static SurfaceLoad Uniform(Geometry.Region region, Geometry.FdVector3d force, LoadCase loadCase, bool loadProjection = false, string comment = "")
        {
            return  new SurfaceLoad(region, force, loadCase, loadProjection, comment);
        }

        /// <summary>
        /// Create variable SurfaceLoad
        /// </summary>
        /// <param name="region"></param>
        /// <param name="direction"></param>
        /// <param name="loadLocationValue"></param>
        /// <param name="loadCase"></param>
        /// <param name="loadProjection">False: Intensity meant along action line (eg. dead load). True: Intensity meant perpendicular to direction of load (eg. snow load).</param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static SurfaceLoad Variable(Geometry.Region region, Geometry.FdVector3d direction, List<LoadLocationValue> loadLocationValue, LoadCase loadCase, bool loadProjection = false, string comment = "")
        {
            if (loadLocationValue.Count != 3)
            {
                throw new System.ArgumentException("loadLocationValue must contain 3 items");
            }

            return new SurfaceLoad(region, loadLocationValue, direction, loadCase, loadProjection, comment);
        }


    }
}