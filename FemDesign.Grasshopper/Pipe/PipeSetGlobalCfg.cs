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
    public class PipeSetGlobalCfg : GH_AsyncComponent
    {
        public PipeSetGlobalCfg() : base("FEM-Design.SetGlobalCfg", "SetGlobalCfg", "SetGlobalCfg of a model.", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationSetGlobalCfgWorker(this);
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("GlobalCfg", "GlobalCfg", "GlobalCfg file path. If file path is not provided, the component will read the cmdglobalcfg.xml file in the grasshopper library folder.", GH_ParamAccess.item);
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
        public override Guid ComponentGuid => new Guid("{58B5D154-A7AB-4D05-878D-010BF05EA7D6}");
        public override GH_Exposure Exposure => GH_Exposure.obscure;
    }

    public class ApplicationSetGlobalCfgWorker : WorkerInstance
    {
        /* INPUT/OUTPUT */
        private FemDesignConnection _connection = null;
        private string _globalCfgPath = null;
        private bool _runNode = true;
        private bool _success = false;

        public ApplicationSetGlobalCfgWorker(GH_Component component) : base(component) { }


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

            if (_globalCfgPath == null)
            {
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                _globalCfgPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assemblyLocation), @"cmdglobalcfg.xml");
            }

            var _globalCfg = Calculate.CmdGlobalCfg.DeserializeCmdGlobalCfgFromFilePath(_globalCfgPath);
            _connection.SetGlobalConfig(_globalCfg);
            _success = true;

            Done();
        }

        public override WorkerInstance Duplicate() => new ApplicationSetGlobalCfgWorker(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref _connection)) return;
            DA.GetData("GlobalCfg", ref _globalCfgPath);
            DA.GetData("RunNode", ref _runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            DA.SetData("Connection", _connection);
            DA.SetData("Success", _success);
        }
    }
}