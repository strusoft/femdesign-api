using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StruSoft.Interop.StruXml.Data
{
    public partial class Bar_stiffness_factor_record
    {
        public Bar_stiffness_factor_record(double area, double shearArea1, double shearArea2, double torsionalConstant, double bendingAxis1, double bendingAxis2)
        {
            this.Crosssectional_area = area;
            this.Shear_area_direction_1 = shearArea1;
            this.Shear_area_direction_2 = shearArea2;
            this.Torsional_constant = torsionalConstant;
            this.Inertia_about_axis_1 = bendingAxis1;
            this.Inertia_about_axis_2 = bendingAxis2;
        }
    }
}
