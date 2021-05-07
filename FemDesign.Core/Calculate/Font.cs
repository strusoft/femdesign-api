// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// FONT
    /// </summary>
    public partial class Font
    {
        [XmlElement("name")]
        public string Name = "Tahoma"; // SZID
        [XmlElement("type")]
        public string Type = "ANSI_CHARSET"; // FONTTYPE
        [XmlElement("size")]
        public string Size = "0.003"; // REAL_PLUS
        [XmlElement("width")]
        public string Width = "1"; // REAL_PLUS
        [XmlElement("slant")]
        public string Slant = "0"; // FONTSLANT
        public Font()
        {

        }
    }
}