// https://strusoft.com/
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion


namespace FemDesign.Application
{
    [IsVisibleInDynamoLibrary(false)]
    public class DocTable
    {
        [XmlElement("listdll", Order = 1)]
        public string listdll = "";
        [XmlElement("listproc", Order = 2)]
        public string listProc { get; set; }
        [XmlElement("font", Order = 3)]
        public Font font = new Font();
        [XmlElement("version", Order = 4)]
        public string version { get; set; }
        [XmlElement("index", Order = 5)]
        public string index = "0";
        [XmlElement("suffix", Order = 6)]
        public string suffix = "";
        [XmlElement("restype", Order = 7)]
        public string restype = "0";
        private DocTable()
        {
            // parameterless constructor for serialization
        }
        public DocTable(string listProc, string version)
        {
            this.listProc = listProc;
            this.version = version;
        }
    }
}