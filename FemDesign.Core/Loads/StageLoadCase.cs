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
        public string _stageType;

        private Stage _stage;
        [XmlIgnore]
        public Stage Stage { get { return _stage; }  set { _stage = value; _stageType = $"cs.{value.Id}"; } }

        private const string _finalStage = "final_cs";
        public bool IsFinalStage => _stageType == _finalStage;
        public int StageIndex => IsFinalStage ? -1 : int.Parse(_stageType.Substring(3));

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        private StageLoadCase()
        {
        }

        public StageLoadCase(Stage stage, double gamma)
        {
            Gamma = gamma;
            Stage = stage;
        }

        public static StageLoadCase FinalStage(double gamma)
        {
            return new StageLoadCase()
            {
                _stageType = _finalStage,
                Gamma = gamma
            };
        }
    }
}
