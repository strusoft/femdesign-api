// https://strusoft.com/
using System.Globalization;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign
{
    /// <summary>
    /// cover_type
    /// </summary>
    [System.Serializable]
    public class Cover: EntityBase
    {
        private static int coverInstance = 0; // used for numbering in name
        [XmlAttribute("name")]
        /// <summary>
        /// Name (identifier)
        /// </summary>
        public string name { get; set; }
        
        [XmlElement("load_bearing_direction", Order = 1)]
        /// <summary>
        /// Load bearing direction (point_type_3d)
        /// </summary>
        public Geometry.FdVector3d loadBearingDirection { get; set; }

        [XmlElement("region", Order = 2)]
        /// <summary>
        /// Region (region_type).
        /// </summary>
        public Geometry.Region region { get; set; }

        [XmlElement("supporting_structures", Order = 3)]
        /// <summary>
        /// Supporting structures (cover_referencelist_type)
        /// </summary>
        public CoverReferenceList supportingStructures { get; set; } 

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Cover()
        {
            
        }

        /// <summary>
        /// Construct Cover.
        /// </summary>
        /// <param name="region">Region of cover.</param>
        /// <param name="supportingStructures">Guidlist of supporting structure.</param>
        /// <param name="loadBearingDirection">Vector, if null a TwoWay cover is defined.</param>
        private Cover(Geometry.Region region, CoverReferenceList supportingStructures, Geometry.FdVector3d loadBearingDirection)
        {
            coverInstance++;
            this.EntityCreated();
            string name = "C0." + coverInstance.ToString();
            this.loadBearingDirection = loadBearingDirection;
            this.region = region;
            this.supportingStructures = supportingStructures;
        }

        /// Create OneWayCover.
        internal static Cover OneWayCover(Geometry.Region region, List<object> supportingStructures, Geometry.FdVector3d loadBearingDirection)
        {
            // get supportingStructures.guid
            CoverReferenceList _supportingStructures = CoverReferenceList.FromObjectList(supportingStructures);

            // create cover
            Cover _cover = new Cover(region, _supportingStructures, loadBearingDirection);

            return _cover;
        }

        /// Create TwoWayCover.
        internal static Cover TwoWayCover(Geometry.Region region, List<object> supportingStructures)
        {
            // get supportingStructures.guid
            CoverReferenceList _supportingStructures = CoverReferenceList.FromObjectList(supportingStructures);

            // create cover
            Cover _cover = new Cover(region, _supportingStructures, null);

            return _cover;
        }


        #region grasshopper
        /// <summary>
        /// Create Rhino brep from underlying Region of Cover.
        /// </summary>
        internal Rhino.Geometry.Brep GetRhinoSurface()
        {
            return this.region.ToRhinoBrep();
        }
        
        /// <summary>
        /// Create Rhino curves from underlying Edges in Region of Cover.
        /// </summary>
        internal List<Rhino.Geometry.Curve> GetRhinoCurves()
        {
            return this.region.ToRhinoCurves();
        }

        #endregion
    }
}      