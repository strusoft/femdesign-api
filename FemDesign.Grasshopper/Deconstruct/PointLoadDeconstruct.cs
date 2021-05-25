// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class PointLoadDeconstruct: GH_Component
    {
       public PointLoadDeconstruct(): base("PointLoad.Deconstruct", "Deconstruct", "Deconstruct a PointLoad.", "FemDesign", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("PointLoad", "PointLoad", "PointLoad. Use GenericLoadObject.SortLoads to extract PointLoads.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddTextParameter("Type", "Type", "Type.", GH_ParamAccess.item);
           pManager.AddPointParameter("Point", "Point", "Point." , GH_ParamAccess.item);
           pManager.AddVectorParameter("Direction", "Direction", "Direction.", GH_ParamAccess.item);
           pManager.AddNumberParameter("q", "q", "Load intensity.", GH_ParamAccess.item);
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
            if (obj.PointLoad != null)
            {
                DA.SetData(0, obj.PointLoad.Guid);
                DA.SetData(1, obj.PointLoad.LoadType);
                DA.SetData(2, obj.PointLoad.GetRhinoGeometry());
                DA.SetData(3, obj.PointLoad.Direction.ToRhino());
                DA.SetData(4, obj.PointLoad.Load.Value);
                DA.SetData(5, obj.PointLoad.LoadCase);
                DA.SetData(6, obj.PointLoad.Comment);
            }
            else
            {
                throw new System.ArgumentException("Type must be PointLoad. PointLoadDeconstruct failed.");
            }
            
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.LoadsDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("145f6339-bf19-4d29-9e81-9e5e0d137f87"); }
       }
    }
}