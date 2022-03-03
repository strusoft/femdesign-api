// https://strusoft.com/

using System.Xml.Serialization;
using System.Collections.Generic;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_case (child of load_combination_type)
    /// </summary>
    [System.Serializable]
    public partial class ModelLoadCase
    {
        [XmlAttribute("guid")]
        public System.Guid Guid { get; set; } // common_load_case --> guidtype indexed_guid
        [XmlAttribute("gamma")]
        public double Gamma { get; set; } // double
        public ModelLoadCase()
        {
            // parameterless constructor for serialization
        }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="guid">LoadCase guid reference.</param>
        /// <param name="gamma">Gamma value.</param>
        public ModelLoadCase(System.Guid guid, double gamma)
        {
            this.Guid = guid;
            this.Gamma = gamma;
        }     
    }
}