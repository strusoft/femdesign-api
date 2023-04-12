// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using GrasshopperAsyncComponent;

namespace FemDesign.Grasshopper
{
    public class InteractionSurface_Pipe : GH_AsyncComponent
    {
        public InteractionSurface_Pipe() : base("InteractionSurface", "InteractionSurface", "", CategoryName.Name(), "Reinforcement")
        {
            BaseWorker = new InteractionSurfaceWorker();
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Bar", "Bar", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("Offset", "Offset", "", GH_ParamAccess.item, 0.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("fUlt", "fUlt", "", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.Register_MeshParam("InteractionSurface", "InteractionSurface", "", GH_ParamAccess.item);
            pManager.Register_DoubleParam("N", "N", "", GH_ParamAccess.item);
            pManager.Register_DoubleParam("My", "My", "", GH_ParamAccess.item);
            pManager.Register_DoubleParam("Mz", "Mz", "", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Success", "Success", "", GH_ParamAccess.item);
        }
        protected override System.Drawing.Bitmap Icon => null;

        public override Guid ComponentGuid => new Guid("881D7F8D-FB90-44E9-BB5B-2FD6A3119B0D");
        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }

    /// <summary>
    /// https://github.com/specklesystems/GrasshopperAsyncComponent
    /// </summary>
    public class InteractionSurfaceWorker : WorkerInstance
    {
        /* INPUT */
        FemDesign.Bars.Bar bar = null;
        double offset = 0.0;
        bool fUlt = true;

        FemDesignConnection connection = null;
        bool runNode = true;

        /* OUTPUT */

        double my;
        double mz;
        double n;
        Rhino.Geometry.Mesh interSrf = null;
        bool success = false;

        public InteractionSurfaceWorker() : base(null) { }

        public override void DoWork(Action<string, double> ReportProgress, Action Done)
        {
            if (runNode)
            {
                var intSrf = connection.RunInteractionSurface(bar, offset, fUlt)[0];
                interSrf = intSrf.ToRhino();
                success = true;
            }
            else
            {
                success = false;
            }

            Done();
        }

        public override WorkerInstance Duplicate() => new InteractionSurfaceWorker();

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref connection)) return;
            if (!DA.GetData("Bar", ref bar)) return;
            DA.GetData("Offset", ref offset);
            DA.GetData("fUlt", ref fUlt);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            DA.SetData("Connection", connection);
            DA.SetData("InteractionSurface", interSrf);
            DA.SetData("My", my);
            DA.SetData("Mz", mz);
            DA.SetData("N", n);
            DA.SetData("Success", success);
        }
    }
}