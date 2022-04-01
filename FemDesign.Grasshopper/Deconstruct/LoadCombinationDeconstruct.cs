// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class LoadCombinationDeconstruct: GH_Component
    {
       public LoadCombinationDeconstruct(): base("LoadCombination.Deconstruct", "Deconstruct", "Deconstruct a LoadCombination.", "FEM-Design", "Deconstruct")
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
           pManager.AddTextParameter("Type", "Type", "Type." , GH_ParamAccess.list);
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

            List<object> listGuid = new List<object>();
            listGuid.Add(obj.Guid);

            List<object> listName = new List<object>();
            listName.Add(obj.Name);

            List<object> listObjectType = new List<object>();
            listObjectType.Add(obj.Type.ToString());


            // return
            DA.SetDataList(0, listGuid);
            DA.SetDataList(1, listName);
            DA.SetDataList(2, listObjectType);
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
    }
}