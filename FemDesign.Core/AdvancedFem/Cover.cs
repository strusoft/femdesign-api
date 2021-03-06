// https://strusoft.com/
using System.Globalization;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign
{
    /// <summary>
    /// cover_type
    /// </summary>
    [System.Serializable]
    public partial class Cover: EntityBase, IStructureElement
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
        public Cover(Geometry.Region region, CoverReferenceList supportingStructures, Geometry.FdVector3d loadBearingDirection, string identifier)
        {
            this.EntityCreated();
            this.Identifier = identifier;
            this.Region = region;
            this.SupportingStructures = supportingStructures;
            this.LoadBearingDirection = loadBearingDirection;
        }

        /// Create OneWayCover.
        public static Cover OneWayCover(Geometry.Region region, List<object> supportingStructures, Geometry.FdVector3d loadBearingDirection, string identifier)
        {
            // get supportingStructures.guid
            CoverReferenceList _supportingStructures = CoverReferenceList.FromObjectList(supportingStructures);

            // create cover
            Cover _cover = new Cover(region, _supportingStructures, loadBearingDirection, identifier);

            return _cover;
        }

        /// Create TwoWayCover.
        public static Cover TwoWayCover(Geometry.Region region, List<object> supportingStructures, string identifier)
        {
            // get supportingStructures.guid
            CoverReferenceList _supportingStructures = CoverReferenceList.FromObjectList(supportingStructures);

            // create cover
            Cover _cover = new Cover(region, _supportingStructures, null, identifier);

            return _cover;
        }


    }
}      