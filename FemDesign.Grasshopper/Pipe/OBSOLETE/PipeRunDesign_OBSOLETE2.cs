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
using FemDesign.Materials;

namespace FemDesign.Grasshopper
{
    public class PipeRunDesign_OBSOLETE2 : GH_AsyncComponent
    {
        public PipeRunDesign_OBSOLETE2() : base("FEM-Design.RunDesign", "RunDesign", "Run design of model.\nDO NOT USE THE COMPONENT IF YOU WANT TO PERFORM ITERATIVE ANALYSIS (i.e. Galapos)", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationRunDesignWorker_OBSOLETE2(this);
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("Mode", "Mode", "Design mode: rc, steel or timber.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Design", "Design", "Design.", GH_ParamAccess.item);
            pManager.AddGenericParameter("DesignGroup", "DesignGroup", "DesignGroup.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_RunDesign;
        public override Guid ComponentGuid => new Guid("{1B0FB74E-B047-40B4-B9C1-89159860C188}");
        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }

    public class ApplicationRunDesignWorker_OBSOLETE2 : WorkerInstance
    {
        /* INPUT/OUTPUT */

        private FemDesignConnection _connection = null;
        private Calculate.Design _design = null;
        private List<Calculate.CmdDesignGroup> designGroups = new List<Calculate.CmdDesignGroup>();
        private string _mode = "Steel";
        private bool _runNode = true;
        private bool _success = false;

        public ApplicationRunDesignWorker_OBSOLETE2(GH_Component component) : base(component) { }

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
            var _userModule = FemDesign.GenericClasses.EnumParser.Parse<Calculate.CmdUserModule>(_mode);

            _connection.RunDesign(_userModule, _design, designGroups);
            _success = true;

            Done();
        }

        public override WorkerInstance Duplicate() => new ApplicationRunDesignWorker_OBSOLETE2(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref _connection)) return;
            if (!DA.GetData("Mode", ref _mode)) return;
            if (!DA.GetData("Design", ref _design)) return;
            if (!DA.GetDataList("DesignGroup", designGroups)) return;
            DA.GetData("RunNode", ref _runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            DA.SetData("Connection", _connection);
            DA.SetData("Success", _success);
        }
    }
}