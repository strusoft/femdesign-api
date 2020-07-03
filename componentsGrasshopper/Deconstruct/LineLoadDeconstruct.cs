// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.GH
{
    public class LineLoadDeconstruct: GH_Component
    {
       public LineLoadDeconstruct(): base("LineLoad.Deconstruct", "Deconstruct", "Deconstruct a LineLoad.", "FemDesign", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("LineLoad", "LineLoad", "LineLoad. Use GenericLoadObject.SortLoads to extract LineLoads.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddTextParameter("Type", "Type", "Type.", GH_ParamAccess.item);
           pManager.AddCurveParameter("Curve", "Curve", "Curve." , GH_ParamAccess.item);
           pManager.AddVectorParameter("Direction", "Direction", "Direction.", GH_ParamAccess.item);
           pManager.AddNumberParameter("q1", "q1", "Load intensity.", GH_ParamAccess.item);
           pManager.AddNumberParameter("q2", "q2", "Load intensity.", GH_ParamAccess.item);
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
            if (obj.lineLoad != null)
            {
                DA.SetData(0, obj.lineLoad.guid);
                DA.SetData(1, obj.lineLoad.loadType);
                DA.SetData(2, obj.lineLoad.GetRhinoGeometry());
                DA.SetData(3, obj.lineLoad.direction.ToRhino());
                DA.SetData(4, obj.lineLoad.load[0].val);
                DA.SetData(5, obj.lineLoad.load[1].val);
                DA.SetData(6, obj.lineLoad.loadCase);
                DA.SetData(7, obj.lineLoad.comment);
            }
            else
            {
                throw new System.ArgumentException("Type must be LineLoad. LineLoadDeconstruct failed.");
            }
            
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.LineLoadDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("898c511a-bc4b-40e0-81e0-16437416f8ad"); }
       }
    }
}