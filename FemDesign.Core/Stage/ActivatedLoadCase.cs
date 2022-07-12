using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign
{
    [System.Serializable]
    public partial class ActivatedLoadCase
    {
        [XmlAttribute("case")]
        public Guid Guid { get; set; }

        [XmlIgnore]
        FemDesign.Loads.LoadCase LoadCase { get; set; }

        [XmlAttribute("factor")]
        public double _factor { get; set; }

        [XmlIgnore]
        public double Factor 
        {
            get { return this._factor; }
            set { this._factor = RestrictedDouble.NonNegMax_1e30(value); }
        }

        [XmlAttribute("partitioning")]
        public PartitioningType Type { get; set; }

        private ActivatedLoadCase()
        {

        }

        public ActivatedLoadCase(Loads.LoadCase loadCase, double factor, PartitioningType type)
        {
            this.LoadCase = loadCase;
            this.Factor = factor;
            this.Type = type;
            this.Guid = loadCase.Guid;
        }
    }

    public enum PartitioningType
    {
        only_in_this_stage,
        from_this_stage_on,
        shifted_from_first_stage,
        only_stage_activated_elem
    }
}
