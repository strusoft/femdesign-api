// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

using FemDesign.GenericClasses;
using FemDesign.Loads;

namespace FemDesign.Grasshopper
{
    public class Stage : GH_Component
    {
        public Stage() : base("Stage", "Stage", "Creates a construction stage.", "FEM-Design", "Construction Stage")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("Index", "Index", "Stage Index. The indexes will reflect the stage order.", GH_ParamAccess.item);
            pManager.AddTextParameter("Description", "Description", "Description", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Elements", "Elements", "", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("ActivatedLoadCase", "ActivatedLoadCase", "", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("InitialStressState", "InitialStressState", "", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Stages", "Stages", "", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            int index = 1;
            DA.GetData(0, ref index);

            string description = $"STAGE_{index}";
            DA.GetData(1, ref description);

            var elements = new List<IStageElement>();
            DA.GetDataList(2, elements);

            ActivatedLoadCase loadCase = null;
            DA.GetData(3, ref loadCase);

            bool initialStressState = false;
            DA.GetData(4, ref initialStressState);

            var stage = new FemDesign.Stage(index, description, loadCase, elements, initialStressState);

            // return
            DA.SetData(0, stage);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {

                return FemDesign.Properties.Resources.StageDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{46D60990-DFDE-4193-A24B-3B8879059F34}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}