// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class LoadCaseGetLoadCaseByName: GH_Component
    {
        public LoadCaseGetLoadCaseByName(): base("LoadCase.GetLoadCaseFromListByName", "GetLoadCaseByName", "Returns a LoadCase from a list of LoadCases by name. The first LoadCase with a matching name will be returned.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCases", "LoadCases", "List of LoadCase.", GH_ParamAccess.list);
            pManager.AddTextParameter("Name", "Name", "Name of LoadCase.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            List<FemDesign.Loads.LoadCase> loadCases = new List<FemDesign.Loads.LoadCase>();
            string name = null;
            if (!DA.GetDataList(0, loadCases)) { return; }
            if (!DA.GetData(1, ref name)) { return; }
            if (loadCases == null || name == null) { return; }

            //
            FemDesign.Loads.LoadCase obj = FemDesign.Loads.LoadCase.LoadCaseFromListByName(loadCases, name);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {

                return FemDesign.Properties.Resources.LoadCaseGetLoadCaseByName;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("113874e1-2a2f-4695-8326-475abaaec549"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.senary;

    }
}