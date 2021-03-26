// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Calculate
{

    /// <summary>
    /// fdscript.xsd
    /// bsc root
    /// </summary>
    [XmlRoot("doctable")]
    public class DocTable
    {
        [XmlIgnore]
        private const int ALL = -65536;
        
        [XmlElement("listproc")]
        public ResultType ListProc { get; set; }
        
        [XmlElement("index")]
        public int CaseIndex { get; set; }
        
        [XmlAttribute("command")]
        public string Command { get; set; }

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
            Command = "";
            
        }
    }
}
