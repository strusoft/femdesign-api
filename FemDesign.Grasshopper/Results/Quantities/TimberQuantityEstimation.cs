using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class TimberQuantityEstimation : FEM_Design_API_Component
    {
        /// <summary>
        /// Initializes a new instance of the TimberQuantityEstimation class.
        /// </summary>
        public TimberQuantityEstimation()
          : base("TimberQuantityEstimation",
                "TimberQuantityEstimation",
                "Read the Timber Quantity Estimation results for the entire model",
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
            pManager.Register_DoubleParam("Subtotal", "Subtotal", "");
            pManager.Register_DoubleParam("TotalWeight", "TotalWeight", "");
            pManager.Register_DoubleParam("PaintedArea", "PaintedArea", "");
            pManager.Register_DoubleParam("CO2Footprint", "CO2Footprint", "kg CO2e");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata

            List<FemDesign.Results.QuantityEstimationTimber> iResult = new List<FemDesign.Results.QuantityEstimationTimber>();
            DA.GetDataList("Result", iResult);

            var storey = iResult.Select(x => x.Storey);
            var structure = iResult.Select(x => x.Structure);
            var id = iResult.Select(x => x.Id);
            var quality = iResult.Select(x => x.Quality);
            var section = iResult.Select(x => x.Section);
            var unitWeigth = iResult.Select(x => x.UnitWeight);
            var subTotal = iResult.Select(x => x.SubTotal);
            var totalWeight = iResult.Select(x => x.TotalWeight);
            var paintedArea = iResult.Select(x => x.PaintedArea);
            var co2 = iResult.Select(x => x.CO2Footprint);


            // Set output
            DA.SetDataList("Storey", storey);
            DA.SetDataList("Structure", structure);
            DA.SetDataList("Id", id);
            DA.SetDataList("Quality", quality);
            DA.SetDataList("Section", section);
            DA.SetDataList("UnitWeight", unitWeigth);
            DA.SetDataList("Subtotal", subTotal);
            DA.SetDataList("TotalWeight", totalWeight);
            DA.SetDataList("PaintedArea", paintedArea);
            DA.SetDataList("CO2Footprint", co2);
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
                return FemDesign.Properties.Resources.Quantity;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("{76655DA2-078F-4C4A-9740-840BDC21FC6A}"); }
        }
    }
}