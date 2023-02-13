// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using System.Linq;
using System.Windows.Forms;

using GrasshopperAsyncComponent;

namespace FemDesign.Grasshopper
{
    public class PipeReadResults : GH_AsyncComponent
    {
        public PipeReadResults() : base("FEM-Design.GetResults", "GetResults", "Read Results from a model. .csv list files are saved in the same work directory as StruxmlPath.\nDO NOT USE THE COMPONENT IF YOU WANT TO PERFORM ITERATIVE ANALYSIS (i.e. Galapos)", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationReadResultWorker(this);
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("ResultType", "ResultType", "ResultType", GH_ParamAccess.item);
            pManager.AddTextParameter("Case Name", "Case Name", "Name of Load Case to return the results.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Combination Name", "Combo Name", "Name of Load Combination to return the results.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Options", "Options", "Settings for output location. Default is 'ByStep' and 'Vertices'", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Units", "Units", "Specify the Result Units for some specific type. \n" +
                "Default Units are: Length.m, Angle.deg, SectionalData.m, Force.kN, Mass.kg, Displacement.m, Stress.Pa", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Results", "Results", "Results.", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => base.Icon;
        public override Guid ComponentGuid => new Guid("{57A6F72C-8312-412B-A6F3-2D92F9BC0C1F}");
        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }

    public class ApplicationReadResultWorker : WorkerInstance
    {
        /* INPUT/OUTPUT */
        public FemDesignConnection _connection = null;
        private Calculate.Options _options = null;
        private Results.UnitResults _units = null;
        private string _resultType;
        private List<string> _case = new List<string>();
        private List<string> _combo = new List<string>();

        private List<Results.IResult> _results = new List<Results.IResult>();
        private bool _runNode = true;
        private bool _success = false;

        private Verbosity _verbosity = Verbosity.Normal;

        public ApplicationReadResultWorker(GH_Component component) : base(component) { }

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

            //if (!_connection.HasResult())
            //{
            //    _success = false;
            //    Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The open model does not contain any results!");
            //    return;
            //}

            if (!_combo.Any() && !_case.Any())
            {
                Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Input parameter Case Name or Combo Name failed to collect data.");
                return;
            }

            // Run the Analysis
            var _type = $"FemDesign.Results.{_resultType}, FemDesign.Core";
            Type type = Type.GetType(_type);

            if(_case.Any())
            {
                foreach(var item in _case)
                {
                    var res = _connection._getLoadCaseResults(type, item, _units, _options);
                    _results.AddRange(res);
                }
            }

            if (_combo.Any())
            {
                foreach(var item in _combo)
                {
                    var res = _connection._getLoadCombinationResults(type, item, _units, _options);
                    _results.AddRange(res);
                }
            }

            //if (!_combo.Any() && !_case.Any())
            //{
            //    var resCase = _connection._getAllLoadCaseResults(type, _units, _options);
            //    _results.AddRange(resCase);
                
            //    var resCombo = _connection._getAllLoadCombinationResults(type, _units, _options);
            //    _results.AddRange(resCombo);
            //}

            _success = true;
            Done();
        }

        public override WorkerInstance Duplicate() => new ApplicationReadResultWorker(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref _connection)) return;
            DA.GetData("ResultType", ref _resultType);
            DA.GetDataList("Case Name", _case);
            DA.GetDataList("Combination Name", _combo);
            DA.GetData("Units", ref _units);
            DA.GetData("Options", ref _options);
            DA.GetData("RunNode", ref _runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            DA.SetData("Connection", _connection);
            DA.SetDataList("Results", _results);
            DA.SetData("Success", _success);
        }
    }
}