// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class StagesDeconstruct : GH_Component
    {
        public StagesDeconstruct() : base("Stages.Deconstruct", "Deconstruct", "Deconstruct Stages.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Stage", "Stage", "Stage.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddIntegerParameter("Id", "Id", "Stage id", GH_ParamAccess.item);
            pManager.AddGenericParameter("Elements", "Elements", "", GH_ParamAccess.list);
            pManager.AddTextParameter("Description", "Description", "", GH_ParamAccess.item);
            pManager.AddBooleanParameter("InitialStressState", "InitialState", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("ActivatedLoadCases", "ActivatedLoadCases", "", GH_ParamAccess.list);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Stage stage = null;
            if (!DA.GetData(0, ref stage)) return;

            var id = stage.Id;
            var elements = stage.Elements;
            var description = stage.Description;
            var initialStressState = stage.InitialStressState;
            var activeLoadCases = stage.ActivatedLoadCases;

            DA.SetData(0, id);
            DA.SetDataList(1, elements);
            DA.SetData(2, description);
            DA.SetData(3, initialStressState);
            DA.SetDataList(4, activeLoadCases);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{E654E0FE-DF2D-4A44-9A1D-CCF9013349A6}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary;

    }
}