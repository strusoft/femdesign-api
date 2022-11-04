// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Geometry
{
    /// <summary>
    /// region_group_type
    /// </summary>
    [System.Serializable]
    public partial class RegionGroup
    {
        [XmlElement("region")]
        public List<Region> Regions = new List<Region>(); // sequence: region_type

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private RegionGroup()
        {

        }

        /// <summary>
        /// Construct region group from single region
        /// </summary>
        public RegionGroup(Region region)
        {
            this.Regions.Add(region);
        }

        /// <summary>
        /// Construct region group from list of regions
        /// </summary>
        public RegionGroup(List<Region> regions)
        {
            this.Regions = regions;
        }


    }
}