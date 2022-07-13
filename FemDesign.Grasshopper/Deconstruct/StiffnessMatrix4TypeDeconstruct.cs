// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class StiffnessMatrix4TypeDeconstruct: GH_Component
    {
       public StiffnessMatrix4TypeDeconstruct(): base("StiffnessMatrix4Type.Deconstruct", "Deconstruct", "Deconstruct a StiffnessMatrix4Type element.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("StiffnessMatrix4Type", "StiffnessMatrix4Type", "StiffnessMatrix4Type.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddNumberParameter("XX", "XX", "XX. [kN/m]", GH_ParamAccess.item);
           pManager.AddNumberParameter("XY", "XY", "XY. [kN/m]", GH_ParamAccess.item);
           pManager.AddNumberParameter("YY", "YY", "YY. [kN/m]", GH_ParamAccess.item);
           pManager.AddNumberParameter("GXY", "GXY", "GXY. [kN/m]", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.ModellingTools.StiffnessMatrix4Type obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }

            // return
            DA.SetData(0, obj.XX);
            DA.SetData(1, obj.XY);
            DA.SetData(2, obj.YY);
            DA.SetData(3, obj.GXY);
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.StiffnessMatrix4TypeDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("d89fb59c-93db-4492-83c2-6fbcab20b1eb"); }
       }
        public override GH_Exposure Exposure => GH_Exposure.senary;

    }
}