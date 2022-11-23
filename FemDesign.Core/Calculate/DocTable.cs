// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;


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
        public string FemDesignVersion { get; set; } = "2100";
        
        [XmlElement("listproc")]
        public ListProc ListProc { get; set; }

        [XmlElement("suffix")]
        public string Suffix { get; set; }


        // micro pattern to avoid an empty element
        // https://stackoverflow.com/a/610630/14969396
        [XmlIgnore]
        public int? CaseIndex { get; set; }

        [XmlElement("index")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public int _xmlSomeValue { get { return CaseIndex.Value; } set { CaseIndex = value; } }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool _xmlSomeValueSpecified { get { return CaseIndex.HasValue; } }



        [XmlIgnore]
        public bool AllCaseComb
        {
            get
            {
                if(Suffix == null) { return true; }
                else { return false; }
            }
        }

        [XmlElement("units")]
        public List<FemDesign.Results.Units> Units { get; set; }

        [XmlElement("options")]
        public Options Option { get; set; }
        
        [XmlElement("restype")]
        public int ResType { get; set; }

        [XmlIgnore]
        public static readonly string _loadCaseSuffix = $"Ultimate - Load case: {MapCase._oname}"; // we can only return Ultimate. BP to update their code.
        public static readonly string _loadCombSuffix = $"Load Comb.: {MapComb._oname}";

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private DocTable()
        {

        }

        /// <summary>
        /// DocTable Constructor
        /// The name of LoadCase will be specified in cmdlistgen object
        /// </summary>
        /// <param name="resultType"></param>
        public DocTable(ListProc resultType, FemDesign.Results.UnitResults unitResult = null, bool allCaseCombo = false)
        {
            ListProc = resultType;

            var isLoadCase = resultType.ToString().Contains("LoadCase");
            var isLoadComb = resultType.ToString().Contains("LoadCombination");
            if (!allCaseCombo)
            {
                if (isLoadCase)
                {
                    Suffix = _loadCaseSuffix;
                }
                else if (isLoadComb)
                {
                    Suffix = _loadCombSuffix;
                }
            }
            else
            {
                // return all the output related to the analysis
                // i.e eigen frequency will return all the eigen values
                CaseIndex = GetDefaultCaseIndex(resultType);
            }

            Units = Results.Units.GetUnits(unitResult);
            ResType = GetResType(resultType);
            Option = Options.GetOptions(resultType);
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
            if (r.StartsWith("QuantityEstimation") || r.EndsWith("Utilization") || r.Contains("MaxComb") || r.StartsWith("FeaNode") || r.StartsWith("FeaBar") || r.StartsWith("FeaShell") || r.StartsWith("EigenFrequencies") || r.Contains("MaxOfLoadCombinationMinMax"))
                return 0;
            if (r.EndsWith("LoadCase"))
                return 1;
            if (r.EndsWith("LoadCombination"))
                return 3;
            if (r.StartsWith("NodalVibrationShape"))
                return 6;

            throw new NotImplementedException($"'restype' index for {r} is not implemented.");
        }

        private int GetDefaultCaseIndex(ListProc resultType)
        {
            string r = resultType.ToString();
            if (r.StartsWith("QuantityEstimation") || r.EndsWith("Utilization") || r.Contains("MaxComb") || r.StartsWith("FeaNode") || r.StartsWith("FeaBar") || r.StartsWith("FeaShell") || r.StartsWith("EigenFrequencies") || r.Contains("MaxOfLoadCombinationMinMax"))
                return 0;
            if (r.EndsWith("LoadCase"))
                return -65536; // All load cases
            if (r.EndsWith("LoadCombination") || r.StartsWith("NodalVibrationShape"))
                return -1; // All load combinations

            throw new FormatException($"Default case index of ResultType.{resultType} not known.");
        }
    }
}
