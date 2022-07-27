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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="autoDesign"></param>
        /// <param name="check"></param>
        /// <param name="loadCombination">True if you want the design to be based on Load Combination. False if you want the design to be based on Load Group</param>
        public Design(bool autoDesign = false, bool check = true, bool loadCombination = true)
        {
            if (loadCombination) this.CMax = "";
            else this.GMax = "";
            this.AutoDesign = autoDesign;
            this.Check = check;
        }
    }
}