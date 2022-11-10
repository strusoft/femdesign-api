using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FemDesign.Foundations
{
    [System.Serializable]
    public partial class CuboidPlinth
    {
        [XmlAttribute("a")]
        public double a { get; set; }

        [XmlAttribute("b")]
        public double b { get; set; }

        [XmlAttribute("h")]
        public double h { get; set; }

        private CuboidPlinth()
        {
        }

        public CuboidPlinth(double a, double b, double h)
        {
            this.a = a;
            this.b = b;
            this.h = h;
        }


    }
}
