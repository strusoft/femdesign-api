using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using FemDesign.GenericClasses;

namespace FemDesign
{
    public enum PTCLoadCase
    {
        /// <summary>
        /// PTC T0
        /// </summary>
        T0,
        /// <summary>
        /// PTC T8
        /// </summary>
        T8,
    }

    [System.Serializable]
    public partial class ActivatedLoadCase
    {

        [XmlAttribute("case")]
        public string _case;

        public bool IsLoadCase => Guid.TryParse(_case, out Guid _);
        public Guid LoadCaseGuid => IsLoadCase ? new Guid(_case) : throw new Exception($"Case \"{_case}\" is not a guid of a load case.");

        private static readonly Regex IndexedGuidPattern = new Regex("(?<guid>[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12})#(?<index>[1-9][0-9]{0,4})");
        public bool IsMovingLoad => IndexedGuidPattern.IsMatch(_case);
        public int MovingLoadIndex => IsMovingLoad
            ? int.Parse(IndexedGuidPattern.Match(_case).Groups["index"].Value)
            : throw new Exception($"Case \"{_case}\" is not an indexed guid of a moving load case.");

        private static readonly Regex PTCLoadCasePattern = new Regex("ptc_t(0|8)");
        public bool IsPTCLoadCase => PTCLoadCasePattern.IsMatch(_case);
        public PTCLoadCase PTCLoadCase => IsPTCLoadCase ? (_case.EndsWith("0") ? PTCLoadCase.T0 : PTCLoadCase.T8) : throw new Exception($"Case \"{_case}\" is not a PTC load case.");

        [XmlIgnore]
        public string LoadCaseDisplayName { get; internal set; }

        [XmlAttribute("factor")]
        public double _factor { get; set; }

        [XmlIgnore]
        public double Factor
        {
            get { return this._factor; }
            set { this._factor = RestrictedDouble.NonNegMax_1e30(value); }
        }

        [XmlAttribute("partitioning")]
        public ActivationType Type { get; set; }

        /// <summary>
        /// Private construction for serialization
        /// </summary>
        private ActivatedLoadCase()
        {

        }

        /// <summary>
        /// Create a (construction stage) activated load case.
        /// </summary>
        /// <param name="loadCase">The load case to be activated.</param>
        /// <param name="factor">Load case factor.</param>
        /// <param name="partitioning">Partitioning.</param>
        public ActivatedLoadCase(Loads.LoadCase loadCase, double factor, ActivationType partitioning)
        {
            this.LoadCaseDisplayName = loadCase.Name;
            this._case = loadCase.Guid.ToString();
            Initialize(factor, partitioning);
        }

        /// <summary>
        /// Create a (construction stage) activated load case of a PTC load case.
        /// </summary>
        /// <param name="loadCase">The PTC load case to be activated.</param>
        /// <param name="factor">Load case factor.</param>
        /// <param name="type">Activation type.</param>
        public ActivatedLoadCase(PTCLoadCase loadCase, double factor, ActivationType type)
        {
            this.LoadCaseDisplayName =
                loadCase == PTCLoadCase.T0 ? "PTC T0" :
                loadCase == PTCLoadCase.T8 ? "PTC T8" :
                throw new Exception("Not a valid PTCLoadCase value");
            this._case =
                loadCase == PTCLoadCase.T0 ? "ptc_t0" :
                loadCase == PTCLoadCase.T8 ? "ptc_t8" :
                throw new Exception("Not a valid PTCLoadCase value");
            Initialize(factor, type);
        }

        /// <summary>
        /// Create a (construction stage) activated load case of a Moving load case.
        /// </summary>
        /// <param name="movingLoad">The moving load to be activated.</param>
        /// <param name="index">The step/index of the moving load.</param>
        /// <param name="factor">Load case factor.</param>
        /// <param name="type">Activation type.</param>
        public ActivatedLoadCase(StruSoft.Interop.StruXml.Data.Moving_load_type movingLoad, int index, double factor, ActivationType type)
        {
            this.LoadCaseDisplayName = movingLoad.Name + "-" + index; // E.g "MVL-42"
            this._case = movingLoad.Guid.ToString() + "#" + index;
            Initialize(factor, type);
        }

        private void Initialize(double factor, ActivationType type)
        {
            this.Factor = factor;
            this.Type = type;
        }

        public override string ToString()
        {
            return $"ActivatedLoadCase {this.LoadCaseDisplayName} {this.Factor} {this.Type}";
        }
    }

    public enum ActivationType
    {
        /// <summary>
        /// Only in this stage
        /// </summary>
        [Parseable("only_in_this_stage", "0", "OnlyInThisStage")]
        [XmlEnum("only_in_this_stage")]
        OnlyInThisStage,
        /// <summary>
        /// From this stage on
        /// </summary>
        [Parseable("from_this_stage_on", "1", "FromThisStageOn")]
        [XmlEnum("from_this_stage_on")]
        FromThisStageOn,
        /// <summary>
        /// Shifted from first stage
        /// </summary>
        [Parseable("shifted_from_first_stage", "2", "ShiftedFromFirstStage")]
        [XmlEnum("shifted_from_first_stage")]
        ShiftedFromFirstStage,
        /// <summary>
        /// Only stage activated elements
        /// </summary>
        [Parseable("only_stage_activated_elem", "3", "OnlyStageActivatedElements")]
        [XmlEnum("only_stage_activated_elem")]
        OnlyStageActivatedElements
    }
}
