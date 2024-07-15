using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Calculate
{
    /// <summary>
    /// Masonry configuration
    /// </summary>
    [System.Serializable]
    public partial class MasonryConfig : CONFIG
    {
        [XmlAttribute("type")]
        public string Type = "CCMSCONFIG";

        [XmlAttribute("fIgnoreAnnexForShearStrength")]
        public int _ignoreAnnexForShearStrength = 0;

        [XmlIgnore]
        public bool IgnoreAnnexForShearStrength
        {
            get
            {
                return System.Convert.ToBoolean(this._ignoreAnnexForShearStrength);
            }
            set
            {
                this._ignoreAnnexForShearStrength = System.Convert.ToInt32(value);
            }
        }

        /// <summary>
        /// Stripe width for masonry [m]
        /// </summary>
        [XmlAttribute("StripeWidth")]
        public double StripeWidth { get; set; } = 1.0;

        private MasonryConfig()
        {

        }

        public MasonryConfig(bool ignoreShearStrength, double stripeWidth)
        {
            IgnoreAnnexForShearStrength = ignoreShearStrength;
            StripeWidth = stripeWidth;
        }
    }
}
