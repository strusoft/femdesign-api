using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.ComponentModel;

namespace FemDesign.Foundations
{
    public partial class Insulation
    {

        [XmlAttribute("e_modulus")]
        public double E_modulus { get; set; }

        [XmlAttribute("thickness")]
        public double Thickness { get; set; }

        [XmlAttribute("density")]
        public double Density { get; set; }

        [XmlAttribute("gamma_m_u")]
        public double Gamma_m_u { get; set; }

        [XmlAttribute("gamma_m_uas")]
        public double Gamma_m_uas { get; set; }

        [XmlAttribute("limit_stress")]
        public double Limit_stress { get; set; }

        public Insulation()
        {
        }

        public Insulation(double e, double thickness, double density, double limitStress, double gamma_m_u = 1.2, double gamma_m_uas = 1.0)
        {
            this.E_modulus = e;
            this.Thickness = thickness;
            this.Density = density;
            this.Gamma_m_u = gamma_m_u;
            this.Gamma_m_uas = gamma_m_uas;
            this.Limit_stress = limitStress;
        }
    }
}
