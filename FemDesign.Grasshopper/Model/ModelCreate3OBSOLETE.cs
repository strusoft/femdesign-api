// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Linq;
using FemDesign.GenericClasses;


namespace FemDesign.Grasshopper
{
    public class ModelCreate3OBSOLETE : GH_Component
    {
        public ModelCreate3OBSOLETE() : base("Model.Create", "Create", "Create new model. Add entities to model. Nested lists are not supported.", CategoryName.Name(), SubCategoryName.Cat6())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("CountryCode", "CountryCode", "National annex of calculation code D/DK/EST/FIN/GB/H/N/PL/RO/S/TR", GH_ParamAccess.item, "S");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Bars", "Bars", "Single bar element or list of bar elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("FictitiousBars", "FictBars", "Single fictitious bar element or list of fictitious bar elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Shells", "Shells", "Single shell element or list of shell elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("FictitiousShells", "FictShells", "Single fictitious shell element or list of fictitious shell elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Panels", "Panels", "Panel element or list of Panel elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Covers", "Covers", "Single cover element or list of cover elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Loads", "Loads", "Single PointLoad, LineLoad, SurfaceLoad or PressureLoad element or list of PointLoad, LineLoad, SurfaceLoad or PressureLoad to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("LoadCases", "LoadCases", "Single LoadCase element or list of LoadCase elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("LoadCombinations", "LoadCombinations", "Single LoadCombination element or list of LoadCombination elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("LoadGroups", "LoadGroups", "Single load group or list of LoadGroup elements to add. Nested lists are not supported", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Supports", "Supports", "Single PointSupport, LineSupport or SurfaceSupport element or list of PointSupport, LineSupport or SurfaceSupport elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Axes", "Axes", "Axis element or list of Axis elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Storeys", "Storeys", "Storey element or list of Storey elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get indata
            string countryCode = "S";
            DA.GetData("CountryCode", ref countryCode);

            List<FemDesign.Bars.Bar> bars = new List<FemDesign.Bars.Bar>();
            DA.GetDataList("Bars", bars);

            List<FemDesign.ModellingTools.FictitiousBar> fictBars = new List<FemDesign.ModellingTools.FictitiousBar>();
            DA.GetDataList("FictitiousBars", fictBars);

            List<FemDesign.Shells.Slab> slabs = new List<FemDesign.Shells.Slab>();
            DA.GetDataList("Shells", slabs);

            List<FemDesign.ModellingTools.FictitiousShell> fictShells = new List<FemDesign.ModellingTools.FictitiousShell>();
            DA.GetDataList("FictitiousShells", fictShells);

            List<FemDesign.Shells.Panel> panels = new List<Shells.Panel>();
            DA.GetDataList("Panels", panels);

            List<FemDesign.Cover> covers = new List<FemDesign.Cover>();
            DA.GetDataList("Covers", covers);

            List<FemDesign.GenericClasses.ILoadElement> loads = new List<FemDesign.GenericClasses.ILoadElement>();
            DA.GetDataList("Loads", loads);

            List<FemDesign.Loads.LoadCase> loadCases = new List<FemDesign.Loads.LoadCase>();
            DA.GetDataList("LoadCases", loadCases);

            List<FemDesign.Loads.LoadCombination> loadCombinations = new List<FemDesign.Loads.LoadCombination>();
            DA.GetDataList("LoadCombinations", loadCombinations);

            List<FemDesign.GenericClasses.ISupportElement> supports = new List<FemDesign.GenericClasses.ISupportElement>();
            DA.GetDataList("Supports", supports);

            List<FemDesign.StructureGrid.Axis> axes = new List<StructureGrid.Axis>();
            DA.GetDataList("Axes", axes);

            List<FemDesign.StructureGrid.Storey> storeys = new List<StructureGrid.Storey>();
            DA.GetDataList("Storeys", storeys);

            List<FemDesign.Loads.ModelGeneralLoadGroup> loadGroups = new List<FemDesign.Loads.ModelGeneralLoadGroup>();
            DA.GetDataList("LoadGroups", loadGroups);

            // Ensure that the component recieves the load cases that are included in the load groups
            bool loadCasesProvided = true;
            if (loadGroups.Any())
            {
                foreach (Loads.ModelGeneralLoadGroup loadGroup in loadGroups)
                {
                    loadCasesProvided = loadGroup.GetLoadCases().All(i => loadCases.Contains(i));
                    if (!loadCasesProvided)
                    {
                        this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Must provide all load cases used in load groups");
                        return;
                    }
                }
            }
            
            // Create model
            List<object> _loads = loads.Cast<object>().ToList();

            Model model = new Model(EnumParser.Parse<Country>(countryCode));
            model.AddEntities(bars, fictBars, slabs, fictShells, panels, covers, _loads, loadCases, loadCombinations, supports, storeys, axes, loadGroups, false);

            DA.SetData("FdModel", model);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ModelCreate;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("75413c19-cdef-4434-bd8f-530b860d3350");}
        }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }
}