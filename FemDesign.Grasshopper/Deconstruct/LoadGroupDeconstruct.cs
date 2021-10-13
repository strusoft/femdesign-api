// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class LoadGroupDeconstruct: GH_Component
    {
       public LoadGroupDeconstruct(): base("LoadGroup.Deconstruct", "Deconstruct", "Deconstruct a LoadGroup.", "FemDesign", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("LoadGroup", "LoadGroup", "LoadGroup.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Name", "Name", "Name.", GH_ParamAccess.item);
           pManager.AddTextParameter("Type", "Type", "Type.", GH_ParamAccess.item);
           pManager.AddTextParameter("LoadCaseRelationship", "LoadCaseRelationship", "LoadCaseRelationship.", GH_ParamAccess.item);
           pManager.AddGenericParameter("LoadCases", "LoadCases", "LoadCases", GH_ParamAccess.list);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.Loads.LoadGroup obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }
            
            // return
            DA.SetData(0, obj.Name);
            DA.SetData(1, obj.Type);
            DA.SetData(2, obj.LoadCaseRelation);
            DA.SetDataList(3, obj.LoadCases);

        }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.LoadCaseDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("e3bdb4d8-6b64-47bc-a3ce-37aa75af4673"); }
       }
    }
}