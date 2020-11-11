// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class BarsTruss: GH_Component
    {
        public BarsTruss(): base("Bars.Truss", "Truss", "Create a bar element of type truss.", "FemDesign", "Bars")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Line", "Line", "LineCurve", GH_ParamAccess.item);
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Section", "Section", "Section.", GH_ParamAccess.item);
            pManager.AddVectorParameter("LocalY", "LocalY", "Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. Optional, local y-axis from Curve coordinate system at mid-point used if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item, "T");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "Bar.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            
            Curve curve = null;
            if (!DA.GetData(0, ref curve)) { return; }
            
            FemDesign.Materials.Material material = null;
            if (!DA.GetData(1, ref material)) { return; }
            
            FemDesign.Sections.Section section = null;
            if (!DA.GetData(2, ref section)) { return; }

            Vector3d v = Vector3d.Zero;
            if (!DA.GetData(3, ref v))
            {
                // pass
            }

            string identifier = "T";
            if (!DA.GetData(4, ref identifier))
            {
                // pass
            }

            if (curve == null || material == null || section == null || identifier == null) { return; }

            // convert geometry
            if (curve.GetType() != typeof(LineCurve))
            {
                throw new System.ArgumentException("Curve must be a LineCurve");
            }
            FemDesign.Geometry.Edge edge = FemDesign.Geometry.Edge.FromRhinoLineCurve((LineCurve)curve);

            // bar
            FemDesign.Bars.Bar bar = FemDesign.Bars.Bar.Truss(identifier, edge, material, section);

            // set local y-axis
            if (!v.Equals(Vector3d.Zero))
            {
                bar.BarPart.LocalY = FemDesign.Geometry.FdVector3d.FromRhino(v);
            }

            // else orient coordinate system to GCS
            else
            {
                bar.BarPart.OrientCoordinateSystemToGCS();
            }

            // return
            DA.SetData(0, bar);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.TrussDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("bfc07633-529a-4a98-a45b-ce657e916f83"); }
        }
    }
}