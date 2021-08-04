// https://strusoft.com/

using System.Xml.Serialization;
using System.Globalization;

namespace FemDesign.Releases
{
    /// <summary>
    /// stiff_base_type
    /// </summary>
    [System.Serializable]
    public partial class PlasticityType
    {
        [XmlAttribute("neg")]
        public string _neg;

        [XmlIgnore]
        public double? Neg
        {
            get
            {
                return string.IsNullOrEmpty(_neg) ? (double?)null : (double?)double.Parse(this._neg, CultureInfo.InvariantCulture);
            }
            set
            {
                this._neg = value == null ? null : RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture);
            }
        }

        [XmlAttribute("pos")]
        public string _pos;

        [XmlIgnore]
        public double? Pos
        {
            get
            {
                return string.IsNullOrEmpty(_pos) ? (double?)null : (double?)double.Parse(this._pos, CultureInfo.InvariantCulture);
            }
            set
            {
                this._pos = value == null ? null : RestrictedDouble.NonNegMax_1e15((double)value).ToString(CultureInfo.InvariantCulture);
            }
        }
    }

    /// <summary>
    /// stiff_base_type
    /// </summary>
    [System.Serializable]
    public partial class PlasticityType2
    {
        [XmlAttribute("neg")]
        public double _neg;

        [XmlIgnore]
        public double Neg
        {
            get
            {
                return this._neg;
            }
            set
            {
                this._neg = RestrictedDouble.NonNegMax_1e15(value);
            }
        }

        [XmlAttribute("pos")]
        public double _pos;

        [XmlIgnore]
        public double Pos
        {
            get
            {
                return this._pos;
            }
            set
            {
                this._pos = RestrictedDouble.NonNegMax_1e15(value);
            }
        }
    }
}
