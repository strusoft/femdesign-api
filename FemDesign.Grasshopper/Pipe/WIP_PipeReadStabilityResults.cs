//// https://strusoft.com/
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Windows.Forms;
//using System.Reflection;

//using Grasshopper;
//using Grasshopper.Kernel;
//using Grasshopper.Kernel.Data;
//using GrasshopperAsyncComponent;

//using FemDesign.Calculate;
//using FemDesign.Results.Utils;


//namespace FemDesign.Grasshopper
//{
//    public class PipeStabilityResults : GH_AsyncComponent
//    {
//        public PipeStabilityResults() : base("FEM-Design.GetStabilityResults", "StabilityResults", "Read the stability results from a model. .csv list files are saved in the same work directory as StruxmlPath.\nDO NOT USE THE COMPONENT IF YOU WANT TO PERFORM ITERATIVE ANALYSIS (i.e. Galapos)", CategoryName.Name(), SubCategoryName.Cat8())
//        {
//            BaseWorker = new ApplicationReadStabilityResultWorker(this);
//        }
//        protected override void RegisterInputParams(GH_InputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
//            pManager.AddTextParameter("Combination Name", "Combo Name", "Optional parameter. If not defined, all load combinations will be listed.", GH_ParamAccess.list);
//            pManager[pManager.ParamCount - 1].Optional = true;
//            pManager.AddIntegerParameter("ShapeId", "ShapeId", "Buckling shape identifier must be greater or equal to 1. Optional parameter. If not defined, all shapes will be listed.", GH_ParamAccess.list);
//            pManager[pManager.ParamCount - 1].Optional = true;
//            pManager.AddGenericParameter("Elements", "Elements", "Elements for which the results will be return. Default will return the values for all elements.", GH_ParamAccess.list);
//            pManager[pManager.ParamCount - 1].Optional = true;
//            pManager.AddGenericParameter("Options", "Options", "Settings for output location. Default is 'ByStep' and 'Vertices'", GH_ParamAccess.item);
//            pManager[pManager.ParamCount - 1].Optional = true;
//            pManager.AddGenericParameter("Units", "Units", "Specify the Result Units for some specific type. \n" +
//                "Default Units are: Length.m, Angle.deg, SectionalData.m, Force.kN, Mass.kg, Displacement.m, Stress.Pa", GH_ParamAccess.item);
//            pManager[pManager.ParamCount - 1].Optional = true;
//            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
//            pManager[pManager.ParamCount - 1].Optional = true;
//        }
//        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
//        {
//            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
//            pManager.AddGenericParameter("BucklingShapes", "Shapes", "Buckling shape results.", GH_ParamAccess.tree);
//            pManager.AddGenericParameter("CriticalParameter", "CritParam", "Critical parameters.", GH_ParamAccess.tree);
//            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
//        }

//        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_readresult;

//        public override Guid ComponentGuid => new Guid("{3D04EE79-E067-431B-94BF-8289809410F1}");
//        public override GH_Exposure Exposure => GH_Exposure.tertiary;
//    }

//    public class ApplicationReadStabilityResultWorker : WorkerInstance
//    {
//        /* METHODS */

//        public dynamic _getStabilityResults(Type resultType, string loadCombination = null, int? shapeId = null, Results.UnitResults units = null, Options options = null)
//        {
//            var methodName = nameof(FemDesignConnection.GetStabilityResults);
//            MethodInfo genericMethod = _connection.GetType().GetMethod(methodName).MakeGenericMethod(resultType);
//            dynamic results = genericMethod.Invoke(_connection, new object[] { loadCombination, shapeId, units, options });

//            return results;
//        }
//        public dynamic _getEigenResults(Type resultType, List<string> loadCombination, List<int> shapeId, List<FemDesign.GenericClasses.IStructureElement> elements, Results.UnitResults units = null, Options options = null)
//        {
//            var methodName = nameof(FemDesignConnection.GetEigenResults);
//            MethodInfo genericMethod = _connection.GetType().GetMethod(methodName).MakeGenericMethod(resultType);
//            dynamic results = genericMethod.Invoke(_connection, new object[] { loadCombination, shapeId, elements, units, options });

//            return results;
//        }

