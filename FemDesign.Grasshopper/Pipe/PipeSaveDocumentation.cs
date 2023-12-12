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
    public class PipeDocx : GH_AsyncComponent
    {
        public PipeDocx() : base("FEM-Design.Documentation", "SaveDocx", "Save documentation of a model.", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationSaveDocxWorker(this);
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("Docx", "Docx", "Docx file path for the documentation output.", GH_ParamAccess.item);
            pManager.AddTextParameter("Template", "Template", ".dsc file path.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.Docx;
        public override Guid ComponentGuid => new Guid("{48633F87-356E-4E10-AC86-189DAD5AD0B0}");
        public override GH_Exposure Exposure => GH_Exposure.primary;
    }

    public class ApplicationSaveDocxWorker : WorkerInstance
    {
        /* INPUT/OUTPUT */
        private FemDesignConnection _connection = null;
        private string _docxFilePath = null;
        private string _dscFilePath = null;
        private bool _runNode = true;
        private bool _success = false;
        private string filePath = null;


        public ApplicationSaveDocxWorker(GH_Component component) : base(component) { }


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

                if (_connection == null)
                {
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

                _connection.SaveDocx(_docxFilePath, _dscFilePath);
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

        public override WorkerInstance Duplicate() => new ApplicationSaveDocxWorker(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref _connection)) return;
            DA.GetData("Docx", ref _docxFilePath);
            DA.GetData("Template", ref _dscFilePath);
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