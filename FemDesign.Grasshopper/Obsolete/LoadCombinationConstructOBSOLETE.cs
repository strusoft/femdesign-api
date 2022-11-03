// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Special;
using FemDesign.Loads;
using FemDesign.Grasshopper.Extension.ComponentExtension;

namespace FemDesign.Grasshopper
{
    public class LoadCombinationConstructOBSOLETE : GH_Component
    {
        public LoadCombinationConstructOBSOLETE() : base("LoadCombination.Construct", "Construct", "Construct a LoadCombination from a LoadCase or a list of LoadCases.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of LoadCombination.", GH_ParamAccess.item);
            pManager.AddTextParameter("Type", "Type", "Connect 'ValueList' to get the options.\nultimate_ordinary\nultimate_accidental\nultimate_seismic\nserviceability_quasi_permanent\nserviceability_frequent\nserviceability_characteristic.", GH_ParamAccess.item, "ultimate_ordinary");
            pManager[1].Optional = true;
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase to include in LoadCombination. Single LoadCase or list of LoadCases.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Gamma", "Gamma", "Gamma value for respective LoadCase. Single value or list of values. [-]", GH_ParamAccess.list);
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

            DA.GetData(1, ref type);


            if (!DA.GetDataList(2, loadCases)) { return; }
            if (!DA.GetDataList(3, gammas)) { return; }

            var _type = FemDesign.GenericClasses.EnumParser.Parse<LoadCombType>(type);

            if (name == null || type == null || loadCases == null || gammas == null) { return; }


            LoadCombination obj = new LoadCombination(name, _type, loadCases, gammas);

            DA.SetData(0, obj);
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 1, new List<string>
            { "ultimate_ordinary",
                "ultimate_accidental",
                "ultimate_seismic",
                "serviceability_quasi_permanent",
                "serviceability_frequent",
                "serviceability_characteristic"
            }, null, 0);
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

        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }
}
