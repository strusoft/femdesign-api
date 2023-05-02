using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class StabilityImperfection : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the StabilityImperfection class.
        /// </summary>
        public StabilityImperfection()
          : base("ImperfectionCritical",
                "ImperfectionCritical",
                "Read the Imperfection/critical parameter results",
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
            pManager.AddTextParameter("CaseIdentifier", "CaseIdentifier", "CaseIdentifier.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Shape", "Shape", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("CriticalParam", "CriticalParam", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("Amplitude", "Amplitude", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata

            List<dynamic> iResult = new List<dynamic>();
            DA.GetDataList(0, iResult);

            string iLoadCase = null;
            DA.GetData(1, ref iLoadCase);

            var myRes = iResult.Where(x => x.Value.CaseIdentifier == iLoadCase);

            DA.SetDataList(0, myRes.Select(x => x.Value.CaseIdentifier).Distinct());
            DA.SetDataList(1, myRes.Select(x => x.Value.Shape));
            DA.SetDataList(2, myRes.Select(x => x.Value.CriticalParam));

            try
            {
                DA.SetDataList(3, myRes.Select(x => x.Value.Amplitude));
            }
            catch
            {
                DA.SetDataList(3, null);
            }
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;

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
            get { return new Guid("{2509502D-19AB-4181-B6BA-F4901A51C2F6}"); }
        }
    }
}