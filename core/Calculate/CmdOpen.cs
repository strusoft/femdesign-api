// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Calculate
{

    /// <summary>
    /// fdscript.xsd
    /// CMDOPEN
    /// </summary>
    public class CmdOpen
    {
        [XmlAttribute("command")]
        public string Command = "; CXL CS2SHELL OPEN"; // token, fixed
        [XmlElement("filename")]
        public string Filename { get; set; } // SZPATH
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdOpen()
        {
            
        }
        public CmdOpen(string filepath)
        {
            this.Filename = filepath;
        }
    }
}