using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StruSoft.Interop.StruXml.Data
{
    public partial class Stiffness_record
    {
        public override string ToString()
        {
            return $"Stiffness Kx'({this.Kx_neg} {this.Kx_pos}), Ky'({this.Ky_neg} {this.Ky_pos}), Kz'({this.Kz_neg} {this.Kz_pos}), Cx'({this.Cx_neg} {this.Cx_pos}), Cy'({this.Cy_neg} {this.Cy_pos}), Cz'({this.Cz_neg} {this.Cz_pos})";
        }
    }
}
