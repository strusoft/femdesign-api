// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.GH
{
    public class GenericLoadObjectSortLoads: GH_Component
    {
       public GenericLoadObjectSortLoads(): base("GenericLoadObject.SortLoads", "SortLoads", "Sort a list of GenericLoadObject into lists classified by each respective type of load.", "FemDesign", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("GenericLoadObject", "GenericLoadObject", "GenericLoadObject.", GH_ParamAccess.list);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddGenericParameter("PointLoad", "PointLoad", "PointLoad.", GH_ParamAccess.list);
           pManager.AddGenericParameter("LineLoad", "LineLoad", "LineLoad.", GH_ParamAccess.list);
           pManager.AddGenericParameter("SurfaceLoad", "SurfaceLoad", "SurfaceLoad." , GH_ParamAccess.list);
           pManager.AddGenericParameter("PressureLoad", "PressureLoad", "PressureLoad.", GH_ParamAccess.list);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            List<FemDesign.Loads.GenericLoadObject> objs = new List<FemDesign.Loads.GenericLoadObject>();
            if (!DA.GetDataList(0, objs))
            {
                return;
            }
            if (objs == null)
            {
                return;
            }

            var r0 = new List<FemDesign.Loads.GenericLoadObject>();
            var r1 = new List<FemDesign.Loads.GenericLoadObject>();
            var r2 = new List<FemDesign.Loads.GenericLoadObject>();
            var r3 = new List<FemDesign.Loads.GenericLoadObject>();

            foreach (FemDesign.Loads.GenericLoadObject obj in objs)
            {
                if (obj.pointLoad != null)
                {
                    r0.Add(obj);
                }
                
                else if (obj.lineLoad != null)
                {
                    r1.Add(obj);
                }
                
                else if (obj.surfaceLoad != null)
                {
                    r2.Add(obj);
                }
                
                else if (obj.pressureLoad != null)
                {
                    r3.Add(obj);
                }
                else
                {
                    throw new System.ArgumentException("Type not supported. SortLoads failed.");
                }
            }

            // return
            DA.SetDataList(0, r0);
            DA.SetDataList(1, r1);
            DA.SetDataList(2, r2);
            DA.SetDataList(3, r3);            
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
           get { return new Guid("145f6331-bf19-4d29-9e81-9e5e0d137f87"); }
       }
    }
}