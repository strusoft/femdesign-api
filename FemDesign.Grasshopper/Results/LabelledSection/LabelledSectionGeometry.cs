// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class LabelledSectionGeometry : FEM_Design_API_Component
    {
        public LabelledSectionGeometry() : base("LabelledSection", "LabelledSection", "Define LabelledSection from a Line or Polyline", CategoryName.Name(), SubCategoryName.Cat7b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "Line or Polyline", GH_ParamAccess.item);
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item, "LS");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LabelledSection", "LabelledSection", "", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            Rhino.Geometry.Curve curve = null;
            DA.GetData(0, ref curve);

            if (!curve.TryGetPolyline(out Polyline poly))
                throw new ArgumentException("Input Curve is not 'Line' or 'Polyline'");


            string identifier = "LS";
            DA.GetData(1, ref identifier);

            var points = new List<FemDesign.Geometry.Point3d>();
            for (int index = 0; index < poly.Count; index++)
            {
                points.Add(poly.ElementAt(index).FromRhino());
            }

            var labelledSection = new FemDesign.AuxiliaryResults.LabelledSection(points, identifier);

            // output
            DA.SetData(0, labelledSection);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LabelledSection;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{B8EF1A4A-C8F6-48A3-92E0-B6DE7CE16182}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}