// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// FONT
    /// </summary>
    public class Font
    {
        [XmlElement("name")]
        public string name = "Tahoma"; // SZID
        [XmlElement("type")]
        public string type = "ANSI_CHARSET"; // FONTTYPE
        [XmlElement("size")]
        public string size = "0.003"; // REAL_PLUS
        [XmlElement("width")]
        public string width = "1"; // REAL_PLUS
        [XmlElement("slant")]
        public string slant = "0"; // FONTSLANT
        public Font()
        {

        }
    }
}