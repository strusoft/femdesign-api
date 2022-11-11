using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class SurfaceSupportResultant : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PointSupportReaction class.
        /// </summary>
        public SurfaceSupportResultant()
          : base("SurfaceSupportResultant",
                "SurfaceSupportResultant",
                "Read the Surface Support Resultant",
                CategoryName.Name(), SubCategoryName.Cat7b())
        {

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "Result", "Result to be Parse", GH_ParamAccess.list);
            pManager.AddTextParameter("Case/Combination Name", "Case/Comb Name", "Name of Load Case/Load Combination for which to return the results.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_StringParam("CaseIdentifier", "CaseIdentifier", "CaseIdentifier.");
            pManager.Register_GenericParam("Name", "Name", "Element Name");
            pManager.Register_DoubleParam("Fx", "Fx", "");
            pManager.Register_DoubleParam("Fy", "Fy", "");
            pManager.Register_DoubleParam("Fz", "Fz", "");
            pManager.Register_DoubleParam("Mx", "Mx", "");
            pManager.Register_DoubleParam("My", "My", "");
            pManager.Register_DoubleParam("Mz", "Mz", "");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            List<FemDesign.Results.SurfaceSupportResultant> iResult = new List<FemDesign.Results.SurfaceSupportResultant>();
            DA.GetDataList("Result", iResult);

            string iLoadCase = null;
            DA.GetData(1, ref iLoadCase);

            // Return the unique load case - load combination
            // it might be remove to optimise the speed
            var uniqueLoadCases = iResult.Select(n => n.CaseIdentifier).Distinct().ToList();

            // Select the Nodal Displacement for the selected Load Case - Load Combination
            if (uniqueLoadCases.Contains(iLoadCase, StringComparer.OrdinalIgnoreCase))
            {
                iResult = iResult.Where(n => String.Equals(n.CaseIdentifier, iLoadCase, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            else
            {
                var warning = $"Load Case '{iLoadCase}' does not exist";
                throw new ArgumentException(warning);
            }

            var caseIdentifier = new List<string>();
            var name = new List<string>();
            var fx = new List<double>();
            var fy = new List<double>();
            var fz = new List<double>();
            var mx = new List<double>();
            var my = new List<double>();
            var mz = new List<double>();

            foreach(var result in iResult)
            {
                caseIdentifier.Add(result.CaseIdentifier);
                name.Add(result.Name);
                fx.Add(result.Fx);
                fy.Add(result.Fy);
                fz.Add(result.Fz);
                mx.Add(result.Mx);
                my.Add(result.My);
                mz.Add(result.Mz);
            }


            DA.SetDataList(0, caseIdentifier);
            DA.SetDataList(1, name);
            DA.SetDataList(2, fx);
            DA.SetDataList(3, fy);
            DA.SetDataList(4, fz);
            DA.SetDataList(5, mx);
            DA.SetDataList(6, my);
            DA.SetDataList(7, mz);
        }

        public override GH_Exposure Exposure => GH_Exposure.quinary;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return FemDesign.Properties.Resources.results;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{5712F504-A99A-4B21-A842-56FC20014690}"); }
        }
    }
}