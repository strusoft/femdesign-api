// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class EdgeConnectionDeconstruct : GH_Component
    {
        public EdgeConnectionDeconstruct() : base("EdgeConnection.Deconstruct", "Deconstruct", "Deconstruct a EdgeConnection element.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("EdgeConnection", "EdgeConnection", "EdgeConnection.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "Guid of edge connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("AnalyticalID", "AnalyticalID", "Analytical element ID.", GH_ParamAccess.item);
            pManager.AddCurveParameter("Edge", "Edge", "", GH_ParamAccess.item);
            pManager.AddPlaneParameter("LocalPlane", "LocalPlane", "Plane oriented according to the edge local coordinate system.", GH_ParamAccess.item);
            pManager.AddTextParameter("PredefinedName", "PredefinedName", "Name of predefined type.", GH_ParamAccess.item);
            pManager.AddTextParameter("PredefinedGuid", "PredefinedGuid", "Guid of predefined type.", GH_ParamAccess.item);
            pManager.AddTextParameter("Friction", "Friction", "Friction.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Motions", "Motions", "Motions", GH_ParamAccess.item);
            pManager.AddGenericParameter("Rotations", "Rotations", "Rotations", GH_ParamAccess.item);
            pManager.Register_GenericParam("RigidityGroup", "RigidityGroup", "RigidityGroup");
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Shells.EdgeConnection obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }

            // return
            DA.SetData(0, obj.Guid);
            DA.SetData(1, obj.Name);

            var edgeCurve = obj.Edge.ToRhino();
            DA.SetData(2, obj.Edge.ToRhino());

            if (!edgeCurve.IsLinear())
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Arc edge detected in the panel. Local plane is not calculated");
                DA.SetData(3, null);
            }
            else // it is a line
            {
                edgeCurve.Domain = new Rhino.Geometry.Interval(0, 1.0);
                var midPoint = edgeCurve.PointAt(0.5).FromRhino();
                var localX = (edgeCurve.PointAtEnd - edgeCurve.PointAtStart).FromRhino();
                var localZ = obj.Normal;
                var localY = localX.Cross(localZ);
                var localPlane = new FemDesign.Geometry.CoordinateSystem(midPoint, localX, localY).ToRhino();
                DA.SetData(3, localPlane);
            }

            // catch pre-defined rigidity
            if (obj.Rigidity != null)
            {
                DA.SetData(4, null);
                DA.SetData(5, null);
                if (obj.Rigidity._friction == null)
                {
                    DA.SetData(6, null);
                }
                else
                {
                    DA.SetData(6, obj.Rigidity.Friction);
                }
                DA.SetData(7, obj.Rigidity.Motions);
                DA.SetData(8, obj.Rigidity.Rotations);
            }
            else if (obj.Rigidity == null && obj.RigidityGroup != null)
            {
                DA.SetDataList(9, obj.RigidityGroup.Springs);
            }
            else
            {
                DA.SetData(4, obj.PredefRigidity.Name);
                DA.SetData(5, obj.PredefRigidity.Guid);
                if (obj.PredefRigidity.Rigidity._friction == null)
                {
                    DA.SetData(6, null);
                }
                else
                {
                    DA.SetData(6, obj.PredefRigidity.Rigidity.Friction);
                }
                DA.SetData(7, obj.PredefRigidity.Rigidity.Motions);
                DA.SetData(8, obj.PredefRigidity.Rigidity.Rotations);
            }
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.EdgeConnectionDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{82FD1C58-ED3B-4014-A076-7014F5EC6221}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}