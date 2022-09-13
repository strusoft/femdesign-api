// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.Releases;


namespace FemDesign.Grasshopper
{
    public class SurfaceSupportConstruct : GH_Component
    {
        public SurfaceSupportConstruct() : base("SurfaceSupport.Construct", "Construct", "Construct a SurfaceSupport element.", CategoryName.Name(),
            SubCategoryName.Cat1())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface defining the SurfaceSupport.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Motions", "Motions", "Motions release for the surface support.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Plastic Limits Forces Motions", "PlaLimM", "Plastic limits forces for motion springs. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true; 
            pManager.AddVectorParameter("LocalX", "LocalX", "Set local x-axis. Vector must be perpendicular to surface local z-axis. Local y-axis will be adjusted accordingly. Optional, local x-axis from surface coordinate system used if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("LocalZ", "LocalZ", "Set local z-axis. Vector must be perpendicular to surface local x-axis. Local y-axis will be adjusted accordingly. Optional, local z-axis from surface coordinate system used if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item, "S");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SurfaceSupport", "SurfaceSupport", "Define SurfaceSupport.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Brep surface = null;
            Motions motions = null;
            MotionsPlasticLimits motionsPlasticLimit = null;
            Rhino.Geometry.Vector3d x = Vector3d.Zero;
            Rhino.Geometry.Vector3d z = Vector3d.Zero;
            string identifier = "S";
            if (!DA.GetData(0, ref surface))
            {
                return;
            }
            if (!DA.GetData(1, ref motions))
            {
                return;
            }
            DA.GetData(2, ref motionsPlasticLimit);
            DA.GetData(3, ref x);
            DA.GetData(4, ref z);
            DA.GetData(5, ref identifier);

            if (surface == null || motions == null || identifier == null)
            {
                return;
            }

            Geometry.Region region = surface.FromRhino();
            Supports.SurfaceSupport obj = new Supports.SurfaceSupport(region, motions, motionsPlasticLimit, identifier);

            // Set local x-axis
            if (!x.Equals(Vector3d.Zero))
            {
                obj.CoordinateSystem.SetXAroundZ(x.FromRhino());
            }

            // Set local z-axis
            if (!z.Equals(Vector3d.Zero))
            {
                obj.CoordinateSystem.SetZAroundX(z.FromRhino());
            }

            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SurfaceSupport;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("bb903392-e157-4802-954c-7208d9a48b2b"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}