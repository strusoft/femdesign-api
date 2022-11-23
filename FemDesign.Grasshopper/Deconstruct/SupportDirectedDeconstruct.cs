// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class SupportDirectedDeconstruct : GH_Component
    {
        public SupportDirectedDeconstruct() : base("SupportDirected.Deconstruct", "Deconstruct", "Deconstruct a PointSupportDirected, LineSupportDirected element.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("SupportDirected", "SupportDirected", "PointSupportDirected, LineSupportDirected.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
            pManager.AddTextParameter("Name", "Name", "Name", GH_ParamAccess.item);
            pManager.AddGenericParameter("Geometry", "Geometry", "Geometry. [m]", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Direction", "Direction", GH_ParamAccess.item);
            pManager.AddGenericParameter("Mov", "Mov", "Mov", GH_ParamAccess.item);
            pManager.AddGenericParameter("Rot", "Rot", "Rot", GH_ParamAccess.item);
            pManager.AddGenericParameter("Plastic Limits Forces Motions", "PlaLimMotions", "Plastic limits forces for motion springs.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.GenericClasses.ISupportElement support = null;
            if (!DA.GetData(0, ref support))
            {
                return;
            }
            if (support == null)
            {
                return;
            }

            if (support.GetType() == typeof(Supports.PointSupport))
            {
                var obj = (Supports.PointSupport)support;
                DA.SetData(0, obj.Guid);
                DA.SetData(1, obj.Name);
                DA.SetData(2, obj.GetRhinoGeometry());
                DA.SetData(3, obj.Directed.Direction.ToRhino());
                DA.SetData(4, obj.Directed.Movement);
                DA.SetData(5, obj.Directed.Rotation);
                DA.SetData(6, obj.Directed.PlasticLimitForces);
            }
            else if (support.GetType() == typeof(Supports.LineSupport))
            {
                var obj = (Supports.LineSupport)support;
                DA.SetData(0, obj.Guid);
                DA.SetData(1, obj.Name);
                DA.SetData(2, obj.GetRhinoGeometry());
                DA.SetData(3, obj.Directed.Direction.ToRhino());
                DA.SetData(4, obj.Directed.Movement);
                DA.SetData(5, obj.Directed.Rotation);
                DA.SetData(6, obj.Directed.PlasticLimitForces);
            }
            else
            {
                throw new ArgumentException("Type is not supported. LoadsDeconstruct failed.");
            }
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SupportsDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{9F01F494-A2BD-4799-9BB9-78639608E2F3}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}