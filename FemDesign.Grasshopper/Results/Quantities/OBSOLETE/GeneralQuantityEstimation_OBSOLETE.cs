﻿using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class GeneralQuantityEstimation_OBSOLETE : FEM_Design_API_Component
    {
        /// <summary>
        /// Initializes a new instance of the GeneralQuantityEstimation class.
        /// </summary>
        public GeneralQuantityEstimation_OBSOLETE()
          : base("GeneralQuantityEstimation",
                "GeneralQuantityEstimation",
                "Read the Steel General Estimation results for the entire model",
                CategoryName.Name(), SubCategoryName.Cat7b())
        {

        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Result", "Result", "Result to be Parse", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.Register_StringParam("Storey", "Storey", "");
            pManager.Register_StringParam("Structure", "Structure", "");
            pManager.Register_StringParam("Id", "Id", "");
            pManager.Register_StringParam("Quality", "Quality", "");
            pManager.Register_StringParam("Section", "Section", "");
            pManager.Register_DoubleParam("UnitWeight", "UnitWeight", "");
            pManager.Register_DoubleParam("SubTotal", "SubTotal", "");
            pManager.Register_DoubleParam("TotalWeight", "TotalWeight", "");
            pManager.Register_DoubleParam("PaintedArea", "PaintedArea", "");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata

            List<FemDesign.Results.QuantityEstimationGeneral> iResult = new List<FemDesign.Results.QuantityEstimationGeneral>();
            DA.GetDataList("Result", iResult);

            var storey = iResult.Select(x => x.Storey);
            var structure = iResult.Select(x => x.Structure);
            var id = iResult.Select(x => x.Id);
            var quality = iResult.Select(x => x.Quality);
            var section = iResult.Select(x => x.Section);
            var unitWeight = iResult.Select(x => x.UnitWeight);
            var subTotal = iResult.Select(x => x.SubTotal);
            var totalWeight = iResult.Select(x => x.TotalWeight);
            var paintedArea = iResult.Select(x => x.PaintedArea);


            // Set output
            DA.SetDataList("Storey", storey);
            DA.SetDataList("Structure", structure);
            DA.SetDataList("Id", id);
            DA.SetDataList("Quality", quality);
            DA.SetDataList("Section", section);
            DA.SetDataList("UnitWeight", unitWeight);
            DA.SetDataList("SubTotal", subTotal);
            DA.SetDataList("TotalWeight", totalWeight);
            DA.SetDataList("PaintedArea", paintedArea);
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return FemDesign.Properties.Resources.Quantity;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{187AA397-AF08-46D1-BDA9-914738186FFD}"); }
        }
    }
}