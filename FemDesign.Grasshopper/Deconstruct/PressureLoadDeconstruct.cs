// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class PressureLoadDeconstruct: GH_Component
    {
       public PressureLoadDeconstruct(): base("PressureLoad.Deconstruct", "Deconstruct", "Deconstruct a PressureLoad.", "FemDesign", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("PressureLoad", "PressureLoad", "PressureLoad.  Use GenericLoadObject.SortLoads to extract PointLoads", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddTextParameter("Type", "Type", "Type.", GH_ParamAccess.item);
           pManager.AddSurfaceParameter("Surface", "Surface", "Surface." , GH_ParamAccess.item);
           pManager.AddVectorParameter("Direction", "Direction", "Direction.", GH_ParamAccess.item);
           pManager.AddNumberParameter("z0", "z0", "Surface level of soil/water (on the global Z axis).", GH_ParamAccess.item);
           pManager.AddNumberParameter("q0", "q0", "Load intensity at the surface level.", GH_ParamAccess.item);
           pManager.AddNumberParameter("q1", "q1", "Increment of load intensity per meter (along the global Z axis).", GH_ParamAccess.item);
           pManager.AddTextParameter("LoadCaseGuid", "LoadCaseGuid", "LoadCase guid reference.", GH_ParamAccess.item);
           pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.Loads.GenericLoadObject obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }

            // return
            DA.SetData(0, obj.GetType());
            if (obj.PressureLoad != null)
            {
                DA.SetData(0, obj.PressureLoad.Guid);
                DA.SetData(1, obj.PressureLoad.LoadType);
                DA.SetData(2, obj.PressureLoad.GetRhinoGeometry());
                DA.SetData(3, obj.PressureLoad.Direction.ToRhino());
                DA.SetData(4, obj.PressureLoad.Z0);
                DA.SetData(5, obj.PressureLoad.Q0);
                DA.SetData(6, obj.PressureLoad.Qh);
                DA.SetData(7, obj.PressureLoad.LoadCase);
                DA.SetData(8, obj.PressureLoad.Comment);
            }
            else
            {
                throw new System.ArgumentException("Type must be PressureLoad. PressureLoadDeconstruct failed.");
            }
            
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.PressureLoadDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("8bba0bf0-7587-4dd8-97dc-ff18a91c52b5"); }
       }
    }
}