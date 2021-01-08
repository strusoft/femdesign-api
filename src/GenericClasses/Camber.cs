// https://strusoft.com/
using System.Xml.Serialization;


namespace FemDesign
{
    [System.Serializable]
    public class Camber
    {
        [XmlAttribute("force")]
        public double _force;

        [XmlIgnore]
        public double Force
        {
            get
            {
                return this._force;
            }
            set
            {
                this._force = RestrictedDouble.NonNegMax_1e10(value);
            }
        }

        [XmlAttribute("e")]
        public double _eccentricity;
        [XmlIgnore]
        public double Eccentricity
        {
            get
            {
                return this._eccentricity;
            }
            set
            {
                this._eccentricity = RestrictedDouble.NonNegMax_10000(value);
            }
        }
    }
}