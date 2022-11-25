// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;

namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// CMDCALCULATION
    /// </summary>
    [System.Serializable]
    public partial class CmdCalculation
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

        public CmdCalculation(Design design)
        {
            this.Design = design;
        }
    }


    /// <summary>
    /// fdscript.xsd
    /// CMDCALCULATION
    /// </summary>
    [XmlRoot("cmdcalculation")]
    [System.Serializable]
    public partial class CmdCalculationPipe : CmdCommand
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
        private CmdCalculationPipe()
        {

        }
        public CmdCalculationPipe(Analysis analysis)
        {
            this.Analysis = analysis;
        }

        public CmdCalculationPipe(Analysis analysis, Design design)
        {
            this.Analysis = analysis;
            this.Design = design;
        }

        public CmdCalculationPipe(Design design)
        {
            this.Design = design;
        }

        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdCalculationPipe>(this);
        }
    }


}