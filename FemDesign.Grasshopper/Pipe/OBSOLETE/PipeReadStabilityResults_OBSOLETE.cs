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
    public class PipeStabilityResults_OBSOLETE : GH_AsyncComponent
    {
        public PipeStabilityResults_OBSOLETE() : base("FEM-Design.GetStabilityResults", "StabilityResults", "Read the stability results from a model. .csv list files are saved in the same work directory as StruxmlPath.\nDO NOT USE THE COMPONENT IF YOU WANT TO PERFORM ITERATIVE ANALYSIS (i.e. Galapos)", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationReadStabilityResultWorker_OBSOLETE(this);
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("Combination Name", "Combo Name", "Optional parameter. If not defined, all load combinations will be listed.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("ShapeId", "ShapeId", "Shape identifier must be greater or equal to 1. Optional parameter. If not defined, all shapes will be listed.", GH_ParamAccess.item);
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
            pManager.AddGenericParameter("BucklingShapes", "Shapes", "Buckling shape results.", GH_ParamAccess.list);
            pManager.AddGenericParameter("CriticalParameter", "CritParam", "Critical parameters.", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_readresult;

        public override Guid ComponentGuid => new Guid("{0FD2F66A-B989-4696-BDB4-E2BCDCBCB7C6}");
        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }

    public class ApplicationReadStabilityResultWorker_OBSOLETE : WorkerInstance
    {
        public dynamic _getStabilityResults(Type resultType, string loadCombination = null, int? shapeId = null, Results.UnitResults units = null, Options options = null)
        {
            var methodName = nameof(FemDesignConnection.GetStabilityResults);
            MethodInfo genericMethod = _connection.GetType().GetMethod(methodName).MakeGenericMethod(resultType);
            dynamic results = genericMethod.Invoke(_connection, new object[] { loadCombination, shapeId, units, options });

            return results;
        }

        /* INPUT/OUTPUT */
        public FemDesignConnection _connection = null;
        private string _combo = null;
        private int? _shapeId = null;
        private Results.UnitResults _units = null;
        private Calculate.Options _options = null;
        private bool _runNode = true;

        private dynamic _bucklingResults = new List<FemDesign.Results.IResult>();
        private dynamic _critParameterResults = new List<FemDesign.Results.IResult>();
        private bool _success = false;


        private Type _resultType = typeof(FemDesign.Results.NodalBucklingShape);
        private Type _critParamType = typeof(FemDesign.Results.CriticalParameter);
        private Verbosity _verbosity = Verbosity.Normal;


        public ApplicationReadStabilityResultWorker_OBSOLETE(GH_Component component) : base(component) { }

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

                // Run the Analysis

                ReportProgress("", "");

                try
                {
                    _bucklingResults = _getStabilityResults(_resultType, _combo, _shapeId, _units, _options);
                    _critParameterResults = _getStabilityResults(_critParamType, _combo, _shapeId, _units, _options);
                }
                catch (Exception ex)
                {
                    RuntimeMessages.Add((GH_RuntimeMessageLevel.Error, ex.InnerException.Message));
                    _success = false;
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
        public override WorkerInstance Duplicate() => new ApplicationReadStabilityResultWorker_OBSOLETE(Parent);

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

            DA.SetData(0, _connection);
            DA.SetDataList(1, _bucklingResults);
            DA.SetDataList(2, _critParameterResults);
            DA.SetData(3, _success);
        }
    }
}