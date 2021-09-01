using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace FemDesign.Reinforcement
{
    public class ReinforcementPtc : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ReinforcementPtc class.
        /// </summary>
        public ReinforcementPtc(): base("Post-Tensioned Cable", "PTC", "Description", "FemDesign", "Reinforcement")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("bar", "bar", "FemDesign.Bars.Bar", GH_ParamAccess.item);
            pManager.AddGenericParameter("shape", "shape", "FemDesign.Reinforcement.PtcShapeType", GH_ParamAccess.item);
            pManager.AddGenericParameter("losses", "losses", "FemDesign.Reinforcement.PtcLosses", GH_ParamAccess.item);
            pManager.AddGenericParameter("manufacturing", "manufacturing", "FemDesign.Reinforcement.PtcManufacturingType", GH_ParamAccess.item);
            pManager.AddGenericParameter("strandData", "strandData", "FemDesign.Reinforcement.PtcStrandLibType", GH_ParamAccess.item);
            pManager.AddTextParameter("jackingSide", "jackingSide", "FemDesign.Reinforcement.JackingSide", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("jackingStress", "jackingStress", "Stress", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("numberOfStrands", "numberOfStrands", "Number of strands", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("identifier", "identifier", "", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("cable", "cable", "Post-tensioned cable.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Bars.Bar bar = null;
            PtcShapeType shape = null;
            PtcLosses losses = null;
            PtcManufacturingType manufacturing = null;
            PtcStrandLibType strandData = null;
            string jackingSide = "start";
            double jackingStress = 200.0;
            int numberOfStrands = 3;
            string identifier = "PTC";

            DA.GetData("bar", ref bar);
            DA.GetData("shape", ref shape);
            DA.GetData("losses", ref losses);
            DA.GetData("manufacturing", ref manufacturing);
            DA.GetData("strandData", ref strandData);
            DA.GetData("jackingSide", ref jackingSide);
            DA.GetData("jackingStress", ref jackingStress);
            DA.GetData("numberOfStrands", ref numberOfStrands);
            DA.GetData("identifier", ref identifier);

            if (bar == null || shape == null || losses == null || manufacturing == null || strandData == null)
                return;

            JackingSide side = GenericClasses.EnumParser.Parse<JackingSide>(jackingSide);

            var ptc = new Ptc(bar, shape, losses, manufacturing, strandData, side, jackingStress, numberOfStrands, identifier);

            DA.SetData("cable", ptc);
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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2eb80cef-f25f-47ef-b4be-3bd0254cbd33"); }
        }
    }
}