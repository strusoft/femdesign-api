using System;
using System.Linq;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Rhino.Geometry;
using FemDesign.Reinforcement;
using FemDesign.Grasshopper.Extension.ComponentExtension;

namespace FemDesign.Grasshopper
{
    public class ReinforcementPtc : FEM_Design_API_Component
    {
        public static readonly List<string> JackingSideValueList = Enum.GetNames(typeof(StruSoft.Interop.StruXml.Data.Ptc_jacking_side)).ToList();
        public static string JackingSideValueListDescription
        {
            get
            {
                var str = "";
                foreach (var j in JackingSideValueList)
                {
                    str += "\n" + j;
                }
                return str;
            }
        }

        /// <summary>
        /// Initializes a new instance of the ReinforcementPtc class.
        /// </summary>
        public ReinforcementPtc(): base("PTC.Define", "Define", "Create post-tensioning cables.", "FEM-Design", "Reinforcement")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Line", "Line", "Straight line.", GH_ParamAccess.item);
            pManager.AddGenericParameter("PTC.Shape", "Shape", "Cable shape.", GH_ParamAccess.item);
            pManager.AddGenericParameter("PTC.Losses", "Losses", "Short and long term losses.", GH_ParamAccess.item);
            pManager.AddGenericParameter("PTC.Manufacturing", "Manufacturing", "Cable manufacturing.", GH_ParamAccess.item);
            pManager.AddGenericParameter("PTC.Strand", "Strand", "Post-tensioning strands.", GH_ParamAccess.item);
            pManager.AddTextParameter("JackingSide", "JackingSide", $"Connect 'Value List' to get the options:{JackingSideValueListDescription}\n\nOptional, default value if undefined. Default value: '{JackingSideValueList[0]}'.", GH_ParamAccess.item, JackingSideValueList[0]);
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
            ValueListUtils.UpdateValueLists(this, 5, JackingSideValueList, null, GH_ValueListMode.DropDown);
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
            string jackingSide = JackingSideValueList[0];
            double jackingStress = 1416;
            int numberOfStrands = 3;
            string identifier = "PTC";

            if (!DA.GetData(0, ref curve)) { return; }
            if (!DA.GetData(1, ref shape)) { return; }
            if (!DA.GetData(2, ref losses)) { return; }
            if (!DA.GetData(3, ref manufacturing)) { return; }
            if (!DA.GetData(4, ref strandData)) { return; }
            DA.GetData(5, ref jackingSide);
            DA.GetData(6, ref jackingStress);
            DA.GetData(7, ref numberOfStrands);
            DA.GetData(8, ref identifier);

            if (curve == null || shape == null || losses == null || manufacturing == null || strandData == null) { return; }


            JackingSide side = FemDesign.GenericClasses.EnumParser.Parse<JackingSide>(jackingSide);

            // convert geometry
            if (!curve.IsLinear())
            {
                throw new System.ArgumentException("Curve must be a Line");
            }
            FemDesign.Geometry.LineEdge fdLine = FemDesign.Grasshopper.Convert.FromRhino2(curve);

            var ptc = new Ptc(fdLine, shape, losses, manufacturing, strandData, side, jackingStress, numberOfStrands, identifier);

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