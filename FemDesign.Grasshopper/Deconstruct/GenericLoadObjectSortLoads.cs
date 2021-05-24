// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class GenericLoadObjectSortLoads : GH_Component
    {
        public GenericLoadObjectSortLoads() : base("GenericLoadObject.SortLoads", "SortLoads", "Sort a list of GenericLoadObject into lists classified by each respective type of load.", "FemDesign", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("GenericLoadObject", "GenericLoadObject", "GenericLoadObject.", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PointLoad", "PtLoad", "PointLoad.", GH_ParamAccess.list);
            pManager.AddGenericParameter("LineLoad", "LnLoad", "LineLoad.", GH_ParamAccess.list);
            pManager.AddGenericParameter("LineStressLoad", "LnStressLoad", "LineStressLoad.", GH_ParamAccess.list);
            pManager.AddGenericParameter("LineTemperatureLoad", "LnTmpLoad", "LineTemperatureLoad.", GH_ParamAccess.list);
            pManager.AddGenericParameter("SurfaceLoad", "SrfLoad", "SurfaceLoad.", GH_ParamAccess.list);
            pManager.AddGenericParameter("SurfaceTemperatureLoad", "SrfTmpLoad", "SurfaceTemperatureLoad.", GH_ParamAccess.list);
            pManager.AddGenericParameter("PressureLoad", "PressureLoad", "PressureLoad.", GH_ParamAccess.list);
            pManager.AddGenericParameter("FootfallLoads", "FootfallLoads", "Footfall.", GH_ParamAccess.list);
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
            var r4 = new List<FemDesign.Loads.GenericLoadObject>();
            var r5 = new List<FemDesign.Loads.GenericLoadObject>();
            var r6 = new List<FemDesign.Loads.GenericLoadObject>();
            var r7 = new List<FemDesign.Loads.GenericLoadObject>();

            foreach (FemDesign.Loads.GenericLoadObject obj in objs)
            {
                if (obj.PointLoad != null)
                {
                    r0.Add(obj);
                }

                else if (obj.LineLoad != null)
                {
                    r1.Add(obj);
                }

                else if (obj.LineStressLoad != null)
                {
                    r2.Add(obj);
                }

                else if (obj.LineTemperatureLoad != null)
                {
                    r3.Add(obj);
                }

                else if (obj.SurfaceLoad != null)
                {
                    r4.Add(obj);
                }

                else if (obj.SurfaceTemperatureLoad != null)
                {
                    r5.Add(obj);
                }

                else if (obj.PressureLoad != null)
                {
                    r6.Add(obj);
                }
                else if (obj.Footfall != null)
                {
                    r7.Add(obj);
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
            DA.SetDataList(4, r4);
            DA.SetDataList(5, r5);
            DA.SetDataList(6, r6);
            DA.SetDataList(7, r7);
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