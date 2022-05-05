// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class LoadCombinationDeconstruct_OBSOLETE: GH_Component
    {
       public LoadCombinationDeconstruct_OBSOLETE(): base("LoadCombination.Deconstruct", "Deconstruct", "Deconstruct a LoadCombination.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("LoadCombination", "LoadCombination", "LoadCombination.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddTextParameter("Name", "Name", "Name.", GH_ParamAccess.item);
           pManager.AddTextParameter("Type", "Type", "Type." , GH_ParamAccess.item);
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

            // return
            DA.SetData(0, obj.Guid);
            DA.SetData(1, obj.Name);
            DA.SetData(2, obj.Type.ToString());
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
           get { return new Guid("84b65b65-0aff-4ec5-99eb-d6fce2a728e0"); }
       }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

    }
}