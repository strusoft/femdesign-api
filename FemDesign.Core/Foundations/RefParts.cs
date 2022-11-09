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
        [XmlAttribute("ref_support")]
        public Guid RefSupport { get; set; }

        [XmlAttribute("ref_slab")]
        public Guid RefSlab { get; set; }

        private RefParts()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public RefParts(bool refSlab = false)
        {
            this.RefSupport = Guid.NewGuid();                   // What ref is referencing with the Guid?
            if (refSlab) { this.RefSlab = Guid.NewGuid(); }     // What ref is referencing with the Guid?
        }

    }
}
