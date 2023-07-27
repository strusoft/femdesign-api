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
            pManager.AddGenericParameter("CriticalParameter", "CritParam", "Critical parameters.", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_readresult;

        public override Guid ComponentGuid => new Guid("{D0BEDA49-8BF8-49AB-8784-CCD0F6422E88}");
        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }

    public class ApplicationReadStabilityResultWorker : WorkerInstance
    {
        public dynamic _getStabilityResults(Type resultType, string loadCombination = null, int? shapeId = null, Results.UnitResults units = null, Options options = null)
        {
            var methodName = nameof(FemDesignConnection.GetStabilityResults);
            MethodInfo genericMethod = _connection.GetType().GetMethod(methodName).MakeGenericMethod(resultType);
            dynamic results = genericMethod.Invoke(_connection, new object[] { loadCombination, shapeId, units, options });

            return results;
        }

        DataTree<dynamic> CreateFilteredTree(List<dynamic> results, List<string> loadCombinations = null, List<int> shapeIds = null)
        {
            DataTree<dynamic> filteredTree = new DataTree<dynamic>();

            for (int i = 0; i < results.Count; i++)
            {
                var elem = results[i];
                string elemCaseIdentifier = elem.Value.CaseIdentifier;
                int elemShape = elem.Value.Shape;

                if (loadCombinations.Contains(elemCaseIdentifier) && shapeIds.Contains(elemShape))
                {
                    int caseIndex = loadCombinations.IndexOf(elemCaseIdentifier);
                    int shapeIndex = shapeIds.IndexOf(elemShape);

                    filteredTree.Add(elem, new GH_Path(caseIndex, shapeIndex));
                }
            }
            return filteredTree;
        }

        //DataTree<dynamic> CreateResultsTree(List<dynamic> results, List<string> loadCombinations = null, List<int> shapeIds = null)
        //{
        //    var uniqueCaseId = results.Select(x => x.Value.CaseIdentifier).Distinct().ToList();
        //    var uniqueShape = results.Select(x => x.Value.Shape).Distinct().ToList();
        //    DataTree<dynamic> resultsTree = new DataTree<dynamic>();

        //    for (int i = 0; i < uniqueCaseId.Count; i++)
        //    {
        //        var allResultsByCaseId = results.Where(r => r.Value.CaseIdentifier == uniqueCaseId[i]).ToList();

        //        for (int j = 0; j < uniqueShape.Count; j++)
        //        {
        //            var pathData = allResultsByCaseId.Where(s => s.Value.Shape == uniqueShape[j]);

        //            if(loadCombinations.Count == 0 || shapeIds.Count == 0 || (loadCombinations.Contains(uniqueCaseId[i]) && shapeIds.Contains(uniqueShape[j])))
        //            {
        //                resultsTree.AddRange(pathData, new GH_Path(i, j));
        //            }
        //        }
        //    }
        //    return resultsTree;
        //}

        DataTree<FemDesign.Results.NodalBucklingShape> CreateResultsTree(List<FemDesign.Results.NodalBucklingShape> results, List<string> loadCombinations = null, List<int> shapeIds = null)
        {
            var uniqueCaseId = results.Select(x => x.CaseIdentifier).Distinct().ToList();
            var uniqueShape = results.Select(x => x.Shape).Distinct().ToList();
            DataTree<FemDesign.Results.NodalBucklingShape> resultsTree = new DataTree<FemDesign.Results.NodalBucklingShape>();

            for (int i = 0; i < uniqueCaseId.Count; i++)
            {
                var allResultsByCaseId = results.Where(r => r.CaseIdentifier == uniqueCaseId[i]).ToList();

                for (int j = 0; j < uniqueShape.Count; j++)
                {
                    var pathData = allResultsByCaseId.Where(s => s.Shape == uniqueShape[j]).ToList();

                    if (!loadCombinations.Any() || !shapeIds.Any())
                    {
                        resultsTree.AddRange(pathData, new GH_Path(i, j));
                    }
                    else if (loadCombinations.Contains(uniqueCaseId[i]) && shapeIds.Contains(uniqueShape[j]))
                    {
                        resultsTree.AddRange(pathData, new GH_Path(i, j));
                    }
                }
            }
            return resultsTree;
        }

        DataTree<FemDesign.Results.NodalBucklingShape> CreateAllResultsTree(List<FemDesign.Results.NodalBucklingShape> results)
        {
            var uniqueCaseId = results.Select(x => x.CaseIdentifier).Distinct().ToList();
            var uniqueShape = results.Select(x => x.Shape).Distinct().ToList();
            DataTree<FemDesign.Results.NodalBucklingShape> resultsTree = new DataTree<FemDesign.Results.NodalBucklingShape>();

            for (int i = 0; i < uniqueCaseId.Count; i++)
            {
                var allResultsByCaseId = results.Where(r => r.CaseIdentifier == uniqueCaseId[i]).ToList();

                for (int j = 0; j < uniqueShape.Count; j++)
                {
                    var pathData = allResultsByCaseId.Where(s => s.Shape == uniqueShape[j]).ToList();
                    resultsTree.AddRange(pathData, new GH_Path(i, j));
                }
            }

            // remove empty branches
            var emptyPath = new List<GH_Path>();
            for (int i = 0; i < resultsTree.BranchCount; i++)
            {
                var path = resultsTree.Paths[i];
                var branch = resultsTree.Branches[i];

                if (!branch.Any())
                {
                    emptyPath.Add(path);
                }
            }
            foreach(var item in emptyPath)
            {
                resultsTree.RemovePath(item);
            }

            return resultsTree;
        }

        DataTree<FemDesign.Results.NodalBucklingShape> FilterTree(DataTree<FemDesign.Results.NodalBucklingShape> tree, List<string> loadCombinations = null, List<int> shapeIds = null)
        {
            var removable = new List<GH_Path>();
            for (int i = 0; i < tree.BranchCount; i++)
            {
                var path = tree.Paths[i];
                var branch = tree.Branches[i].ToList();
                
                if ((loadCombinations.Any()) && (!loadCombinations.Contains(branch[0].CaseIdentifier, StringComparer.OrdinalIgnoreCase)))
                {
                    removable.Add(path);
                }
                if ((shapeIds.Any()) && (!shapeIds.Contains(branch[0].Shape)))
                {
                    removable.Add(path);
                }
            }
            foreach (var item in removable)
            {
                tree.RemovePath(item);
            }
            return tree;
        }





        /* INPUT/OUTPUT */
        public FemDesignConnection _connection = null;
        private List<string> _combos = new List<string>();
        private List<int> _shapeIds = new List<int>();
        private Results.UnitResults _units = null;
        private Calculate.Options _options = null;
        private bool _runNode = true;

        private DataTree<FemDesign.Results.NodalBucklingShape> _bucklingTree = new DataTree<FemDesign.Results.NodalBucklingShape>();
        private bool _success = false;


        private Type _resultType = typeof(FemDesign.Results.NodalBucklingShape);
        private Type _critParamType = typeof(FemDesign.Results.CriticalParameter);
        private Verbosity _verbosity = Verbosity.Normal;
        //
        //private dynamic _bucklingResults = new List<FemDesign.Results.IResult>();
        private dynamic _critParameterResults = new List<FemDesign.Results.IResult>();


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
                
                try
                {
                    List<FemDesign.Results.NodalBucklingShape> bucklingRes = _getStabilityResults(_resultType, null, null, _units, _options);
                    //_bucklingTree = CreateResultsTree(_bucklingResults, _combos, _shapeIds);
                    var tree = CreateAllResultsTree(bucklingRes);
                    _bucklingTree = FilterTree(tree, _combos, _shapeIds);

                    if (_combos.Any())
                    {
                        foreach(var comb in _combos)
                        {
                            if(_shapeIds.Any())
                            {
                                foreach(var id in _shapeIds)
                                {
                                    var res = _getStabilityResults(_critParamType, comb, id, _units, _options);
                                    _critParameterResults.AddRange(res);
                                }
                            }
                            else
                            {
                                var res = _getStabilityResults(_critParamType, comb, null, _units, _options);
                                _critParameterResults.AddRange(res);
                            }
                        }
                    }
                    else
                    {
                        if (_shapeIds.Any())
                        {
                            foreach (var id in _shapeIds)
                            {
                                var res = _getStabilityResults(_critParamType, null, id, _units, _options);
                                _critParameterResults.AddRange(res);
                            }
                        }
                        else
                        {
                            var res = _getStabilityResults(_critParamType, null, null, _units, _options);
                            _critParameterResults.AddRange(res);
                        }
                    }
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
            DA.SetDataTree(1, _bucklingTree);
            DA.SetDataList(2, _critParameterResults);
            DA.SetData(3, _success);
        }
    }
}