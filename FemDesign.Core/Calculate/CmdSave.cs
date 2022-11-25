// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;



namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// CMDSAVE
    /// </summary>
    [System.Serializable]
    public partial class CmdSave
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

        public CmdSave(string filepath)
        {
            this.FilePath = Path.GetFullPath(filepath);
        }

    }



    /// <summary>
    /// fdscript.xsd
    /// CMDSAVE
    /// </summary>
    [XmlRoot("cmdsave")]
    [System.Serializable]
    public partial class CmdSavePipe : CmdCommand
    {
        [XmlAttribute("command")]
        public string Command = "; CXL CS2SHELL SAVE"; // token, fixed
        [XmlElement("filename")]
        public string FilePath { get; set; } // SZPATH

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdSavePipe()
        {

        }

        public CmdSavePipe(string filepath)
        {
            this.FilePath = Path.GetFullPath(filepath);
        }


        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdSavePipe>(this);
        }
    }


}