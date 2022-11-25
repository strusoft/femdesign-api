// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;

namespace FemDesign.Calculate
{

    /// <summary>
    /// fdscript.xsd
    /// CMDCALCULATION
    /// </summary>
    [XmlRoot("cmdcalculation")]
    [System.Serializable]
    public partial class CmdCalculation : CmdCommand
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

        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdCalculation>(this);
        }
    }


}