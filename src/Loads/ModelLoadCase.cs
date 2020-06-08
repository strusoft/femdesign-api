// https://strusoft.com/

using System.Xml.Serialization;

namespace FemDesign.Loads
{
    /// <summary>
    /// load_case (child of load_combination_type)
    /// </summary>
    public class ModelLoadCase
    {
        [XmlAttribute("guid")]
        public System.Guid guid { get; set; } // common_load_case --> guidtype indexed_guid
        [XmlAttribute("gamma")]
        public double gamma { get; set; } // double
        public ModelLoadCase()
        {
            // parameterless constructor for serialization
        }

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="_guid">LoadCase guid reference.</param>
        /// <param name="_gamma">Gamma value.</param>
        public ModelLoadCase(System.Guid _guid, double _gamma)
        {
            this.guid = _guid;
            this.gamma = _gamma;
        }     
    }
}