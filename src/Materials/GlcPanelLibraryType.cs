using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Materials
{
    [System.Serializable()]
    [IsVisibleInDynamoLibrary(false)]
    public class GlcPanelTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<GlcPanelLibraryType> GlcPanelLibraryTypes { get; set; }

        public GlcPanelTypes()
        {

        }
    }

    [System.Serializable()]
    [IsVisibleInDynamoLibrary(false)]
    public class GlcPanelLibraryType: LibraryBase
    {
        [XmlElement("glc_panel_data", Order = 1)]
        public GlcDataType GlcPanelData {get; set;}

        public GlcPanelLibraryType()
        {
            
        }
    }
}