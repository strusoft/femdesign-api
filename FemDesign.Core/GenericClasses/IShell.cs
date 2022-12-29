using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.GenericClasses
{
    public interface IShell
    {
        void UpdateMaterial(Materials.Material material);
    }
}
