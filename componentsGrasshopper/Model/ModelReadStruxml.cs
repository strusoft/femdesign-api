// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.GH
{
    public class ModelReadStruxml: GH_Component
    {
        public ModelReadStruxml(): base("Model.ReadStruxml", "ReadStruxml", "Read model from .struxml. Add entities to model. Note: Only supported elements will loaded from the .struxml model.", "FemDesign", "Model")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("FilePathStruxml", "FilePath", "File path to .struxml file.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Bars", "Bars", "Single bar element or list of bar elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Shells", "Shells", "Single shell element or list of shell elements to add. Nested lists are not supported.", GH_ParamAccess.list);
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
            // set reference

            // get indata
            string filePath = null;
            if (!DA.GetData(0, ref filePath))
            {
                return;
            }
            List<FemDesign.Bars.Bar> bars = new List<FemDesign.Bars.Bar>();
            if (!DA.GetDataList(1, bars))
            {
                // pass
            }
            List<FemDesign.Shells.Slab> slabs = new List<FemDesign.Shells.Slab>();
            if (!DA.GetDataList(2, slabs))
            {
                // pass
            }
            List<FemDesign.Cover> covers = new List<FemDesign.Cover>();
            if (!DA.GetDataList(3, covers))
            {
                // pass
            }
            List<FemDesign.Loads.GenericLoadObject> loads = new List<FemDesign.Loads.GenericLoadObject>();
            if (!DA.GetDataList(4, loads))
            {
                // pass
            }
            List<FemDesign.Loads.LoadCase> loadCases = new List<FemDesign.Loads.LoadCase>();
            if (!DA.GetDataList(5, loadCases))
            {
                // pass
            }
            List<FemDesign.Loads.LoadCombination> loadCombinations = new List<FemDesign.Loads.LoadCombination>();
            if (!DA.GetDataList(6, loadCombinations))
            {
                // pass
            }
            List<FemDesign.Supports.GenericSupportObject> supports = new List<FemDesign.Supports.GenericSupportObject>();
            if (!DA.GetDataList(7, supports))
            {
                // pass
            }
            List<FemDesign.StructureGrid.Storey> storeys = new List<StructureGrid.Storey>();
            if (!DA.GetDataList(8, storeys))
            {
                // pass
            }
            List<FemDesign.StructureGrid.Axis> axes = new List<StructureGrid.Axis>();
            if (!DA.GetDataList(9, axes))
            {
                // pass
            }
            
            if (filePath == null) 
            {
                return;
            }

            //
            List<object> _loads = FemDesign.Loads.GenericLoadObject.ToObjectList(loads);
            List<object> _supports = FemDesign.Supports.GenericSupportObject.ToObjectList(supports);

            //
            FemDesign.Model obj = FemDesign.Model.DeserializeFromFilePath(filePath);
            obj.AddEntities(bars, slabs, covers, _loads, loadCases, loadCombinations, _supports, storeys, axes);

            // return
            DA.SetData(0, obj);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ModelFromStruxml;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("4e60944b-bad8-4211-9c24-0b62012bbacf"); }
        }
    }
}