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

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // set reference
            string countryCode = "S";
            List<FemDesign.Bars.Bar> bars = new List<FemDesign.Bars.Bar>();
            List<FemDesign.Shells.Slab> slabs = new List<FemDesign.Shells.Slab>();
            List<FemDesign.Cover> covers = new List<FemDesign.Cover>();
            List<FemDesign.Loads.GenericLoadObject> loads = new List<FemDesign.Loads.GenericLoadObject>();
            List<FemDesign.Loads.LoadCase> loadCases = new List<FemDesign.Loads.LoadCase>();
            List<FemDesign.Loads.LoadCombination> loadCombinations = new List<FemDesign.Loads.LoadCombination>();
            List<FemDesign.Supports.GenericSupportObject> supports = new List<FemDesign.Supports.GenericSupportObject>();

            // get indata
            if (!DA.GetData(0, ref countryCode))
            {
                // pass
            }
            if (!DA.GetDataList(1, bars))
            {
                // pass
            }
            if (!DA.GetDataList(2, slabs))
            {
                // pass
            }
            if (!DA.GetDataList(3, covers))
            {
                // pass
            }
            if (!DA.GetDataList(4, loads))
            {
                // pass
            }
            if (!DA.GetDataList(5, loadCases))
            {
                // pass
            }
            if (!DA.GetDataList(6, loadCombinations))
            {
                // pass
            }
            if (!DA.GetDataList(7, supports))
            {
                // pass
            }

            // supports
            List<object> _loads = FemDesign.Loads.GenericLoadObject.ToObjectList(loads);
            List<object> _supports = FemDesign.Supports.GenericSupportObject.ToObjectList(supports);
            
            //
            FemDesign.Model _obj = new FemDesign.Model(countryCode);
            _obj.AddEntities(bars, slabs, covers, _loads, loadCases, loadCombinations, _supports);

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