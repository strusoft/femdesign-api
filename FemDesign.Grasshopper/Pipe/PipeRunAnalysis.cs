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
    public class PipeRunAnalysis : GH_AsyncComponent
    {
        public PipeRunAnalysis() : base("FEM-Design.RunAnalysis", "RunAnalysis", "Run analysis of model.\nDO NOT USE THE COMPONENT IF YOU WANT TO PERFORM ITERATIVE ANALYSIS (i.e. Galapos)", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationRunAnalysisWorker(this);
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
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_RunAnalysis;

        public override Guid ComponentGuid => new Guid("d74ac5fb-42ff-49de-977a-aa71849c73ea");
        public override GH_Exposure Exposure => GH_Exposure.secondary;
    }

    public class ApplicationRunAnalysisWorker : WorkerInstance
    {
        /* INPUT/OUTPUT */
        private FemDesignConnection _connection = null;
        private Calculate.Analysis _analysis = null;
        private bool _runNode = true;
        private bool _success = false;

        public ApplicationRunAnalysisWorker(GH_Component component) : base(component) { }


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

            void onOutput(string message)
            {
                // TODO: Maybe check for errors, warnings and important info and display to the users?
                //Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, message);

                if (message.StartsWith("Dialog message auto answered: "))
                    Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, message);


                // TODO: Report progress based on last message from FEM-Design
                double progress = -1.0;
                if (message.StartsWith("Starting script")) progress = 10.0;
                else if (message.StartsWith("Prepare mesh - Starting")) progress = 20.0;
                else if (message.StartsWith("Generate mesh - Starting")) progress = 30.0;
                else if (message.StartsWith("Generate mesh - Finishing")) progress = 40.0;
                else if (message.StartsWith("Prepare mesh - Finishing")) progress = 50.0;
                // ...
                else if (message.StartsWith("Dlg message ## #Total calculation time:")) progress = 100.0;


                if (progress < 0)
                    ReportProgress(Id, 0.0);
                else
                    ReportProgress(Id, progress);

                Rhino.RhinoApp.WriteLine(message);
            }

            _connection.SetVerbosity(_connection.Verbosity);
            _connection.OnOutput += onOutput;

            // Run the Analysis
            _connection.RunAnalysis(_analysis);
            _success = true;

            _connection.OnOutput -= onOutput;

            Done();
        }

        public override WorkerInstance Duplicate() => new ApplicationRunAnalysisWorker(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref _connection)) return;
            if (!DA.GetData("Analysis", ref _analysis)) return;
            DA.GetData("RunNode", ref _runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            DA.SetData("Connection", _connection);
            DA.SetData("Success", _success);
        }
    }
}