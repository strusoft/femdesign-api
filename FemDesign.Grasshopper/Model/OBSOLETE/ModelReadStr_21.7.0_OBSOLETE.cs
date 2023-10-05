// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using System.Text.RegularExpressions;
namespace FemDesign.Grasshopper
{
    public class ModelReadStr_21_7_0_OBSOLETE : FEM_Design_API_Component
    {
        public ModelReadStr_21_7_0_OBSOLETE() : base("Model.ReadStr", "ReadStr", "Read model from .str file.", CategoryName.Name(), SubCategoryName.Cat6())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("StrPath", "StrPath", "File path to FEM-Design model (.str) file.", GH_ParamAccess.item);
            pManager.AddTextParameter("ResultTypes", "ResultTypes", "Results to be extracted from model. This might require the model to have been analysed. Item or list.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Units", "Units", "Specify the Result Units for some specific type. \n" +
                "Default Units are: Length.m, Angle.deg, SectionalData.m, Force.kN, Mass.kg, Displacement.m, Stress.Pa", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Options", "Options", "Settings for output location. Default is 'ByStep' and 'Vertices'", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Model", "Model", "Model.", GH_ParamAccess.item);
            pManager.Register_GenericParam("FiniteElement", "FiniteElement", "FiniteElement.");
            pManager.AddGenericParameter("Results", "Results", "Results.", GH_ParamAccess.tree);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get input
            string filePath = null;
            List<string> resultTypes = new List<string>();


            Results.FiniteElement FiniteElement = null;

            DA.GetData("StrPath", ref filePath);
            if (filePath == null)
            {
                return;
            }

            DA.GetDataList("ResultTypes", resultTypes);

            FemDesign.Calculate.Options options = new FemDesign.Calculate.Options();
            DA.GetData("Options", ref options);

            bool runNode = true;
            if (!DA.GetData("RunNode", ref runNode))
            {
                // pass
            }

            // Units
            var units = Results.UnitResults.Default();
            DA.GetData("Units", ref units);

            // RunNode
            if (runNode)
            {
                // It needs to check if model has been runned
                // Always Return the FeaNode Result
                resultTypes.Insert(0, "FemNode");
                resultTypes.Insert(1, "FemBar");
                resultTypes.Insert(2, "FemShell");

                var notValidResultTypes = new List<string>();
                var _resultTypes = resultTypes.Select(r =>
                {
                    var sucess = Results.ResultTypes.All.TryGetValue(r, out Type value);
                    if (sucess)
                        return value;
                    else
                    {
                        notValidResultTypes.Add(r);
                        return null;
                    }
                });
                if (notValidResultTypes.Count() != 0)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "The following strings are not valid result types: " + string.Join(", ", notValidResultTypes));
                    return;
                }

                // Create Bsc files from resultTypes
                var bscPathsFromResultTypes = Calculate.Bsc.BscPathFromResultTypes(_resultTypes, filePath, units, options);

                // Create FdScript
                var fdScript = FemDesign.Calculate.FdScript.ReadStr(filePath, bscPathsFromResultTypes);

                // Run FdScript
                var app = new FemDesign.Calculate.Application();
                bool hasExited = app.RunFdScript(fdScript, false, true, false);

                // Read model and results
                var model = Model.DeserializeFromFilePath(fdScript.StruxmlPath);


                if (_FileName.IsASCII(filePath))
                    this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "File path has special characters. This might cause problems.");

                IEnumerable<Results.IResult> results = Enumerable.Empty<Results.IResult>();

                List<Results.FemNode> feaNodeRes = new List<Results.FemNode>();
                List<Results.FemBar> feaBarRes = new List<Results.FemBar>();
                List<Results.FemShell> feaShellRes = new List<Results.FemShell>();

                if (resultTypes != null && resultTypes.Any())
                {
                    foreach (var cmd in fdScript.CmdListGen)
                    {
                        string path = cmd.OutFile;
                        try
                        {
                            if (path.Contains("FemNode"))
                            {
                                feaNodeRes = Results.ResultsReader.Parse(path).Cast<Results.FemNode>().ToList();
                            }
                            else if (path.Contains("FemBar"))
                            {
                                feaBarRes = Results.ResultsReader.Parse(path).Cast<Results.FemBar>().ToList();
                            }
                            else if (path.Contains("FemShell"))
                            {
                                feaShellRes = Results.ResultsReader.Parse(path).Cast<Results.FemShell>().ToList();
                            }
                            else
                            {
                                var _results = Results.ResultsReader.Parse(path);
                                results = results.Concat(_results);
                            }
                        }
                        catch (Exception e)
                        {
                            throw new Exception(e.InnerException.Message);
                        }
                    }
                }

                FiniteElement = new FemDesign.Results.FiniteElement(feaNodeRes, feaBarRes, feaShellRes);

                var resultGroups = results.GroupBy(t => t.GetType()).ToList();
                // Convert Data in DataTree structure
                var resultsTree = new DataTree<object>();

                var i = 0;
                foreach (var resGroup in resultGroups)
                {
                    resultsTree.AddRange(resGroup.AsEnumerable(), new GH_Path(i));
                    i++;
                }

                // Set output
                DA.SetData("Model", model);
                DA.SetData("FiniteElement", FiniteElement);
                DA.SetDataTree(2, resultsTree);
            }
            else
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "RunNode is set to false!");
            }
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ModelReadStr;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{D9152EDD-6BC8-4F4C-AD81-B9B2E8B51192}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

    }
}