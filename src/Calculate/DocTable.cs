// https://strusoft.com/
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Calculate
{

    /// <summary>
    /// cmddoctable
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class CmdDocTable
    {   
        [XmlElement("doctable", Order = 1)]
        public DocTable DocTable { get; set; }

        [XmlAttribute("command")]
        public string _command = "; CXL $MODULE DOCTABLE";

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdDocTable()
        {

        }

        /// <summary>
        /// CmdDocTable constructor
        /// </summary>
        /// <param name="docTable">DocTable</param>
        public CmdDocTable(DocTable docTable)
        {
            this.DocTable = docTable;
        }
    }
    
    /// <summary>
    /// doctable
    /// </summary>
    [System.Serializable]
    public class DocTable
    {
        [XmlIgnore]
        private const int ALL = -65536;
        
        [XmlElement("listproc")]
        public ResultType ListProc { get; set; }
        
        [XmlElement("index")]
        public int CaseIndex { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private DocTable()
        {

        }

        /// <summary>
        /// DocTable constructor
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="caseIndex">Defaults to all loadcases or loadcombinations</param>
        public DocTable(ResultType resultType, int caseIndex = ALL)
        {
            ListProc = resultType;
            CaseIndex = caseIndex;            
        }
    }
}
