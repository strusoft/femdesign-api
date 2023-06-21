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
            pManager.AddTextParameter("Combination Name", "Combo Name", "Optional parameter. If not defined, all load combinations will be listed.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("ShapeId", "ShapeId", "Shape identifier must be greater or equal to 1. Optional parameter. If not defined, all shapes will be listed.", GH_ParamAccess.list);
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
            pManager.AddGenericParameter("BucklingShapes", "Shapes", "Buckling shape results.", GH_ParamAccess.tree);
            pManager.AddGenericParameter("CriticalParameter", "CritParam", "Critical parameters.", GH_ParamAccess.tree);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_readresult;

        public override Guid ComponentGuid => new Guid("{0FD2F66A-B989-4696-BDB4-E2BCDCBCB7C6}");
        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }

    public class ApplicationReadStabilityResultWorker : WorkerInstance
    {
        public DataTree<FemDesign.Results.NodalBucklingShape> _getTreeFromResults(List<FemDesign.Results.NodalBucklingShape> results)
        {
            //var orderedResults = new List<List<dynamic>>();
            var tree = new DataTree<FemDesign.Results.NodalBucklingShape>();
                                   
            List<FemDesign.Results.NodalBucklingShape> bucklingResults = results;
                
            var unicCombNames = bucklingResults.Select(r => r.CaseIdentifier).Distinct().ToList();
            var unicShapes = bucklingResults.Select(r => r.Shape).Distinct().ToList();

            for (int i = 0; i < unicCombNames.Count; i++)
            {
                for (int j = 0; j < unicShapes.Count; j++)
                {
                    //orderedResults[i][j] = bucklingResults.Where(r => ((r.CaseIdentifier == unicCombNames[i]) && (r.Shape == unicShapes[j]))).ToList();
                    var res = bucklingResults.Where(r => ((r.CaseIdentifier == unicCombNames[i]) && (r.Shape == unicShapes[j]))).ToList();
                    tree.AddRange(res, new GH_Path(i,j));
                }
            }
            return tree;
            
        }
        public DataTree<FemDesign.Results.CriticalParameter> _getTreeFromResults(List<FemDesign.Results.CriticalParameter> results)
        {
            //var orderedResults = new List<List<dynamic>>();
            var tree = new DataTree<FemDesign.Results.CriticalParameter>();

            List<FemDesign.Results.CriticalParameter> bucklingResults = results;

            var unicCombNames = bucklingResults.Select(r => r.CaseIdentifier).Distinct().ToList();
            var unicShapes = bucklingResults.Select(r => r.Shape).Distinct().ToList();

            for (int i = 0; i < unicCombNames.Count; i++)
            {
                for (int j = 0; j < unicShapes.Count; j++)
                {
                    //orderedResults[i][j] = bucklingResults.Where(r => ((r.CaseIdentifier == unicCombNames[i]) && (r.Shape == unicShapes[j]))).ToList();
                    var res = bucklingResults.Where(r => ((r.CaseIdentifier == unicCombNames[i]) && (r.Shape == unicShapes[j]))).ToList();
                    tree.AddRange(res, new GH_Path(i, j));
                }
            }
            return tree;

        }

        public List _getNodalBuckling(Type resultType, List<string> loadCombinations = null, List<int?> shapeIds = null, Results.UnitResults units = null, Options options = null)
        {
            var methodName = nameof(FemDesignConnection.GetStabilityResults);
            MethodInfo genericMethod = _connection.GetType().GetMethod(methodName).MakeGenericMethod(resultType);
            dynamic results = new List<dynamic>();

            if (loadCombinations.Count == 0)
            {
                if (shapeIds.Count == 0)
                {
                    dynamic res = genericMethod.Invoke(_connection, new object[] { null, null, units, options });
                    results.AddRange(res);
                }
                else
                {
                    foreach (var id in shapeIds)
                    {
                        dynamic res = genericMethod.Invoke(_connection, new object[] { null, id, units, options });
                        results.AddRange(res);
                    }
                }
            }
            else
            {
                if(shapeIds.Count == 0)
                {
                    foreach (var comb in loadCombinations)
                    {
                        dynamic res = genericMethod.Invoke(_connection, new object[] { comb, null, units, options });
                        results.AddRange(res);
                    }
                }
                else
                {
                    foreach (var comb in loadCombinations)
                    {
                        foreach (var id in shapeIds)
                        {
                            dynamic res = genericMethod.Invoke(_connection, new object[] { comb, id, units, options });
                            results.AddRange(res);
                        }
                    }
                }
            }

            return results;
        }

        /* INPUT/OUTPUT */
        public FemDesignConnection _connection = null;
        private Calculate.Options _options = null;
        private Results.UnitResults _units = null;
        private Type _bucklingResultType = typeof(FemDesign.Results.NodalBucklingShape);
        private Type _critParamType = typeof(FemDesign.Results.CriticalParameter);
        private List<string> _combos = new List<string>();
        private List<int?> _shapeIds = new List<int?>();

        //private List<Results.IResult> _bucklingResults = new List<Results.IResult>();
        //private List<Results.IResult> _critParams = new List<Results.IResult>();
        private DataTree<FemDesign.Results.NodalBucklingShape> _bucklingResultsTree = new DataTree<FemDesign.Results.NodalBucklingShape>();
        private DataTree<Results.CriticalParameter> _critParamsTree = new DataTree<FemDesign.Results.CriticalParameter>();
        private bool _runNode = true;
        private bool _success = false;

        private Verbosity _verbosity = Verbosity.Normal;

        public ApplicationReadStabilityResultWorker(GH_Component component) : base(component) { }

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


                var bucklingResults = new List<FemDesign.Results.NodalBucklingShape>();
                var critParameterResults = new List<FemDesign.Results.CriticalParameter>();

                try
                {
                    bucklingResults = _getStabilityResults(_bucklingResultType, _combos, _shapeIds, _units, _options);
                    _bucklingResultsTree = _getTreeFromResults(bucklingResults);

                    critParameterResults = _getStabilityResults(_critParamType, _combos, _shapeIds, _units, _options);
                    _critParamsTree = _getTreeFromResults(critParameterResults);
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

        public override WorkerInstance Duplicate() => new ApplicationReadStabilityResultWorker(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData(0, ref _connection)) return;
            DA.GetDataList(1, _combos);
            DA.GetDataList(2, _shapeIds);
            DA.GetData(3, ref _units);
            DA.GetData(4, ref _options);
            DA.GetData(5, ref _runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            foreach (var (level, message) in RuntimeMessages)
            {
                Parent.AddRuntimeMessage(level, message);
            }
                  
            DA.SetData(0, _connection);
            DA.SetDataTree(1, _bucklingResultsTree);
            DA.SetDataTree(2, _critParamsTree);
            DA.SetData(3, _success);
        }
    }
}