// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign.Materials;


namespace FemDesign.Materials
{
    [System.Serializable]
    public partial class OrthotropicPanelTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<OrthotropicPanelLibraryType> OrthotropicPanelLibraryTypes { get; set; }
    }

    /// <summary>
    /// timberpanel_lib_type
    /// </summary>
    [System.Serializable]
    public partial class OrthotropicPanelLibraryType: LibraryBase, IPanelLibraryType
    {
        [XmlElement("timber_panel_data", Order = 1)]
        public TimberPanelData TimberPanelData { get; set; }

        public OrthotropicPanelLibraryType()
        {

        }
    }
}