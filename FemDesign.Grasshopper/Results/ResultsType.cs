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
            Name = "ResultsType";
            NickName = "ResultsType";
            Description = "ResultsType enum.";
            Category = "FEM-Design";
            SubCategory = "Results";


            ListItems.Clear();
            ListMode = GH_ValueListMode.CheckList;

            // It needs to be update when we create new Results Deconstructor
            // It should automatically get the name from the Enum


            var values = new List<string>
            { "NodalDisplacement","PointSupportReaction","BarDisplacement", "BarInternalForce", "BarStress", "LineSupportReaction", "ShellDisplacement", "ShellInternalForce", "ShellStress", "NodalVibrationShape", "EigenFrequencies", "QuantityEstimationConcrete", "QuantityEstimationSteel", "QuantityEstimationTimber", "QuantityEstimationTimberPanel"};

            GH_ValueListItem vi;
            foreach (string value in values)
            {
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