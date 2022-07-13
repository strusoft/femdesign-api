// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class LoadCaseDeconstruct: GH_Component
    {
       public LoadCaseDeconstruct(): base("LoadCase.Deconstruct", "Deconstruct", "Deconstruct a LoadCase.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddTextParameter("Name", "Name", "Name.", GH_ParamAccess.item);
           pManager.AddTextParameter("Type", "Type", "Type.", GH_ParamAccess.item);
           pManager.AddTextParameter("DurationClass", "DurationClass", "DurationClass.", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.Loads.LoadCase obj = null;
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
            DA.SetData(3, obj.DurationClass.ToString());
            
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
           get { return new Guid("e3bda4d8-6b64-47bc-a3ce-37aa74ac4673"); }
       }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}