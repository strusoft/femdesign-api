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
        public string FemDesignVersion { get; set; } = FdScript.Version;
        
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
        private static readonly string _loadCaseSuffix = $"Ultimate - Load case: {MapCase._oname}"; // we can only return Ultimate. BP to update their code.
        private static readonly string _loadCombSuffix = $"Load Comb.: {MapComb._oname}";

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
        public DocTable(ListProc resultType, FemDesign.Results.UnitResults unitResult = null, bool allCaseCombo = false, Options options = null)
        {
            ListProc = resultType;

            var isLoadCase = resultType.IsLoadCase();
            var isLoadComb = resultType.IsLoadCombination();
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

            Option = options ?? Options.GetOptions(resultType);
        }

        /// <summary>
        /// DocTable to return specific analysis results by shape identifiers.
        /// </summary>
        /// <param name="resultType"></param>
        /// <param name="loadCombination"></param>
        /// <param name="shapeID"></param>
        /// <param name="unitResult"></param>
        /// <param name="options"></param>
        internal DocTable(ListProc resultType, string loadCombination, int shapeID, FemDesign.Results.UnitResults unitResult = null, Options options = null)
        {
            // check input
            if (shapeID <= 0)
                throw new Exception("Invalid shapeID. Parameter must be a positive, non-zero number!");

            ListProc = resultType;

            if(resultType != ListProc.NodalVibrationShape)
            {
                if (loadCombination == null)
                    throw new Exception("loadCombination input cannot be null!");

                Suffix = $"{loadCombination} / {shapeID}";
            }
            else
            {
                Suffix = $"{shapeID}";
            }
            
            ResType = GetResType(resultType);
            CaseIndex = 0;  //  If the CaseIndex is set to its default value (see 'GetDefaultCaseIndex()' method), FD will ignore the suffix and the all results will be listed.
                            //  To get the specified result cases, use 0, and suffix will override the index in the batch file.
            Units = Results.Units.GetUnits(unitResult);
            Option = options ?? Options.GetOptions(resultType);
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
            if (r.StartsWith("QuantityEstimation") || r.EndsWith("Utilization") || r.Contains("MaxComb") || r.Contains("MaxLoadGroup") || r.StartsWith("FemNode") || r.StartsWith("FemBar") || r.StartsWith("FemShell") || r.StartsWith("EigenFrequencies") || r.Contains("MaxOfLoadCombinationMinMax") || r == "CriticalParameters" || r == "ImperfectionFactors")
                return 0;
            if (r.EndsWith("LoadCase"))
                return 1;
            if (r.EndsWith("LoadCombination"))
                return 3;
            if (r.EndsWith("BucklingShape"))
                return 5;
            if (r.StartsWith("NodalVibrationShape"))
                return 6;

            throw new NotImplementedException($"'restype' index for {r} is not implemented.");
        }

        private int GetDefaultCaseIndex(ListProc resultType)
        {
            string r = resultType.ToString();
            if (r.StartsWith("QuantityEstimation") || r.EndsWith("Utilization") || r.Contains("MaxComb") || r.Contains("MaxLoadGroup") || r.StartsWith("FemNode") || r.StartsWith("FemBar") || r.StartsWith("FemShell") || r.StartsWith("EigenFrequencies") || r.Contains("MaxOfLoadCombinationMinMax") || r == "CriticalParameters" || r == "ImperfectionFactors")
                return 0;
            if (r.EndsWith("LoadCase"))
                return -65536; // All load cases
            if (r.EndsWith("LoadCombination") || r.StartsWith("NodalVibrationShape") || r.EndsWith("BucklingShape"))
                return -1; // All load combinations

            throw new FormatException($"Default case index of ResultType.{resultType} not known.");
        }
    }
}
