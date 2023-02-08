// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Grasshopper.Kernel.Data;
using System.Linq;
using System.Windows.Forms;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using GrasshopperAsyncComponent;

namespace FemDesign.Grasshopper
{
    public class PipeRunDesign : GH_AsyncComponent
    {
        public PipeRunDesign() : base("FEM-Design.RunDesign", "RunDesign", "Run design of model.", CategoryName.Name(), SubCategoryName.Cat7())
        {
            BaseWorker = new ApplicationRunDesignWorker(this);
        }

        protected override void ExpireDownStreamObjects()
        {
            foreach (IGH_Param item in Params.Output)
            {
                item.ExpireSolution(recompute: false);
            }
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("Mode", "Mode", "Design mode: rc, steel or timber.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Design", "Design", "Design.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 0, Enum.GetNames(typeof(FemDesign.Calculate.CmdUserModule)).ToList(), null, GH_ValueListMode.DropDown);
        }

        protected override System.Drawing.Bitmap Icon => base.Icon;
        public override Guid ComponentGuid => new Guid("{A4EBF6E6-14DA-4082-A19E-9E06FA956481}");
        public override GH_Exposure Exposure => GH_Exposure.secondary;
    }

    public class ApplicationRunDesignWorker : WorkerInstance
    {
        /* INPUT/OUTPUT */
        private FemDesignConnection _connection = null;
        private Calculate.Design _design = null;
        private string _mode = "Steel";
        private bool _runNode = true;
        private bool _success = false;

        public ApplicationRunDesignWorker(GH_Component component) : base(component) { }

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
                else
                    ReportProgress(Id, progress);

                Rhino.RhinoApp.WriteLine(message);
            }

            // Run the Analysis
            var _userModule = FemDesign.GenericClasses.EnumParser.Parse<Calculate.CmdUserModule>(_mode);
            _connection.RunDesign( _userModule , _design);
            _success = true;

            Done();
        }

        public override WorkerInstance Duplicate() => new ApplicationRunDesignWorker(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref _connection)) return;
            if (!DA.GetData("Mode", ref _mode)) return;
            if (!DA.GetData("Design", ref _design)) return;
            DA.GetData("RunNode", ref _runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            DA.SetData("Connection", _connection);
            DA.SetData("Success", _success);
        }
    }
}