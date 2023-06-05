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
using FemDesign.Calculate;

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
            pManager.AddTextParameter("Log", "Log", "", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_RunAnalysis;
        public override Guid ComponentGuid => new Guid("{C8DF0C6F-4A9E-4AEF-A114-6932C3AB7820}");
        public override GH_Exposure Exposure => GH_Exposure.secondary;

        private class ApplicationRunAnalysisWorker : WorkerInstance
        {
            /* INPUT/OUTPUT */
            private FemDesignConnection _connection = null;
            private Calculate.Analysis _analysis = null;
            private bool _runNode = true;

            public ApplicationRunAnalysisWorker(GH_Component component) : base(component) { }


            public override void DoWork(Action<string, string> ReportProgress, Action Done)
            {
                try
                {
                    if (_runNode == false)
                    {
                        _success = false;
                        RuntimeMessages.Add( (GH_RuntimeMessageLevel.Warning, "Run node set to false.") );
                        Done();
                        return;
                    }

                    if (_analysis == null)
                    {
                        throw new Exception("Analysis is null.");
                    }

                    if (_connection == null)
                    {
                        RuntimeMessages.Add((GH_RuntimeMessageLevel.Warning, "Connection is null."));
                        Done();
                        return;
                    }

                    if (_connection.IsDisconnected)
                    {
                        _success = false;
                        throw new Exception("Connection to FEM-Design have been lost.");
                    }

                    if (_connection.HasExited)
                    {
                        _success = false;
                        throw new Exception("FEM-Design have been closed.");
                    }


                    _connection.SetVerbosity(_connection.Verbosity);
                    _connection.OnOutput += onOutput;

                    // Run the Analysis
                    ReportProgress("", "");

                    _connection.RunAnalysis(_analysis);

                    _connection.OnOutput -= onOutput;
                    _success = true;

                }
                catch (Exception ex)
                {
                    RuntimeMessages.Add( (GH_RuntimeMessageLevel.Error, ex.Message) );
                    _success = false;
                    _connection = null;
                }

                Done();
            }


            public override WorkerInstance Duplicate() => new ApplicationRunAnalysisWorker(Parent);

            public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
            {
                DA.GetData("Connection", ref _connection);
                DA.GetData("Analysis", ref _analysis);
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

}