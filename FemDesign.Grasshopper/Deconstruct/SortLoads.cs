// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class SortLoads : GH_Component
    {
        public SortLoads() : base("Loads.SortLoads", "SortLoads", "Sort a list of Loads (List<ILoadElement>) into lists classified by each respective type of load.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Loads", "Loads", "List of Loads (List<ILoadElement>).", GH_ParamAccess.list);
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
            List<FemDesign.GenericClasses.ILoadElement> objs = new List<FemDesign.GenericClasses.ILoadElement>();
            if (!DA.GetDataList(0, objs))
            {
                return;
            }
            if (objs == null)
            {
                return;
            }

            var r0 = new List<FemDesign.Loads.PointLoad>();
            var r1 = new List<FemDesign.Loads.LineLoad>();
            var r2 = new List<FemDesign.Loads.LineStressLoad>();
            var r3 = new List<FemDesign.Loads.LineTemperatureLoad>();
            var r4 = new List<FemDesign.Loads.SurfaceLoad>();
            var r5 = new List<FemDesign.Loads.SurfaceTemperatureLoad>();
            var r6 = new List<FemDesign.Loads.PressureLoad>();
            var r7 = new List<FemDesign.Loads.Footfall>();

            foreach (FemDesign.GenericClasses.ILoadElement load in objs)
            {
                if (load.GetType() == typeof(Loads.PointLoad))
                {
                    r0.Add((Loads.PointLoad)load);
                }

                else if (load.GetType() == typeof(Loads.LineLoad))
                {
                    r1.Add((Loads.LineLoad)load);
                }

                else if (load.GetType() == typeof(Loads.LineStressLoad))
                {
                    r2.Add((Loads.LineStressLoad)load);
                }

                else if (load.GetType() == typeof(Loads.LineTemperatureLoad))
                {
                    r3.Add((Loads.LineTemperatureLoad)load);
                }

                else if (load.GetType() == typeof(Loads.SurfaceLoad))
                {
                    r4.Add((Loads.SurfaceLoad)load);
                }

                else if (load.GetType() == typeof(Loads.SurfaceTemperatureLoad))
                {
                    r5.Add((Loads.SurfaceTemperatureLoad)load);
                }

                else if (load.GetType() == typeof(Loads.PressureLoad))
                {
                    r6.Add((Loads.PressureLoad)load);
                 }
                else if (load.GetType() == typeof(Loads.Footfall))
                {
                    r7.Add((Loads.Footfall)load);
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

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}