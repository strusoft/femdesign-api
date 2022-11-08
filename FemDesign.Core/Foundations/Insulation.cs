using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Foundations
{
    public partial class Insulation : StruSoft.Interop.StruXml.Data.Foundation_insulation_type
    {
        public Insulation(double e, double thickness, double density, double gamma_m_u = 1.2, double gamma_m_uas = 1/2, double limitStress = 1.0)
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
