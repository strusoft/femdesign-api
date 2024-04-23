// https://strusoft.com/
using FemDesign.Loads;
using System.Xml.Serialization;


namespace FemDesign
{
    [System.Serializable]
    public partial class ForceLoadBase: LoadBase
    {
        /// <summary>
        /// Specifies if the load vector describes a force or a moment
        /// </summary>
        [XmlAttribute("load_type")] // force_load_type
        public Loads.ForceLoadType LoadType { get; set; }

    }

    [System.Serializable]
    public partial class SupportMotionBase : LoadBase
    {
        [XmlAttribute("load_type")]
        public SupportMotionType SupportMotionType { get; set; }
    }

}