// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.Loads;

namespace FemDesign.Grasshopper
{
    public class StageLoad : GH_Component
    {
        public StageLoad() : base("StageLoad", "StageLoad", "Creates a stage load.", "FEM-Design", "Construction Stage")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCase", "LoadCase", "Name of LoadCase.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Factor", "Factor", "Factor", GH_ParamAccess.item, 1.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("Partitioning", "Partitioning", "0 - only_in_this_stage\n1 - from_this_stage_on\n2 - shifted_from_first_stage\n3 - only_stage_activated_elem.\nDefault: from_this_stage_on.", GH_ParamAccess.item, 1);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ActivatedLoadCase", "ActivatedLoadCase", "", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Loads.LoadCase loadCase = null;
            if (!DA.GetData(0, ref loadCase)) { return; }

            double factor = 1;
            DA.GetData(1, ref factor);

            int partitioning = 1;
            PartitioningType type = (PartitioningType) partitioning; //from_this_stage_on
            DA.GetData(2, ref partitioning);

            if (Enum.IsDefined(typeof(PartitioningType), partitioning))
            {
                type = (PartitioningType) partitioning;
            }

            var activatedLoadCase = new FemDesign.ActivatedLoadCase(loadCase, factor, type);

            // return
            DA.SetData(0, activatedLoadCase);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.StageActivatedLoad;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{FEEF1ECE-9462-4DD6-A0E3-894678B5EFEC}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}