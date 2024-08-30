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
        public PipeSetGlobalCfg() : base("FEM-Design.SetGlobalConfigurations", "SetGlobalCfg", "Set global settings for a FEM-Design model using a global configuration file. It defines the calculation settings that will instruct FEM-Design in operation like creating the finite element mesh.", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationSetGlobalCfgWorker(this);
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddGenericParameter("GlobalConfig", "GlobCfg", "Filepath of the global configuration file or GlobConfig objects.\nIf file path is not provided, the component will read the cmdglobalcfg.xml file in the package manager library folder.\n%AppData%\\McNeel\\Rhinoceros\\packages\\7.0\\FemDesign\\", GH_ParamAccess.list);
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
        public override Guid ComponentGuid => new Guid("{FDEDD8F7-99CC-48F0-8FF8-2946921DC9F6}");
        public override GH_Exposure Exposure => GH_Exposure.obscure;
    }

    public class ApplicationSetGlobalCfgWorker : WorkerInstance
    {
        /* INPUT/OUTPUT */
        private FemDesignConnection _connection = null;
        private List<dynamic> _globCfg = new List<dynamic>();
        private bool _runNode = true;
        private bool _success = false;

        public ApplicationSetGlobalCfgWorker(GH_Component component) : base(component) { }


        public override void DoWork(Action<string, string> ReportProgress, Action Done)
        {
            try
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

                if (_globCfg.Count == 0)
                {
                    string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                    var _globCfgfilePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(assemblyLocation), @"cmdglobalcfg.xml");
                    _connection.SetConfig(_globCfgfilePath);
                }
                else
                {
                    foreach (var config in _globCfg)
                    {
                        // Check if the value is a string
                        if (config.Value is string filePath)
                        {
                            _connection.SetGlobalConfig(filePath);
                        }
                        // Check if the value is of type FemDesign.Calculate.CONFIG
                        else if (config.Value is FemDesign.Calculate.GlobConfig globConfig)
                        {
                            _connection.SetGlobalConfig(globConfig);
                        }
                    }
                }

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

        public override WorkerInstance Duplicate() => new ApplicationSetGlobalCfgWorker(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref _connection)) return;
            DA.GetDataList("GlobalConfig", _globCfg);
            DA.GetData("RunNode", ref _runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            foreach (var (level, message) in RuntimeMessages)
            {
                Parent.AddRuntimeMessage(level, message);
            }

            DA.SetData("Connection", _connection);
            DA.SetData("Success", _success);
        }
    }
}