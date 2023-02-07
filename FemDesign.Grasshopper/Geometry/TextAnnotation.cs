// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class TextAnnotation : GH_Component
    {
        public TextAnnotation() : base("TextAnnotation", "TextAnnotation", "Create a text annotation.", CategoryName.Name(), SubCategoryName.CatLast())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("TextAnnotation", "TextAnnotation", "TextAnnotation", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddPlaneParameter("Point|Plane", "Point|Plane", "Position and orientation of text. [m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Text", "Text", "Text.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TextAnnotation", "TextAnnotation", "TextAnnotation.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddPlaneParameter("Plane", "Plane", "Position and orientation of text. [m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Text", "Text", "Text.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var textAnnot = new FemDesign.Geometry.TextAnnotation();
            if (DA.GetData(0, ref textAnnot))
            {
                // pass
            }
            else
            {
                textAnnot.Initialize();
            }

            var plane = Plane.WorldXY;
            if (!DA.GetData(1, ref plane)) 
            { 
                // pass
            }

            string text = null;
            if (!DA.GetData(2, ref text))
            {
                return;
            }

            textAnnot.Position = plane.Origin.FromRhino();
            textAnnot.LocalX = plane.XAxis.FromRhino();
            textAnnot.LocalY = plane.YAxis.FromRhino();
            textAnnot.Text = text;

            DA.SetData(0, textAnnot);
            DA.SetData(1, new Plane(textAnnot.Position.ToRhino(), textAnnot.LocalX.ToRhino(), textAnnot.LocalY.ToRhino()));
            DA.SetData(2, textAnnot.Text);
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
            get { return new Guid("24767AB6-9D7E-4DE3-BB1A-5F0AF6470987"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary;

    }
}