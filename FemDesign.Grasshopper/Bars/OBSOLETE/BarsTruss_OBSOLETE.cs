// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class BarsTruss_OBSOLETE: GH_Component
    {
        public BarsTruss_OBSOLETE(): base("Bars.Truss", "Truss", "Create a bar element of type truss.", CategoryName.Name(),
            SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Line", "Line", "LineCurve", GH_ParamAccess.item);
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Section", "Section", "Section.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("LocalY", "LocalY", "Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("OrientLCS", "OrientLCS", "Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.", GH_ParamAccess.item, true);
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
            if (!DA.GetData(1, ref material))
            {
                material = FemDesign.Materials.MaterialDatabase.GetDefault().MaterialByName("C30/37");
            }

            FemDesign.Sections.Section section = null;
            if (!DA.GetData(2, ref section))
            {
                section = FemDesign.Sections.SectionDatabase.GetDefault().SectionByName("Concrete sections, Rectangle, 250x600");
            }

            Vector3d v = Vector3d.Zero;
            if (!DA.GetData(3, ref v))
            {
                // pass
            }

            bool orientLCS = true;
            if (!DA.GetData(4, ref orientLCS))
            {
                // pass
            }

            string identifier = "T";
            if (!DA.GetData(5, ref identifier))
            {
                // pass
            }

            // convert geometry
            if (curve.GetType() != typeof(LineCurve))
            {
                throw new System.ArgumentException("Curve must be a LineCurve");
            }
            FemDesign.Geometry.Edge edge = Convert.FromRhinoLineCurve((LineCurve)curve);

            // bar
            FemDesign.Bars.Bar bar = new Bars.Truss(edge, material, section, identifier);

            // set local y-axis
            if (!v.Equals(Vector3d.Zero))
            {
                bar.BarPart.LocalY = v.FromRhino();
            }

            // else orient coordinate system to GCS
            else
            {
                if (orientLCS)
                {
                    bar.BarPart.OrientCoordinateSystemToGCS();
                }
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
        public override GH_Exposure Exposure => GH_Exposure.hidden;

    }
}