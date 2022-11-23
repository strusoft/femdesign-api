// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using FemDesign.Supports;


namespace FemDesign.Grasshopper
{
    public class SortSupportsOBSOLETE : GH_Component
    {
        public SortSupportsOBSOLETE() : base("Supports.SortSupports", "SortSupports", "Sort a list of Support objects into lists classified by each respective type of support.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Supports", "Supports", "Supports.", GH_ParamAccess.list);
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
            List<GenericClasses.ISupportElement> objs = new List<GenericClasses.ISupportElement>();
            if (!DA.GetDataList(0, objs))
            {
                return;
            }
            if (objs == null)
            {
                return;
            }

            var pointSupports = new List<Supports.PointSupport>();
            var lineSupports = new List<LineSupport>();
            var surfaceSupports = new List<SurfaceSupport>();

            foreach (GenericClasses.ISupportElement obj in objs)
            {
                if (obj.GetType() == typeof(Supports.PointSupport))
                {
                    pointSupports.Add((Supports.PointSupport)obj);
                }

                else if (obj.GetType() == typeof(LineSupport))
                {
                    lineSupports.Add((LineSupport)obj);
                }
                else if (obj.GetType() == typeof(SurfaceSupport))
                {
                    surfaceSupports.Add((SurfaceSupport)obj);
                }
                else
                {
                    throw new ArgumentException("Type not supported. SortSupports failed.");
                }
            }

            DA.SetDataList("PointSupport", pointSupports);
            DA.SetDataList("LineSupport", lineSupports);
            DA.SetDataList("SurfaceSupport", surfaceSupports);
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

        public override GH_Exposure Exposure => GH_Exposure.hidden;

    }
}