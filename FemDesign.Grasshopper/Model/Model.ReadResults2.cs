// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using System.Linq;
using System.Windows.Forms;

using GrasshopperAsyncComponent;

namespace FemDesign.Grasshopper
{
    public class ApplicationReadResult2 : GH_AsyncComponent
    {
        public ApplicationReadResult2() : base("Application.ReadResults", "ReadResults", "Read Results from a model. .csv list files and .docx documentation files are saved in the same work directory as StruxmlPath.", CategoryName.Name(), SubCategoryName.Cat7a())
        {
            BaseWorker = new ApplicationReadResultWorker(this);
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("ResultType", "ResultType", "ResultType", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Case/Combination Name", "Case/Comb Name", "Name of Load Case/Load Combination to return the results.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Units", "Units", "", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Options", "Options", "", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Results", "Results", "Results.", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => base.Icon;
        public override Guid ComponentGuid => new Guid("{57A6F72C-8312-412B-A6F3-2D92F9BC0C1F}");
        public override GH_Exposure Exposure => GH_Exposure.primary;
    }

    public class ApplicationReadResultWorker : WorkerInstance
    {
        /* INPUT/OUTPUT */
        private FemDesignConnection _connection = null;
        private Calculate.Options _options = null;
        private Results.UnitResults _units = null;
        private string _analysis;
        private List<Results.NodalDisplacement> _results = null;
        private bool _runNode = false;
        private bool _success = false;

        private Verbosity _verbosity = Verbosity.Normal;

        public ApplicationReadResultWorker(GH_Component component) : base(component) { }

        public override void DoWork(Action<string, double> ReportProgress, Action Done)
        {
            if (_runNode == false)
            {
                _success = false;
                Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Run node set to false.");
                ReportProgress(Id, 0.0);
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

                Rhino.RhinoApp.WriteLine(message);
            }

            _connection.SetVerbosity(_connection.Verbosity);
            _connection.OnOutput += onOutput;

            // Run the Analysis
            _results = _connection.GetResults<Results.NodalDisplacement>();
            _success = true;

            _connection.OnOutput -= onOutput;

            Done();
        }

        public override WorkerInstance Duplicate() => new ApplicationReadResultWorker(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref _connection)) return;
            DA.GetData("ResultType", ref _analysis);
            DA.GetData("Case/Combination Name", ref _analysis);
            DA.GetData("Units", ref _units);
            DA.GetData("Options", ref _options);
            DA.GetData("RunNode", ref _runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            DA.SetData("Connection", _connection);
            DA.SetDataList("Results", _results);
            DA.SetData("Success", _success);
        }
    }
}