// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Eto.Forms;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class DimensionLinear : GH_Component
    {
        public DimensionLinear() : base("LinearDimension", "LnDim", "Create a linear dimension.", CategoryName.Name(), "ModellingTools")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Point|Plane", "Point|Plane", "Position of dimension line and orientation of dimension. Distances will be measured along the plane x-axis.", GH_ParamAccess.item);
            pManager.AddPointParameter("ReferencePoints", "RefPoints", "Points on dimension line to measure between along plane X-axis.", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LinearDimension", "LnDim", "Linear dimension.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var plane = Plane.WorldXY;
            if (!DA.GetData(0, ref plane))
            {
                return;
            }

            List<Point3d> refPoints = new List<Point3d>();
            if (!DA.GetDataList(1, refPoints))
            {
                return;
            }

            var dim = new Drawing.DimensionLinear(refPoints.Select(x => x.FromRhino()).ToList(), plane.ToPlane());

            DA.SetData(0, dim);
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
            get { return new Guid("6888FFFF-4B85-4582-8BE6-04F83D727C85"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary;
    }
    public class DimensionLinearDeconstruct : GH_Component
    {
        public DimensionLinearDeconstruct() : base("LinearDimensionDeconstruct", "LnDimDecon", "Deconstruct or modify a linear dimension.", CategoryName.Name(), "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LinearDimension", "LnDim", "Linear dimension.", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Point|Plane", "Point|Plane", "Position and orientation of text. [m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddPointParameter("ReferencePoints", "RefPoints", "Points on dimension line to measure between.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LinearDimension", "LnDim", "Linear dimension.", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "Plane", "Position and orientation of text. [m]", GH_ParamAccess.item);
            pManager.AddPointParameter("ReferencePoints", "RefPoints", "Points on dimension line to measure between.", GH_ParamAccess.list);
            pManager.AddNumberParameter("Distances", "Dists", "Distances between reference point along plane X-axis.", GH_ParamAccess.list);

        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Drawing.DimensionLinear dim = null;
            if (!DA.GetData(0, ref dim))
            {
                return;
            }

            var plane = dim.Plane.ToRhinoPlane();
            if (DA.GetData(1, ref plane))
            {
                dim.Plane = plane.ToPlane();
            }

            List<Point3d> refPoints = dim.ReferencePoints.Select(x => x.ToRhino()).ToList();
            if (!DA.GetDataList(2, refPoints))
            {
                dim.ReferencePoints = refPoints.Select(x => x.FromRhino()).ToList();
            }

            DA.SetData(0, dim);
            DA.SetData(1, plane);
            DA.SetDataList(2, refPoints);
            DA.SetDataList(3, dim.Distances);
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
            get { return new Guid("7D89524B-5FAC-4C0E-AD73-65AA08FCC2E6"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
    }
}