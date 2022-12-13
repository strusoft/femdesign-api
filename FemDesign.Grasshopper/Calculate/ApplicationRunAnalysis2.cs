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
    public class ApplicationRunAnalysis2 : GH_AsyncComponent
    {
        public ApplicationRunAnalysis2() : base("Application.RunAnalysis", "RunAnalysis", "Run analysis of model. .csv list files and .docx documentation files are saved in the same work directory as StruxmlPath.", CategoryName.Name(), SubCategoryName.Cat7a())
        {
            BaseWorker = new ApplicationRunAnalysisWorker(this);
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Analysis", "Analysis", "Analysis.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            //pManager.Register_GenericParam("FdFeaModel", "FdFeaModel", "FemDesign Finite Element Geometries(nodes, bars, shells).");
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => base.Icon;
        public override Guid ComponentGuid => new Guid("d74ac5fb-42ff-49de-977a-aa71849c73ea");
        public override GH_Exposure Exposure => GH_Exposure.primary;

        //public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        //{
        //    base.AppendAdditionalMenuItems(menu);
        //    Menu_AppendItem(menu, "Cancel the analysis (if possible)", (s, e) =>
        //    {
        //        RequestCancellation();
        //        this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Analysis could not be cancelled.");
        //    });
        //}
    }

    public class ApplicationRunAnalysisWorker : WorkerInstance
    {
        /* INPUT/OUTPUT */
        private FemDesignConnection _connection = null;
        private Calculate.Analysis _analysis = null;
        private bool _runNode = false;
        private bool _success = false;

        private Verbosity _verbosity = Verbosity.Normal;

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
            //DA.GetData("FdFeaModel", ref connection);
            DA.SetData("Success", _success);
        }
    }
}
