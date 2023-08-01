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
    public class PipeResultPoints : GH_AsyncComponent
    {
        public PipeResultPoints() : base("FEM-Design.CreateResPoints", "CreateResPoints", "Create result points.\nDO NOT USE THE COMPONENT IF YOU WANT TO PERFORM ITERATIVE ANALYSIS (i.e. Galapos)", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationCreateResultPoints(this);
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddGenericParameter("ResultPoints", "ResultPoints", "ResultPoints.", GH_ParamAccess.list);
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("Log", "Log", "", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_readresult;
        public override Guid ComponentGuid => new Guid("{6D03BF35-92DB-4207-99EA-91372E9E7A70}");
        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        private class ApplicationCreateResultPoints : WorkerInstance
        {
            /* INPUT/OUTPUT */
            private FemDesignConnection _connection = null;
            private List<FemDesign.AuxiliaryResults.ResultPoint> _resultPoint = new List<FemDesign.AuxiliaryResults.ResultPoint>();
            private bool _runNode = true;

            public ApplicationCreateResultPoints(GH_Component component) : base(component) { }


            public override void DoWork(Action<string, string> ReportProgress, Action Done)
            {
                try
                {
                    if (_runNode == false)
                    {
                        _success = false;
                        RuntimeMessages.Add((GH_RuntimeMessageLevel.Warning, "Run node set to false."));
                        Done();
                        return;
                    }

                    if (_resultPoint.Count() == 0)
                    {
                        RuntimeMessages.Add((GH_RuntimeMessageLevel.Warning, "ResultPoints is empty"));
                        Done();
                        return;
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

                    var resPoints = _resultPoint.Select(x => (CmdResultPoint)x).ToList();
                    _connection.CreateResultPoint(resPoints);

                    _connection.OnOutput -= onOutput;
                    _success = true;

                }
                catch (Exception ex)
                {
                    RuntimeMessages.Add((GH_RuntimeMessageLevel.Error, ex.Message));
                    _success = false;
                    _connection = null;
                }

                Done();
            }


            public override WorkerInstance Duplicate() => new ApplicationCreateResultPoints(Parent);

            public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
            {
                DA.GetData("Connection", ref _connection);
                DA.GetDataList("ResultPoints", _resultPoint);
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