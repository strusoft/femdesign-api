// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

using FemDesign.GenericClasses;
using FemDesign.Loads;

namespace FemDesign.Grasshopper
{
    public class StageComponent : GH_Component
    {
        public StageComponent() : base("Stage", "Stage", "Creates a construction stage.", CategoryName.Name(),
            SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Index", "Index", "Stage Index. The indexes will reflect the stage order.", GH_ParamAccess.item);
            pManager.AddTextParameter("Description", "Description", "Description", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Elements", "Elements", "", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("ActivatedLoadCase", "ActivatedLoadCase", "", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("InitialStressState", "InitialStressState", "", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Stage", "Stage", "", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int index = 1;
            DA.GetData(0, ref index);

            string description = $"STAGE_{index}";
            DA.GetData(1, ref description);

            var elements = new List<IStageElement>();
            DA.GetDataList(2, elements);

            List<FemDesign.ActivatedLoadCase> activatedLoadCases = new List<FemDesign.ActivatedLoadCase>();
            DA.GetDataList(3, activatedLoadCases);

            bool initialStressState = false;
            DA.GetData(4, ref initialStressState);

            var stage = new FemDesign.Stage(index, description, activatedLoadCases, elements, initialStressState);

            DA.SetData(0, stage);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {

                return FemDesign.Properties.Resources.Stages;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{46D60990-DFDE-4193-A24B-3B8879059F34}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}