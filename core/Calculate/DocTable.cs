// https://strusoft.com/
using System;
using System.Xml.Serialization;


namespace FemDesign.Calculate
{

    /// <summary>
    /// cmddoctable
    /// </summary>
    [System.Serializable]
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
        private const int ALL = -1;
        
        [XmlElement("version")]
        public string FemDesignVersion { get; set; } = "2000";
        [XmlElement("listproc")]
        public ResultType ListProc { get; set; }
        
        [XmlElement("index")]
        public int CaseIndex { get; set; }
        
        [XmlElement("restype")]
        public int ResType { get; set; }

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
            ResType = GetResType(resultType);
        }

        private int GetResType(ResultType resultType)
        {
            /*
            LT_CASE = 1,
            LT_CS = 2,  (construction stage)
            LT_COMB = 3,
            ...
            */

            string r = resultType.ToString();
            if (r.StartsWith("frCas"))
                return 1;
            if (r.StartsWith("frComb"))
                return 3;

            throw new NotImplementedException($"'restype' index for {r} is not implemented.");
        }
    }
}
