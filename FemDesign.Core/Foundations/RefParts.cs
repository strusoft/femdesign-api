using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace FemDesign.Foundations
{
    public partial class RefParts
    {
        [XmlAttribute("ref_slab")]
        public Guid RefSupport { get; set; }

        [XmlAttribute("ref_slab")]
        public Guid RefSlab { get; set; }

        /// <summary>
        /// to understand what ref are doing
        /// </summary>
        public RefParts()
        {
            this.RefSupport = new Guid();
            this.RefSlab = new Guid();
        }
    }
}
