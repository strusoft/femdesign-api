using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Materials
{   [System.Serializable()]
    [IsVisibleInDynamoLibrary(true)]
    public class CltPanelTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<CltPanelLibraryType> CltPanelLibraryTypes { get; set; }

        public CltPanelTypes()
        {

        }
    }

    [System.Serializable()]
    [IsVisibleInDynamoLibrary(true)]
    public class CltPanelLibraryType: LibraryBase
    {
        [XmlElement("clt_panel_data")]
        public CltDataType CltPanelData { get; set; }

        public CltPanelLibraryType()
        {

        }
    }
}