// https://strusoft.com/
using System.IO;
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// CMDSAVEDOCX
    /// </summary>
    public partial class CmdSaveDocx
    {
        [XmlAttribute("command")]
        public string Command = "$ DOC SAVEDOCX"; // token, fixed
        [XmlElement("filename")]
        public string FilePath { get; set; } // SZPATH
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdSaveDocx()
        {
            
        }
        public CmdSaveDocx(string filePath)
        {
            //
            string extension = Path.GetExtension(filePath);
            if (extension != ".docx")
            {
                throw new System.ArgumentException("Incorrect file-extension. Expected .dsc. CmdSaveDocx failed.");
            }

            //
            this.FilePath = filePath;
        }
    }
}