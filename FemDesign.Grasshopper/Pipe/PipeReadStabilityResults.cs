// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using System.Linq;
using System.Windows.Forms;

using GrasshopperAsyncComponent;
using FemDesign.Calculate;
using System.Reflection;

namespace FemDesign.Grasshopper
{
    public class PipeStabilityResults : GH_AsyncComponent
    {
        public PipeStabilityResults() : base("FEM-Design.GetStabilityResults", "StabilityResults", "Read the stability results from a model. .csv list files are saved in the same work directory as StruxmlPath.\nDO NOT USE THE COMPONENT IF YOU WANT TO PERFORM ITERATIVE ANALYSIS (i.e. Galapos)", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationReadStabilityResultWorker(this);
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("Combination Name", "Combo Name", "Optional parameter. If not defined, all load combinations will be listed.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("ShapeId", "ShapeId", "Shape identifier must be larger than or equal to 1. Optional parameter. If not defined, all shapes will be listed.", GH_ParamAccess.item, 1);
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

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_readresult;

        public override Guid ComponentGuid => new Guid("{0FD2F66A-B989-4696-BDB4-E2BCDCBCB7C6}");
        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }

    public class ApplicationReadStabilityResultWorker : WorkerInstance
    {
        public dynamic _getStabilityResults(Type resultType, string loadCombination = null, int? shapeId = null, Results.UnitResults units = null, Options options = null)
        {
            MethodInfo genericMethod = _connection.GetType().GetMethod("GetStabilityResults").MakeGenericMethod(resultType);
            dynamic result = genericMethod.Invoke(_connection, new object[] { loadCombination, shapeId, units, options });
            
            return result;
        }


        /* INPUT/OUTPUT */
        public FemDesignConnection _connection = null;
        private Calculate.Options _options = null;
        private Results.UnitResults _units = null;
        private Type _resultType = typeof(FemDesign.Results.NodalBucklingShape);
        private string _combo = null;
        private int? _shapeId = null;

        private List<Results.IResult> _results = new List<Results.IResult>();
        private bool _runNode = true;
        private bool _success = false;

        private Verbosity _verbosity = Verbosity.Normal;

        public ApplicationReadStabilityResultWorker(GH_Component component) : base(component) { }

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

            _connection.SetVerbosity(_connection.Verbosity);
            _connection.OnOutput += onOutput;
            var res = _getStabilityResults(_resultType, _combo, _shapeId, _units, _options);
            _connection.OnOutput -= onOutput;
            _results.AddRange(res);

            _success = true;
            Done();
        }

        public override WorkerInstance Duplicate() => new ApplicationReadStabilityResultWorker(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref _connection)) return;
            DA.GetData("Combination Name", ref _combo);
            DA.GetData("ShapeId", ref _shapeId);
            DA.GetData("Units", ref _units);
            DA.GetData("Options", ref _options);
            DA.GetData("RunNode", ref _runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            foreach (var (level, message) in RuntimeMessages)
            {
                Parent.AddRuntimeMessage(level, message);
            }

            DA.SetData("Connection", _connection);
            DA.SetDataList("Results", _results);
            DA.SetData("Success", _success);
        }
    }
}