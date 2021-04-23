// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class SurfaceSupportDefine: GH_Component
    {
        public SurfaceSupportDefine(): base("SurfaceSupport.Define", "Define", "Create a SurfaceSupport element.", "FemDesign", "Supports")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface defining the SurfaceSupport.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Motions", "Motions", "Motions release for the surface support.", GH_ParamAccess.item);
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
            //
            Brep surface = null;
            if (!DA.GetData(0, ref surface))
            {
                return;
            }
            FemDesign.Releases.Motions motions = null;
            if (!DA.GetData(1, ref motions))
            {
                return;
            }
            Rhino.Geometry.Vector3d x = Vector3d.Zero;
            if (!DA.GetData(2, ref x))
            {
                // pass
            }

            Rhino.Geometry.Vector3d z = Vector3d.Zero;
            if (!DA.GetData(3, ref z))
            {
                // pass
            }
            string identifier = "S";
            if (!DA.GetData(4, ref identifier))
            {
                // pass
            }
            if (surface == null || motions == null || identifier == null)
            {
                return;
            }

            // convert geometry
            FemDesign.Geometry.Region region = surface.FromRhino();
                       
            //
            FemDesign.Supports.SurfaceSupport obj = new Supports.SurfaceSupport(region, motions, identifier);

            // set local x-axis
            if (!x.Equals(Vector3d.Zero))
            {
                obj.CoordinateSystem.SetXAroundZ(x.FromRhino());
            }

            // set local z-axis
            if (!z.Equals(Vector3d.Zero))
            {
                obj.CoordinateSystem.SetZAroundX(z.FromRhino());
            }


            // generic support object
            FemDesign.Supports.GenericSupportObject genericObj = new FemDesign.Supports.GenericSupportObject(obj);


            // return
            DA.SetData(0, genericObj);

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
            get { return new Guid("bb903392-e157-4802-954c-7208d9a48b2b"); }
        }
    }
}