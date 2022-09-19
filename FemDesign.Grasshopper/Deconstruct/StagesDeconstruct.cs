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

            pManager.Register_IntegerParam("Id", "Id", "");
            pManager.Register_GenericParam("Elements", "Elements", "");
            pManager.Register_StringParam("Description", "Description", "");
            pManager.Register_BooleanParam("InitialState", "InitialState", "");
            pManager.Register_GenericParam("ActivatedLoadCase", "ActivatedLoadCase", "");
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // set references
            FemDesign.Stage stage = null;
            if (!DA.GetData(0, ref stage))
            {
                return;
            }

            var id = stage.Id;
            var elements = stage.Elements;
            var description = stage.Description;
            var initialState = stage.InitialState;
            var activeLoadCase = stage.ActivatedLoadCase;

            // return data
            DA.SetData(0, id);
            DA.SetDataList(1, elements);
            DA.SetData(2, description);
            DA.SetData(3, initialState);
            DA.SetData(4, activeLoadCase);

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