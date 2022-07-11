using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign
{
    public partial class Stage
    {
        [XmlElement("activated_load_case")]
        public ActivatedLoadCase ActivatedLoadCase { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

        [XmlAttribute("initial_stress_state")]
        public bool InitialState { get; set; } = false;
    }
}
