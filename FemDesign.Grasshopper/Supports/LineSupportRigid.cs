// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class LineSupportRigid: GH_Component
    {
        public LineSupportRigid(): base("LineSupport.Rigid", "Rigid", "Create a Rigid LineSupport element.",
            CategoryName.Name(),
            SubCategoryName.Cat1())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "Curve along where to place the LineSupport.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("MovingLocal", "MovingLocal", "LCS changes direction along line? True/false.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddVectorParameter("LocalY", "LocalY", "Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. This parameter overrides OrientLCS", GH_ParamAccess.item);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddBooleanParameter("OrientLCS", "OrientLCS", "Orient LCS to GCS? If true the LCS of this object will be oriented to the GCS trying to align local z to global z if possible or align local y to global y if possible (if object is vertical). If false local y-axis from Curve coordinate system at mid-point will be used.", GH_ParamAccess.item, true);
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
            bool orientLCS = true;
            if (!DA.GetData(3, ref orientLCS))
            {
                // pass
            }
            string identifier = "S";
            if (!DA.GetData(4, ref identifier))
            {
                // pass
            }
            if (curve == null || identifier == null)
            {
                return;
            }

            // convert geometry
            FemDesign.Geometry.Edge edge = Convert.FromRhinoLineOrArc1(curve);
            
            var obj = FemDesign.Supports.LineSupport.Rigid(edge, movingLocal, identifier);

            // set local y-axis
            if (!v.Equals(Vector3d.Zero))
            {
                obj.Group.LocalY = v.FromRhino();
            }

            // else orient coordinate system to GCS
            else
            {
                if (orientLCS)
                {  
                    obj.Group.OrientCoordinateSystemToGCS();
                }
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

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}