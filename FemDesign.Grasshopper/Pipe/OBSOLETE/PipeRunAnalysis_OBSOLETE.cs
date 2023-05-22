// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using System.Linq;
using System.Windows.Forms;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using GrasshopperAsyncComponent;

namespace FemDesign.Grasshopper
{
    public class PipeRunAnalysis_OBSOLETE : GH_AsyncComponent
    {
        public PipeRunAnalysis_OBSOLETE() : base("FEM-Design.RunAnalysis", "RunAnalysis", "Run analysis of model.\nDO NOT USE THE COMPONENT IF YOU WANT TO PERFORM ITERATIVE ANALYSIS (i.e. Galapos)", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationRunAnalysisWorker_OBSOLETE(this);
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Analysis", "Analysis", "Analysis.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("Log", "Log", "", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_RunAnalysis;

        public override Guid ComponentGuid => new Guid("d74ac5fb-42ff-49de-977a-aa71849c73ea");
        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }

    public class ApplicationRunAnalysisWorker_OBSOLETE : WorkerInstance
    {
        /* INPUT/OUTPUT */
        private FemDesignConnection _connection = null;
        private Calculate.Analysis _analysis = null;
        private bool _runNode = true;

        public ApplicationRunAnalysisWorker_OBSOLETE(GH_Component component) : base(component) { }


        public override void DoWork(Action<string, double> ReportProgress, Action Done)
        {

            if (_runNode == false)
            {
                _success = false;
                Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Run node set to false.");
                ReportProgress(Id, 0.0);
                return;
            }

            if (_connection == null)
            {
                _success = false;
                Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Connection is null.");
                return;
            }

            if (_connection.IsDisconnected)
            {
                _success = false;
                Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Connection to FEM-Design have been lost.");
                return;
            }

            if (_connection.HasExited)
            {
                _success = false;
                Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "FEM-Design have been closed.");
                return;
            }

            _connection.SetVerbosity(_connection.Verbosity);
            _connection.OnOutput += onOutput;

            // Run the Analysis
            _connection.RunAnalysis(_analysis);

            _connection.OnOutput -= onOutput;

            Done();
        }


        public override WorkerInstance Duplicate() => new ApplicationRunAnalysisWorker_OBSOLETE(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref _connection)) return;
            if (!DA.GetData("Analysis", ref _analysis)) return;
            DA.GetData("RunNode", ref _runNode);
        }


        public override void SetData(IGH_DataAccess DA)
        {
            foreach (var (level, message) in RuntimeMessages)
            {
                Parent.AddRuntimeMessage(level, message);
            }

            DA.SetData("Connection", _connection);
            DA.SetDataList("Log", _log);
            DA.SetData("Success", _success);
        }
    }
}