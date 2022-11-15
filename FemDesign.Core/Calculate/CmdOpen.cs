// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;

namespace FemDesign.Calculate
{

    /// <summary>
    /// fdscript.xsd
    /// CMDOPEN
    /// </summary>
    [XmlRoot("cmdopen")]
    [System.Serializable]
    public partial class CmdOpen : CmdCommand
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

        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdOpen>(this);
        }
    }
}