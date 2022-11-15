// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// CMDSAVE
    /// </summary>
    [XmlRoot("fdscript", Namespace = "urn:strusoft")]
    [System.Serializable]
    public partial class CmdSave : CmdCommand
    {
        [XmlAttribute("command")]
        public string Command = "; CXL CS2SHELL SAVE"; // token, fixed
        [XmlElement("filename")]
        public string FilePath { get; set; } // SZPATH
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdSave()
        {
            
        }
        public CmdSave(string filePath)
        {
            this.FilePath = filePath;
        }
    }
}