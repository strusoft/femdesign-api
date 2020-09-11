// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class LineSupportHinged: GH_Component
    {
        public LineSupportHinged(): base("LineSupport.Hinged", "Hinged", "Create a Hinged LineSupport element.", "FemDesign", "Supports")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "Curve along where to place the LineSupport.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("MovingLocal", "MovingLocal", "LCS changes direction along line? True/false.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item, "S");
           pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LineSupport", "LineSupport", "Hinged LineSupport.", GH_ParamAccess.item);
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
            string identifier = "S";
            if (!DA.GetData(2, ref identifier))
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
            obj.lineSupport = FemDesign.Supports.LineSupport.Hinged(edge, movingLocal, identifier);

            // return
            DA.SetData(0, obj);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LineSupportHinged;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("ad4edebb-e571-440f-bd7c-e59424666fc9"); }
        }
    }
}