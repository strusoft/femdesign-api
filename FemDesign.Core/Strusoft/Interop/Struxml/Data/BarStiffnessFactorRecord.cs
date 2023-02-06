using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign;

namespace StruSoft.Interop.StruXml.Data
{
    public partial class Bar_stiffness_factor_record
    {
        public Bar_stiffness_factor_record(double area, double shearArea1, double shearArea2, double torsionalConstant, double bendingAxis1, double bendingAxis2)
        {
            this.Crosssectional_area = RestrictedDouble.NonNegMax_10(area);
            this.Shear_area_direction_1 = RestrictedDouble.NonNegMax_10(shearArea1);
            this.Shear_area_direction_2 = RestrictedDouble.NonNegMax_10(shearArea2);
            this.Torsional_constant = RestrictedDouble.NonNegMax_10(torsionalConstant);
            this.Inertia_about_axis_1 = RestrictedDouble.NonNegMax_10(bendingAxis1);
            this.Inertia_about_axis_2 = RestrictedDouble.NonNegMax_10(bendingAxis2);
        }
    }
}
