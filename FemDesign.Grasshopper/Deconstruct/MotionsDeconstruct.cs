// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class MotionsDeconstruct: GH_Component
    {
       public MotionsDeconstruct(): base("Motions.Deconstruct", "Deconstruct", "Deconstruct a Motions element.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("Motions", "Motions", "Motions.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("x_neg", "x_neg", "x_neg. [kN/m] or [kn/m/m]", GH_ParamAccess.item);
           pManager.AddTextParameter("x_pos", "x_pos", "x_pos. [kN/m] or [kn/m/m]", GH_ParamAccess.item);
           pManager.AddTextParameter("y_neg", "y_neg", "y_neg. [kN/m] or [kn/m/m]", GH_ParamAccess.item);
           pManager.AddTextParameter("y_pos", "y_pos", "y_pos. [kN/m] or [kn/m/m]", GH_ParamAccess.item);
           pManager.AddTextParameter("z_neg", "z_neg", "z_neg. [kN/m] or [kn/m/m]", GH_ParamAccess.item);
           pManager.AddTextParameter("z_pos", "z_pos", "z_pos. [kN/m] or [kn/m/m]", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
           FemDesign.Releases.Motions obj = null;
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
                return FemDesign.Properties.Resources.MotionsDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("06f71deb-3f8f-44d7-9ba7-ce6fe8cfd686"); }
       }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}