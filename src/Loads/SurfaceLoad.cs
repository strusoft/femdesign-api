// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Loads
{
    /// <summary>
    /// surface_load_type
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
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

        /// Create uniform SurfaceLoad.
        /// Internal static method used by GH components and Dynamo nodes.
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
        
        /// Create variable SurfaceLoad.
        /// Internal static method used by GH components and Dynamo nodes.
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

        #region dynamo
        /// <summary>
        /// Create a uniform surface load.
        /// Surfaces created by Surface.ByLoft method might cause errors, please use Surface.ByPatch or Surface.ByPerimeterPoints.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="surface">Surface. Surface must be flat.</param>
        /// <param name="force">Force.</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="comment">Comment.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static SurfaceLoad Uniform(Autodesk.DesignScript.Geometry.Surface surface, Autodesk.DesignScript.Geometry.Vector force, LoadCase loadCase, string comment = "")
        {
            // get fdGeometry
            Geometry.Region region = Geometry.Region.FromDynamo(surface);
            Geometry.FdVector3d _force = Geometry.FdVector3d.FromDynamo(force);

            // create SurfaceLoad
            SurfaceLoad surfaceLoad = SurfaceLoad.Uniform(region, _force, loadCase, comment);

            // return
            return surfaceLoad;
        }

        /// <summary>
        /// Create a variable surface load.
        /// Surfaces created by Surface.ByLoft method might cause errors, please use Surface.ByPatch or Surface.ByPerimeterPoints.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="surface">Surface. Surface must be flat.</param>
        /// <param name="direction">Vector. Direction of force.</param>
        /// <param name="loadLocationValue">Loads. List of 3 items [q1, q2, q3].</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="comment">Comment.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static SurfaceLoad Variable(Autodesk.DesignScript.Geometry.Surface surface, Autodesk.DesignScript.Geometry.Vector direction, List<LoadLocationValue> loadLocationValue, LoadCase loadCase, string comment = "")
        {
            if (loadLocationValue.Count != 3)
            {
                throw new System.ArgumentException("loads must contain 3 items");
            }

            // get fdGeometry
            Geometry.Region region = Geometry.Region.FromDynamo(surface);
            Geometry.FdVector3d loadDirection = Geometry.FdVector3d.FromDynamo(direction).Normalize();

            // create SurfaceLoad
            SurfaceLoad _surfaceLoad = SurfaceLoad.Variable(region, loadDirection, loadLocationValue, loadCase, comment);
            return _surfaceLoad;
        }

        /// <summary>
        /// Convert surface of SurfaceLoad to a Dynamo surface.
        /// </summary>
        internal Autodesk.DesignScript.Geometry.Surface GetDynamoGeometry()
        {
            return this.region.ToDynamoSurface();
        }
        #endregion

    }
}