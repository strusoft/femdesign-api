// https://strusoft.com/
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion


namespace FemDesign.Application
{
    // fdscript.xsd
    // CMDDOCTABLE
    [IsVisibleInDynamoLibrary(false)]
    public class CmdDocTable
    {
        [XmlAttribute("command")]
        public string command = "; CXL $MODULE DOCTABLE";
        [XmlElement("doctable")]
        public DocTable docTable { get; set; } // DOCTABLE
        private CmdDocTable()
        {
            // parameterless constructor for serialization
        }
        public CmdDocTable(string listProc, string version)
        {
            this.docTable = new DocTable(listProc, version);
        }
    }
}