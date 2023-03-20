using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Rhino.Geometry;

using FemDesign.Grasshopper.Extension.ComponentExtension;

namespace FemDesign.Reinforcement
{
    public class ReinforcementPtc : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ReinforcementPtc class.
        /// </summary>
        public ReinforcementPtc(): base("PTC.Define", "Define", "Create post-tensioning cables for bars. Curved bars are not supported.", "FEM-Design", "Reinforcement")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Curve", "Curve", "Line curve.", GH_ParamAccess.item);
            pManager.AddGenericParameter("PTC.Shape", "Shape", "Cable shape.", GH_ParamAccess.item);
            pManager.AddGenericParameter("PTC.Losses", "Losses", "Short and long term losses.", GH_ParamAccess.item);
            pManager.AddGenericParameter("PTC.Manufacturing", "Manufacturing", "Cable manufacturing.", GH_ParamAccess.item);
            pManager.AddGenericParameter("PTC.Strand", "Strand", "Post-tensioning strands.", GH_ParamAccess.item);
            pManager.AddTextParameter("JackingSide", "JackingSide", "Connect 'Value List' to get the options.\nJacking side type:\nstart\nend\nstart_then_end\nend_then_start.\n\nOptional, default value if undefined. Default value is start.", GH_ParamAccess.item,"start");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("JackingStress", "JackingStress", "Jacking stress [N/mm2]. Optional, default value if undefined. Default value is 1416 N/mm2.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("NumberOfStrands", "NumberOfStrands", "Number of strands. Identifier. Optional, default value if undefined. Default value is 3.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PTC", "PTC", "Post-tensioning cables.", GH_ParamAccess.item);
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 5, new List<string>{"start", "end", "start_then_end", "end_then_start" }, null, GH_ValueListMode.DropDown);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = null;
            PtcShapeType shape = null;
            PtcLosses losses = null;
            PtcManufacturingType manufacturing = null;
            PtcStrandLibType strandData = null;
            string jackingSide = "start";
            double jackingStress = 1416;
            int numberOfStrands = 3;
            string identifier = "PTC";

            if (!DA.GetData("Curve", ref curve)) { return; }
            if (!DA.GetData("Shape", ref shape)) { return; }
            if (!DA.GetData("Losses", ref losses)) { return; }
            if (!DA.GetData("Manufacturing", ref manufacturing)) { return; }
            if (!DA.GetData("StrandData", ref strandData)) { return; }
            DA.GetData("JackingSide", ref jackingSide);
            DA.GetData("JackingStress", ref jackingStress);
            DA.GetData("NumberOfStrands", ref numberOfStrands);
            DA.GetData("Identifier", ref identifier);

            if (curve == null || shape == null || losses == null || manufacturing == null || strandData == null) { return; }

            // convert geometry
            FemDesign.Geometry.Edge line = curve.FromRhinoLineOrArc1();

            if(!line.IsLine())
            {
                throw new System.ArgumentException("Curve parameter is not straight. PTC can only be added to lines.");
            }

            JackingSide side = GenericClasses.EnumParser.Parse<JackingSide>(jackingSide);

            var ptc = new Ptc(line, shape, losses, manufacturing, strandData, side, jackingStress, numberOfStrands, identifier);

            DA.SetData(0, ptc);
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
            get { return new Guid("15A60FE2-1BAF-41B2-993B-7F541D56F42C"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}