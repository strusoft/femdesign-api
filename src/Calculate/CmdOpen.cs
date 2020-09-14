// https://strusoft.com/
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion


namespace FemDesign.Calculate
{

    /// <summary>
    /// fdscript.xsd
    /// CMDOPEN
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
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