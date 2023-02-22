using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

using Struxml = StruSoft.Interop.StruXml.Data;
using FemDesign.Grasshopper.Extension.ComponentExtension;

namespace FemDesign.Grasshopper
{
    public class DimensionLinear : GH_Component
    {
        public static readonly List<string> ArrowTypeValueList = Enum.GetNames(typeof(Struxml.Arrowtype_type)).ToList();
        public static string ArrowTypeValueListDescription
        {
            get
            {
                var str = "";
                foreach (var a in ArrowTypeValueList)
                {
                    str += "\n" + a;
                }
                return str;
            }
        }
        public DimensionLinear() : base("LinearDimension", "LnDim", "Create a linear dimension.", CategoryName.Name(), "ModellingTools")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Point|Plane", "Point|Plane", "Position of dimension line and orientation of dimension. Distances will be measured along the plane x-axis.", GH_ParamAccess.item);
            pManager.AddPointParameter("ReferencePoints", "RefPoints", "Points on dimension line to measure between along plane X-axis.", GH_ParamAccess.list);
            pManager.AddNumberParameter("FontSize", "FontSize", "Font size of text. [m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("ArrowType", "ArrowType", $"Dimension line arrow type. Connect 'ValueList' to get the options: {ArrowTypeValueListDescription}", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LinearDimension", "LnDim", "Linear dimension.", GH_ParamAccess.item);
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 3, ArrowTypeValueList, null, 0);
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

            double size = 0;
            if (DA.GetData(2, ref size))
            {
                dim.Font.Size = size;    
            }

            string arrowType = null;
            if (DA.GetData(3, ref arrowType))
            {
                if (Enum.TryParse(arrowType, out Struxml.Arrowtype_type arrowTypeEnum))
                {
                    dim.Arrow.Type = arrowTypeEnum;
                }
                else
                {
                    throw new System.ArgumentException($"Invalid horisontal alignment value: {arrowType}, must be one of the following values: {ArrowTypeValueListDescription}");
                }
            }

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
            pManager.AddNumberParameter("FontSize", "FontSize", "Font size of text. [m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("ArrowType", "ArrowType", $"Dimension line arrow type. Connect 'ValueList' to get the options: {DimensionLinear.ArrowTypeValueListDescription}", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LinearDimension", "LnDim", "Linear dimension.", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "Plane", "Position and orientation of text. [m]", GH_ParamAccess.item);
            pManager.AddPointParameter("ReferencePoints", "RefPoints", "Points on dimension line to measure between.", GH_ParamAccess.list);
            pManager.AddNumberParameter("FontSize", "FontSize", "Font size of text. [m]", GH_ParamAccess.item);
            pManager.AddTextParameter("ArrowType", "ArrowType", $"Dimension line arrow type.", GH_ParamAccess.item);
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 4, DimensionLinear.ArrowTypeValueList, null, 0);
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

            double size = dim.Font.Size;
            if (DA.GetData(3, ref size))
            {
                dim.Font.Size = size;    
            }

            string arrowType = dim.Arrow.Type.ToString();
            if (DA.GetData(4, ref arrowType))
            {
                if (Enum.TryParse(arrowType, out Struxml.Arrowtype_type arrowTypeEnum))
                {
                    dim.Arrow.Type = arrowTypeEnum;
                }
                else
                {
                    throw new System.ArgumentException($"Invalid horisontal alignment value: {arrowType}, must be one of the following values: {DimensionLinear.ArrowTypeValueListDescription}");
                }
            }

            DA.SetData(0, dim);
            DA.SetData(1, plane);
            DA.SetDataList(2, refPoints);
            DA.SetData(3, size);
            DA.SetData(4, arrowType);
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