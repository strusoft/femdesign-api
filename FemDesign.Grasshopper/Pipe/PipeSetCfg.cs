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
using System.Dynamic;

namespace FemDesign.Grasshopper
{
    public class PipeSetCfg : GH_AsyncComponent
    {
        public PipeSetCfg() : base("FEM-Design.SetConfigurations", "SetCfg", "Set design settings for a FEM-Design model using a configuration file.", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationSetCfgWorker(this);
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Cfg", "Cfg", "Filepath of the configuration file or Config objects.\nIf file path is not provided, the component will read the cfg.xml file in the package manager library folder.\n%AppData%\\McNeel\\Rhinoceros\\packages\\7.0\\FemDesign\\", GH_ParamAccess.list);
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
        public override Guid ComponentGuid => new Guid("{AADEF422-856D-4798-9936-125B614F1D8C}");
        public override GH_Exposure Exposure => GH_Exposure.obscure;
    }

    public class ApplicationSetCfgWorker : WorkerInstance
    {
        /* INPUT/OUTPUT */
        private FemDesignConnection _connection = null;
        private bool _runNode = true;
        private bool _success = false;
        private List<dynamic> _cfg = new List<dynamic>();


        public ApplicationSetCfgWorker(GH_Component component) : base(component) { }


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

            if (_cfg.Count == 0)
            {
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                var _cfgfilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assemblyLocation), @"cfg.xml");
                _connection.SetConfig(_cfgfilePath);
            }
            else
            {
                foreach (var cfg in _cfg)
                {
                    // Check if the value is a string
                    if (cfg.Value is string filePath)
                    {
                        _connection.SetConfig(filePath);
                    }
                    // Check if the value is of type FemDesign.Calculate.CONFIG
                    else if (cfg.Value is FemDesign.Calculate.CONFIG config)
                    {
                        _connection.SetConfig(config);
                    }
                }
            }
            _success = true;

            Done();
        }

        public override WorkerInstance Duplicate() => new ApplicationSetCfgWorker(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref _connection)) return;
            DA.GetDataList("Cfg", _cfg);
            DA.GetData("RunNode", ref _runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            DA.SetData("Connection", _connection);
            DA.SetData("Success", _success);
        }
    }
}