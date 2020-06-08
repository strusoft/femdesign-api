// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// surface_load_type
    /// </summary>
    public class SurfaceLoad: ForceLoadBase
    {
        // attributes
        [XmlAttribute("load_projection")]
        public bool loadProjection { get; set; } // bool

        // elements        
        [XmlElement("region", Order = 1)]
        public Geometry.Region region { get; set; } // region_type
        [XmlElement("direction", Order = 2)]
        public Geometry.FdVector3d direction { get; set; } // point_type_3d
        [XmlElement("load", Order = 3)]
        public List<LoadLocationValue> load = new List<LoadLocationValue>(); // location_value

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private SurfaceLoad()
        {

        }

        /// <summary>
        /// Private constructor accessed by static methods.
        /// </summary>
        private SurfaceLoad(Geometry.Region region, List<LoadLocationValue> loads, Geometry.FdVector3d loadDirection, LoadCase loadCase, string comment, bool loadProjection, string loadType)
        {
            this.EntityCreated();
            this.loadCase = loadCase.guid;
            this.comment = comment;
            this.loadProjection = loadProjection;
            this.loadType = loadType;
            this.region = region;
            this.direction = loadDirection;
            foreach (LoadLocationValue _load in loads)
            {
                this.load.Add(_load);
            }
        }

        /// <summary>
        /// Create uniform SurfaceLoad.
        /// Internal static method used by GH components and Dynamo nodes.
        /// </summary>
        internal static SurfaceLoad Uniform(Geometry.Region region, Geometry.FdVector3d force, LoadCase loadCase, string comment = "")
        {
            // create load as list of loads
            List<LoadLocationValue> load = new List<LoadLocationValue>{new LoadLocationValue(region.coordinateSystem.origin, force.Length())};

            // create load direction
            Geometry.FdVector3d loadDirection = force.Normalize();

            // presets
            bool loadProjection = false;
            string loadType = "force";

            // create SurfaceLoad
            SurfaceLoad surfaceLoad =  new SurfaceLoad(region, load, loadDirection, loadCase, comment, loadProjection, loadType);

            // return
            return surfaceLoad;
        }
        
        /// <summary>
        /// Create variable SurfaceLoad.
        /// Internal static method used by GH components and Dynamo nodes.
        /// </summary>
        internal static SurfaceLoad Variable(Geometry.Region region, Geometry.FdVector3d direction, List<LoadLocationValue> loadLocationValue, LoadCase loadCase, string comment = "")
        {
            if (loadLocationValue.Count != 3)
            {
                throw new System.ArgumentException("loadLocationValue must contain 3 items");
            }

            // presets
            bool loadProjection = false;
            string loadType = "force";

            // create SurfaceLoad
            SurfaceLoad surfaceLoad =  new SurfaceLoad(region, loadLocationValue, direction, loadCase, comment, loadProjection, loadType);

            // return
            return surfaceLoad;
        }


        #region grasshopper

        /// <summary>
        /// Convert surface of SurfaceLoad to a Rhino brep.
        /// </summary>
        internal Rhino.Geometry.Brep GetRhinoGeometry()
        {
            return this.region.ToRhinoBrep();
        }
        #endregion
    }
}