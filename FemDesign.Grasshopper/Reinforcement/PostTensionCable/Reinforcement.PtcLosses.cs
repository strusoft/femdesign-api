using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FemDesign.Reinforcement
{
    public class ReinforcementPtcLosses : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public ReinforcementPtcLosses() : base("PtcLosses", "Losses", "Description", "FEM-Design", "Reinforcement")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("CurvatureCoefficient", "CurvatureCoefficient", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("WobbleCoefficient", "WobbleCoefficient", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("AnchorageSetSlip", "AnchorageSetSlip", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("ElasticShortening", "ElasticShortening", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("CreepStress", "CreepStress", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("ShrinkageStress", "ShrinkageStress", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("RelaxationStress", "RelaxationStress", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PtcLosses", "Losses", "", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double curvatureCoefficient = 0.0;
            double wobbleCoefficient = 0.0;
            double anchorageSetSlip = 0.0;
            double elasticShortening = 0.0;
            double creepStress = 0.0;
            double shrinkageStress = 0.0;
            double relaxationStress = 0.0;
            DA.GetData("CurvatureCoefficient", ref curvatureCoefficient);
            DA.GetData("WobbleCoefficient", ref wobbleCoefficient);
            DA.GetData("AnchorageSetSlip", ref anchorageSetSlip);
            DA.GetData("ElasticShortening", ref elasticShortening);
            DA.GetData("CreepStress", ref creepStress);
            DA.GetData("ShrinkageStress", ref shrinkageStress);
            DA.GetData("RelaxationStress", ref relaxationStress);

            var losses = new PtcLosses(curvatureCoefficient, wobbleCoefficient, anchorageSetSlip, elasticShortening, creepStress, shrinkageStress, relaxationStress);

            DA.SetData("PtcLosses", losses);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return FemDesign.Properties.Resources.PtcLosses;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("6323fbff-e53a-40dd-b368-8d60f04fec3b"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}