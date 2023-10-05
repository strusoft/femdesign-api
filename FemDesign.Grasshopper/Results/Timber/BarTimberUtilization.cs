using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class BarTimberUtilization : FEM_Design_API_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public BarTimberUtilization()
          : base("BarTimberUtilization",
                "BarTimberUtilization",
                "Read the Bar timber utilization results for the entire model",
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
            pManager.Register_DoubleParam("T", "T", "Combined bending and axial tension - 6.2.3");
            pManager.Register_DoubleParam("C", "C", "Combined bending and compression tension - 6.1.4, 6.2.4");
            pManager.Register_DoubleParam("S", "S", "Combined shear and torsion - 6.1.7, 6.1.8");
            pManager.Register_DoubleParam("FB1", "FB1", "Flexural buckling around axis 1 - 6.3.2");
            pManager.Register_DoubleParam("FB2", "FB2", "Flexural buckling around axis 2 - 6.3.2");
            pManager.Register_DoubleParam("Ltb", "Ltb", "Lateral torsional buckling - 6.3.3");
            pManager.Register_StringParam("CaseIdentifier", "CaseIdentifier", "CaseIdentifier.");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<FemDesign.Results.BarTimberUtilization> iResult = new List<FemDesign.Results.BarTimberUtilization>();
            if (!DA.GetDataList("Result", iResult)) return;

            string loadCombination = null;
            DA.GetData(1, ref loadCombination);

            // Return the unique load case - load combination
            var uniqueLoadCases = iResult.Select(n => n.CaseIdentifier).Distinct().ToList();

            if (loadCombination != null)
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
            var t = iResult.Select(x => x.T);
            var c = iResult.Select(x => x.C);
            var s = iResult.Select(x => x.S);
            var fb1 = iResult.Select(x => x.FB1);
            var fb2 = iResult.Select(x => x.FB2);
            var ltb = iResult.Select(x => x.Ltb);
            var caseIdentifier = iResult.Select(x => x.CaseIdentifier);


            // Set output
            DA.SetDataList(0, id);
            DA.SetDataList(1, section);
            DA.SetDataList(2, max);
            DA.SetDataList(3, t);
            DA.SetDataList(4, c);
            DA.SetDataList(5, s);
            DA.SetDataList(6, fb1);
            DA.SetDataList(7, fb2);
            DA.SetDataList(8, ltb);
            DA.SetDataList(9, caseIdentifier);
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
            get { return new Guid("{2289381E-45E4-46E7-9D48-B01C01CCE7BC}"); }
        }
    }
}