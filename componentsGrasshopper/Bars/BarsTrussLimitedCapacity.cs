// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class BarsTrussLimitedCapacity: GH_Component
    {
        public BarsTrussLimitedCapacity(): base("Bars.TrussLimitedCapacity", "TrussLimitedCapacity", "Create a bar element of type truss with limited capacity in compression and tension.", "FemDesign", "Bars")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Line", "Line", "LineCurve", GH_ParamAccess.item);
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Section", "Section", "Section.", GH_ParamAccess.item);
            pManager.AddNumberParameter("LimitCompression", "LimitCompression", "Compression force limit", GH_ParamAccess.item);
            pManager.AddBooleanParameter("BehaviourCompression", "BehaviourCompression", "True if plastic behaviour. False if brittle behaviour", GH_ParamAccess.item);
            pManager.AddNumberParameter("LimitTension", "LimitTension", "Tension force limit", GH_ParamAccess.item);
            pManager.AddBooleanParameter("BehaviourTension", "BehaviourTension", "True if plastic behaviour. False if brittle behaviour", GH_ParamAccess.item);
            pManager.AddVectorParameter("LocalY", "LocalY", "Set local y-axis. Vector must be perpendicular to Curve mid-point local x-axis. Optional, local y-axis from Curve coordinate system at mid-point used if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item, "T");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "Bar.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            Curve curve = null;
            if (!DA.GetData(0, ref curve)) { return; }

            FemDesign.Materials.Material material = null;
            if (!DA.GetData(1, ref material)) { return; }

            FemDesign.Sections.Section section = null;
            if (!DA.GetData(2, ref section)) { return; }

            double limitCompression = 0;
            if (!DA.GetData(3, ref limitCompression)) { return; }

            bool plasticCompression = false;
            if (!DA.GetData(4, ref plasticCompression)) { return; }
            
            double limitTension = 0;
            if (!DA.GetData(5, ref limitTension)) { return; }
            
            bool plasticTension = false;
            if (!DA.GetData(6, ref plasticTension)) { return; }

            Vector3d v = Vector3d.Zero;
            if (!DA.GetData(7, ref v))
            {
                // pass
            }

            string identifier = "T";
            if (!DA.GetData(8, ref identifier))
            {
                // pass
            }
            if (curve == null || material == null || section == null || identifier == null) { return; }

            // convert geometry
            if (curve.GetType() != typeof(LineCurve))
            {
                throw new System.ArgumentException("Curve must be a LineCurve");
            }
            FemDesign.Geometry.Edge edge = FemDesign.Geometry.Edge.FromRhinoLineCurve((LineCurve)curve);

            // bar
            FemDesign.Bars.Bar bar = FemDesign.Bars.Bar.Truss(identifier, edge, material, section, limitCompression, limitTension, plasticCompression, plasticTension);

            // set local y-axis
            if (!v.Equals(Vector3d.Zero))
            {
                bar.barPart.localY = FemDesign.Geometry.FdVector3d.FromRhino(v);
            }

            // return
            DA.SetData(0, bar);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.TrussLimitedCapacity;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("49eb9ae7-d644-4d22-b6d5-e1f96ffcba51"); }
        }
    }
}