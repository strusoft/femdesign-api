using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class BarSteelUtilization : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public BarSteelUtilization()
          : base("BarSteelUtilization",
                "BarSteelUtilization",
                "Read the Bar steel utilization results for the entire model",
                CategoryName.Name(), SubCategoryName.Cat7b())
        {

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "Result", "Result to be Parse", GH_ParamAccess.list);
            pManager.AddTextParameter("Combination Name", "Case/Comb Name", "Name of Load Case/Load Combination for which to return the results.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_StringParam("Id", "Id", "");
            pManager.Register_StringParam("SectionName", "SectionName", "Applied profile");
            pManager.Register_DoubleParam("Max", "Max", "Maximum utilisation from all the verifications");
            pManager.Register_DoubleParam("RCS", "RCS", "Resistance of cross-section - Part 1-1: 6.2.1");
            pManager.Register_DoubleParam("FB", "FB", "Flexural buckling - Part 1-1: 6.3.1");
            pManager.Register_DoubleParam("TFB", "TFB", "Torsional flexural buckling - Part 1-1: 6.3.1");
            pManager.Register_DoubleParam("LTBt", "LTBt", "Lateral torsional buckling, top flange - Part 1-1: 6.3.2.4");
            pManager.Register_DoubleParam("LTBb", "LTBb", "Lateral torsional buckling, bottom flange - Part 1-1: 6.3.2.4");
            pManager.Register_DoubleParam("SB", "SB", "Shear buckling - Part 1-5: 5");
            pManager.Register_DoubleParam("IA", "IA", "Interaction - Part 1-1: 6.3.3");
            pManager.Register_StringParam("CaseIdentifier", "CaseIdentifier", "CaseIdentifier.");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<FemDesign.Results.BarSteelUtilization> iResult = new List<FemDesign.Results.BarSteelUtilization>();
            if(!DA.GetDataList("Result", iResult)) return;

            string loadCombination = null;
            DA.GetData(1, ref loadCombination);

            // Return the unique load case - load combination
            var uniqueLoadCases = iResult.Select(n => n.CaseIdentifier).Distinct().ToList();

            if(loadCombination != null)
            {
                if (uniqueLoadCases.Contains(loadCombination, StringComparer.OrdinalIgnoreCase))
                {
                    iResult = iResult.Where(n => String.Equals(n.CaseIdentifier, loadCombination, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else
                {
                    var warning = $"Load Combination '{loadCombination}' does not exist";
                    throw new ArgumentException(warning);
                }
            }





            var id = iResult.Select(x => x.Id);
            var section = iResult.Select(x => x.Section);
            var max = iResult.Select(x => x.Max);
            var rcs = iResult.Select(x => x.RCS);
            var fb = iResult.Select(x => x.FB);
            var tfb = iResult.Select(x => x.TFB);
            var ltbt = iResult.Select(x => x.LTBt);
            var ltbb = iResult.Select(x => x.LTBb);
            var sb = iResult.Select(x => x.SB);
            var ia = iResult.Select(x => x.IA);
            var caseIdentifier = iResult.Select(x => x.CaseIdentifier);


            // Set output
            DA.SetDataList(0, id);
            DA.SetDataList(1, section);
            DA.SetDataList(2, max);
            DA.SetDataList(3, rcs);
            DA.SetDataList(4, fb);
            DA.SetDataList(5, tfb);
            DA.SetDataList(6, ltbt);
            DA.SetDataList(7, ltbb);
            DA.SetDataList(8, sb);
            DA.SetDataList(9, ia);
            DA.SetDataList(10, caseIdentifier);
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return FemDesign.Properties.Resources.Results;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{005B0B34-029B-4B96-9F98-E45DB7AA87EA}"); }
        }
    }
}