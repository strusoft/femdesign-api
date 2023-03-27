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
using System.Reflection;

namespace FemDesign.Grasshopper
{
    public class PipeCfg : GH_AsyncComponent
    {
        public PipeCfg() : base("FEM-Design.SetCfg", "SetCfg", "SetCfg of a model.", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationSetCfgWorker(this);
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("Cfg", "Cfg", "Cfg file path. If file path is not provided, the component will read the cfg.xml file in the grasshopper library folder.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_Config;
        public override Guid ComponentGuid => new Guid("{0A439F82-F10E-47D0-9EB6-406E33AECA9C}");
        public override GH_Exposure Exposure => GH_Exposure.obscure;
    }

    public class ApplicationSetCfgWorker : WorkerInstance
    {
        /* INPUT/OUTPUT */
        private FemDesignConnection _connection = null;
        private string _globalCfgPath = null;
        private bool _runNode = true;
        private bool _success = false;
        private string filePath = null;


        public ApplicationSetCfgWorker(GH_Component component) : base(component) { }


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

            // Run the Analysis

            if (filePath == null)
            {
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                filePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assemblyLocation), @"cfg.xml");
            }


            var _cfg = new Calculate.CmdConfig(filePath);
            _connection.SetConfig(_cfg);
            _success = true;

            Done();
        }

        public override WorkerInstance Duplicate() => new ApplicationSetCfgWorker(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref _connection)) return;
            DA.GetData("Cfg", ref _globalCfgPath);
            DA.GetData("RunNode", ref _runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            DA.SetData("Connection", _connection);
            DA.SetData("Success", _success);
        }
    }
}