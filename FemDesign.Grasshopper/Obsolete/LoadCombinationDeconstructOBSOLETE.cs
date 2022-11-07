// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class LoadCombinationDeconstructOBSOLETE : GH_Component
    {
        public LoadCombinationDeconstructOBSOLETE() : base("LoadCombination.Deconstruct", "Deconstruct", "Deconstruct a LoadCombination.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCombination", "LoadCombination", "LoadCombination.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.list);
            pManager.AddTextParameter("Name", "Name", "Name.", GH_ParamAccess.list);
            pManager.AddTextParameter("Type", "Type", "Type.", GH_ParamAccess.list);
            pManager.AddTextParameter("LoadCases", "LoadCases", "LoadCases.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Gammas", "Gammas", "Gammas.", GH_ParamAccess.list);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Loads.LoadCombination obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }

            // The following code is to convert 'item' to 'list object'
            // It is required to construct the Load Combination without graftening the data

            var guidList = new List<object> { obj.Guid };

            var nameList = new List<object> { obj.Name };

            var objectTypeList = new List<object> { obj.Type.ToString() };


            // return
            DA.SetDataList(0, guidList);
            DA.SetDataList(1, nameList);
            DA.SetDataList(2, objectTypeList);
            DA.SetDataList(3, obj.GetLoadCaseGuidsAsString());
            DA.SetDataList(4, obj.GetGammas());

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LoadCombinationDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("c1cf57c6-be8c-42a8-a489-4145c2d88896"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

    }
}
