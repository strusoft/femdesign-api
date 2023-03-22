// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using GrasshopperAsyncComponent;

namespace FemDesign.Grasshopper
{
    public class PipeGetModel : GH_AsyncComponent
    {
        public PipeGetModel() : base("FEM-Design.GetModel", "GetModel", "Get the current open Model in FEM-Design.", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new GetModelWorker();
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Model", "Model", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Success", "Success", "", GH_ParamAccess.item);
        }
        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_readresult;

        public override Guid ComponentGuid => new Guid("{F27FD051-B752-4C8B-B9E6-48DBC7E3ABAF}");
        public override GH_Exposure Exposure => GH_Exposure.primary;
    }

    /// <summary>
    /// https://github.com/specklesystems/GrasshopperAsyncComponent
    /// </summary>
    public class GetModelWorker : WorkerInstance
    {
        /* INPUT */
        Model model = null;
        FemDesignConnection connection = null;
        bool runNode = false;

        /* OUTPUT */
        bool success = false;

        public GetModelWorker() : base(null) { }

        public override void DoWork(Action<string, double> ReportProgress, Action Done)
        {
            //// ?? Check for task cancellation!
            //if (CancellationToken.IsCancellationRequested) return;

            if (runNode)
            {
                model = connection.GetModel();
                success = true;
            }
            else
            {
                success = false;
            }

            Done();
        }

        public override WorkerInstance Duplicate() => new GetModelWorker();

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref connection)) return;
            DA.GetData("RunNode", ref runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            DA.SetData("Connection", connection);
            DA.SetData("Model", model);
            DA.SetData("Success", success);
        }
    }
}