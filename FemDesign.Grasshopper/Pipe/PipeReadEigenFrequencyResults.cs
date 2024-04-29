﻿// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using GrasshopperAsyncComponent;

using FemDesign.Calculate;
using FemDesign.Results.Utils;


namespace FemDesign.Grasshopper
{
    public class PipeEigenFrequencyResults : GH_AsyncComponent
    {
        public PipeEigenFrequencyResults() : base("FEM-Design.GetEigenfrequencyResults", "EigenfrequencyResults", "Read the eigenfrequency results from a model. .csv list files are saved in the same work directory as StruxmlPath.\nDO NOT USE THE COMPONENT IF YOU WANT TO PERFORM ITERATIVE ANALYSIS (i.e. Galapos)", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationReadEigenFrequencyResultWorker(this);
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("ShapeId", "ShapeId", "Vibration shape identifier must be greater or equal to 1. Optional parameter. " +
                "If not defined, all shapes will be listed.", GH_ParamAccess.list);
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
            pManager.AddGenericParameter("VibrationShapes", "Shapes", "Vibration shape results.", GH_ParamAccess.tree);
            pManager.AddGenericParameter("Eigenfrequencies", "Eigenfrequencies", "Eigenfrequency results.", GH_ParamAccess.tree);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_readresult;

        public override Guid ComponentGuid => new Guid("{285AA531-CFCF-43A7-95DC-6101D5F674B4}");
        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }

    public class ApplicationReadEigenFrequencyResultWorker : WorkerInstance
    {
        public dynamic _getResults(Type resultType, Results.UnitResults units = null, Options options = null)
        {
            string methodName = nameof(FemDesign.FemDesignConnection._getResults);
            MethodInfo genericMethod = _connection.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(resultType);
            dynamic results = genericMethod.Invoke(_connection, new object[] { units, options, null, true });

            return results;
        }

        /* INPUT/OUTPUT */
        public FemDesignConnection _connection = null;
        private List<int> _shapeIds = new List<int>();
        private Results.UnitResults _units = null;
        private Calculate.Options _options = null;
        private bool _runNode = true;

        private DataTree<FemDesign.Results.NodalVibration> _vibrationTree = new DataTree<FemDesign.Results.NodalVibration>();
        private DataTree<FemDesign.Results.EigenFrequencies> _frequencyTree = new DataTree<FemDesign.Results.EigenFrequencies>();
        private bool _success = false;

        private Type _vibrationType = typeof(FemDesign.Results.NodalVibration);
        private Type _frequencyType = typeof(FemDesign.Results.EigenFrequencies);
        private Verbosity _verbosity = Verbosity.Normal;
               

        public ApplicationReadEigenFrequencyResultWorker(GH_Component component) : base(component) { }

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

                ReportProgress("", "");
                _success = true;


                // get results
                var vibrationRes = (List<FemDesign.Results.NodalVibration>)this._getResults(_vibrationType, _units, _options);
                var frequencyRes = (List<FemDesign.Results.EigenFrequencies>)this._getResults(_frequencyType, _units, _options);

                if (vibrationRes.Count == 0 && frequencyRes.Count == 0)
                {
                    RuntimeMessages.Add((GH_RuntimeMessageLevel.Warning, "Eigenfrequencies results have not been found. Have you run the eigenfrequencies analysis?"));
                    _success = false;
                    _connection = null;
                    Done();
                    return;
                }


                // filter results by shape idetifier
                string vibPropName = nameof(FemDesign.Results.NodalVibration.ShapeId);
                string freqPropName = nameof(FemDesign.Results.EigenFrequencies.ShapeId);
                if (_shapeIds.Any())
                {
                    vibrationRes = vibrationRes.FilterResultsByShapeId(vibPropName, _shapeIds);
                    frequencyRes = frequencyRes.FilterResultsByShapeId(freqPropName, _shapeIds);
                }

                // create a tree
                _vibrationTree = vibrationRes.CreateResultTree(vibPropName);
                _frequencyTree = frequencyRes.CreateResultTree(freqPropName);

            }
            catch (Exception ex)
            {
                RuntimeMessages.Add((GH_RuntimeMessageLevel.Error, ex.Message));
                _success = false;
                _connection = null;
            }

            Done();
        }
        public override WorkerInstance Duplicate() => new ApplicationReadEigenFrequencyResultWorker(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData(0, ref _connection)) return;
            DA.GetDataList(1, _shapeIds);
            DA.GetData(2, ref _options);
            DA.GetData(3, ref _units);
            DA.GetData(4, ref _runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            foreach (var (level, message) in RuntimeMessages)
            {
                Parent.AddRuntimeMessage(level, message);
            }

            DA.SetData(0, _connection);
            DA.SetDataTree(1, _vibrationTree);
            DA.SetDataTree(2, _frequencyTree);
            DA.SetData(3, _success);
        }
    }
}