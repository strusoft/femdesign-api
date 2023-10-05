using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using Grasshopper.Kernel;



namespace FemDesign.Grasshopper
{
    public class ImperfectionCritical : FEM_Design_API_Component
    {
        /// <summary>
        /// Initializes a new instance of the ImperfectionCritical class.
        /// </summary>
        public ImperfectionCritical()
          : base("ImperfectionCritical",
                "ImperfectionCritical",
                "Read the imperfection factor/critical parameter results.",
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
            pManager.AddTextParameter("Combination Name", "CombNames", "Load combination names.", GH_ParamAccess.list);
            pManager.AddIntegerParameter("ShapeId", "ShapeId", "Shape index", GH_ParamAccess.list);
            pManager.AddNumberParameter("CriticalParameter", "CritParam", "Critical parameters.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Amplitude", "Amplitude", "Amplitude results. Only for ImperfectionFactor result types.", GH_ParamAccess.list);
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

            int? shapeId = null;
            DA.GetData(2, ref shapeId);


            // get filtered results
            var filteredResults = iResult;
            if (combName != null)
            {
                var allCaseIds = filteredResults.Select(r => (string)r.Value.CaseIdentifier);
                if (!allCaseIds.Contains(combName, StringComparer.OrdinalIgnoreCase))
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, $"Incorrect or unknown load combination name: {combName}.");
                }
                filteredResults = filteredResults.Where(r => String.Equals((string)r.Value.CaseIdentifier, combName, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (shapeId != null)
            {
                var maxShapeId = filteredResults.Select(r => (int)r.Value.Shape).Max();
                if ((shapeId < 1) || (shapeId > maxShapeId))
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, $"ShapeId {shapeId} is out of range.");
                }
                filteredResults = filteredResults.Where(r => (int)r.Value.Shape == shapeId).ToList();
            }


            // get output
            var combOut = filteredResults.Select(x => x.Value.CaseIdentifier).ToList();
            var shapeOut = filteredResults.Select(x => x.Value.Shape).ToList();
            var critParamOut = filteredResults.Select(x => x.Value.CriticalParam).ToList();
            var amplitudeOut = filteredResults.Select(x => x.Value.Amplitude).ToList();

            DA.SetDataList(0, combOut);
            DA.SetDataList(1, shapeOut);
            DA.SetDataList(2, critParamOut);

            try
            {
                DA.SetDataList(3, amplitudeOut);
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