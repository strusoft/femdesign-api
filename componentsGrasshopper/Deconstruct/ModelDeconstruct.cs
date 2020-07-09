// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.GH
{
    public class ModelDeconstruct: GH_Component
    {
        public ModelDeconstruct(): base("Model.Deconstruct", "Deconstruct", "Deconstruct Model.", "FemDesign", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            
            pManager.AddTextParameter("CountryCode", "CountryCode", "National annex of calculation code D/DK/EST/FIN/GB/H/N/PL/RO/S/TR", GH_ParamAccess.item);
            pManager.AddGenericParameter("Bars", "Bars", "Single bar element or list of bar elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Shells", "Shells", "Single shell element or list of shell elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Covers", "Covers", "Single cover element or list of cover elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Loads", "Loads", "Single GenericLoad element or list of GenericLoad elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("LoadCases", "LoadCases", "Single LoadCase element or list of LoadCase elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("LoadCombinations", "LoadCombinations", "Single LoadCombination element or list of LoadCombination elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Supports", "Supports", "Single Support element or list of Support elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Axes", "Axes", "Single axis element or list of axis elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Storeys", "Storeys", "Single storey element or list of storey elements.", GH_ParamAccess.list);

        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // set references
            FemDesign.Model model = null;
            if (!DA.GetData(0, ref model))
            {
                return;
            }
            if (model == null)
            {
                return;
            }

            // return data
            DA.SetData(0, model.country);
            DA.SetDataList(1, model.GetBars());
            DA.SetDataList(2, model.GetSlabs());
            DA.SetDataList(3, model.entities.advancedFem.cover);
            DA.SetDataList(4, model.entities.loads.GetGenericLoadObjects());
            DA.SetDataList(5, model.entities.loads.loadCase);
            DA.SetDataList(6, model.entities.loads.loadCombination);
            DA.SetDataList(7, model.entities.supports.GetGenericSupportObjects());
            DA.SetDataList(8, model.entities.axes.axis);
            DA.SetDataList(9, model.entities.storeys.storey);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ModelDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("f9e00ab9-ae48-4cc7-a370-b563e13c6978"); }
        }
    }
}