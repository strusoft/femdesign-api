// https://strusoft.com/

using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Loads
{
    /// <summary>
    /// pressure_load_type.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class PressureLoad: ForceLoadBase
    {
        // attributes
        [XmlAttribute("load_projection")]
        public bool loadProjection { get; set; } // bool
        
        [XmlAttribute("z0")]
        public double _z0; // abs_max_1e20
        [XmlIgnore]
        public double z0
        {
            get {return this._z0;}
            set {this._z0 = RestrictedDouble.AbsMax_1e20(value);}
        }
        
        [XmlAttribute("q0")]
        public double _q0; // abs_max_1e20
        [XmlIgnore]
        public double q0
        {
            get {return this._q0;}
            set {this._q0 = RestrictedDouble.AbsMax_1e20(value);}
        }
        
        [XmlAttribute("qh")]
        public double _qh; // abs_max_1e20
        [XmlIgnore]
        public double qh
        {
            get {return this._qh;}
            set {this._qh = RestrictedDouble.AbsMax_1e20(value);}
        }

        // elements
        [XmlElement("region", Order = 1)]
        public Geometry.Region region { get; set; } // region_type
        [XmlElement("direction", Order = 2)]
        public Geometry.FdVector3d direction { get; set; } // point_type_3c

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private PressureLoad()
        {

        }

        /// <summary>
        /// Private constructor.
        /// </summary>
        /// <param name="loadDirection">Vector. Direction of force.</param>
        /// <param name="z0">Surface level of soil/water (on the global Z axis).</param>
        /// <param name="q0">Load intensity at the surface level.</param>
        /// <param name="qh">Increment of load intensity per meter (along the global Z axis).</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="comment">Comment.</param>
        /// <param name="loadProjection"></param>
        /// <param name="loadType"></param>
        private PressureLoad(Geometry.Region region, Geometry.FdVector3d loadDirection, double z0, double q0, double qh, LoadCase loadCase, string comment, bool loadProjection, string loadType)
        {
            // base
            this.EntityCreated();
            this.loadCase = loadCase.guid;
            this.comment = comment;
            this.loadProjection = loadProjection;
            this.loadType = loadType;
            this.region = region;
            this.direction = loadDirection; 

            // specific
            this.z0 = z0;
            this.q0 = q0;
            this.qh = qh;
        }

        
        /// Define new PressureLoad.
        /// Internal method used for GH components and Dynamo nodes.
        internal static PressureLoad Define(Geometry.Region region, Geometry.FdVector3d loadDirection, LoadCase loadCase,  double z0, double q0, double qh, string comment)
        {
            //
            bool loadProjection = false;
            string loadType = "force";            

            // return
            return new PressureLoad(region, loadDirection, z0, q0, qh, loadCase, comment, loadProjection, loadType);
        }

        #region dynamo
        /// <summary>
        /// Create a hydrostatic surface load.
        /// Surfaces created by Surface.ByLoft method might cause errors, please use Surface.ByPatch or Surface.ByPerimeterPoints.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="surface">Surface. Surface must be flat.</param>
        /// <param name="direction">Vector. Direction of force.</param>
        /// <param name="z0">Surface level of soil/water (on the global Z axis).</param>
        /// <param name="q0">Load intensity at the surface level.</param>
        /// <param name="qh">Increment of load intensity per meter (along the global Z axis).</param>
        /// <param name="loadCase">LoadCase.</param>
        /// <param name="comment">Comment.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static PressureLoad Define(Autodesk.DesignScript.Geometry.Surface surface, Autodesk.DesignScript.Geometry.Vector direction, double z0, double q0, double qh, LoadCase loadCase, string comment = "")
        {
            // get fdGeometry
            Geometry.Region region = Geometry.Region.FromDynamo(surface);

            // normalize direction
            Geometry.FdVector3d _loadDirection = Geometry.FdVector3d.FromDynamo(direction).Normalize();

            // create SurfaceLoad
            PressureLoad _pressureLoad = PressureLoad.Define(region, _loadDirection, loadCase, z0, q0, qh, comment);
            return _pressureLoad;
        }

        /// <summary>
        /// Convert surface of PressureLoad to a Dynamo surface.
        /// </summary>
        internal Autodesk.DesignScript.Geometry.Surface GetDynamoGeometry()
        {
            return this.region.ToDynamoSurface();
        }
        #endregion

        #region grasshopper
        
        /// <summary>
        /// Convert surface of PressureLoad to a Rhino brep.
        /// </summary>
        internal Rhino.Geometry.Brep GetRhinoGeometry()
        {
            return this.region.ToRhinoBrep();
        }
        #endregion
    }
}