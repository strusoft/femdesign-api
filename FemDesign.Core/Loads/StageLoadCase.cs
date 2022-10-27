using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FemDesign.Loads
{
    public class StageLoadCase : LoadCombinationCaseBase
    {
        [XmlAttribute("type")]
        public string Stage;

        private const string _finalStage = "final_cs";
        public bool IsFinalStage => Stage == _finalStage;
        public int StageIndex => IsFinalStage ? -1 : int.Parse(Stage.Substring(3));

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private StageLoadCase()
        {
        }

        public StageLoadCase(Stage stage, double gamma)
        {
            Gamma = gamma;
            Stage = $"cs.{stage.Id}";
        }

        public static StageLoadCase FinalStage(double gamma)
        {
            return new StageLoadCase()
            {
                Stage = _finalStage,
                Gamma = gamma
            };
        }
    }
}
