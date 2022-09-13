// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class MassConversionTableConstruct : GH_Component
    {
        public MassConversionTableConstruct(): base("MassConversionTable.Construct", "Construct", "Define a MassConversionTable from a LoadCase or a list of LoadCases. The MassConversionTable is considered as a load type and should be added to the model as a load.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase to include in MassConversionTable. Single LoadCase or list of LoadCases.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Factor", "Factor", "Factor for mass conversion of each respective LoadCase. Single value or list of values. [-]", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("MassConversionTable", "MassConversionTable.", "MassConversionTable. The MassConversionTable is considered as a load type and should be added to the model as a load.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            List<FemDesign.Loads.LoadCase> loadCases = new List<FemDesign.Loads.LoadCase>();
            if (!DA.GetDataList(0, loadCases))
            { 
                return;
            }
            
            List<double> factors = new List<double>();
            if (!DA.GetDataList(1, factors))
            {
                return;
            }

            if (loadCases == null || factors == null) { return; }
            
            //
            FemDesign.Loads.MassConversionTable obj = new FemDesign.Loads.MassConversionTable(factors, loadCases);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.MassConversionTableDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("ab358779-285c-40da-8f1e-be8d271b7702"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;

    }
}