// https://strusoft.com/
using System.IO;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion


namespace FemDesign.Application
{
    [IsVisibleInDynamoLibrary(false)]
    [XmlRoot("fdscript")]
    public class FdBatch
    {
        [XmlIgnore]
        public string batchPath { get; set; }
        [XmlElement("fdscriptheader", Order = 1)]
        public FdScriptHeader fdScriptHeader { get; set; }
        [XmlElement("cmddoctable", Order = 2)]
        public CmdDocTable cmdDocTable { get; set; }
        [XmlElement("cmdendsession", Order = 3)]
        public CmdEndSession cmdEndSession = new CmdEndSession();
        private FdBatch()
        {
            // parameterless constructor for serialization
        }
        internal static void Create(string listProc, string bscPath)
        {
            FdBatch batch = new FdBatch();
            batch.batchPath = bscPath;
            batch.fdScriptHeader = new FdScriptHeader("FEM-Design Batch Template", "batchtable.log");
            batch.cmdDocTable = new CmdDocTable(listProc, batch.fdScriptHeader.version);
            batch.Serialize();
        }
        private void Serialize()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FdBatch));
            using (TextWriter writer = new StreamWriter(this.batchPath))
            {
                serializer.Serialize(writer, this);
            }   
        }
    }
}