// https://strusoft.com/
using System.IO;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// CMDSAVEDOCX
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class CmdSaveDocx
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