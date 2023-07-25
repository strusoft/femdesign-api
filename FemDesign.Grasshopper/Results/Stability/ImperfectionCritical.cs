using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results.Utils;
using Grasshopper.Kernel.Data;
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

            int? shapeId = null;
            DA.GetData(2, ref shapeId);


            // Filter results by load combination and shape identifier
            string loadCombPropertyName;
            string shapeIdPropertyName;


            if (iResult.GetType() == typeof(ImperfectionFactor))
            {
                loadCombPropertyName = nameof(ImperfectionFactor.CaseIdentifier);
                shapeIdPropertyName = nameof(ImperfectionFactor.Shape);
            }
            else if (iResult.GetType() == typeof(CriticalParameter))
            {
                loadCombPropertyName = nameof(CriticalParameter.CaseIdentifier);
                shapeIdPropertyName = nameof(CriticalParameter.Shape);
            }
            else
            {
                throw new ArgumentException("This method cannot be used with the specified type.");
            }


            List<dynamic> filteredResults = iResult;
            if (combName != null)
            {
                filteredResults = Results.Utils.UtilResultMethods.FilterResultsByLoadCombination(filteredResults, loadCombPropertyName, combName);
            }
            if (shapeId != null)
            {
                filteredResults = Results.Utils.UtilResultMethods.FilterResultsByShapeId(filteredResults, shapeIdPropertyName, (int)shapeId);
            }



            //DataTree<dynamic> CreateTreeFromResultsByCaseIdAndShape(dynamic result)
            //{
            //    var uniqueCaseId = iResult.Select(x => x.Value.CaseIdentifier).Distinct().ToList();
            //    var uniqueShape = iResult.Select(x => x.Value.Shape).Distinct().ToList();
            //    DataTree<dynamic> allResultsTree = new DataTree<dynamic>();

            //    for (int i = 0; i < uniqueCaseId.Count; i++)
            //    {
            //        var allResultsByCaseId = iResult.Where(r => r.Value.CaseIdentifier == uniqueCaseId[i]).ToList();

            //        for (int j = 0; j < uniqueShape.Count; j++)
            //        {
            //            var pathData = allResultsByCaseId.Where(s => s.Value.Shape == uniqueShape[j]);

            //            allResultsTree.AddRange(pathData, new GH_Path(i, j));
            //            j++;
            //        }
            //        i++;
            //    }
            //    return allResultsTree;
            //}


            

            DA.SetDataList(0, filteredResults.Select(x => x.Value.CaseIdentifier));
            DA.SetDataList(1, filteredResults.Select(x => x.Value.Shape));
            DA.SetDataList(2, filteredResults.Select(x => x.Value.CriticalParam));



            try
            {
                DA.SetDataList(3, filteredResults.Select(x => x.Value.Amplitude));
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