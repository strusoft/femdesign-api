using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign
{
    public partial class ActivatedLoadCase
    {
        [XmlAttribute("case")]
        public Guid Guid
        {
            get { return this.LoadCase.Guid; }
        }

        [XmlIgnore]
        FemDesign.Loads.LoadCase LoadCase { get; set; }

        [XmlAttribute("factor")]
        public double Factor { get; set; }

        [XmlAttribute("partitioning")]
        public PartitioningType Type { get; set; }
    }

    public enum PartitioningType
    {
        only_in_this_stage,
        from_this_stage_on,
        shifted_from_first_stage,
        only_stage_activated_elem
    }
}
