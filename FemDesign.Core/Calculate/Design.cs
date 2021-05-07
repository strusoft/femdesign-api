// https://strusoft.com/
using System.Xml.Serialization;

namespace FemDesign.Calculate
{

    /// <summary>
    /// fdscript.xsd
    /// DESIGNCALC
    /// </summary>
    public class Design
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
        
        private Design(bool autoDesign, bool check)
        {
            this.CMax = "";
            this.AutoDesign = autoDesign;
            this.Check = check;
        }

        /// <summary>Set parameters for design.</summary>
        /// <remarks>Create</remarks>
        /// <param name="autoDesign">Auto-design elements.</param>
        /// <param name="check">Check elements.</param>
        public static Design Define(bool autoDesign, bool check)
        {
            return new Design(autoDesign, check);
        }
    }
}