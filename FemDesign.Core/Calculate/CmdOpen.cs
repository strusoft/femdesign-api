// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;

namespace FemDesign.Calculate
{

    /// <summary>
    /// fdscript.xsd
    /// CMDOPEN
    /// </summary>
    public partial class CmdOpen
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
            this.Filename = Path.GetFullPath(filepath);
        }
    }



    /// <summary>
    /// fdscript.xsd
    /// CMDOPEN
    /// </summary>
    [XmlRoot("cmdopen")]
    [System.Serializable]
    public partial class CmdOpenPipe : CmdCommand
    {
        [XmlAttribute("command")]
        public string Command = "; CXL CS2SHELL OPEN"; // token, fixed
        [XmlElement("filename")]
        public string Filename { get; set; } // SZPATH

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdOpenPipe()
        {

        }
        public CmdOpenPipe(string filepath)
        {
            this.Filename = Path.GetFullPath(filepath);
        }

        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdOpenPipe>(this);
        }
    }

}