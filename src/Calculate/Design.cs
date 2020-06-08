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
        public string cMax { get; set; } // choice?
        [XmlElement("gmax")]
        public string gMax { get; set; } // choice?
        [XmlElement("autodesign")]
        public bool autoDesign { get; set; } // bool
        [XmlElement("check")]
        public bool check { get; set; } // bool

        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Design()
        {

        }
        
        private Design(bool _autoDesign, bool _check)
        {
            this.cMax = "";
            this.autoDesign = _autoDesign;
            this.check = _check;
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