// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class SurfaceReinforcementDeconstruct: GH_Component
    {
       public SurfaceReinforcementDeconstruct(): base("SurfaceReinforcement.Deconstruct", "Deconstruct", "Deconstruct a SurfaceReinforcement element.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("SurfaceReinforcement", "SurfaceReinforcement", "SurfaceReinforcement.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddBrepParameter("Surface", "Surface", "Surface", GH_ParamAccess.item);
           pManager.AddGenericParameter("Straight", "Straight", "Straight.", GH_ParamAccess.item);
           pManager.AddGenericParameter("Wire", "Wire", "Wire.", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.Reinforcement.SurfaceReinforcement obj = null;
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
            DA.SetData(1, obj.Region.ToRhinoBrep());
            DA.SetData(2, obj.Straight);
            DA.SetData(3, obj.Wire);
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.SurfaceReinforcementDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("185f6331-bf19-4d89-9e81-9e5e0d137f87"); }
       }
        public override GH_Exposure Exposure => GH_Exposure.quinary;

    }
}