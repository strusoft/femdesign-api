// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using FemDesign.GenericClasses;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public class BarReinforcementLongitudinal: GH_Component
    {
        public BarReinforcementLongitudinal(): base("BarReinforcement.LongitudinalBar", "LongitudinalBar", "Create a longitudinal reinforcement bar.", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Wire", "Wire", "Longitudinal rebar material and type.", GH_ParamAccess.item);
            pManager.AddNumberParameter("YPos", "YPos", "Y-position, of longitudinal rebar, in host bar local coordinate system. [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("ZPos", "ZPos", "Z-position, of longitudinal rebar, in host bar local coordinate system. [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("StartAnchorage", "StartAnchorage", "Measure representing start anchorage of longitudinal rebar. [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("EndAnchorage", "EndAnchorage", "Measure representing end anchorage of longitudinal rebar. [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("Start", "Start", "Start x-position, of longitudinal rebar, in host bar local coordinate system.  [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("End", "End", "End x-position, of longitudinal rebar, in host bar local coordinate system.  [m]", GH_ParamAccess.item);
            pManager.AddBooleanParameter("AuxiliaryBar", "AuxBar", "Is bar auxiliary?", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BarReinforcement", "BarReinf", "Longitudinal reinforcement bar.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Reinforcement.Wire wire = null;
            if (!DA.GetData("Wire", ref wire))
            {
                return;
            }

            double yPos = 0;
            if (!DA.GetData("YPos", ref yPos))
            {
                return;
            }

            double zPos = 0;
            if (!DA.GetData("ZPos", ref zPos))
            {
                return;
            }

            double startAnchorage = 0;
            if (!DA.GetData("StartAnchorage", ref startAnchorage))
            {
                return;
            }

            double endAnchorage = 0;
            if (!DA.GetData("EndAnchorage", ref endAnchorage))
            {
                return;
            }

            double start = 0;
            if (!DA.GetData("Start", ref start))
            {
                return;
            }

            double end = 0;
            if (!DA.GetData("End", ref end))
            {
                return;
            }

            bool auxiliary = false;
            if (!DA.GetData("AuxiliaryBar", ref auxiliary))
            {
            }

            // create Longitudinal
            var pos = new FemDesign.Geometry.Point2d(yPos, zPos);
            var longBar = new FemDesign.Reinforcement.LongitudinalBar(pos, startAnchorage, endAnchorage, start, end, auxiliary);

            // create bar reinforcement without base bar reference
            var barReinf = new FemDesign.Reinforcement.BarReinforcement(Guid.Empty, wire, longBar);

            //
            DA.SetData("BarReinforcement", barReinf);                
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LongitudinalBar;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("f83c3e91-5d1d-47fc-bb9d-cb2f708e4d3a"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
    }  
}