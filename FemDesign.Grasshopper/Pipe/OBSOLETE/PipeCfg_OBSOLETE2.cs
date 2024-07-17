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
    public class PipeCfg_OBSOLETE2 : GH_AsyncComponent
    {
        public PipeCfg_OBSOLETE2() : base("FEM-Design.SetConfigurations", "SetCfg", "Set design settings for a FEM-Design model using a configuration file.", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationSetCfgWorker_OBSOLETE2(this);
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("Cfg", "Cfg", "Filepath of the configuration file. If file path is not provided, the component will read the cfg.xml file in the package manager library folder.\n%AppData%\\McNeel\\Rhinoceros\\packages\\7.0\\FemDesign\\", GH_ParamAccess.item);
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
        public override Guid ComponentGuid => new Guid("{D6D91401-2842-454C-A43D-8FA483378F85}");
        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }

    public class ApplicationSetCfgWorker_OBSOLETE2 : WorkerInstance
    {
        /* INPUT/OUTPUT */
        private FemDesignConnection _connection = null;
        private bool _runNode = true;
        private bool _success = false;
        private string _cfgfilePath = null;


        public ApplicationSetCfgWorker_OBSOLETE2(GH_Component component) : base(component) { }


        public override void DoWork(Action<string, string> ReportProgress, Action Done)
        {
            if (_runNode == false)
            {
                _success = false;
                Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Run node set to false.");
                ReportProgress(Id, 0.0.ToString());
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

            if (_cfgfilePath == null)
            {
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                _cfgfilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assemblyLocation), @"cfg.xml");
            }

            _connection.SetConfig(_cfgfilePath);
            _success = true;

            Done();
        }

        public override WorkerInstance Duplicate() => new ApplicationSetCfgWorker_OBSOLETE2(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref _connection)) return;
            DA.GetData("Cfg", ref _cfgfilePath);
            DA.GetData("RunNode", ref _runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            DA.SetData("Connection", _connection);
            DA.SetData("Success", _success);
        }
    }
}