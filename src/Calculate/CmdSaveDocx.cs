// https://strusoft.com/
using System.IO;
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// CMDSAVEDOCX
    /// </summary>
    public class CmdSaveDocx
    {
        [XmlAttribute("command")]
        public string command = "$ DOC SAVEDOCX"; // token, fixed
        [XmlElement("filename")]
        public string filePath { get; set; } // SZPATH
        
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
            this.filePath = filePath;
        }
    }
}