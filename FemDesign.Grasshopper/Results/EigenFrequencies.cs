using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using System.Linq;
using Rhino.Geometry;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class EigenFrequencies : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public EigenFrequencies()
          : base("EigenFrequencies",
                "EigenFrequencies",
                "Read the Eigen Frequencies for the entire model",
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
            pManager.Register_IntegerParam("ShapeId", "ShapeId", "ShapeId");
            pManager.Register_DoubleParam("Frequency", "Frequency", "Frequency");
            pManager.Register_DoubleParam("Period", "Period", "Period");
            pManager.Register_DoubleParam("ModalMass", "ModalMass", "ModalMass");
            pManager.Register_DoubleParam("MassParticipantXi", "MassParticipantXi", "MassParticipantXi");
            pManager.Register_DoubleParam("MassParticipantYi", "MassParticipantYi", "MassParticipantYi");
            pManager.Register_DoubleParam("MassParticipantZi", "MassParticipantZi", "MassParticipantZi");
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata

            List<FemDesign.Results.EigenFrequencies> iResult = new List<FemDesign.Results.EigenFrequencies>();
            DA.GetDataList("Result", iResult);

            var shapeId = iResult.Select(x => x.ShapeId).ToList();
            var frequency = iResult.Select(x => x.Frequency).ToList();
            var period = iResult.Select(x => x.Period).ToList();
            var modalMass = iResult.Select(x => x.ModalMass).ToList();
            var massParticipantXi = iResult.Select(x => x.MassParticipantXi).ToList();
            var massParticipantYi = iResult.Select(x => x.MassParticipantYi).ToList();
            var massParticipantZi = iResult.Select(x => x.MassParticipantZi).ToList();


            // Set output
            DA.SetDataList("ShapeId", shapeId);
            DA.SetDataList("Frequency", frequency);
            DA.SetDataList("Period", period);
            DA.SetDataList("ModalMass", modalMass);
            DA.SetDataList("MassParticipantXi", massParticipantXi);
            DA.SetDataList("MassParticipantYi", massParticipantYi);
            DA.SetDataList("MassParticipantZi", massParticipantZi);
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
                return FemDesign.Properties.Resources.results;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("67540318-11F0-4CB3-8E40-81DB2C6F259D"); }
        }
    }
}