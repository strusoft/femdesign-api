using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class ImperfectionCritical : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ImperfectionCritical class.
        /// </summary>
        public ImperfectionCritical()
          : base("ImperfectionCritical",
                "ImperfectionCritical",
                "Read the Imperfection factor/critical parameter results.",
                CategoryName.Name(), SubCategoryName.Cat7b())
        {

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "Result", "Result to be Parse", GH_ParamAccess.list);
            pManager.AddTextParameter("Combination Name", "CombName", "Name of Load Combination for which to return the results.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("Shape", "Shape", "Shape indexes start from '1' as per FEM-Design", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("CaseIdentifier", "CaseIdentifier", "CaseIdentifier.", GH_ParamAccess.list);
            pManager.AddIntegerParameter("ShapeId", "ShapeId", "Shape index", GH_ParamAccess.list);
            pManager.AddNumberParameter("CriticalParameter", "CritParam", "Critical parameters.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Amplitude", "Amplitude", "", GH_ParamAccess.list);
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

            string combName = null;
            DA.GetData(1, ref combName);

            var myRes = iResult.Where(x => x.Value.CaseIdentifier == combName);

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
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "No amplitude data available for this result type.");
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
            get { return new Guid("B4FF1A9B-E67B-4A1E-A087-0A12A4B9D884"); }
        }
    }
}