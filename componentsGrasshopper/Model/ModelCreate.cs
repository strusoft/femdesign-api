// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.GH
{
    public class ModelCreate: GH_Component
    {
        public ModelCreate(): base("Model.Create", "Create", "Create new model. Add entities to model. Nested lists are not supported.", "FemDesign", "Model")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("CountryCode", "CountryCode", "National annex of calculation code D/DK/EST/FIN/GB/H/N/PL/RO/S/TR", GH_ParamAccess.item, "S");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Bars", "Bars", "Single bar element or list of bar elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("FictitiousBars", "FictBars", "Single fictitious bar element or list of fictitious bar elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager.AddGenericParameter("ConnectedLines", "ConnLines", "Single connected line element or list of connected line elements to add. Nested lists are not supported.", GH_ParamAccess.list);
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
            pManager.AddGenericParameter("Supports", "Supports", "Single PointSupport, LineSupport or SurfaceSupport element or list of PointSupport, LineSupport or SurfaceSupport elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Storeys", "Storeys", "Storey element or list of Storey elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Axes", "Axes", "Axis element or list of Axis elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            string countryCode = "S";
            if (!DA.GetData(0, ref countryCode))
            {
                // pass
            }
 
            List<FemDesign.Bars.Bar> bars = new List<FemDesign.Bars.Bar>();
            if (!DA.GetDataList(1, bars))
            {
                // pass
            }

            List<FemDesign.ModellingTools.FictitiousBar> fictBars = new List<FemDesign.ModellingTools.FictitiousBar>();
            if (!DA.GetDataList(2, fictBars))
            {
                // pass
            }

            List<FemDesign.ModellingTools.ConnectedLines> connLines = new List<FemDesign.ModellingTools.ConnectedLines>();
            if (!DA.GetDataList(3, connLines))
            {
                // pass
            }
 
            List<FemDesign.Shells.Slab> slabs = new List<FemDesign.Shells.Slab>();
            if (!DA.GetDataList(4, slabs))
            {
                // pass
            }

            List<FemDesign.ModellingTools.FictitiousShell> fictShells = new List<FemDesign.ModellingTools.FictitiousShell>();
            if (!DA.GetDataList(5, fictShells))
            {
                // pass
            }

            List<FemDesign.Shells.Panel> panels = new List<Shells.Panel>();
            {
                if (!DA.GetDataList(6, panels))
                {
                    // pass
                }
            }
  
            List<FemDesign.Cover> covers = new List<FemDesign.Cover>();
            if (!DA.GetDataList(7, covers))
            {
                // pass
            }
  
            List<FemDesign.Loads.GenericLoadObject> loads = new List<FemDesign.Loads.GenericLoadObject>();
            if (!DA.GetDataList(8, loads))
            {
                // pass
            }
 
            List<FemDesign.Loads.LoadCase> loadCases = new List<FemDesign.Loads.LoadCase>();
            if (!DA.GetDataList(9, loadCases))
            {
                // pass
            }

            List<FemDesign.Loads.LoadCombination> loadCombinations = new List<FemDesign.Loads.LoadCombination>();
            if (!DA.GetDataList(10, loadCombinations))
            {
                // pass
            }

            List<FemDesign.Supports.GenericSupportObject> supports = new List<FemDesign.Supports.GenericSupportObject>();
            if (!DA.GetDataList(11, supports))
            {
                // pass
            }

            List<FemDesign.StructureGrid.Storey> storeys = new List<StructureGrid.Storey>();
            if (!DA.GetDataList(12, storeys))
            {
                // pass
            }

            List<FemDesign.StructureGrid.Axis> axes = new List<StructureGrid.Axis>();
            if (!DA.GetDataList(13, axes))
            {
                // pass
            }

            // supports
            List<object> _loads = FemDesign.Loads.GenericLoadObject.ToObjectList(loads);
            List<object> _supports = FemDesign.Supports.GenericSupportObject.ToObjectList(supports);
            
            //
            FemDesign.Model _obj = new FemDesign.Model(countryCode);
            _obj.AddEntities(bars, fictBars, connLines, slabs, fictShells, panels, covers, _loads, loadCases, loadCombinations, _supports, storeys, axes, false);

            // return
            DA.SetData(0, _obj);

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
            get { return new Guid("57879d49-01e5-48a1-a8f8-93d09554858c"); }
        }
    }
}