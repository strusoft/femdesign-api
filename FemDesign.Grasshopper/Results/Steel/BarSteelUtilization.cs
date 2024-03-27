using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;
using Grasshopper.Kernel.Data;
using Rhino.Commands;
using FemDesign.Loads;

namespace FemDesign.Grasshopper
{
    public class BarSteelUtilization : FEM_Design_API_Component
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
            pManager.Register_StringParam("CaseIdentifier", "CaseIdentifier", "CaseIdentifier.");
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
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<FemDesign.Results.BarSteelUtilization> results = new List<FemDesign.Results.BarSteelUtilization>();
            DA.GetDataList("Result", results);

            string loadCombination = null;
            DA.GetData(1, ref loadCombination);

            if (loadCombination != null)
            {
                results = results.Where(x => x.CaseIdentifier == loadCombination).ToList();
            }


            // group iResults by loadCombination and output a datatrees
            DataTree<string> caseIdentifier = new DataTree<string>();
            DataTree<string> id = new DataTree<string>();
            DataTree<string> sectionName = new DataTree<string>();
            DataTree<double> max = new DataTree<double>();
            DataTree<double> rcs = new DataTree<double>();
            DataTree<double> fb = new DataTree<double>();
            DataTree<double> tfb = new DataTree<double>();
            DataTree<double> ltbT = new DataTree<double>();
            DataTree<double> ltbB = new DataTree<double>();
            DataTree<double> sb = new DataTree<double>();
            DataTree<double> ia = new DataTree<double>();

            var grouping = results.GroupBy(x => x.CaseIdentifier);

            int iteration = DA.Iteration;
            int grpCounter = 0;
            foreach (var group in grouping)
            {
                var caseId = group.Key;
                var elementIds = group.Select(x => x.Id);
                var sectionNames = group.Select(x => x.Section);
                var maxs = group.Select(x => x.Max);
                var rcss = group.Select(x => x.RCS);
                var fbs = group.Select(x => x.FB);
                var tfbs = group.Select(x => x.TFB);
                var ltbTs = group.Select(x => x.LTBt);
                var ltbBs = group.Select(x => x.LTBb);
                var sbs = group.Select(x => x.SB);
                var ias = group.Select(x => x.IA);

                caseIdentifier.Add(caseId, new GH_Path(iteration, grpCounter));
                id.AddRange(elementIds, new GH_Path(iteration, grpCounter));
                sectionName.AddRange(sectionNames, new GH_Path(iteration, grpCounter));
                max.AddRange(maxs, new GH_Path(iteration, grpCounter));
                rcs.AddRange(rcss, new GH_Path(iteration, grpCounter));
                fb.AddRange(fbs, new GH_Path(iteration, grpCounter));
                tfb.AddRange(tfbs, new GH_Path(iteration, grpCounter));
                ltbT.AddRange(ltbTs, new GH_Path(iteration, grpCounter));
                ltbB.AddRange(ltbBs, new GH_Path(iteration, grpCounter));
                sb.AddRange(sbs, new GH_Path(iteration, grpCounter));
                ia.AddRange(ias, new GH_Path(iteration, grpCounter));

                grpCounter++;
            }

            // output
            DA.SetDataTree(0, caseIdentifier);
            DA.SetDataTree(1, id);
            DA.SetDataTree(2, sectionName);
            DA.SetDataTree(3, max);
            DA.SetDataTree(4, rcs);
            DA.SetDataTree(5, fb);
            DA.SetDataTree(6, tfb);
            DA.SetDataTree(7, ltbT);
            DA.SetDataTree(8, ltbB);
            DA.SetDataTree(9, sb);
            DA.SetDataTree(10, ia);
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

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
            get { return new Guid("{A92848C4-2664-432B-9838-27D33B9D787B}"); }
        }
    }
}