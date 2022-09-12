using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class ConcreteQuantityEstimation : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public ConcreteQuantityEstimation()
          : base("ConcreteQuantityEstimation",
                "ConcreteQuantityEstimation",
                "Read the Concrete Quantity Estimation results for the entire model",
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
            pManager.Register_DoubleParam("Subtotal", "Subtotal", "");
            pManager.Register_DoubleParam("Volume", "Volume", "");
            pManager.Register_DoubleParam("TotalWeight", "TotalWeight", "");
            pManager.Register_DoubleParam("Formwork", "Formwork", "");
            pManager.Register_DoubleParam("Reinforcement", "Reinforcement", "");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata

            List<FemDesign.Results.QuantityEstimationConcrete> iResult = new List<FemDesign.Results.QuantityEstimationConcrete>();
            DA.GetDataList("Result", iResult);


            var structure = iResult.Select(x => x.Structure);
            var storey = iResult.Select(x => x.Storey);
            var id = iResult.Select(x => x.Id);
            var section = iResult.Select(x => x.Section);
            var quality = iResult.Select(x => x.Quality);
            var subTotal = iResult.Select(x => x.SubTotal);
            var volume = iResult.Select(x => x.Volume);
            var totalWeight = iResult.Select(x => x.TotalWeight);
            var formwork = iResult.Select(x => x.Formwork);
            var reinforcement = iResult.Select(x => x.Reinforcement);


            // Set output
            DA.SetDataList("Storey", storey);
            DA.SetDataList("Structure", structure);
            DA.SetDataList("Id", id);
            DA.SetDataList("Quality", quality);
            DA.SetDataList("Section", section);
            DA.SetDataList("Subtotal", subTotal);
            DA.SetDataList("Volume", volume);
            DA.SetDataList("TotalWeight", totalWeight);
            DA.SetDataList("Formwork", formwork);
            DA.SetDataList("Reinforcement", reinforcement);
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
            get { return new Guid("{A7C11ACB-788E-40F9-BDD9-3129B7AFFAEB}"); }
        }
    }
}