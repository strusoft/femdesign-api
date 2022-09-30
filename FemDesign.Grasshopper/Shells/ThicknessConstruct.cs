// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class Thickness_Define: GH_Component
    {
        public Thickness_Define(): base("Thickness.Construct", "Construct", "Construct a Thickness object. Thickness objects are used to define Plates and Walls with variable thickness, [t1, t2, t3] and [t1, t2] respectively.", CategoryName.Name(), SubCategoryName.Cat2b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Point", "Position of thickness value.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Value", "Val", "Value of thickness at position [m]", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Thickness", "Thickness", "Thickness.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //
            Point3d point = Point3d.Origin;
            double val = 0;
            if (!DA.GetData(0, ref point))
            {
                return;
            }
            if (!DA.GetData(1, ref val))
            {
                return;
            }

            // convert geometry
            FemDesign.Geometry.Point3d fdPoint = point.FromRhino();

            //
            FemDesign.Shells.Thickness obj = new FemDesign.Shells.Thickness(fdPoint, val);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ThicknessDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("9a2ed9d4-9942-4755-a6a1-bfc079766193"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}