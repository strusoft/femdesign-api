// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class LineSupportDefine: GH_Component
    {
        public LineSupportDefine(): base("LineSupport.Define", "Define", "Define a LineSupport element.", "FemDesign", "Supports")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "Curve along where to place the LineSupport.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Motions", "Motions", "Motion springs.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Rotations", "Rotations", "Rotation springs.", GH_ParamAccess.item);
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
            pManager.AddGenericParameter("LineSupport", "LineSupport", "LineSupport.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //
            Curve curve = null;
            FemDesign.Releases.Motions motions = null;
            FemDesign.Releases.Rotations rotations = null;
            bool movingLocal = false;
            if (!DA.GetData(0, ref curve))
            {
                return;
            }
            if (!DA.GetData(1, ref motions))
            {
                return;
            }
            if (!DA.GetData(2, ref rotations))
            {
                return;
            }
            if (!DA.GetData(3, ref movingLocal))
            {
                // pass
            }
            Vector3d v = Vector3d.Zero;
            if (!DA.GetData(4, ref v))
            {
                // pass
            }
            bool orientLCS = true;
            if (!DA.GetData(5, ref orientLCS))
            {
                // pass
            }
            string identifier = "S";
            if (!DA.GetData(6, ref identifier))
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
            obj.LineSupport = new FemDesign.Supports.LineSupport(edge, motions, rotations, movingLocal, identifier);

            // set local y-axis
            if (!v.Equals(Vector3d.Zero))
            {
                obj.LineSupport.Group.LocalY = FemDesign.Geometry.FdVector3d.FromRhino(v);
            }

            // else orient coordinate system to GCS
            else
            {
                if (orientLCS)
                {  
                    obj.LineSupport.Group.OrientCoordinateSystemToGCS();
                }
            }

            // return
            DA.SetData(0, obj);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LineSupportDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("d9254806-ee56-4805-93e6-d9497c735f1c"); }
        }
    }
}