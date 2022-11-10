using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.ComponentModel;

namespace FemDesign.Foundations
{
    public partial class ExtrudedSolid
    {
        [XmlAttribute("thickness")]
        public double Thickness { get; set; }

        [XmlAttribute("abobe")]
        [DefaultValue(false)]
        public bool Above { get; set; }

        [XmlElement("region")]
        public FemDesign.Geometry.Region Region { get; set; }

        [XmlElement("cuboid_plinth")]
        public CuboidPlinth Plinth { get; set; }

        private ExtrudedSolid()
        {
        }

        public ExtrudedSolid(double thickness, Geometry.Region region, bool above = false, CuboidPlinth plinth = null)
        {
            this.Thickness = thickness;
            this.Region = region;
            this.Above = above;
            this.Plinth = plinth;
        }

    }
}
