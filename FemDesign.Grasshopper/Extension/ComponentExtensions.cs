﻿using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;

// The following code has been taken from an open-source library

namespace FemDesign.Grasshopper.Extension.ComponentExtension
{
    public static class ValueListUtils
    {
        public static void UpdateValueLists(GH_Component component, int inputIndex, List<string> names, List<int> values = null, GH_ValueListMode listMode = GH_ValueListMode.DropDown)
        {
            if (names.Count == 0)
            {
                return;
            }

            foreach (var source in component.Params.Input[inputIndex].Sources.OfType<GH_ValueList>())
            {
                if (!names.SequenceEqual(source.ListItems.Select(x => x.Name)))
                {
                    source.ListItems.Clear();
                    int num = 0;

                    if (values == null || values.Count != names.Count)
                    {
                        foreach (var name in names)
                        {
                            source.ListItems.Add(new GH_ValueListItem(name, $"\"{name}\""));
                        }
                    }
                    else
                    {
                        foreach (var name in names)
                        {
                            source.ListItems.Add(new GH_ValueListItem(name, values[num++].ToString()));
                        }
                    }

                    var selectedItems = source.SelectedItems.Select(x => x.Name).ToList();
                    foreach (var selectedItem in selectedItems)
                    {
                        int index = names.IndexOf(selectedItem);
                        if (index != -1)
                        {
                            source.ToggleItem(index);
                        }
                    }

                    source.ListMode = listMode;
                    source.ExpireSolution(true);
                }
            }
        }
    }
}