// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign.Materials
{
    [System.Serializable]
    public class TimberPanelTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<TimberPanelLibraryType> TimberPanelLibraryTypes { get; set; }
    }

    /// <summary>
    /// timberpanel_lib_type
    /// </summary>
    [System.Serializable]
    public class TimberPanelLibraryType: LibraryBase
    {
        [XmlElement("timber_panel_data", Order = 1)]
        public TimberPanelData TimberPanelData { get; set; }

        public TimberPanelLibraryType()
        {

        }
    }
}