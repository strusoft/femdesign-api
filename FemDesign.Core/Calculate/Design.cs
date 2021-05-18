// https://strusoft.com/
using System.Xml.Serialization;

namespace FemDesign.Calculate
{

    /// <summary>
    /// fdscript.xsd
    /// DESIGNCALC
    /// </summary>
    public partial class Design
    {
        [XmlElement("cmax")]
        public string CMax { get; set; } // choice?
        [XmlElement("gmax")]
        public string GMax { get; set; } // choice?
        [XmlElement("autodesign")]
        public bool AutoDesign { get; set; } // bool
        [XmlElement("check")]
        public bool Check { get; set; } // bool

        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Design()
        {

        }
        
        public Design(bool autoDesign, bool check)
        {
            this.CMax = "";
            this.AutoDesign = autoDesign;
            this.Check = check;
        }
    }
}