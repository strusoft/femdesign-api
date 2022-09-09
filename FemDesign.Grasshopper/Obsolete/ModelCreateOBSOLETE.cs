// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Linq;

namespace FemDesign.Grasshopper
{
    public class ModelCreateOBSOLETE : GH_Component
    {
        public ModelCreateOBSOLETE() : base("Model.Create", "Create", "Create new model. Add entities to model. Nested lists are not supported.", CategoryName.Name(), SubCategoryName.Cat6())
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
            // Get indata
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

            // Create model
            List<object> _loads = loads.Cast<object>().ToList();

            Country country = GenericClasses.EnumParser.Parse<Country>(countryCode);
            Model model = new Model(country);
            model.AddEntities(bars, fictBars, slabs, fictShells, panels, covers, _loads, loadCases, loadCombinations, supports, storeys, axes, null, false);

            DA.SetData(0, model);
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
        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }
}