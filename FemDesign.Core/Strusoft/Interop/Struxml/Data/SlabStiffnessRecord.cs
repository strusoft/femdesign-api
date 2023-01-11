using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;

namespace StruSoft.Interop.StruXml.Data
{
    public partial class Slab_stiffness_record
    {
        public Slab_stiffness_record(double bending11, double bending22, double bending12, double membrane11, double membrane22, double membrane12, double shear13, double shear23)
        {
            this.Bending_1_1 = RestrictedDouble.NonNegMax_10(bending11);
            this.Bending_2_2 = RestrictedDouble.NonNegMax_10(bending22);
            this.Bending_1_2 = RestrictedDouble.NonNegMax_10(bending12);
            this.Membran_1_1 = RestrictedDouble.NonNegMax_10(membrane11);
            this.Membran_2_2 = RestrictedDouble.NonNegMax_10(membrane22);
            this.Membran_1_2 = RestrictedDouble.NonNegMax_10(membrane12);
            this.Shear_1_3 = RestrictedDouble.NonNegMax_10(shear13);
            this.Shear_2_3 = RestrictedDouble.NonNegMax_10(shear23);
        }

        public Slab_stiffness_record()
        {
        }
    }
}
