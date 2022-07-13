// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class RotationsDeconstruct: GH_Component
    {
       public RotationsDeconstruct(): base("Rotations.Deconstruct", "Deconstruct", "Deconstruct a Rotations element.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("Rotations", "Rotations", "Rotations.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("x_neg", "x_neg", "x_neg. [kNm/rad] or [kNm/m/rad]", GH_ParamAccess.item);
           pManager.AddTextParameter("x_pos", "x_pos", "x_pos. [kNm/rad] or [kNm/m/rad]", GH_ParamAccess.item);
           pManager.AddTextParameter("y_neg", "y_neg", "y_neg. [kNm/rad] or [kNm/m/rad]", GH_ParamAccess.item);
           pManager.AddTextParameter("y_pos", "y_pos", "y_pos. [kNm/rad] or [kNm/m/rad]", GH_ParamAccess.item);
           pManager.AddTextParameter("z_neg", "z_neg", "z_neg. [kNm/rad] or [kNm/m/rad]", GH_ParamAccess.item);
           pManager.AddTextParameter("z_pos", "z_pos", "z_pos. [kNm/rad] or [kNm/m/rad]", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
           FemDesign.Releases.Rotations obj = null;
           if (!DA.GetData(0, ref obj))
           {
               return;
           }
           if (obj == null)
           {
               return;
           }

           // 
           DA.SetData(0, obj.XNeg);
           DA.SetData(1, obj.XPos);
           DA.SetData(2, obj.YNeg);
           DA.SetData(3, obj.YPos);
           DA.SetData(4, obj.ZNeg);
           DA.SetData(5, obj.ZPos);

       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.RotationsDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("89ecd988-3ae7-4b51-92fe-1a0b4cf2dbcf"); }
       }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}