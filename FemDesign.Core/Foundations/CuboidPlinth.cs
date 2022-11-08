using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Foundations
{
    public partial class CuboidPlinth
    {
        public double a { get; set; }
        public double b { get; set; }
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
