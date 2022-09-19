// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using FemDesign.GenericClasses;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public class BarReinforcementStirrups: GH_Component
    {
        public BarReinforcementStirrups(): base("BarReinforcement.Stirrups", "Stirrups", "Add stirrup reinforcement to a bar.", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Wire", "Wire", "Stirrup rebar material and type.", GH_ParamAccess.item);
            pManager.AddBrepParameter("Profile", "Profile", "Surface representing the profile of the stirrup in the host bar local coordinate system.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Start", "Start", "Start x-position, of stirrup reinforcement, in host bar local coordinate system. [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("End", "End", "End x-position, of stirrup reinforcement, in host bar local coordinate system. [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("Spacing", "Spacing", "Parameter representing spacing of stirrups. [m]", GH_ParamAccess.item);

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BarReinforcement", "BarReinf", "Longitudinal bar reinforcement.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
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
            if (!DA.GetData("Start", ref startParam))
            {
                return;
            }

            double endParam = 0;
            if (!DA.GetData("End", ref endParam))
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
            var stirrups = new FemDesign.Reinforcement.Stirrups(region, startParam, endParam, spacing);

            // create bar reinforcement
            var barReinf = new FemDesign.Reinforcement.BarReinforcement(Guid.Empty, wire, stirrups);

            //
            DA.SetData("BarReinforcement", barReinf);                
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

        public override GH_Exposure Exposure => GH_Exposure.secondary;
    }  
}