// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// CMDSAVE
    /// </summary>
    public class CmdSave
    {
        [XmlAttribute("command")]
        public string command = "; CXL CS2SHELL SAVE"; // token, fixed
        [XmlElement("filename")]
        public string filePath { get; set; } // SZPATH
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdSave()
        {
            
        }
        public CmdSave(string filePath)
        {
            this.filePath = filePath;
        }
    }
}