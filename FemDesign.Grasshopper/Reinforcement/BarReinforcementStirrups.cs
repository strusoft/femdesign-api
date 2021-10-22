// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using FemDesign.GenericClasses;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public class BarReinforcementStirrups: GH_Component
    {
        public BarReinforcementStirrups(): base("BarReinforcement.Stirrups", "Stirrups", "Add stirrup reinforcement to a bar. Curved bars are not supported.", "FemDesign", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "Bar to add stirrups to", GH_ParamAccess.item);
            pManager.AddGenericParameter("Wire", "Wire", "Stirrup rebar material and type.", GH_ParamAccess.item);
            pManager.AddBrepParameter("Profile", "Profile", "Surface representing the profile of the stirrup.", GH_ParamAccess.item);
            pManager.AddNumberParameter("StartParameter", "StartParam", "Parameter representing start position of stirrups. 0 is start of bar and 1 is end of bar", GH_ParamAccess.item);
            pManager.AddNumberParameter("EndParameter", "EndParam", "Parameter representing start position of stirrups. 0 is start of bar and 1 is end of bar", GH_ParamAccess.item);
            pManager.AddNumberParameter("Spacing", "Spacing", "Parameter representing spacing of stirrups.", GH_ParamAccess.item);

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "Bar with stirrups added", GH_ParamAccess.item);
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

            Rhino.Geometry.Brep profile = null;
            if (!DA.GetData("Profile", ref profile))
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

            double spacing = 0;
            if (!DA.GetData("Spacing", ref spacing))
            {
                return;
            }

            // transform profile to region
            var region = profile.FromRhino();

            // create stirrups
            var stirrups = new FemDesign.Reinforcement.Stirrups(bar, region, startParam, endParam, spacing);

            // create bar reinforcement
            var barReinf = new FemDesign.Reinforcement.BarReinforcement(bar, wire, stirrups);

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
                return FemDesign.Properties.Resources.Stirrups;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("c8d371c3-b485-4810-9953-822e62e32bee"); }
        }
    }  
}