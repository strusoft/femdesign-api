// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Linq;

namespace FemDesign.Grasshopper
{
    public class ModelAddElementsOBSOLETE : GH_Component
    {
        public ModelAddElementsOBSOLETE() : base("Model.AddElements", "AddElements", "Add elements to an existing model. Nested lists are not supported.", "FemDesign", "Model")
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
            if (!DA.GetData(0, ref model))
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

            List<FemDesign.Shells.Slab> slabs = new List<FemDesign.Shells.Slab>();
            if (!DA.GetDataList(3, slabs))
            {
                // pass
            }

            List<FemDesign.ModellingTools.FictitiousShell> fictShells = new List<FemDesign.ModellingTools.FictitiousShell>();
            if (!DA.GetDataList(4, fictShells))
            {
                // pass
            }

            List<FemDesign.Shells.Panel> panels = new List<Shells.Panel>();
            {
                if (!DA.GetDataList(5, panels))
                {
                    // pass
                }
            }

            List<FemDesign.Cover> covers = new List<FemDesign.Cover>();
            if (!DA.GetDataList(6, covers))
            {
                // pass
            }

            List<FemDesign.GenericClasses.ILoadElement> loads = new List<FemDesign.GenericClasses.ILoadElement>();
            if (!DA.GetDataList(7, loads))
            {
                // pass
            }

            List<FemDesign.Loads.LoadCase> loadCases = new List<FemDesign.Loads.LoadCase>();
            if (!DA.GetDataList(8, loadCases))
            {
                // pass
            }

            List<FemDesign.Loads.LoadCombination> loadCombinations = new List<FemDesign.Loads.LoadCombination>();
            if (!DA.GetDataList(9, loadCombinations))
            {
                // pass
            }

            List<FemDesign.GenericClasses.ISupportElement> supports = new List<FemDesign.GenericClasses.ISupportElement>();
            if (!DA.GetDataList(10, supports))
            {
                // pass
            }

            List<FemDesign.StructureGrid.Storey> storeys = new List<StructureGrid.Storey>();
            if (!DA.GetDataList(11, storeys))
            {
                // pass
            }

            List<FemDesign.StructureGrid.Axis> axes = new List<StructureGrid.Axis>();
            if (!DA.GetDataList(12, axes))
            {
                // pass
            }

            bool overwrite = false;
            if (!DA.GetData(13, ref overwrite))
            {
                // pass
            }

            // cast ILoads
            List<object> _loads = loads.Cast<object>().ToList();

            // clone model
            var clone = model.DeepClone();

            clone.AddEntities(bars, fictBars, slabs, fictShells, panels, covers, _loads, loadCases, loadCombinations, supports, storeys, axes, null, overwrite);

            DA.SetData(0, clone);
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
            get { return new Guid("17494607-2eff-4988-b887-ac3290e63e3a"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }
}