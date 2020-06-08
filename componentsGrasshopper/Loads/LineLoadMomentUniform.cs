// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class LineLoadMomentUniform: GH_Component
    {
        public LineLoadMomentUniform(): base("LineLoad.MomentUniform", "MomentUniform", "Creates a uniform moment line load.", "FemDesign", "Loads")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "Curve defining the line load.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Moment", "Moment", "Moment.", GH_ParamAccess.item);
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase.", GH_ParamAccess.item);
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[3].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LineLoad", "LineLoad", "LineLoad.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Curve curve = null;
            Vector3d moment = Vector3d.Zero;
            FemDesign.Loads.LoadCase loadCase = null;
            string comment = null;
            if (!DA.GetData(0, ref curve)) { return; }
            if (!DA.GetData(1, ref moment)) { return; }
            if (!DA.GetData(2, ref loadCase)) { return; }
            if (!DA.GetData(3, ref comment))
            {
                // pass
            }
            if (curve == null || moment == null || loadCase == null) { return; }

            //
            FemDesign.Geometry.Edge edge = FemDesign.Geometry.Edge.FromRhinoLineOrArc1(curve);
            FemDesign.Geometry.FdVector3d _moment = FemDesign.Geometry.FdVector3d.FromRhino(moment);
            FemDesign.Loads.GenericLoadObject obj = new FemDesign.Loads.GenericLoadObject();
            obj.lineLoad = new FemDesign.Loads.LineLoad(edge, _moment, _moment, loadCase, comment, "constant", false, "moment");

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LineLoadMoment;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("2a90c1aa-96fe-4e1f-abd2-62244c966f51"); }
        }
    }
}