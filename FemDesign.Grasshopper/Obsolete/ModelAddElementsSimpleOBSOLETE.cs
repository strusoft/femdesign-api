// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Linq;

namespace FemDesign.Grasshopper
{
    public class ModelAddElementsSimpleOBSOLETE : GH_Component
    {
        public ModelAddElementsSimpleOBSOLETE() : base("Model.AddElementsSimple", "AddElements", "Add elements to an existing model. Nested lists are not supported.", "FemDesign", "Model")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel to add elements to.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Structure Elements", "Elements", "Single structure element or list of structure elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Loads", "Loads", "Single PointLoad, LineLoad, SurfaceLoad or PressureLoad element or list of PointLoad, LineLoad, SurfaceLoad or PressureLoad to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("LoadCases", "LoadCases", "Single LoadCase element or list of LoadCase elements to add. Nested lists are not supported.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("LoadCombinations", "LoadCombinations", "Single LoadCombination element or list of LoadCombination elements to add. Nested lists are not supported.", GH_ParamAccess.list);
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
            if (!DA.GetData("FdModel", ref model))
            {
                // pass
            }

            List<FemDesign.GenericClasses.IStructureElement> elements = new List<FemDesign.GenericClasses.IStructureElement>();
            DA.GetDataList("Structure Elements", elements);

            List<FemDesign.GenericClasses.ILoadElement> loads = new List<FemDesign.GenericClasses.ILoadElement>();
            DA.GetDataList("Loads", loads);

            List<FemDesign.Loads.LoadCase> loadCases = new List<FemDesign.Loads.LoadCase>();
            DA.GetDataList("LoadCases", loadCases);

            List<FemDesign.Loads.LoadCombination> loadCombinations = new List<FemDesign.Loads.LoadCombination>();
            DA.GetDataList("LoadCombinations", loadCombinations);

            bool overwrite = false;
            DA.GetData("Overwrite", ref overwrite);

            var clone = model.DeepClone();
            clone.AddElements(elements, overwrite);
            clone.AddLoads(loads, overwrite);
            clone.AddLoadCases(loadCases, overwrite);
            clone.AddLoadCombinations(loadCombinations, overwrite);

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
            get { return new Guid("17494607-2eff-4988-b887-ac3290e63e3b"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }
}