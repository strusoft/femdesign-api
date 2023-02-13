// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using GrasshopperAsyncComponent;

namespace FemDesign.Grasshopper
{
    public class PipeOpen : GH_AsyncComponent
    {
        public PipeOpen() : base("FEM-Design.OpenModel", "OpenModel", "Open model in FEM-Design.", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ModelOpenWorker();
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel to open or file path.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Success", "Success", "", GH_ParamAccess.item);
        }
        protected override System.Drawing.Bitmap Icon => base.Icon;
        public override Guid ComponentGuid => new Guid("96dc72e0-c0c1-4081-ac2b-56be85905fb2");
        public override GH_Exposure Exposure => GH_Exposure.primary;
    }

    /// <summary>
    /// https://github.com/specklesystems/GrasshopperAsyncComponent
    /// </summary>
    public class ModelOpenWorker : WorkerInstance
    {
        /* INPUT */
        dynamic model = null;
        FemDesignConnection connection = null;
        bool runNode = false;

        /* OUTPUT */
        bool success = false;

        public ModelOpenWorker() : base(null) { }

        public override void DoWork(Action<string, double> ReportProgress, Action Done)
        {
            //// ?? Check for task cancellation!
            //if (CancellationToken.IsCancellationRequested) return;

            if (runNode)
            {
                connection.Open(model.Value);
                success = true;
            }
            else
            {
                success = false;
            }

            Done();
        }

        public override WorkerInstance Duplicate() => new ModelOpenWorker();

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref connection)) return;
            if (!DA.GetData("FdModel", ref model)) return;
            DA.GetData("RunNode", ref runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            DA.SetData("Connection", connection);
            DA.SetData("Success", success);
        }
    }
}