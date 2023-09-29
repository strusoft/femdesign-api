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
            Category = CategoryName.Name();
            SubCategory = SubCategoryName.Cat7b();


            ListItems.Clear();
            ListMode = GH_ValueListMode.DropDown;

            // It needs to be update when we create new Results Deconstructor
            // It should automatically get the name from the Enum


            var values = new List<string>
            {
                nameof(FemDesign.Results.NodalDisplacement),
                nameof(FemDesign.Results.PointSupportReaction),
                nameof(FemDesign.Results.PointSupportReactionMinMax),
                nameof(FemDesign.Results.PointConnectionForce),
                nameof(FemDesign.Results.BarDisplacement),
                nameof(FemDesign.Results.BarInternalForce),
                nameof(FemDesign.Results.BarStress),
                nameof(FemDesign.Results.LineSupportReaction),
                nameof(FemDesign.Results.LineConnectionResultant),
                nameof(FemDesign.Results.LineConnectionForce),
                nameof(FemDesign.Results.SurfaceSupportReaction),
                nameof(FemDesign.Results.ShellDisplacement),
                nameof(FemDesign.Results.ShellInternalForce),
                nameof(FemDesign.Results.ShellStress),
                nameof(FemDesign.Results.CLTShellUtilization),
                nameof(FemDesign.Results.CLTFireUtilization),
                nameof(FemDesign.Results.RCBarUtilization),
                nameof(FemDesign.Results.LabelledSectionInternalForce),
                nameof(FemDesign.Results.LabelledSectionResultant),
                nameof(FemDesign.Results.NodalVibration),
                nameof(FemDesign.Results.EigenFrequencies),
                nameof(FemDesign.Results.NodalBucklingShape),
                nameof(FemDesign.Results.CriticalParameter),
                nameof(FemDesign.Results.ImperfectionFactor),
                nameof(FemDesign.Results.QuantityEstimationConcrete),
                nameof(FemDesign.Results.QuantityEstimationSteel),
                nameof(FemDesign.Results.QuantityEstimationTimber),
                nameof(FemDesign.Results.QuantityEstimationTimberPanel),
                nameof(FemDesign.Results.QuantityEstimationGeneral),
                nameof(FemDesign.Results.QuantityEstimationReinforcement),
                nameof(FemDesign.Results.FiniteElement),
                nameof(FemDesign.Results.BarTimberUtilization),
                nameof(FemDesign.Results.BarSteelUtilization)
            };
            
            GH_ValueListItem vi;
            foreach (string value in values)
            {
                if (value.Contains("---"))
                {
                    vi = new GH_ValueListItem(value, String.Format("\"{0}\"", ""));
                }
                else
                {
                    vi = new GH_ValueListItem(value, String.Format("\"{0}\"", value));
                }
                ListItems.Add(vi);
            }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ResultType;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("c7d38a7a-32da-49ba-a6b5-f182ac8e6212"); }
        }
    }
}