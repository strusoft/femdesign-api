using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace FemDesign.Shells
{
    public partial class SlabStiffnessFactors
    {
        [XmlElement("factors")]
        public List<StruSoft.Interop.StruXml.Data.Slab_stiffness_record> StiffnessModifiers { get; set; }
    }
}
