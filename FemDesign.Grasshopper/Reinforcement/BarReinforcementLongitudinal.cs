// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using FemDesign.GenericClasses;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public class BarReinforcementLongitudinal: GH_Component
    {
        public BarReinforcementLongitudinal(): base("BarReinforcement.LongitudinalBar", "LongitudinalBar", "Add longitudinal reinforcement to a bar. Curved bars are not supported.", "FemDesign", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "Bar to add longitudinal rebars to", GH_ParamAccess.item);
            pManager.AddGenericParameter("Wire", "Wire", "Longitudinal rebar material and type.", GH_ParamAccess.item);
            pManager.AddNumberParameter("YPos", "YPos", "YPos", GH_ParamAccess.item);
            pManager.AddNumberParameter("ZPos", "ZPos", "ZPos", GH_ParamAccess.item);
            pManager.AddNumberParameter("StartAnchorage", "StartAnchorage", "Measure representing start anchorage of longitudinal rebar in meter.", GH_ParamAccess.item);
            pManager.AddNumberParameter("EndAnchorage", "EndAnchorage", "Measure representing end anchorage of longitudinal rebar in meter.", GH_ParamAccess.item);
            pManager.AddNumberParameter("StartParameter", "StartParam", "Parameter representing start position of longitudinal rebar. 0 is start of bar and 1 is end of bar", GH_ParamAccess.item);
            pManager.AddNumberParameter("EndParameter", "EndParam", "Parameter representing start position of longitudinal rebar. 0 is start of bar and 1 is end of bar", GH_ParamAccess.item);
            pManager.AddBooleanParameter("AuxiliaryBar", "AuxBar", "Is bar auxiliary?", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "Bar with longitudinal rebar added", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Bars.Bar bar = null;
            if (!DA.GetData("Bar", ref bar))
            {
                return;
            }

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

            double startParam = 0;
            if (!DA.GetData("StartParameter", ref startParam))
            {
                return;
            }

            double endParam = 0;
            if (!DA.GetData("EndParameter", ref endParam))
            {
                return;
            }

            bool auxiliary = false;
            if (!DA.GetData("AuxiliaryBar", ref auxiliary))
            {
            }

            // create Longitudinal
            var pos = new FemDesign.Geometry.FdPoint2d(yPos, zPos);
            var longBar = new FemDesign.Reinforcement.LongitudinalBar(bar, pos, startAnchorage, endAnchorage, startParam, endParam, auxiliary);

            // create bar reinforcement
            var barReinf = new FemDesign.Reinforcement.BarReinforcement(bar, wire, longBar);

            // add to bar
            var clone = bar.DeepClone();
            clone.Reinforcement.Add(barReinf);

            //
            DA.SetData("Bar", clone);                
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
    }  
}