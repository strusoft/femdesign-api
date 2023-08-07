// https://strusoft.com/
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
                        
        public DataTree<FemDesign.Results.NodalBucklingShape> CreateResultTree(List<FemDesign.Results.NodalBucklingShape> results)
        {
            // create 2D data tree
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

            // sort and remove empty branches
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
            foreach (var item in emptyPath)
            {
                resultsTree.RemovePath(item);
            }

            return resultsTree;
        }

        public DataTree<FemDesign.Results.CriticalParameter> CreateResultTree(List<FemDesign.Results.CriticalParameter> results)
        {
            // create 1D data tree
            var uniqueCaseId = results.Select(x => x.CaseIdentifier).Distinct().ToList();
            DataTree<FemDesign.Results.CriticalParameter> resultsTree = new DataTree<FemDesign.Results.CriticalParameter>();

            for (int i = 0; i < uniqueCaseId.Count; i++)
            {
                var allResultsByCaseId = results.Where(r => r.CaseIdentifier == uniqueCaseId[i]).ToList();
                resultsTree.AddRange(allResultsByCaseId, new GH_Path(i));
            }

            return resultsTree;
        }

        public DataTree<FemDesign.Results.NodalBucklingShape> FilterTree(DataTree<FemDesign.Results.NodalBucklingShape> tree, List<string> loadCombinations = null, List<int> shapeIds = null)
        {
            var removable = new List<GH_Path>();
            DataTree<FemDesign.Results.NodalBucklingShape> filteredTree = tree;

            // sort and remove unnecessary branches 
            for (int i = 0; i < filteredTree.BranchCount; i++)
            {
                var path = filteredTree.Paths[i];
                var branch = filteredTree.Branches[i].ToList();

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
                filteredTree.RemovePath(item);
            }

            // renumber tree path
            if (removable.Any())
            {
                filteredTree = ReNumberTree(filteredTree);
            }

            return filteredTree;
        }

        public DataTree<FemDesign.Results.CriticalParameter> FilterTree(DataTree<FemDesign.Results.CriticalParameter> tree, List<string> loadCombinations = null, List<int> shapeIds = null)
        {
            var removable = new List<GH_Path>();
            DataTree<FemDesign.Results.CriticalParameter> filteredTree = tree;

            // sort and remove unnecessary branches 
            for (int i = 0; i < filteredTree.BranchCount; i++)
            {
                var path = filteredTree.Paths[i];
                var branch = filteredTree.Branches[i].ToList();

                if ((loadCombinations.Any()) && (!loadCombinations.Contains(branch[0].CaseIdentifier, StringComparer.OrdinalIgnoreCase)))
                {
                    removable.Add(path);
                }
                if (shapeIds.Any())
                {
                    for(int j = branch.Count - 1; j >= 0; j--)
                    {
                        if (!shapeIds.Contains(branch[j].Shape))
                        {
                            filteredTree.Branches[i].RemoveAt(j);
                        }
                    }
                }
            }
            foreach (var item in removable)
            {
                filteredTree.RemovePath(item);
            }

            // renumber tree path
            if (removable.Any())
            filteredTree.RenumberPaths();

            return filteredTree;
        }

        public DataTree<FemDesign.Results.NodalBucklingShape> ReNumberTree(DataTree<FemDesign.Results.NodalBucklingShape> tree)
        {
            DataTree<FemDesign.Results.NodalBucklingShape> orderedTree = new DataTree<FemDesign.Results.NodalBucklingShape>();
            int i = 0;
            int j = 0;
            
            orderedTree.AddRange(tree.Branches[0], new GH_Path(0,0));

            for (int b=1; b<tree.Branches.Count; b++)
            {
                var currentBranch = tree.Branches[b];
                var previousBranch = tree.Branches[b - 1];
                                
                if (currentBranch[0].CaseIdentifier != previousBranch[0].CaseIdentifier)
                {
                    i++;
                    j = 0;
                }
                else if (currentBranch[0].Shape != previousBranch[0].Shape)
                {
                    j++;
                }

                var path = new GH_Path(i,j);
                orderedTree.AddRange(currentBranch, path);
            }

            return orderedTree;
        }

        public bool CaseIdIsValid(List<FemDesign.Results.NodalBucklingShape> results, List<string> combos)
        {
            var caseIds = results.Select(x => x.CaseIdentifier).Distinct().ToList();
            foreach (var comb in combos)
            {
                if (!caseIds.Contains(comb, StringComparer.OrdinalIgnoreCase))
                    throw new ArgumentException($"Incorrect or unknown load combination name: {comb}.");
            }
            return true;
        }

        public bool ShapeIdIsValid(List<FemDesign.Results.NodalBucklingShape> results, List<int> shapes)
        {
            var shapeIds = results.Select(x => x.Shape).Distinct().ToList();
            foreach (var shape in shapes)
            {
                if (!shapeIds.Contains(shape))
                    throw new ArgumentException($"ShapeId {shape} is out of range.");
            }
            return true;
        }


        /* INPUT/OUTPUT */
        public FemDesignConnection _connection = null;
        private List<string> _combos = new List<string>();
        private List<int> _shapeIds = new List<int>();
        private Results.UnitResults _units = null;
        private Calculate.Options _options = null;
        private bool _runNode = true;
                
        private DataTree<FemDesign.Results.NodalBucklingShape> _bucklingTree = new DataTree<FemDesign.Results.NodalBucklingShape>();
        private DataTree<FemDesign.Results.CriticalParameter> _critParameterResults = new DataTree<FemDesign.Results.CriticalParameter>();
        private bool _success = false;

        private Type _resultType = typeof(FemDesign.Results.NodalBucklingShape);
        private Type _critParamType = typeof(FemDesign.Results.CriticalParameter);
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

                ReportProgress("", "");
                _success = true;


                // Get stability results

                // get buckling results
                List<FemDesign.Results.NodalBucklingShape> bucklingRes = _getStabilityResults(_resultType, null, null, _units, _options);

                // check validity of filter values
                CaseIdIsValid(bucklingRes, _combos);
                ShapeIdIsValid(bucklingRes, _shapeIds);

                // create tree from buckling results
                var bucklingTree = CreateResultTree(bucklingRes);
                _bucklingTree = FilterTree(bucklingTree, _combos, _shapeIds);

                //create tree from critical parameter results
                List<FemDesign.Results.CriticalParameter> critParamRes = _getStabilityResults(_critParamType, null, null, _units, _options);
                var critParamTree = CreateResultTree(critParamRes);
                _critParameterResults = FilterTree(critParamTree, _combos, _shapeIds);
                                
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
            DA.SetDataTree(2, _critParameterResults);
            DA.SetData(3, _success);
        }
    }
}