//        private DataTree<FemDesign.Results.NodalBucklingShape> CreateResultTree(List<FemDesign.Results.NodalBucklingShape> results)
//        {
//            // create 2D data tree
//            var uniqueCaseId = results.Select(x => x.CaseIdentifier).Distinct().ToList();
//            var uniqueShape = results.Select(x => x.Shape).Distinct().ToList();
//            DataTree<FemDesign.Results.NodalBucklingShape> resultsTree = new DataTree<FemDesign.Results.NodalBucklingShape>();

//            for (int i = 0; i < uniqueCaseId.Count; i++)
//            {
//                var allResultsByCaseId = results.Where(r => r.CaseIdentifier == uniqueCaseId[i]).ToList();

//                for (int j = 0; j < uniqueShape.Count; j++)
//                {
//                    var pathData = allResultsByCaseId.Where(s => s.Shape == uniqueShape[j]).ToList();
//                    resultsTree.AddRange(pathData, new GH_Path(i, j));
//                }
//            }

//            // sort and remove empty branches
//            var emptyPath = new List<GH_Path>();
//            for (int i = 0; i < resultsTree.BranchCount; i++)
//            {
//                var path = resultsTree.Paths[i];
//                var branch = resultsTree.Branches[i];

//                if (!branch.Any())
//                {
//                    emptyPath.Add(path);
//                }
//            }
//            foreach (var item in emptyPath)
//            {
//                resultsTree.RemovePath(item);
//            }

//            return resultsTree;
//        }

//        private DataTree<FemDesign.Results.NodalBucklingShape> FilterTree(DataTree<FemDesign.Results.NodalBucklingShape> tree, List<string> loadCombinations = null, List<int> shapeIds = null)
//        {
//            var removable = new List<GH_Path>();
//            DataTree<FemDesign.Results.NodalBucklingShape> filteredTree = tree;

//            // sort and remove unnecessary branches 
//            for (int i = 0; i < filteredTree.BranchCount; i++)
//            {
//                var path = filteredTree.Paths[i];
//                var branch = filteredTree.Branches[i].ToList();

//                if ((loadCombinations.Any()) && (!loadCombinations.Contains(branch[0].CaseIdentifier, StringComparer.OrdinalIgnoreCase)))
//                {
//                    removable.Add(path);
//                }
//                if ((shapeIds.Any()) && (!shapeIds.Contains(branch[0].Shape)))
//                {
//                    removable.Add(path);
//                }
//            }
//            foreach (var item in removable)
//            {
//                filteredTree.RemovePath(item);
//            }

//            // renumber tree path
//            if (removable.Any())
//            {
//                filteredTree = ReNumberTree(filteredTree);
//            }

//            return filteredTree;
//        }

//        private DataTree<FemDesign.Results.CriticalParameter> FilterTree(DataTree<FemDesign.Results.CriticalParameter> tree, List<string> loadCombinations = null, List<int> shapeIds = null)
//        {
//            var removable = new List<GH_Path>();
//            DataTree<FemDesign.Results.CriticalParameter> filteredTree = tree;

//            // sort and remove unnecessary branches 
//            for (int i = 0; i < filteredTree.BranchCount; i++)
//            {
//                var path = filteredTree.Paths[i];
//                var branch = filteredTree.Branches[i].ToList();

//                if ((loadCombinations.Any()) && (!loadCombinations.Contains(branch[0].CaseIdentifier, StringComparer.OrdinalIgnoreCase)))
//                {
//                    removable.Add(path);
//                }
//                if (shapeIds.Any())
//                {
//                    for(int j = branch.Count - 1; j >= 0; j--)
//                    {
//                        if (!shapeIds.Contains(branch[j].Shape))
//                        {
//                            filteredTree.Branches[i].RemoveAt(j);
//                        }
//                    }
//                }
//            }
//            foreach (var item in removable)
//            {
//                filteredTree.RemovePath(item);
//            }

//            // renumber tree path
//            if (removable.Any())
//            filteredTree.RenumberPaths();

//            return filteredTree;
//        }

