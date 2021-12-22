// https://strusoft.com/
using System.Xml.Serialization;

namespace FemDesign.Reinforcement
{
    /// <summary>
    /// centric (child of surface_rf_type)
    /// </summary>
    [System.Serializable]
    public partial class Centric
    {
        [XmlAttribute("face")]
        public GenericClasses.Face Face;

        [XmlAttribute("cover")]
        public double Cover;

        [XmlIgnore]
        public bool MultiLayer
        {
            get
            {
                if (this.Face == GenericClasses.Face.Mid)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        [XmlIgnore]
        public bool SingleLayer
        {
            get
            {
                if (this.Face == GenericClasses.Face.Mid)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}