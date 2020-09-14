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
            FemDesign.Geometry.Edge edge = FemDesign.Geometry.Edge.FromRhinoLineOrArc1(curve);
            
            //
            FemDesign.Supports.GenericSupportObject obj = new FemDesign.Supports.GenericSupportObject();
            obj.LineSupport = new FemDesign.Supports.LineSupport(edge, motions, rotations, movingLocal, identifier);

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