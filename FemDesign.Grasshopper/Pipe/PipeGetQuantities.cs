﻿// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using System.Linq;
using System.Windows.Forms;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel.Special;
using GrasshopperAsyncComponent;

using System.Reflection;

using FemDesign;
using FemDesign.Calculate;

namespace FemDesign.Grasshopper
{
    public class PipeGetQuantities : GH_AsyncComponent
    {
        public PipeGetQuantities() : base(" FEM-Design.GetQuantities", "GetQuantities", "Get quantities from a model. .csv list files are saved in the same work directory as StruxmlPath.\nDO NOT USE THE COMPONENT IF YOU WANT TO PERFORM ITERATIVE ANALYSIS (i.e. Galapos)", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationGetQuantitiesWorker(this);
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter(
                "QuantityType", "QuantityType", 
                "Quantity type:\n\n" +
                nameof(ListProc.QuantityEstimationConcrete) + "\n" +
                nameof(ListProc.QuantityEstimationReinforcement) + "\n" +
                nameof(ListProc.QuantityEstimationSteel) + "\n" +
                nameof(ListProc.QuantityEstimationTimber) + "\n" +
                nameof(ListProc.QuantityEstimationTimberPanel) + "\n" +
                nameof(ListProc.QuantityEstimationMasonry) + "\n" +
                nameof(ListProc.QuantityEstimationGeneral) + "\n" +
                nameof(ListProc.QuantityEstimationProfiledPanel), GH_ParamAccess.item);
            pManager.AddGenericParameter("Units", "Units", "Specify the Result Units for some specific type. \n" +
                "Default Units are: Length.m, Angle.deg, SectionalData.m, Force.kN, Mass.kg, Displacement.m, Stress.Pa", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Quantities", "Quantities", "Quantities.", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_readresult;

        public override Guid ComponentGuid => new Guid("{81E32E19-C6A6-4E9E-A0B2-EB6CE1BA888F}");
        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }

    public class ApplicationGetQuantitiesWorker : WorkerInstance
    {
        public dynamic _getQuantities(Type resultType, Results.UnitResults units = null)
        {
            var methodName = nameof(FemDesign.FemDesignConnection._getQuantities);
            List<Results.IResult> mixedResults = new List<Results.IResult>();
            MethodInfo genericMethod = _connection.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(resultType);
            dynamic result = genericMethod.Invoke(_connection, new object[] { units, true });
            mixedResults.AddRange(result);
            return mixedResults;
        }


        /* INPUT/OUTPUT */
        public FemDesignConnection _connection = null;
        private Results.UnitResults _units = null;
        private string _resultType = null;

        private List<Results.IResult> _results = new List<Results.IResult>();
        private bool _runNode = true;
        private bool _success = false;

        private Verbosity _verbosity = Verbosity.Normal;

        public ApplicationGetQuantitiesWorker(GH_Component component) : base(component) { }

        public override void DoWork(Action<string, string> ReportProgress, Action Done)
        {
            try
            {
                if (_runNode == false)
                {
                    _success = false;
                    _connection = null;
                    RuntimeMessages.Add( (GH_RuntimeMessageLevel.Warning, "Run node set to false.") );
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
                var _type = $"FemDesign.Results.{_resultType}, FemDesign.Core";
                Type type = Type.GetType(_type);

                var res = _getQuantities(type, _units);
                _results.AddRange(res);
                _success = true;

                if (_results.Count == 0)
                {
                    RuntimeMessages.Add((GH_RuntimeMessageLevel.Warning, $"{_resultType} returns an empty list. Does your model contains any {_resultType.Substring("QuantityEstimation".Length)} elements?"));
                    Done();
                    return;
                }
            }
            catch (Exception ex)
            {
                RuntimeMessages.Add((GH_RuntimeMessageLevel.Error, ex.Message));
                _success = false;
                _connection = null;
            }

            Done();
        }

        public override WorkerInstance Duplicate() => new ApplicationGetQuantitiesWorker(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref _connection)) return;
            DA.GetData("QuantityType", ref _resultType);

            if (_resultType == null || _resultType == "1")
            {
                return;
            }

            DA.GetData("Units", ref _units);
            DA.GetData("RunNode", ref _runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            foreach (var (level, message) in RuntimeMessages)
            {
                Parent.AddRuntimeMessage(level, message);
            }

            DA.SetData("Connection", _connection);
            DA.SetDataList("Quantities", _results);
            DA.SetData("Success", _success);
        }
    }
}