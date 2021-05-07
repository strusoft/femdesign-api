// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Releases
{
    /// <summary>
    /// stiffness_with_friction
    /// </summary>
    [System.Serializable]
    public partial class StiffnessWithFriction: SimpleStiffnessType
    {
        [XmlAttributeAttribute("friction")]
        public double _friction;
        [XmlIgnore]
        public double Friction
        {
            get
            {
                return this._friction;
            }
            set
            {
                this._friction = RestrictedDouble.NonNegMax_1(value);
            }
        }

        public StiffnessWithFriction()
        {

        }
    }
}
