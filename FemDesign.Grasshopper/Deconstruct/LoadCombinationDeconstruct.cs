// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class LoadCombinationDeconstruct : GH_Component
    {
        public LoadCombinationDeconstruct() : base("LoadCombination.Deconstruct", "Deconstruct", "Deconstruct a LoadCombination.", "FEM-Design", "Deconstruct")
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
            pManager.AddGenericParameter("LoadCases", "LoadCases", "LoadCases. Note that load cases may also include a single construction stage and any single of the special load cases such as seisemic, PTC or Neg. shaft friction load case.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Gammas", "Gammas", "Gammas.", GH_ParamAccess.list);
            pManager.AddTextParameter("LoadCaseTypes", "LoadCaseTypes", "Load case types. One of 'Load case', 'stage' or 'Special load case'.", GH_ParamAccess.list);
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

            var pairs = obj.GetCaseAndGammas();

            // TODO: Remove when Moving load load cases is implemented
            pairs = pairs.Where(p => p.CaseType != "Moving load case").ToList();

            DA.SetDataList(0, guidList);
            DA.SetDataList(1, nameList);
            DA.SetDataList(2, objectTypeList);
            DA.SetDataList(3, pairs.Select(p => p.Case));
            DA.SetDataList(4, pairs.Select(p => p.Gamma));
            DA.SetDataList(5, pairs.Select(p => p.CaseType));
        }
        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.LoadCombinationDeconstruct;
        public override Guid ComponentGuid => new Guid("21050992-3259-4a57-a7a6-77b7566e0603");
        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}