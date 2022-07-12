using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign
{
    [System.Serializable]
    public partial class Stage
    {
        [XmlIgnore]
        public int Id { get; set; }

        [XmlAttribute("description")]
        public string Description { get; set; }

        [XmlAttribute("initial_stress_state")]
        public bool InitialState { get; set; } = false;

        [XmlElement("activated_load_case")]
        public ActivatedLoadCase ActivatedLoadCase { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Stage()
        {

        }

        public Stage(int id, string description, ActivatedLoadCase loadCase, bool initialState = false)
        {
            this.Id = id;
            this.Description = description;
            this.ActivatedLoadCase = loadCase;
            this.InitialState = initialState;
        }

    }
}