//        private DataTree<FemDesign.Results.NodalBucklingShape> ReNumberTree(DataTree<FemDesign.Results.NodalBucklingShape> tree)
//        {
//            DataTree<FemDesign.Results.NodalBucklingShape> orderedTree = new DataTree<FemDesign.Results.NodalBucklingShape>();
//            int i = 0;
//            int j = 0;
            
//            orderedTree.AddRange(tree.Branches[0], new GH_Path(0,0));

//            for (int b=1; b<tree.Branches.Count; b++)
//            {
//                var currentBranch = tree.Branches[b];
//                var previousBranch = tree.Branches[b - 1];
                                
//                if (currentBranch[0].CaseIdentifier != previousBranch[0].CaseIdentifier)
//                {
//                    i++;
//                    j = 0;
//                }
//                else if (currentBranch[0].Shape != previousBranch[0].Shape)
//                {
//                    j++;
//                }

//                var path = new GH_Path(i,j);
//                orderedTree.AddRange(currentBranch, path);
//            }

//            return orderedTree;
//        }

//        private bool CaseIdIsValid(List<FemDesign.Results.NodalBucklingShape> results, List<string> combos)
//        {
//            var caseIds = results.Select(x => x.CaseIdentifier).Distinct().ToList();
//            foreach (var comb in combos)
//            {
//                if (!caseIds.Contains(comb, StringComparer.OrdinalIgnoreCase))
//                    throw new ArgumentException($"Incorrect or unknown load combination name: {comb}.");
//            }
//            return true;
//        }

//        private bool ShapeIdIsValid(List<FemDesign.Results.NodalBucklingShape> results, List<int> shapes)
//        {
//            var shapeIds = results.Select(x => x.Shape).Distinct().ToList();
//            foreach (var shape in shapes)
//            {
//                if (!shapeIds.Contains(shape))
//                    throw new ArgumentException($"ShapeId {shape} is out of range.");
//            }
//            return true;
//        }

//        private IEnumerable<T> CheckInput<T>(IEnumerable<T> input, IEnumerable<T> comparer)
//        {
//            if (input.Any())
//            {
//                IEnumerable<T> validItems;
//                IEnumerable<T> invalidItems;
//                string message;

//                //if (input.GetType() == typeof(string))
//                if (typeof(T) == typeof(string))
//                {
//                    validItems = (IEnumerable<T>)Enumerable.Intersect((IEnumerable<string>)input, (IEnumerable<string>)comparer, StringComparer.OrdinalIgnoreCase);
//                    invalidItems = (IEnumerable<T>)Enumerable.Except((IEnumerable<string>)input, (IEnumerable<string>)comparer, StringComparer.OrdinalIgnoreCase);
//                    message = $"Stability results are not available for the following load combinations: {string.Join(", ", invalidItems)}.";
//                }
//                else 
//                {
//                    validItems = input.Intersect(comparer);
//                    invalidItems = input.Except(comparer);
//                    message = $"Stability results are not available for the following shape identifiers: {string.Join(", ", invalidItems)}.";
//                }
                
//                if(validItems.Count() == 0)
//                {
//                    throw new ArgumentException("Stability results are not available for the specified load combinations.");
//                }
//                if(invalidItems.Any())
//                {
//                    RuntimeMessages.Add((GH_RuntimeMessageLevel.Warning, message));
//                }

//                return validItems;
//            }
//            else
//            {
//                return comparer;
//            }
//        }


//        /* INPUT/OUTPUT */
//        public FemDesignConnection _connection = null;
//        private List<string> _combos = new List<string>();
//        private List<int> _shapeIds = new List<int>();
//        private List<FemDesign.GenericClasses.IStructureElement> _elements = new List<FemDesign.GenericClasses.IStructureElement>();
//        private Results.UnitResults _units = null;
//        private Calculate.Options _options = null;
//        private bool _runNode = true;
                
//        private DataTree<FemDesign.Results.NodalBucklingShape> _bucklingTree = new DataTree<FemDesign.Results.NodalBucklingShape>();
//        private DataTree<FemDesign.Results.CriticalParameter> _critParameterResults = new DataTree<FemDesign.Results.CriticalParameter>();
//        private bool _success = false;

//        private Type _resultType = typeof(FemDesign.Results.NodalBucklingShape);
//        private Type _critParamType = typeof(FemDesign.Results.CriticalParameter);
//        private Verbosity _verbosity = Verbosity.Normal;
               

