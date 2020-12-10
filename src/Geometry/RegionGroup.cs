// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Geometry
{
    /// <summary>
    /// region_group_type
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class RegionGroup
    {
        [XmlElement("region")]
        public List<Region> Regions = new List<Region>(); // sequence: region_type

        /// <summary>
        /// Parameterless constructor for serialization
        /// <summary>
        private RegionGroup()
        {

        }

        /// <summary>
        /// Construct region group from single region
        /// <summary>
        public RegionGroup(Region region)
        {
            this.Regions.Add(region);
        }

        /// <summary>
        /// Construct region group from list of regions
        /// <summary>
        public RegionGroup(List<Region> regions)
        {
            this.Regions = regions;
        }

        #region dynamo
        /// <summary>
        /// Get dynamo surfaces of underlying regions
        /// </summary>
        public List<Autodesk.DesignScript.Geometry.Surface> ToDynamo()
        {
            List<Autodesk.DesignScript.Geometry.Surface> surfaces = new List<Autodesk.DesignScript.Geometry.Surface>();
            foreach (Region region in this.Regions)
            {
                surfaces.Add(region.ToDynamoSurface());
            }
            return surfaces;
        }
        #endregion

        #region grasshopper
        /// <summary>
        /// Get rhino breps of underlying regions
        /// </summary>
        public List<Rhino.Geometry.Brep> ToRhino()
        {
            List<Rhino.Geometry.Brep> breps = new List<Rhino.Geometry.Brep>();
            foreach (Region region in this.Regions)
            {
                breps.Add(region.ToRhinoBrep());
            }
            return breps;
        }
        #endregion
    }
}