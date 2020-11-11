// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class LineSupportRigid: GH_Component
    {
        public LineSupportRigid(): base("LineSupport.Rigid", "Rigid", "Create a Rigid LineSupport element.", "FemDesign", "Supports")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "Curve along where to place the LineSupport.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("MovingLocal", "MovingLocal", "LCS changes direction along line? True/false.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("LocalY", "LocalY", "Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. Optional, local y-axis from Curve coordinate system at mid-point used if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item, "S");
           pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LineSupport", "LineSupport", "Rigid LineSupport.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //
            Curve curve = null;
            bool movingLocal = false;
            if (!DA.GetData(0, ref curve))
            {
                return;
            }
            if (!DA.GetData(1, ref movingLocal))
            {
                // pass
            }
            Vector3d v = Vector3d.Zero;
            if (!DA.GetData(2, ref v))
            {
                // pass
            }
            string identifier = "S";
            if (!DA.GetData(3, ref identifier))
            {
                // pass
            }
            if (curve == null || identifier == null)
            {
                return;
            }

            // convert geometry
            FemDesign.Geometry.Edge edge = FemDesign.Geometry.Edge.FromRhinoLineOrArc1(curve);
            
            //
            FemDesign.Supports.GenericSupportObject obj = new FemDesign.Supports.GenericSupportObject();
            obj.LineSupport = FemDesign.Supports.LineSupport.Rigid(edge, movingLocal, identifier);

            // set local y-axis
            if (!v.Equals(Vector3d.Zero))
            {
                obj.LineSupport.Group.LocalY = FemDesign.Geometry.FdVector3d.FromRhino(v);
            }

            // else orient coordinate system to GCS
            else
            {
                obj.LineSupport.Group.OrientCoordinateSystemToGCS();
            }

            // return
            DA.SetData(0, obj);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LineSupportRigid;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("39366abf-0e8b-49cd-8b49-7ec3e0b59204"); }
        }
    }
}