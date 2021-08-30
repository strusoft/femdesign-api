// https://strusoft.com/
using System;
using System.Xml.Serialization;


namespace FemDesign.Calculate
{

    /// <summary>
    /// cmddoctable
    /// </summary>
    [System.Serializable]
    public partial class CmdDocTable
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
    public partial class DocTable
    {
        [XmlElement("version")]
        public string FemDesignVersion { get; set; } = "2000";
        
        [XmlElement("listproc")]
        public ListProc ListProc { get; set; }
        
        [XmlElement("index")]
        public int CaseIndex { get; set; }
        
        [XmlElement("options")]
        public DummyXmlObject options { get; set; } = new DummyXmlObject();
        
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
        public DocTable(ListProc resultType, int? caseIndex = null)
        {
            int cIndex;
            if (caseIndex.HasValue)
                cIndex = caseIndex.Value;
            else
                cIndex = GetDefaultCaseIndex(resultType);

            ListProc = resultType;
            CaseIndex = cIndex;
            ResType = GetResType(resultType);
        }

        private int GetResType(ListProc resultType)
        {
            /*
            LT_CASE = 1,
            LT_CS = 2,  (construction stage)
            LT_COMB = 3,
            ...
            */

            string r = resultType.ToString();
            if (r.StartsWith("QuantityEstimation"))
                return 0;
            if (r.EndsWith("LoadCase"))
                return 1;
            if (r.EndsWith("LoadCombination"))
                return 3;

            throw new NotImplementedException($"'restype' index for {r} is not implemented.");
        }

        private int GetDefaultCaseIndex(ListProc resultType)
        {
            string r = resultType.ToString();
            if (r.StartsWith("QuantityEstimation"))
                return 0;
            if (r.EndsWith("LoadCase"))
                return -65536; // All load cases
            if (r.EndsWith("LoadCombination"))
                return -1; // All load combinations

            throw new FormatException($"Default case index of ResultType.{resultType} not known.");
        }
    }
}
