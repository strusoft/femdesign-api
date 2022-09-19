using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class TimberPanelQuantityEstimation : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public TimberPanelQuantityEstimation()
          : base("TimberPanelQuantityEstimation",
                "TimberPanelQuantityEstimation",
                "Read the Timber Panel Quantity Estimation results for the entire model",
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
            pManager.Register_StringParam("Quality", "Quality", "");
            pManager.Register_StringParam("Id", "Id", "");
            pManager.Register_StringParam("PanelType", "PanelType", "");
            pManager.Register_DoubleParam("Thickness", "Thickness", "");
            pManager.Register_DoubleParam("Length", "Length", "");
            pManager.Register_DoubleParam("Width", "Width", "");
            pManager.Register_DoubleParam("Area", "Area", "");
            pManager.Register_IntegerParam("Pieces", "Pieces", "");
            pManager.Register_DoubleParam("TotalWeight", "TotalWeight", "");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata

            List<FemDesign.Results.QuantityEstimationTimberPanel> iResult = new List<FemDesign.Results.QuantityEstimationTimberPanel>();
            DA.GetDataList("Result", iResult);


            var structure = iResult.Select(x => x.Structure);
            var storey = iResult.Select(x => x.Storey);
            var id = iResult.Select(x => x.Id);
            var quality = iResult.Select(x => x.Quality);
            var panelType = iResult.Select(x => x.PanelType);
            var thickness = iResult.Select(x => x.Thickness);
            var totalWeight = iResult.Select(x => x.TotalWeight);
            var length = iResult.Select(x => x.Length);
            var width = iResult.Select(x => x.Width);
            var area = iResult.Select(x => x.Area);
            var pieces = iResult.Select(x => x.Count);


            // Set output
            DA.SetDataList("Storey", storey);
            DA.SetDataList("Structure", structure);
            DA.SetDataList("Quality", quality);
            DA.SetDataList("Id", id);
            DA.SetDataList("PanelType", panelType);
            DA.SetDataList("Thickness", thickness);
            DA.SetDataList("Length", length);
            DA.SetDataList("Width", width);
            DA.SetDataList("Area", area);
            DA.SetDataList("Pieces", pieces);
            DA.SetDataList("TotalWeight", totalWeight);
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
                return FemDesign.Properties.Resources.quantity;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{95931DAA-9F3A-4931-8AB3-BBE814587AF8}"); }
        }
    }
}