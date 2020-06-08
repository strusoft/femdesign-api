// https://strusoft.com/
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// CMDSAVE
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
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