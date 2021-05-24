// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class GenericSupportObjectSortSupports: GH_Component
    {
       public GenericSupportObjectSortSupports(): base("GenericSupportObject.SortSupports", "SortSupports", "Sort a list of GenericSupportObject into lists classified by each respective type of support.", "FemDesign", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("GenericSupportObject", "GenericSupportObject", "GenericSupportObject.", GH_ParamAccess.list);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddGenericParameter("PointSupport", "PointSupport", "PointSupport.", GH_ParamAccess.list);
           pManager.AddGenericParameter("LineSupport", "LineSupport", "LineSupport.", GH_ParamAccess.list);
           pManager.AddGenericParameter("SurfaceSupport", "SurfaceSupport", "SurfaceSupport.", GH_ParamAccess.list);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            List<FemDesign.Supports.GenericSupportObject> objs = new List<FemDesign.Supports.GenericSupportObject>();
            if (!DA.GetDataList(0, objs))
            {
                return;
            }
            if (objs == null)
            {
                return;
            }

            var r0 = new List<FemDesign.Supports.GenericSupportObject>();
            var r1 = new List<FemDesign.Supports.GenericSupportObject>();
            var r2 = new List<FemDesign.Supports.GenericSupportObject>();

            foreach (FemDesign.Supports.GenericSupportObject obj in objs)
            {
                if (obj.PointSupport != null)
                {
                    r0.Add(obj);
                }
                
                else if (obj.LineSupport != null)
                {
                    r1.Add(obj);
                }
                else if (obj.SurfaceSupport != null)
                {
                    r2.Add(obj);
                }
                else
                {
                    throw new System.ArgumentException("Type not supported. SortSupports failed.");
                }
            }

            // return
            DA.SetDataList(0, r0);
            DA.SetDataList(1, r1);
            DA.SetDataList(2, r2);          
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.SupportsDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("0e3450c6-1841-4628-aa51-96c7ec3ef5f6"); }
       }
    }
}