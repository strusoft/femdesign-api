// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.GH
{
    public class SupportDeconstruct: GH_Component
    {
       public SupportDeconstruct(): base("Support.Deconstruct", "Deconstruct", "Deconstruct a PointSupport or LineSupport element.", "FemDesign", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("Support", "Support", "PointSupport or LineSupport.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {   
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddTextParameter("AnalyticalID", "AnalyticalID", "Analytical ID.", GH_ParamAccess.item);
           pManager.AddGenericParameter("Geometry", "Geometry", "Geometry.", GH_ParamAccess.item);
           pManager.AddGenericParameter("MovingLocal", "MovingLocal", "MovingLocal.", GH_ParamAccess.item);
           pManager.AddVectorParameter("LocalX", "LocalX", "LocalX.", GH_ParamAccess.item);
           pManager.AddVectorParameter("LocalY", "LocalY", "LocalY.", GH_ParamAccess.item);
           pManager.AddGenericParameter("Motions", "Motions", "Motions.", GH_ParamAccess.item);
           pManager.AddGenericParameter("Rotations", "Rotations", "Rotations.", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.Supports.GenericSupportObject obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }
            
            // return
            if (obj.PointSupport != null)
            {   
                DA.SetData(0, obj.PointSupport.Guid);
                DA.SetData(1, obj.PointSupport.Name);
                DA.SetData(2, obj.PointSupport.GetRhinoGeometry());
                DA.SetData(3, "PointSupport has no moving local property.");
                DA.SetData(4, obj.PointSupport.Group.LocalX.ToRhino());
                DA.SetData(5, obj.PointSupport.Group.LocalY.ToRhino());

                // catch pre-defined rigidity
                if (obj.PointSupport.Group.Rigidity != null)
                { 
                    DA.SetData(6, obj.PointSupport.Group.Rigidity.Motions);
                    DA.SetData(7, obj.PointSupport.Group.Rigidity.Rotations);
                }
                else
                {
                    DA.SetData(6, obj.PointSupport.Group.PredefRigidity.Rigidity.Motions);
                    DA.SetData(7, obj.PointSupport.Group.PredefRigidity.Rigidity.Rotations);
                }
            }
            else if (obj.LineSupport != null)
            {   
                DA.SetData(0, obj.LineSupport.Guid);
                DA.SetData(1, obj.LineSupport.Name);
                DA.SetData(2, obj.LineSupport.GetRhinoGeometry());
                DA.SetData(3, obj.LineSupport.MovingLocal);
                DA.SetData(4, obj.LineSupport.Group.LocalX.ToRhino());
                DA.SetData(5, obj.LineSupport.Group.LocalY.ToRhino());

                // catch pre-defined rigidity
                if (obj.LineSupport.Group.Rigidity != null)
                { 
                    DA.SetData(6, obj.LineSupport.Group.Rigidity.Motions);
                    DA.SetData(7, obj.LineSupport.Group.Rigidity.Rotations);
                }
                else
                {
                    DA.SetData(6, obj.LineSupport.Group.PredefRigidity.Rigidity.Motions);
                    DA.SetData(7, obj.LineSupport.Group.PredefRigidity.Rigidity.Rotations);
                }
            }
            else if (obj.SurfaceSupport != null)
            {
                DA.SetData(0, obj.SurfaceSupport.Guid);
                DA.SetData(1, obj.SurfaceSupport.Identifier);
                DA.SetData(2, obj.SurfaceSupport.Region.ToRhinoBrep());
                DA.SetData(3, "SurfaceSupport has no moving local property.");
                DA.SetData(4, obj.SurfaceSupport.CoordinateSystem.LocalX.ToRhino());
                DA.SetData(5, obj.SurfaceSupport.CoordinateSystem.LocalY.ToRhino());

                // catch pre-defined rigidity
                if (obj.SurfaceSupport.Rigidity != null)
                { 
                    DA.SetData(6, obj.SurfaceSupport.Rigidity.Motions);
                }
                else
                {
                    DA.SetData(6, obj.SurfaceSupport.PredefRigidity.Rigidity.Motions);
                }

                DA.SetData(7, "SurfaceSupport has no rotations property");
            }
            else
            {
                throw new System.ArgumentException("Type is not supported. LoadsDeconstruct failed.");
            }
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return null;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("145f6331-bf19-4d29-1e81-9e5e0d137f87"); }
       }
    }
}