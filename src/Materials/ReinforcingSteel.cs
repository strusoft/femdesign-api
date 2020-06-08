// https://strusoft.com/

using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Materials
{
    /// <summary>
    /// rfmaterial_type --> reinforcing_steel.
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class ReinforcingSteel
    {
        [XmlAttribute("fyk")]
        public string fyk {get; set;} // non_neg_max_1e10
        [XmlAttribute("Es")]
        public string Es {get; set;} // non_neg_max_1e10
        [XmlAttribute("Epsilon_uk")]
        public string epsilon_uk {get; set;} // non_neg_max_1e10
        [XmlAttribute("Epsilon_ud")]
        public string epsilon_ud {get; set;} // non_neg_max_1e10
        [XmlAttribute("k")]
        public string k {get; set;} // rc_k_value

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ReinforcingSteel()
        {

        }
    }
}