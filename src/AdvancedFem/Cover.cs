// https://strusoft.com/
using System.Globalization;
using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign
{
    /// <summary>
    /// cover_type
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class Cover: EntityBase
    {
        /// <summary>
        /// Cover instance number
        /// </summary>
        private static int _coverInstance = 0;
        
        /// <summary>
        /// Identifier
        /// </summary>
        [XmlAttribute("name")]
        public string _identifier;
        [XmlIgnore]
        public string Identifier
        {
            get
            {
                return this._identifier;
            }
            set
            {
                Cover._coverInstance++;
                this._identifier = value + "." + Cover._coverInstance.ToString();
            }
        }
        
        /// <summary>
        /// Load bearing direction (point_type_3d)
        /// </summary>
        [XmlElement("load_bearing_direction", Order = 1)]
        public Geometry.FdVector3d LoadBearingDirection { get; set; }

        /// <summary>
        /// Region (region_type).
        /// </summary>
        [XmlElement("region", Order = 2)]
        public Geometry.Region Region { get; set; }

        /// <summary>
        /// Supporting structures (cover_referencelist_type)
        /// </summary>
        [XmlElement("supporting_structures", Order = 3)]
        public CoverReferenceList SupportingStructures { get; set; } 

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Cover()
        {
            
        }

        /// <summary>
        /// Construct a cover
        /// </summary>
        /// <param name="region">Region of cover.</param>
        /// <param name="supportingStructures">Guidlist of supporting structure.</param>
        /// <param name="loadBearingDirection">Vector, if null a TwoWay cover is defined.</param>
        private Cover(Geometry.Region region, CoverReferenceList supportingStructures, Geometry.FdVector3d loadBearingDirection, string identifier)
        {
            this.EntityCreated();
            this.Identifier = identifier;
            this.Region = region;
            this.SupportingStructures = supportingStructures;
            this.LoadBearingDirection = loadBearingDirection;
        }

        /// Create OneWayCover.
        internal static Cover OneWayCover(Geometry.Region region, List<object> supportingStructures, Geometry.FdVector3d loadBearingDirection, string identifier)
        {
            // get supportingStructures.guid
            CoverReferenceList _supportingStructures = CoverReferenceList.FromObjectList(supportingStructures);

            // create cover
            Cover _cover = new Cover(region, _supportingStructures, loadBearingDirection, identifier);

            return _cover;
        }

        /// Create TwoWayCover.
        internal static Cover TwoWayCover(Geometry.Region region, List<object> supportingStructures, string identifier)
        {
            // get supportingStructures.guid
            CoverReferenceList _supportingStructures = CoverReferenceList.FromObjectList(supportingStructures);

            // create cover
            Cover _cover = new Cover(region, _supportingStructures, null, identifier);

            return _cover;
        }

        #region dynamo
        /// <summary>
        /// Create a one way cover. Surfaces created by Surface.ByLoft method might cause errors, please use Surface.ByPatch or Surface.ByPerimeterPoints.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="surface">Surface. Surface must be flat.</param>
        /// <param name="supportingStructures">Single structure element och list of structure elements. List cannot be nested - use flatten.</param>
        /// <param name="loadBearingDirection">Vector of load bearing direction.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Cover OneWayCover(Autodesk.DesignScript.Geometry.Surface surface, [DefaultArgument("[]")] List<object> supportingStructures, Autodesk.DesignScript.Geometry.Vector loadBearingDirection = null, string identifier = "CO")
        {
            // create FlatSurface
            Geometry.Region region = Geometry.Region.FromDynamo(surface);

            // get loadBearingDirection
            Geometry.FdVector3d _loadBearingDirection = Geometry.FdVector3d.FromDynamo(loadBearingDirection).Normalize();

            // return
            return Cover.OneWayCover(region, supportingStructures, _loadBearingDirection, identifier);
        }

        /// <summary>
        /// Create a two way cover. 
        /// Surfaces created by Surface.ByLoft method might cause errors, please use Surface.ByPatch or Surface.ByPerimeterPoints.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <param name="surface">Surface. Surface must be flat.</param>
        /// <param name="supportingStructures">Single structure element or list of structure elements. List cannot be nested - use flatten.</param>
        /// <param name="identifier">Identifier. Optional.</param>
        /// <returns></returns>
        [IsVisibleInDynamoLibrary(true)]
        public static Cover TwoWayCover(Autodesk.DesignScript.Geometry.Surface surface, [DefaultArgument("[]")] List<object> supportingStructures, string identifier = "CO")
        {
            // create FlatSurface
            Geometry.Region region = Geometry.Region.FromDynamo(surface);

            // return
            return Cover.TwoWayCover(region, supportingStructures, identifier);
        }

        /// <summary>
        /// Create Dynamo surface from underlying Region of Cover.
        /// </summary>
        internal Autodesk.DesignScript.Geometry.Surface GetDynamoSurface()
        {
            return this.Region.ToDynamoSurface();
        }

        /// <summary>
        /// Create Dynamo curves from underlying Edges in Region of Cover.
        /// </summary>
        internal  List<List<Autodesk.DesignScript.Geometry.Curve>> GetDynamoCurves()
        {
            return this.Region.ToDynamoCurves();
        }

        #endregion

        #region grasshopper
        /// <summary>
        /// Create Rhino brep from underlying Region of Cover.
        /// </summary>
        internal Rhino.Geometry.Brep GetRhinoSurface()
        {
            return this.Region.ToRhinoBrep();
        }
        
        /// <summary>
        /// Create Rhino curves from underlying Edges in Region of Cover.
        /// </summary>
        internal List<Rhino.Geometry.Curve> GetRhinoCurves()
        {
            return this.Region.ToRhinoCurves();
        }

        #endregion
    }
}      