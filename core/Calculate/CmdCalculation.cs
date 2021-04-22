// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// CMDCALCULATION
    /// </summary>
    public class CmdCalculation
    {
        [XmlElement("analysis")]
        public Analysis Analysis { get; set; } // ANALYSIS
        [XmlElement("design")]
        public Design Design { get; set; } // DESIGNCALC
        [XmlAttribute("command")]
        public string Command = "; CXL $MODULE CALC"; // token
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdCalculation()
        {
            
        }
        public CmdCalculation(Analysis analysis)
        {
            this.Analysis = analysis;
        }
        public CmdCalculation(Analysis analysis, Design design)
        {
            this.Analysis = analysis;
            this.Design = design;
        }
    }
}