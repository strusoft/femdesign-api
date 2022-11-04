using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign.GenericClasses;
using FemDesign.Loads;

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

        /// <summary>
        /// Construction stage.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="description">Stage description (name).</param>
        /// <param name="activatedLoadCases">LoadCases with factor and partitioning for when to be activated.</param>
        /// <param name="elements">Elements to be activated in this stage.</param>
        /// <param name="initialStressState">Initial stress state.</param>
        public Stage(int index, string description, List<ActivatedLoadCase> activatedLoadCases = null, List<IStageElement> elements = null, bool initialStressState = false)
        {
            Initialize(index, description, activatedLoadCases, elements, initialStressState);
        }

        /// <summary>
        /// Construction stage.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="description">Stage description (name).</param>
        /// <param name="loadCases">LoadCases to be activated (with factor 1.0).</param>
        /// <param name="elements">Elements to be activated in this stage.</param>
        /// <param name="partitioning">Partitioning for when to activate load cases.</param>
        /// <param name="initialStressState">Initial stress state.</param>
#if ISDYNAMO // Dynamo may not have any default enum arguments in any constructor in any imported C# libraries it seems like
        public Stage(int index, string description, List<LoadCase> loadCases, List<IStageElement> elements, ActivationType partitioning, bool initialStressState = false)
#else
        public Stage(int index, string description, List<LoadCase> loadCases, List<IStageElement> elements, ActivationType partitioning = ActivationType.OnlyInThisStage, bool initialStressState = false)
#endif
        {
            var activatedLoadCase = loadCases.Select(l => new ActivatedLoadCase(l, 1.0, partitioning)).ToList();
            Initialize(index, description, activatedLoadCase, elements, initialStressState);
        }
        private void Initialize(int index, string description, List<ActivatedLoadCase> activatedLoadCases, List<IStageElement> elements, bool initialStressState)
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

        /// <summary>
        /// Add element to construction stage.
        /// </summary>
        /// <param name="element">Element to be activated in this stage.</param>
        public void AddElement(IStageElement element)
        {
            this.Elements.Add(element);
        }

        /// <summary>
        /// Adds a (construction stage) activated load case.
        /// </summary>
        /// <param name="loadCase">The load case to be activated.</param>
        /// <param name="factor">Load case factor.</param>
        /// <param name="partitioning">Partitioning.</param>
        public void AddLoadCase(LoadCase loadCase, double factor, ActivationType partitioning)
        {
            AddLoadCase(new ActivatedLoadCase(loadCase, factor, partitioning));
        }

        /// <summary>
        /// Adds a (construction stage) activated load case.
        /// </summary>
        public void AddLoadCase(ActivatedLoadCase activatedLoadCase)
        {
            this.ActivatedLoadCases.Add(activatedLoadCase);
        }

        public override string ToString()
        {
            return $"Stage {this.Description}";
        }

    }
}
