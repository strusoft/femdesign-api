
using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Geometry
{
    [IsVisibleInDynamoLibrary(false)]
    public partial class RegionGroup
    {
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
    }
}