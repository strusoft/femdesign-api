// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class ModelDeconstruct_OBSOLETE: GH_Component
    {
        public ModelDeconstruct_OBSOLETE(): base("Model.Deconstruct", "Deconstruct", "Deconstruct Model.", "FEM-Design", "Deconstruct")
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
            pManager.AddGenericParameter("Diaphragms", "Diaphragms", "Single diaphragm element or list of diaphragm elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Panels", "Panels", "Single panel element or list of panel elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Covers", "Covers", "Single cover element or list of cover elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Loads", "Loads", "Single load element or list of load elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("LoadCases", "LoadCases", "Single LoadCase element or list of LoadCase elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("LoadCombinations", "LoadCombinations", "Single LoadCombination element or list of LoadCombination elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("LoadGroups", "LoadGroups", "Single load group or list of LoadGroup elements to add", GH_ParamAccess.list);
            pManager.AddGenericParameter("Supports", "Supports", "Single Support element or list of Support elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Axes", "Axes", "Single axis element or list of axis elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Storeys", "Storeys", "Single storey element or list of storey elements.", GH_ParamAccess.list);

        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // set references
            FemDesign.Model model = null;
            if (!DA.GetData("FdModel", ref model))
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
            DA.SetData("CountryCode", model.Country.ToString());
            DA.SetDataList("Bars", model.Entities.Bars);
            DA.SetDataList("FictitiousBars", model.Entities.AdvancedFem.FictitiousBars);
            DA.SetDataList("Shells", model.Entities.Slabs);
            DA.SetDataList("FictitiousShells", model.Entities.AdvancedFem.FictitiousShells);
            DA.SetDataList("Diaphragms", model.Entities.AdvancedFem.Diaphragms);
            DA.SetDataList("Panels", model.Entities.Panels);
            DA.SetDataList("Covers", model.Entities.AdvancedFem.Covers);
            DA.SetDataList("Loads", model.Entities.Loads.GetLoads());
            DA.SetDataList("LoadCases", model.Entities.Loads.LoadCases);
            DA.SetDataList("LoadCombinations", model.Entities.Loads.LoadCombinations);
            DA.SetDataList("LoadGroups", model.Entities.Loads.GetLoadGroups());
            DA.SetDataList("Supports", model.Entities.Supports.GetSupports());
            DA.SetDataList("Axes", axes);
            DA.SetDataList("Storeys", storeys);
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
            get { return new Guid("fcf74381-24b9-48ad-b006-a999fe020212"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

    }
}