// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class ModelDeconstructOBSOLETE : GH_Component
    {
        public ModelDeconstructOBSOLETE() : base("Model.Deconstruct", "Deconstruct", "Deconstruct Model.", "FEM-Design", "Deconstruct")
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
            pManager.AddGenericParameter("FictitiousBars", "FictBars", "Single fictitious bar element or list of fictitious bar elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Shells", "Shells", "Single shell element or list of shell elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("FictitiousShells", "FictShells", "Single fictitious shell element or list of fictitious shell elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Panels", "Panels", "Single panel element or list of panel elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Covers", "Covers", "Single cover element or list of cover elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Loads", "Loads", "Single load element or list of load elements.", GH_ParamAccess.list);
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

            List<StructureGrid.Axis> axes;
            if (model.Entities.Axes != null)
            {
                axes = model.Entities.Axes.Axis;
            }
            else
            {
                axes = null;
            }

            List<StructureGrid.Storey> storeys;
            if (model.Entities.Storeys != null)
            {
                storeys = model.Entities.Storeys.Storey;
            }
            else
            {
                storeys = null;
            }

            // return data
            DA.SetData(0, model.Country);
            DA.SetDataList(1, model.Entities.Bars);
            DA.SetDataList(2, model.Entities.AdvancedFem.FictitiousBars);
            DA.SetDataList(3, model.Entities.Slabs);
            DA.SetDataList(4, model.Entities.AdvancedFem.FictitiousShells);
            DA.SetDataList(5, model.Entities.Panels);
            DA.SetDataList(6, model.Entities.AdvancedFem.Covers);
            DA.SetDataList(7, model.Entities.Loads.GetLoads());
            DA.SetDataList(8, model.Entities.Loads.LoadCases);
            DA.SetDataList(9, model.Entities.Loads.LoadCombinations);
            DA.SetDataList(10, model.Entities.Supports.GetSupports());
            DA.SetDataList(11, axes);
            DA.SetDataList(12, storeys);
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

        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }
}