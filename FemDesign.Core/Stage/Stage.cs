using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign.GenericClasses;

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
        public bool InitialStressState { get; set; } = false;

        [XmlElement("activated_load_case")]
        public List<ActivatedLoadCase> ActivatedLoadCases { get; set; }

        [XmlIgnore]
        public List<IStageElement> Elements { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Stage()
        {

        }

        public Stage(int index, string description, List<ActivatedLoadCase> activatedLoadCases, List<IStageElement> elements, bool initialStressState = false)
        {
            if (index <= 0)
            {
                throw new ArgumentException("index must be >= 1");
            }
            this.Id = index;
            this.Description = description;
            this.ActivatedLoadCases = activatedLoadCases;
            this.Elements = elements;
            this.InitialStressState = initialStressState;
        }

        public override string ToString()
        {
            return $"Stage {this.Description}";
        }

    }
}
