// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using GrasshopperAsyncComponent;

namespace FemDesign.Grasshopper
{
    public class PipeRunDesign : GH_AsyncComponent
    {
        public PipeRunDesign() : base("FEM-Design.RunDesign", "RunDesign", "Run design of model.\nDO NOT USE THE COMPONENT IF YOU WANT TO PERFORM ITERATIVE ANALYSIS (i.e. Galapos)", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationRunDesignWorker(this);
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Design", "Design", "Design.", GH_ParamAccess.item);
            pManager.AddGenericParameter("DesignGroup", "DesignGroup", "DesignGroup.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("Log", "Log", "", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_RunDesign;
        public override Guid ComponentGuid => new Guid("{920DB78D-34C5-43A8-918E-0A6951E5561D}");
        public override GH_Exposure Exposure => GH_Exposure.secondary;
        private class ApplicationRunDesignWorker : WorkerInstance
        {
            /* INPUT/OUTPUT */

            private FemDesignConnection _connection = null;
            private Calculate.Design _design = null;
            private List<Calculate.CmdDesignGroup> designGroups = new List<Calculate.CmdDesignGroup>();
            private bool _runNode = true;
            private bool _success = false;

            public ApplicationRunDesignWorker(GH_Component component) : base(component) { }

            public override void DoWork(Action<string, string> ReportProgress, Action Done)
            {
                try
                {
                    if (_runNode == false)
                    {
                        _success = false;
                        _connection = null;
                        RuntimeMessages.Add((GH_RuntimeMessageLevel.Warning, "Run node set to false."));
                        Done();
                        return;
                    }

                    if (_design == null)
                    {
                        _connection = null;
                        throw new Exception("Design is null.");
                    }

                    if (_connection == null)
                    {
                        _success = false;
                        RuntimeMessages.Add((GH_RuntimeMessageLevel.Warning, "Connection is null."));
                        Done();
                        return;
                    }

                    if (_connection.IsDisconnected)
                    {
                        _success = false;
                        _connection = null;
                        throw new Exception("Connection to FEM-Design have been lost.");
                    }

                    if (_connection.HasExited)
                    {
                        _success = false;
                        _connection = null;
                        throw new Exception("FEM-Design have been closed.");
                    }


                    _connection.OnOutput += onOutput;

                    // Run the Analysis
                    var _userModule = _design.Mode;

                    ReportProgress("", "");
                    _connection.RunDesign(_userModule, _design, designGroups);


                    if (_design.ApplyChanges == true)
                    {
                        _connection.ApplyDesignChanges();
                        RuntimeMessages.Add((GH_RuntimeMessageLevel.Remark, "'Apply changes' == true. Run a new analysis to validate your model against the new section sizes."));
                    }
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

            public override WorkerInstance Duplicate() => new ApplicationRunDesignWorker(Parent);

            public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
            {
                DA.GetData("Connection", ref _connection);
                DA.GetData("Design", ref _design);
                DA.GetDataList("DesignGroup", designGroups);
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