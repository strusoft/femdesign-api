// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class FictitiousShellConstruct: GH_Component
    {
        public FictitiousShellConstruct(): base("FictitiousShell.Construct", "Construct", "Construct a fictitious shell", "FEM-Design", "ModellingTools")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Srf", "Surface.", GH_ParamAccess.item);
            pManager.AddGenericParameter("StiffnessMatrix4Type", "D", "Membrane stiffness matrix", GH_ParamAccess.item);
            pManager.AddGenericParameter("StiffnessMatrix4Type", "K", "Flexural stiffness matrix", GH_ParamAccess.item);
            pManager.AddGenericParameter("StiffnessMatrix2Type", "H", "Shear stiffness matrix", GH_ParamAccess.item);
            pManager.AddNumberParameter("Density", "Density", "Density [t/m2]", GH_ParamAccess.item, 1);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("t1", "t1", "t1 [m]", GH_ParamAccess.item, 0.1);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("t2", "t2", "t2 [m]", GH_ParamAccess.item, 0.1);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Alpha1", "Alpha1", "Alpha1 [1/°C]", GH_ParamAccess.item, 0.00001);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Alpha2", "Alpha2", "Alpha2 [1/°C]", GH_ParamAccess.item, 0.00001);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("IgnoreInStImpCalc", "IgnoreInStImpCalc", "Ignore in stability/imperfection calculation", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("EdgeConnection", "EdgeConnection", "Optional, rigid if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("LocalX", "LocalX", "Set local x-axis. Vector must be perpendicular to surface local z-axis. Local y-axis will be adjusted accordingly. Optional, local x-axis from surface coordinate system used if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("LocalZ", "LocalZ", "Set local z-axis. Vector must be perpendicular to surface local x-axis. Local y-axis will be adjusted accordingly. Optional, local z-axis from surface coordinate system used if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("AverageSurfaceElementSize", "AvgSrfElemSize", "Finite element size. Set average surface element size. If set to 0 FEM-Design will automatically caculate the average surface element size. [m]", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier.", GH_ParamAccess.item, "FS");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FictitiousShell", "FS", "FictitiousShell", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Rhino.Geometry.Brep brep = null;
            if (!DA.GetData(0, ref brep))
            {
                return;
            }

            ModellingTools.StiffnessMatrix4Type d = null;
            if (!DA.GetData(1, ref d))
            {
                return;
            }

            ModellingTools.StiffnessMatrix4Type k = null;
            if (!DA.GetData(2, ref k))
            {
                return;
            }

            ModellingTools.StiffnessMatrix2Type h = null;
            if (!DA.GetData(3, ref h))
            {
                return;
            }

            double density = 1;
            if (!DA.GetData(4, ref density))
            {
                // pass
            }

            double t1 = 0.1;
            if (!DA.GetData(5, ref t1))
            {
                // pass
            }

            double t2 = 0.1;
            if (!DA.GetData(6, ref t2))
            {
                // pass
            }

            double alpha1 = 0.00001;
            if (!DA.GetData(7, ref alpha1))
            {
                // pass
            }
            
            double alpha2 = 0.00001;
            if (!DA.GetData(8, ref alpha2))
            {
                // pass
            }

            bool ignore = false;
            if (!DA.GetData(9, ref ignore))
            {
                // pass
            }

            Shells.EdgeConnection edgeConnection = Shells.EdgeConnection.Default;
            if (!DA.GetData(10, ref edgeConnection))
            {
                // pass
            }

            Rhino.Geometry.Vector3d x = Vector3d.Zero;
            if (!DA.GetData(11, ref x))
            {
                // pass
            }

            Rhino.Geometry.Vector3d z = Vector3d.Zero;
            if (!DA.GetData(12, ref z))
            {
                // pass
            }

            double mesh = 0;
            if (!DA.GetData(13, ref mesh))
            {
                // pass
            }

            string identifier = "FS";
            if (!DA.GetData(14, ref identifier))
            {
                // pass
            }

            // convert geometry
            Geometry.Region region = brep.FromRhino();

            // add edge connection
            region.SetEdgeConnections(edgeConnection);

            //
            ModellingTools.FictitiousShell obj = new ModellingTools.FictitiousShell(region, d, k, h, density, t1, t2, alpha1, alpha2, ignore, mesh, identifier);

            // set local x-axis
            if (!x.Equals(Vector3d.Zero))
            {
                obj.LocalX = x.FromRhino();
            }

            // set local z-axis
            if (!z.Equals(Vector3d.Zero))
            {
                obj.LocalZ = z.FromRhino();
            }

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.FictShell;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("f9544346-eb4d-455e-804c-fee34b0d16a6"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}