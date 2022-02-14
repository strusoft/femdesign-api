// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Linq;
using FemDesign.GenericClasses;

namespace FemDesign.Grasshopper
{
    public class ModelCreateSimpleOBSOLETE : GH_Component
    {
        public ModelCreateSimpleOBSOLETE() : base("Model.CreateSimple", "Create", "Create new model. Add entities to model. Nested lists are not supported.", "FemDesign", "Model")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("CountryCode", "CountryCode", "National annex of calculation code D/DK/EST/FIN/GB/H/N/PL/RO/S/TR", GH_ParamAccess.item, "S");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Structure Elements", "Elements", "Single structure element or list of structure elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Loads", "Loads", "Single PointLoad, LineLoad, SurfaceLoad or PressureLoad element or list of PointLoad, LineLoad, SurfaceLoad or PressureLoad to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("LoadCases", "LoadCases", "Single LoadCase element or list of LoadCase elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("LoadCombinations", "LoadCombinations", "Single LoadCombination element or list of LoadCombination elements to add. Nested lists are not supported.", GH_ParamAccess.list);
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
            
            List<FemDesign.GenericClasses.IStructureElement> elements = new List<FemDesign.GenericClasses.IStructureElement>();
            DA.GetDataList("Structure Elements", elements);
            
            List<FemDesign.GenericClasses.ILoadElement> loads = new List<FemDesign.GenericClasses.ILoadElement>();
            DA.GetDataList("Loads", loads);
            
            List<FemDesign.Loads.LoadCase> loadCases = new List<FemDesign.Loads.LoadCase>();
            DA.GetDataList("LoadCases", loadCases);
            
            List<FemDesign.Loads.LoadCombination> loadCombinations = new List<FemDesign.Loads.LoadCombination>();
            DA.GetDataList("LoadCombinations", loadCombinations);
            
            // Create model
            Model model = new Model(EnumParser.Parse<Country>(countryCode), elements, loads, loadCases, loadCombinations);

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
            get { return new Guid("57879d49-01e5-48a1-a8f8-93d09554858d"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }
}