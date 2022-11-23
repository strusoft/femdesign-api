// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class SupportDeconstruct : GH_Component
    {
        public SupportDeconstruct() : base("SupportGroup.Deconstruct", "Deconstruct", "Deconstruct a PointSupportGroup, LineSupportGroup or SurfaceSupportGroup element.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("SupportGroup", "SupportGroup", "PointSupportGroup, LineSupportGroup or SurfaceSupportGroup.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
            pManager.AddTextParameter("AnalyticalID", "AnalyticalID", "Analytical ID.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Geometry", "Geometry", "Geometry. [m]", GH_ParamAccess.item);
            pManager.AddGenericParameter("MovingLocal", "MovingLocal", "MovingLocal.", GH_ParamAccess.item);
            pManager.AddVectorParameter("GroupLocalX", "GroupLocalX", "The value refer to Group LocalX and NOT the local coordinate system of the underlying geometry.", GH_ParamAccess.item);
            pManager.AddVectorParameter("GroupLocalY", "GroupLocalY", "The value refer to Group LocalY and NOT the local coordinate system of the underlying geometry.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Motions", "Motions", "Motions.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Rotations", "Rotations", "Rotations.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Plastic Limits Forces Motions", "PlaLimMotions", "Plastic limits forces for motion springs.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Plastic Limits Forces Rotations", "PlaLimRotations", "Plastic limits forces for rotation springs.", GH_ParamAccess.item);
            pManager.Register_GenericParam("RigidityGroup", "RigidityGroup", "RigidityGroup");
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
                DA.SetData(3, "PointSupport has no moving local property.");
                DA.SetData(4, obj.Group.LocalX.ToRhino());
                DA.SetData(5, obj.Group.LocalY.ToRhino());

                // Catch pre-defined rigidity
                if (obj.Group.Rigidity != null)
                {
                    DA.SetData(6, obj.Group.Rigidity.Motions);
                    DA.SetData(7, obj.Group.Rigidity.Rotations);
                    DA.SetData(8, obj.Group.Rigidity.PlasticLimitForces);
                    DA.SetData(9, obj.Group.Rigidity.PlasticLimitMoments);
                }
                else if(obj.Group.Rigidity == null && obj.Group.RigidityGroup != null)
                {
                    DA.SetDataList(10, obj.Group.RigidityGroup.Springs);
                }
                else
                {
                    DA.SetData(6, obj.Group.PredefRigidity.Rigidity.Motions);
                    DA.SetData(7, obj.Group.PredefRigidity.Rigidity.Rotations);
                    DA.SetData(8, obj.Group.PredefRigidity.Rigidity.PlasticLimitForces);
                    DA.SetData(9, obj.Group.PredefRigidity.Rigidity.PlasticLimitMoments);
                }
            }
            else if (support.GetType() == typeof(Supports.LineSupport))
            {
                var obj = (Supports.LineSupport)support;
                DA.SetData(0, obj.Guid);
                DA.SetData(1, obj.Name);
                DA.SetData(2, obj.GetRhinoGeometry());
                DA.SetData(3, obj.MovingLocal);
                DA.SetData(4, obj.Group.LocalX.ToRhino());
                DA.SetData(5, obj.Group.LocalY.ToRhino());

                // Catch pre-defined rigidity
                if (obj.Group.Rigidity != null)
                {
                    DA.SetData(6, obj.Group.Rigidity.Motions);
                    DA.SetData(7, obj.Group.Rigidity.Rotations);
                    DA.SetData(8, obj.Group.Rigidity.PlasticLimitForces);
                    DA.SetData(9, obj.Group.Rigidity.PlasticLimitMoments);

                }
                else if (obj.Group.Rigidity == null && obj.Group.RigidityGroup != null)
                {
                    DA.SetDataList(10, obj.Group.RigidityGroup.Springs);
                }
                else
                {
                    DA.SetData(6, obj.Group.PredefRigidity.Rigidity.Motions);
                    DA.SetData(7, obj.Group.PredefRigidity.Rigidity.Rotations);
                    DA.SetData(8, obj.Group.PredefRigidity.Rigidity.PlasticLimitForces);
                    DA.SetData(9, obj.Group.PredefRigidity.Rigidity.PlasticLimitMoments);

                }
            }
            else if (support.GetType() == typeof(Supports.SurfaceSupport))
            {
                var obj = (Supports.SurfaceSupport)support;
                DA.SetData(0, obj.Guid);
                DA.SetData(1, obj.Name);
                DA.SetData(2, obj.Region.ToRhinoBrep());
                DA.SetData(3, "SurfaceSupport has no moving local property.");
                DA.SetData(4, obj.CoordinateSystem.LocalX.ToRhino());
                DA.SetData(5, obj.CoordinateSystem.LocalY.ToRhino());
                
                // Catch pre-defined rigidity
                if (obj.Rigidity != null)
                {
                    DA.SetData(6, obj.Rigidity.Motions);
                    DA.SetData(8, obj.Rigidity.PlasticLimitForces);

                }
                else
                {
                    DA.SetData(6, obj.PredefRigidity.Rigidity.Motions);
                    DA.SetData(8, obj.PredefRigidity.Rigidity.PlasticLimitForces);
                }

                DA.SetData(7, "SurfaceSupport has no rotations property");
                DA.SetData(9, "SurfaceSupport has no rotations plastic limit moments property");
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
            get { return new Guid("{6FE0FF0B-97A7-4014-B886-137691B8308B}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}