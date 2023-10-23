// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using System.Linq;
using System.Windows.Forms;
using System.Reflection;
using GrasshopperAsyncComponent;
using FemDesign;
using FemDesign.Calculate;
using FemDesign.Results;

namespace FemDesign.Grasshopper
{
    public class PipeReadResults : GH_AsyncComponent
    {
        public PipeReadResults() : base(" FEM-Design.GetCaseCombResults", "CaseCombResults", "Read load cases and load combinations results from a model. .csv list files are saved in the same work directory as StruxmlPath.\nDO NOT USE THE COMPONENT IF YOU WANT TO PERFORM ITERATIVE ANALYSIS (i.e. Galapos)", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationReadResultWorker(this);
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("ResultType", "ResultType", "ResultType", GH_ParamAccess.item);
            pManager.AddTextParameter("Case Name", "Case Name", "Name of Load Case to return the results. Default will return the values for all load cases.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Combination Name", "Combo Name", "Name of Load Combination to return the results. Default will return the values for all load combinations.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Elements", "Elements", "Elements for which the results will be return. Default will return the values for all elements.\nWARNING:\nIf you specified 'Elements', Case/Combination will be overwritten and all load case and load combination will be returned.", GH_ParamAccess.list);
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

        public override Guid ComponentGuid => new Guid("{AC9B1E92-5C07-4F62-A0B3-E5E12254CD05}");
        public override GH_Exposure Exposure => GH_Exposure.tertiary;
        private class ApplicationReadResultWorker : WorkerInstance
        {
            public dynamic _getLoadCaseResults(Type resultType, string loadCase, Results.UnitResults units = null, Options options = null)
            {
                var method = nameof(FemDesign.FemDesignConnection.GetLoadCaseResults);
                List<Results.IResult> mixedResults = new List<Results.IResult>();
                MethodInfo genericMethod = _connection.GetType().GetMethod(method).MakeGenericMethod(resultType);
                dynamic result = genericMethod.Invoke(_connection, new object[] { loadCase, units, options });
                mixedResults.AddRange(result);
                return mixedResults;
            }

            public dynamic _getResults(Type resultType, Results.UnitResults units = null, Options options = null, List<FemDesign.GenericClasses.IStructureElement> elements = null)
            {
                var method = nameof(FemDesign.FemDesignConnection.GetResults);
                List<Results.IResult> mixedResults = new List<Results.IResult>();
                MethodInfo genericMethod = _connection.GetType().GetMethod(method).MakeGenericMethod(resultType);
                dynamic result = genericMethod.Invoke(_connection, new object[] { units, options, elements});
                mixedResults.AddRange(result);
                return mixedResults;
            }

            public dynamic _getLoadCombinationResults(Type resultType, string loadCombination, Results.UnitResults units = null, Options options = null)
            {
                var method = nameof(FemDesign.FemDesignConnection.GetLoadCombinationResults);
                List<Results.IResult> mixedResults = new List<Results.IResult>();
                MethodInfo genericMethod = _connection.GetType().GetMethod(method).MakeGenericMethod(resultType);
                dynamic result = genericMethod.Invoke(_connection, new object[] { loadCombination, units, options });
                mixedResults.AddRange(result);
                return mixedResults;
            }


            public FemDesignConnection _connection = null;
            private Calculate.Options _options = null;
            private Results.UnitResults _units = null;
            private string _resultType;
            private List<string> _case = new List<string>();
            private List<string> _combo = new List<string>();

            List<FemDesign.GenericClasses.IStructureElement> _elements = new List<GenericClasses.IStructureElement>();

            private List<Results.IResult> _results = new List<Results.IResult>();
            private bool _runNode = true;
            private bool _success = false;

            private Verbosity _verbosity = Verbosity.Normal;

            public ApplicationReadResultWorker(GH_Component component) : base(component) { }

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

                    // Run the Analysis
                    var _type = $"FemDesign.Results.{_resultType}, FemDesign.Core";
                    Type type = Type.GetType(_type);
                    if (type == null)
                        throw new ArgumentException($"Class object of name '{_type}' does not exist!");

                    if (!_combo.Any() && !_case.Any())
                    {


                        //if (types.Count != 0)
                        //{
                        //    int i = 0;
                        //    foreach (var type in types)
                        //    {
                        //        var res = _getResults(connection, type, units);
                        //        resultsTree.AddRange(res, new GH_Path(i));
                        //        i++;
                        //    }
                        //}


                        var res = _getResults(type, _units, _options, _elements);
                        _results.AddRange(res);
                    }

                    if (_case.Any())
                    {
                        foreach (var item in _case)
                        {
                            var res = _getLoadCaseResults(type, item, _units, _options);
                            _results.AddRange(res);
                        }
                    }

                    if (_combo.Any())
                    {
                        foreach (var item in _combo)
                        {
                            var res = _getLoadCombinationResults(type, item, _units, _options);
                            _results.AddRange(res);
                        }
                    }
                    _success = true;
                }
                catch(Exception ex)
                {
                    RuntimeMessages.Add(( GH_RuntimeMessageLevel.Error, ex.InnerException.Message ) );
                    _success = false;
                    _connection = null;
                }

                Done();
            }

            public override WorkerInstance Duplicate() => new ApplicationReadResultWorker(Parent);

            public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
            {
                if (!DA.GetData("Connection", ref _connection)) return;
                DA.GetData("ResultType", ref _resultType);
                DA.GetDataList("Case Name", _case);
                DA.GetDataList("Combination Name", _combo);
                DA.GetDataList("Elements", _elements);
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

}