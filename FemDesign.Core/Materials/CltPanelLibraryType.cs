using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Materials
{   [System.Serializable()]
    public partial class CltPanelTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<CltPanelLibraryType> CltPanelLibraryTypes { get; set; }

        public CltPanelTypes()
        {

        }
    }

    [System.Serializable()]
    public partial class CltPanelLibraryType: LibraryBase, IPanelLibraryType
    {
        [XmlElement("clt_panel_data")]
        public CltDataType CltPanelData { get; set; }

        public CltPanelLibraryType()
        {

        }

        public override string ToString()
        {
            return $"CLT Panel - {this.Name}";
        }
    }
}