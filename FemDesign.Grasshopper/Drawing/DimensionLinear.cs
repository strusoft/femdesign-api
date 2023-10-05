using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

using Struxml = StruSoft.Interop.StruXml.Data;
using FemDesign.Grasshopper.Extension.ComponentExtension;

using Grasshopper.Kernel.Special;

namespace FemDesign.Grasshopper
{
    public class DimensionLinear : FEM_Design_API_Component
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
        public static readonly List<string> LengthUnitValueList = Enum.GetNames(typeof(Struxml.Lengthunit_type)).ToList();
        public static string LengthUnitValueListDescription
        {
            get
            {
                var str = "";
                foreach (var a in LengthUnitValueList)
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
            pManager.AddTextParameter("ArrowType", "ArrowType", $"Dimension line arrow type. Connect 'ValueList' to get the options: {ArrowTypeValueListDescription}", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("Decimals", "Decimals", "Number of decimals of measurements.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("LengthUnit", "LengthUnit", $"Length unit of measurements. Connect 'ValueList' to get the options: {LengthUnitValueListDescription}", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("ShowUnit", "ShowUnit", "Show length unit on measurement.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LinearDimension", "LnDim", "Linear dimension.", GH_ParamAccess.item);
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 3, ArrowTypeValueList, null, GH_ValueListMode.DropDown);
            ValueListUtils.updateValueLists(this, 5, LengthUnitValueList, null, GH_ValueListMode.DropDown);
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

            int decimals = 0;
            if (DA.GetData(4, ref decimals))
            {
                dim.Decimals = decimals;    
            }

            string lengthUnit = null;
            if (DA.GetData(5, ref lengthUnit))
            {
                if (Enum.TryParse(lengthUnit, out Struxml.Lengthunit_type lengthUnitEnum))
                {
                    dim.LengthUnit = lengthUnitEnum;
                }
                else
                {
                    throw new System.ArgumentException($"Invalid horisontal alignment value: {lengthUnit}, must be one of the following values: {LengthUnitValueListDescription}");
                }
            }

            bool showUnit = false;
            if (DA.GetData(6, ref showUnit))
            {
                dim.ShowUnit = showUnit;
            }

            DA.SetData(0, dim);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.DimensionLinear;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("6888FFFF-4B85-4582-8BE6-04F83D727C85"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary;
    }
    public class DimensionLinearDeconstruct : FEM_Design_API_Component
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
            pManager.AddTextParameter("ArrowType", "ArrowType", $"Dimension line arrow type. Connect 'ValueList' to get the options: {DimensionLinear.ArrowTypeValueListDescription}", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("Decimals", "Decimals", "Number of decimals of measurements.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("LengthUnit", "LengthUnit", $"Length unit of measurements. Connect 'ValueList' to get the options: {DimensionLinear.LengthUnitValueListDescription}", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("ShowUnit", "ShowUnit", "Show length unit on measurement.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LinearDimension", "LnDim", "Linear dimension.", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "Plane", "Position and orientation of text. [m]", GH_ParamAccess.item);
            pManager.AddPointParameter("ReferencePoints", "RefPoints", "Points on dimension line to measure between.", GH_ParamAccess.list);
            pManager.AddNumberParameter("FontSize", "FontSize", "Font size of text. [m]", GH_ParamAccess.item);
            pManager.AddTextParameter("ArrowType", "ArrowType", $"Dimension line arrow type.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Decimals", "Decimals", "Number of decimals of measurements.", GH_ParamAccess.item);
            pManager.AddTextParameter("LengthUnit", "LengthUnit", "Length unit of measurements", GH_ParamAccess.item);
            pManager.AddBooleanParameter("ShowUnit", "ShowUnit", "Show length unit on measurement.", GH_ParamAccess.item);
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 4, DimensionLinear.ArrowTypeValueList, null, 0);
            ValueListUtils.updateValueLists(this, 6, DimensionLinear.LengthUnitValueList, null, 0);
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

            int decimals = dim.Decimals;
            if (DA.GetData(5, ref decimals))
            {
                dim.Decimals = decimals;    
            }

            string lengthUnit = dim.LengthUnit.ToString();
            if (DA.GetData(6, ref lengthUnit))
            {
                if (Enum.TryParse(lengthUnit, out Struxml.Lengthunit_type lengthUnitEnum))
                {
                    dim.LengthUnit = lengthUnitEnum;
                }
                else
                {
                    throw new System.ArgumentException($"Invalid horisontal alignment value: {lengthUnit}, must be one of the following values: {DimensionLinear.LengthUnitValueListDescription}");
                }
            }

            bool showUnit = dim.ShowUnit;
            if (DA.GetData(7, ref showUnit))
            {
                dim.ShowUnit = showUnit;
            }

            DA.SetData(0, dim);
            DA.SetData(1, plane);
            DA.SetDataList(2, refPoints);
            DA.SetData(3, size);
            DA.SetData(4, arrowType);
            DA.SetData(5, decimals);
            DA.SetData(6, lengthUnit);
            DA.SetData(7, showUnit);

        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.DimensionLinearDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("7D89524B-5FAC-4C0E-AD73-65AA08FCC2E6"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
    }
}