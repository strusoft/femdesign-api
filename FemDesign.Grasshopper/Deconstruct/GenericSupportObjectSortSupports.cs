// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using FemDesign.Supports;


namespace FemDesign.Grasshopper
{
    public class SortSupports : GH_Component
    {
        public SortSupports() : base("Supports.SortSupports", "SortSupports", "Sort a list of Support objects into lists classified by each respective type of support.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Supports", "Supports", "Supports.", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PointSupportDirected", "PointSupportDirected", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("PointSupportGroup", "PointSupportGroup", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("LineSupportDirected", "LineSupportDirected", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("LineSupportGroup", "LineSupportGroup", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("SurfaceSupportGroup", "SurfaceSupportGroup", "SurfaceSupport.", GH_ParamAccess.list);
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

            var pointSupportsGroup = new List<Supports.PointSupport>();
            var pointSupportsDirected = new List<Supports.PointSupport>();

            var lineSupportsGroup = new List<LineSupport>();
            var lineSupportsDirected = new List<LineSupport>();

            var surfaceSupportsGroup = new List<SurfaceSupport>();

            foreach (GenericClasses.ISupportElement obj in objs)
            {
                if (obj.GetType() == typeof(Supports.PointSupport))
                {
                    var pointSupport = (Supports.PointSupport)obj;
                    if(pointSupport.IsDirected)
                    {
                        pointSupportsDirected.Add(pointSupport);
                    }
                    else
                        pointSupportsGroup.Add(pointSupport);
                }
                else if (obj.GetType() == typeof(Supports.LineSupport))
                {
                    var lineSupport = (Supports.LineSupport)obj;
                    if (lineSupport.IsDirected)
                    {
                        lineSupportsDirected.Add(lineSupport);
                    }
                    else
                        lineSupportsGroup.Add(lineSupport);
                }
                else if (obj.GetType() == typeof(SurfaceSupport))
                {
                    surfaceSupportsGroup.Add((SurfaceSupport)obj);
                }
                else
                {
                    throw new ArgumentException("Type not supported. SortSupports failed.");
                }
            }

            DA.SetDataList("PointSupportDirected", pointSupportsDirected);
            DA.SetDataList("PointSupportGroup", pointSupportsGroup);
            DA.SetDataList("LineSupportDirected", lineSupportsDirected);
            DA.SetDataList("LineSupportGroup", lineSupportsGroup);
            DA.SetDataList("SurfaceSupportGroup", surfaceSupportsGroup);
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
            get { return new Guid("{3A45D870-D02C-4D92-94A4-19D35476939B}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}