//        public ApplicationReadStabilityResultWorker(GH_Component component) : base(component) { }

//        public override void DoWork(Action<string, string> ReportProgress, Action Done)
//        {
//            try
//            {
//                if (_runNode == false)
//                {
//                    _success = false;
//                    _connection = null;
//                    RuntimeMessages.Add((GH_RuntimeMessageLevel.Warning, "Run node set to false."));
//                    Done();
//                    return;
//                }

//                if (_connection == null)
//                {
//                    RuntimeMessages.Add((GH_RuntimeMessageLevel.Warning, "Connection is null."));
//                    Done();
//                    return;
//                }

//                if (_connection.IsDisconnected)
//                {
//                    _success = false;
//                    _connection = null;
//                    throw new Exception("Connection to FEM-Design have been lost.");
//                }

//                if (_connection.HasExited)
//                {
//                    _success = false;
//                    _connection = null;
//                    throw new Exception("FEM-Design have been closed.");
//                }

//                ReportProgress("", "");
//                _success = true;


//                // get CriticalParameter results
//                List<FemDesign.Results.CriticalParameter> critParamRes = _getStabilityResults(_critParamType, null, null, _units, _options);

//                // check if results are null
//                if (critParamRes.Count == 0)
//                {
//                    RuntimeMessages.Add((GH_RuntimeMessageLevel.Warning, "Stability results have not been found. Have you run the Stability analysis?"));
//                    _success = false;
//                    _connection = null;
//                    Done();
//                    return;
//                }
                
//                // get load combinations & shape numbers that have been run for stability analysis.                
//                List<string> combsFromCalc;
//                List<int> shapeNumbersByComb;
               
//                string loadCombPropertyName = nameof(Results.CriticalParameter.CaseIdentifier);
//                string shapeIdPropertyName = nameof(Results.CriticalParameter.Shape);
//                (combsFromCalc, shapeNumbersByComb) = critParamRes.GetCombosAndShapes(loadCombPropertyName, shapeIdPropertyName);


//                // check inputs
//                int maxIdFromCalc = shapeNumbersByComb.Max();
//                List<int> idRange = Enumerable.Range(1, maxIdFromCalc).ToList();

//                var comboList = CheckInput(_combos, combsFromCalc).ToList();
//                var shapeList = CheckInput(_shapeIds, idRange).ToList();

//                // create DataTree from CriticalParameter results; order by load combination name
//                string critParamPropName = nameof(FemDesign.Results.CriticalParameter.CaseIdentifier);
//                var critParamTree = critParamRes.CreateResultTree(critParamPropName);
//                _critParameterResults = FilterTree(critParamTree, _combos, _shapeIds);

//                // Get NodalBucklingShape results
//                List<FemDesign.Results.NodalBucklingShape> bucklingRes = _getEigenResults(_resultType, comboList, shapeList, _elements, _units, _options);

//                // create DataTree from NodalBucklingShape results; order by load combination name and shape identifier
//                _bucklingTree = CreateResultTree(bucklingRes);
                                                
//            }
//            catch (Exception ex)
//            {
//                RuntimeMessages.Add((GH_RuntimeMessageLevel.Error, ex.Message));
//                _success = false;
//                _connection = null;
//            }

//            Done();
//        }
//        public override WorkerInstance Duplicate() => new ApplicationReadStabilityResultWorker(Parent);

//        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
//        {
//            if (!DA.GetData(0, ref _connection)) return;
//            DA.GetDataList(1, _combos);
//            DA.GetDataList(2, _shapeIds);
//            DA.GetDataList(3, _elements);
//            DA.GetData(4, ref _options);
//            DA.GetData(5, ref _units);
//            DA.GetData(6, ref _runNode);
//        }

//        public override void SetData(IGH_DataAccess DA)
//        {
//            foreach (var (level, message) in RuntimeMessages)
//            {
//                Parent.AddRuntimeMessage(level, message);
//            }

//            DA.SetData(0, _connection);
//            DA.SetDataTree(1, _bucklingTree);
//            DA.SetDataTree(2, _critParameterResults);
//            DA.SetData(3, _success);
//        }
//    }
//}