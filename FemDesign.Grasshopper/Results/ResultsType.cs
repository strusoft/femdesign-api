// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using GH = Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class ResultsType : GH_ValueList
    {
        public ResultsType()
        {
            Name = "Results.ResultsType";
            NickName = "ResultsType";
            Description = "ResultsType enum.";
            Category = "FEM-Design";
            SubCategory = "Results";


            ListItems.Clear();
            ListMode = GH_ValueListMode.CheckList;

            // It needs to be update when we create new Results Deconstructor
            var values = new List<string>
            { "NodalDisplacement","PointSupportReaction","BarDisplacement", "BarInternalForce", "BarStress", "LineSupportReaction", "ShellDisplacement", "ShellInternalForce", "ShellStress", "NodalVibration", "EigenFrequencies" };

            GH_ValueListItem vi;
            foreach (string value in values)
            {
                if (value.Contains("-"))
                    vi = new GH_ValueListItem(value, null);
                else
                    vi = new GH_ValueListItem(value, String.Format("\"{0}\"", value));
                ListItems.Add(vi);
            }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("c7d38a7a-32da-49ba-a6b5-f182ac8e6212"); }
        }
    }
}