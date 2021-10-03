// https://strusoft.com/

using System.Globalization;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_category
    /// </summary>
    [System.Serializable]
    public partial class LoadCategory : EntityBase
    {
        [XmlAttribute("standard")]
        public string Standard { get; set; } // standardtype
        [XmlAttribute("country")]
        public string Country { get; set; } // eurocodetype
        /// <summary>
        /// Name of Load Category.
        /// </summary>
        /// <value></value>
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("psi_0")]
        public double _psi0 { get; set; }
        [XmlIgnore]
        public double Psi0
        {
            get { return this._psi0; }
            set { this._psi0 = RestrictedDouble.NonNegMax_10(value); }
        }
        [XmlAttribute("psi_1")]
        public double _psi1 { get; set; }
        public double Psi1
        {
            get { return this._psi1; }
            set { this._psi1 = RestrictedDouble.NonNegMax_10(value); }
        }
        [XmlAttribute("psi_2")]
        public double _psi2 { get; set; }
        public double Psi2
        {
            get { return this._psi2; }
            set { this._psi2 = RestrictedDouble.NonNegMax_10(value); }
        }
    }
}
