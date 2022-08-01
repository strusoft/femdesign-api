using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;

namespace FemDesign.Grasshopper.Extension
{
    public static partial class ComponentExtensions
    {
        internal static GH_ValueList SetValueList(this GH_Component GhComp, List<string> ResultTypes, int inputIndex)
        {
            if (GhComp.Params.Input[inputIndex].SourceCount == 1)
            {
                if (GhComp.Params.Input[inputIndex]?.Sources[0] is GH_ValueList)
                {
                    var valueList = GhComp.Params.Input[inputIndex].Sources[0] as GH_ValueList;
                    valueList.ListMode = GH_ValueListMode.DropDown;

                    if (valueList.ListItems.Count != ResultTypes.Count)
                    {
                        valueList.ListItems.Clear();
                        for (int i = 0; i < ResultTypes.Count; i++)
                        {
                            var name = ResultTypes[i];
                            valueList.ListItems.Add(new GH_ValueListItem(name, String.Format("\"{0}\"", name)));
                        }
                    }
                    valueList.SelectItem(1);
                    valueList.Attributes.ExpireLayout();
                    valueList.Attributes.PerformLayout();

                    return valueList;
                }
                return null;
            }
            else
            {
                return null;
            }
        }
    }
}
