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
        [XmlIgnore]
        public bool ApplyChanges { get; set; } // bool
        
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
        /// <param name="applyChanges">True will force FemDesign to apply the new cross sections to the model at the end of the design process.</param>
        public Design(bool autoDesign = false, bool check = true, bool loadCombination = true, bool applyChanges = false)
        {
            if (loadCombination) this.CMax = "";
            else this.GMax = "";
            this.AutoDesign = autoDesign;
            this.Check = check;
            this.ApplyChanges = applyChanges;
        }

        public static Design Default()
        {
            return new Design(false, true, true, false);
        }
    }
}