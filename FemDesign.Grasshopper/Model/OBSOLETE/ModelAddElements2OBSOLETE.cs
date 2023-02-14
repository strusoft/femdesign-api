// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Linq;

namespace FemDesign.Grasshopper
{
    public class ModelAddElements2OBSOLETE: GH_Component
    {
        public ModelAddElements2OBSOLETE(): base("Model.AddElements", "AddElements", "Add elements to an existing model. Nested lists are not supported.", CategoryName.Name(), SubCategoryName.Cat6())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel to add elements to.", GH_ParamAccess.item);
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
            pManager.AddGenericParameter("Storeys", "Storeys", "Storey element or list of Storey elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Axes", "Axes", "Axis element or list of Axis elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Overwrite", "Overwrite", "Overwrite elements sharing GUID and mark as modified?", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            FemDesign.Model model = null;
            DA.GetData("FdModel", ref model);
 
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

            List<FemDesign.Loads.ModelGeneralLoadGroup> loadGroups = new List<FemDesign.Loads.ModelGeneralLoadGroup>();
            DA.GetDataList("LoadGroups", loadGroups);

            List<FemDesign.GenericClasses.ISupportElement> supports = new List<FemDesign.GenericClasses.ISupportElement>();
            DA.GetDataList("Supports", supports);

            List<FemDesign.StructureGrid.Storey> storeys = new List<StructureGrid.Storey>();
            DA.GetDataList("Storeys", storeys);

            List<FemDesign.StructureGrid.Axis> axes = new List<StructureGrid.Axis>();
            DA.GetDataList("Axes", axes);

            bool overwrite = false;
            DA.GetData("Overwrite", ref overwrite);


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

            // cast ILoads
            List<object> _loads = loads.Cast<object>().ToList();

            // clone model
            var clone = model.DeepClone();
            
            clone.AddEntities(bars, fictBars, slabs, fictShells, panels, covers, _loads, loadCases, loadCombinations, supports, storeys, axes, loadGroups, overwrite);
            
            DA.SetData("FdModel", clone);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ModelAddElements;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("a4273a46-b666-49c2-9236-f8fcb79da75c"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }
}