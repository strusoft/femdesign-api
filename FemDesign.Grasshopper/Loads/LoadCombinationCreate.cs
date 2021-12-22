// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.Loads;

namespace FemDesign.Grasshopper
{
    public class LoadCombinationCreate: GH_Component
    {
        public LoadCombinationCreate(): base("LoadCombination.Create", "Create", "Create a LoadCombination from a LoadCase or a list of LoadCases.", "FemDesign", "Loads")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of LoadCombination.", GH_ParamAccess.item);
            pManager.AddTextParameter("Type", "Type", "LoadCombination type. ultimate_ordinary/ultimate_accidental/ultimate_seismic/serviceability_quasi_permanent/serviceability_frequent/serviceability_characteristic.", GH_ParamAccess.item, "ultimate_ordinary");
            pManager[1].Optional = true;
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase to include in LoadCombination. Single LoadCase or list of LoadCases.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Gamma", "Gamma", "Gamma value for respective LoadCase. Single value or list of values.", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCombination", "LoadCombination.", "LoadCombination.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            string name = null, type = "ultimate_ordinary";
            List<LoadCase> loadCases = new List<LoadCase>();
            List<double> gammas = new List<double>();
            if (!DA.GetData(0, ref name)) { return; }
            if (!DA.GetData(1, ref type))
            {
                // pass
            }
            if (!DA.GetDataList(2, loadCases)) { return; }
            if (!DA.GetDataList(3, gammas)) { return; }
            if (name == null || type == null || loadCases == null || gammas == null) { return; }

            var _type = FemDesign.GenericClasses.EnumParser.Parse<LoadCombType>(type);
            LoadCombination obj = new LoadCombination(name, _type, loadCases, gammas);

            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {

                return FemDesign.Properties.Resources.LoadCombinationDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("043982aa-8f6a-41f3-896c-9c9f2f16a8ea"); }
        }
    }   
}