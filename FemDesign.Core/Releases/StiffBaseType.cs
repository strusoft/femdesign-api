// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Releases
{
    /// <summary>
    /// stiff_base_type
    /// </summary>
    [System.Serializable]
    public partial class StiffBaseType
    {
        [XmlIgnore]
        private double _neg;

        [XmlAttribute("neg")]
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

        [XmlIgnore]
        private double _pos;

        [XmlAttribute("pos")]
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

        public override string ToString()
        {
            return $"Neg: {Neg} Pos: {Pos}";
        }

    }
}
