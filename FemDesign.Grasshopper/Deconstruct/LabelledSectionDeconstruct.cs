// https://strusoft.com/
using System;
using System.Linq;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class LabelledSectionDeconstruct : GH_Component
    {
        public LabelledSectionDeconstruct() : base("LabelledSection.Deconstruct", "Deconstruct", "Deconstruct a LabelledSection element.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LabelledSection", "LabelledSection", "LabelledSection.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "", GH_ParamAccess.item);
            pManager.AddTextParameter("Id", "Id", "", GH_ParamAccess.item);
            pManager.AddCurveParameter("Geometry", "Geometry", "Line or Polyline", GH_ParamAccess.item);
            pManager.AddPointParameter("Points", "Points", "", GH_ParamAccess.item);
            pManager.AddPointParameter("BasePoints", "BasePoints", "", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.AuxiliaryResults.LabelledSection labelSection = null;
            if (!DA.GetData(0, ref labelSection))
            {
                return;
            }

            var fdPoints = labelSection._lineSegment == null ? labelSection._polyline.Verticies : labelSection._lineSegment.Verticies;
            var points = fdPoints.Select(x => x.ToRhino());

            Rhino.Geometry.Point3d? basePoint;
            if(labelSection._lineSegment == null)
            {
                basePoint = null;
            }
            else
            {
                basePoint = labelSection._lineSegment.BasePoint.ToRhino();
            }

            var geometry = new Rhino.Geometry.PolylineCurve(points);

            // return
            DA.SetData(0, labelSection.Guid);
            DA.SetData(1, labelSection.Name);
            DA.SetData(2, geometry);
            DA.SetDataList(3, points);
            DA.SetData(4, basePoint);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LabelledSectionDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{0E67CDAB-CE82-4D3A-8FC9-ED14A4032276}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}