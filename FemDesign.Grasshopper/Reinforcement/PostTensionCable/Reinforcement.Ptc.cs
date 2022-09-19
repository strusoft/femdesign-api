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
        public ReinforcementPtc(): base("Bar Post-Tensioned Cable", "BarPTC", "Add PTC to a bar. Curved bars are not supported.", "FEM-Design", "Reinforcement")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "FemDesign.Bars.Bar", GH_ParamAccess.item);
            pManager.AddGenericParameter("Shape", "Shape", "FemDesign.Reinforcement.PtcShapeType", GH_ParamAccess.item);
            pManager.AddGenericParameter("Losses", "Losses", "FemDesign.Reinforcement.PtcLosses", GH_ParamAccess.item);
            pManager.AddGenericParameter("Manufacturing", "Manufacturing", "FemDesign.Reinforcement.PtcManufacturingType", GH_ParamAccess.item);
            pManager.AddGenericParameter("StrandData", "StrandData", "FemDesign.Reinforcement.PtcStrandLibType", GH_ParamAccess.item);
            pManager.AddTextParameter("JackingSide", "JackingSide", "FemDesign.Reinforcement.JackingSide. Should be one of [start, end, start_then_end, end_then_start]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("JackingStress", "JackingStress", "Stress", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("NumberOfStrands", "NumberOfStrands", "Number of strands", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "Bar with post-tension cable added.", GH_ParamAccess.item);
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

            DA.GetData("Bar", ref bar);
            DA.GetData("Shape", ref shape);
            DA.GetData("Losses", ref losses);
            DA.GetData("Manufacturing", ref manufacturing);
            DA.GetData("StrandData", ref strandData);
            DA.GetData("JackingSide", ref jackingSide);
            DA.GetData("JackingStress", ref jackingStress);
            DA.GetData("NumberOfStrands", ref numberOfStrands);
            DA.GetData("Identifier", ref identifier);

            if (bar == null || shape == null || losses == null || manufacturing == null || strandData == null)
                return;

            JackingSide side = GenericClasses.EnumParser.Parse<JackingSide>(jackingSide);

            var ptc = new Ptc(bar, shape, losses, manufacturing, strandData, side, jackingStress, numberOfStrands, identifier);

            // add to bar
            var clone = bar.DeepClone();
            clone.Ptc.Add(ptc);

            DA.SetData("Bar", clone);
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
                return FemDesign.Properties.Resources.Ptc;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2eb80cef-f25f-47ef-b4be-3bd0254cbd33"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}