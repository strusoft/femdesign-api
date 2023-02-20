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
    public class TextAnnotation : GH_Component
    {
        public static readonly List<string> HorAlignValueList = Enum.GetNames(typeof(StruSoft.Interop.StruXml.Data.Hor_align)).ToList();
        public static string HorAlignValueListDescription
        {
            get
            {
                var str = "";
                foreach (var h in HorAlignValueList)
                {
                    str += "\n" + h;
                }
                return str;
            }
        }
        public static readonly List<string> VerAlignValueList = Enum.GetNames(typeof(StruSoft.Interop.StruXml.Data.Ver_align)).ToList();
        public static string VerAlignValueListDescription
        {
            get
            {
                var str = "";
                foreach (var h in VerAlignValueList)
                {
                    str += "\n" + h;
                }
                return str;
            }
        }

        public TextAnnotation() : base("TextAnnotation", "TxtAnnot", "Create a text annotation.", CategoryName.Name(), "ModellingTools")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Point|Plane", "Point|Plane", "Position and orientation of text. [m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Text", "Text", "Text.", GH_ParamAccess.item);
            pManager.AddNumberParameter("FontSize", "FontSize", "Font size of text. [m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddColourParameter("Colour", "Colour", "Colour of text. [ARGB]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("HorisontalAligment", "HorAlign", $"Horisontal alignement of text. Connect 'ValueList' to get the options: {HorAlignValueListDescription}", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("VerticalAligment", "VerAlign", $"Vertical alignement of text. Connect 'ValueList' to get the options: {VerAlignValueListDescription}", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TextAnnotation", "TextAnnotation", "TextAnnotation.", GH_ParamAccess.item);
        }
        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 4, HorAlignValueList, null, 0);
            ValueListUtils.updateValueLists(this, 5, VerAlignValueList, null, 0);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var plane = Plane.WorldXY;
            if (!DA.GetData(0, ref plane))
            {
                // pass
            }

            string text = null;
            if (!DA.GetData(1, ref text))
            {
                return;
            }


            var textAnnot = new Drawing.TextAnnotation(plane.Origin.FromRhino(), plane.XAxis.FromRhino(), plane.YAxis.FromRhino(), text);
            textAnnot.StyleType.Layer = "TEXT";
            textAnnot.StyleType.LayerObj = new StruSoft.Interop.StruXml.Data.Layer_type
            {
                Name = "TEXT",
                Colour = "000000",
                Hidden = false,
                Protected = false
            };

            double size = 0;
            if (DA.GetData(2, ref size))
            {
                textAnnot.StyleType.Font.Size = size;
            }

            System.Drawing.Color color = System.Drawing.Color.Black;
            if (DA.GetData(3, ref color))
            {
                textAnnot.StyleType.SetColor = color;
            }

            string horAlign = null;
            if (DA.GetData(4, ref horAlign))
            {
                if (Enum.TryParse(horAlign, out StruSoft.Interop.StruXml.Data.Hor_align horAlignEnum))
                {
                    textAnnot.StyleType.Font.H_align = horAlignEnum;
                }
                else
                {
                    throw new System.ArgumentException($"Invalid horisontal alignment value: {horAlign}, must be one of the following values: {HorAlignValueListDescription}");
                }
            }

            string verAlign = null;
            if (DA.GetData(5, ref verAlign))
            {
                if (Enum.TryParse(verAlign, out StruSoft.Interop.StruXml.Data.Ver_align verAlignEnum))
                {
                    textAnnot.StyleType.Font.V_align = verAlignEnum;
                }
                else
                {
                    throw new System.ArgumentException($"Invalid vertical alignment value: {verAlign}, must be one of the following values: {VerAlignValueListDescription}");
                }
            }

            DA.SetData(0, textAnnot);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.TextAnnotation;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("24767AB6-9D7E-4DE3-BB1A-5F0AF6470987"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary;
    }
    public class TextAnnotationDeconstruct : GH_Component
    {
        public TextAnnotationDeconstruct() : base("TextAnnotationDeconstruct", "TxtAnnotDecon", "Deconstruct or modify a text annotation.", CategoryName.Name(), "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("TextAnnotation", "TextAnnotation", "TextAnnotation", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Point|Plane", "Point|Plane", "Position and orientation of text. [m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Text", "Text", "Text.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("FontSize", "FontSize", "Font size of text. [m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddColourParameter("Colour", "Colour", "Colour of text. [ARGB]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("HorisontalAligment", "HorAlign", $"Horisontal alignement of text. Connect 'ValueList' to get the options: {TextAnnotation.HorAlignValueListDescription}", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("VerticalAligment", "VerAlign", $"Vertical alignement of text. Connect 'ValueList' to get the options: {TextAnnotation.VerAlignValueListDescription}", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TextAnnotation", "TextAnnotation", "TextAnnotation.", GH_ParamAccess.item);
            pManager.AddPlaneParameter("Plane", "Plane", "Position and orientation of text. [m]", GH_ParamAccess.item);
            pManager.AddTextParameter("Text", "Text", "Text.", GH_ParamAccess.item);
            pManager.AddNumberParameter("FontSize", "FontSize", "Font size of text. [m]", GH_ParamAccess.item);
            pManager.AddColourParameter("Colour", "Colour", "Colour of text [ARGB]", GH_ParamAccess.item);
            pManager.AddTextParameter("HorisontalAligment", "HorAlign", "Horisontal alignement of text", GH_ParamAccess.item);
            pManager.AddTextParameter("VerticalAligment", "VerAlign", "Vertical alignement of text", GH_ParamAccess.item);
        }
        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 5, TextAnnotation.HorAlignValueList, null, 0);
            ValueListUtils.updateValueLists(this, 6, TextAnnotation.VerAlignValueList, null, 0);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Drawing.TextAnnotation origTextAnnot = null;
            if (!DA.GetData(0, ref origTextAnnot))
            {
                return;
            }

            var textAnnot = origTextAnnot.DeepClone();

            var plane = new Plane(textAnnot.Position.ToRhino(), textAnnot.LocalX.ToRhino(), textAnnot.LocalY.ToRhino());
            if (DA.GetData(1, ref plane))
            {
                textAnnot.Position = plane.Origin.FromRhino();
                textAnnot.LocalX = plane.XAxis.FromRhino();
                textAnnot.LocalY = plane.YAxis.FromRhino();
            }

            string text = textAnnot.Text;
            if (DA.GetData(2, ref text))
            {
                textAnnot.Text = text;
            }

            double size = textAnnot.StyleType.Font.Size;
            if (DA.GetData(3, ref size))
            {
                textAnnot.StyleType.Font.Size = size;
            }

             System.Drawing.Color color = textAnnot.StyleType.GetColor;
            if (DA.GetData(4, ref color))
            {
                textAnnot.StyleType.SetColor = color;
            }

            string horAlign = textAnnot.StyleType.Font.H_align.ToString();
            if (DA.GetData(5, ref horAlign))
            {
                if (Enum.TryParse(horAlign, out StruSoft.Interop.StruXml.Data.Hor_align horAlignEnum))
                {
                    textAnnot.StyleType.Font.H_align = horAlignEnum;
                }
                else
                {
                    throw new System.ArgumentException($"Invalid horisontal alignment value: {horAlign}, must be one of the following values: {TextAnnotation.HorAlignValueListDescription}");
                }
            }

            string verAlign = textAnnot.StyleType.Font.V_align.ToString();
            if (DA.GetData(6, ref verAlign))
            {
                if (Enum.TryParse(verAlign, out StruSoft.Interop.StruXml.Data.Ver_align verAlignEnum))
                {
                    textAnnot.StyleType.Font.V_align = verAlignEnum;
                }
                else
                {
                    throw new System.ArgumentException($"Invalid vertical alignment value: {verAlign}, must be one of the following values: {TextAnnotation.VerAlignValueListDescription}");
                }
            }

            DA.SetData(0, textAnnot);
            DA.SetData(1, plane);
            DA.SetData(2, text);
            DA.SetData(3, size);
            DA.SetData(4, color);
            DA.SetData(5, horAlign);
            DA.SetData(6, verAlign);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.TextAnnotationDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("7C35D2FF-75B1-4561-A9BE-B7D22FE93B95"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
    }
}