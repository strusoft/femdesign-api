// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;

namespace FemDesign.Grasshopper
{
    public class ModelReadStr: GH_Component
    {
        public ModelReadStr(): base("Model.ReadStr", "ReadStr", "Read model from .str file.", CategoryName.Name(), SubCategoryName.Cat6())
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
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel.", GH_ParamAccess.item);
            pManager.Register_GenericParam("FdFeaModel", "FdFeaModel", "FdFeaModel.");
            pManager.AddGenericParameter("Results", "Results", "Results.", GH_ParamAccess.tree);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get input
            string filePath = null;
            List<string> resultTypes = new List<string>();
            

            Results.FDfea fdFeaModel = null;

            DA.GetData("StrPath", ref filePath);
            if (filePath == null)
            {
                return;
            }
            DA.GetDataList("ResultTypes", resultTypes);

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
                resultTypes.Insert(0, "FeaNode");
                resultTypes.Insert(1, "FeaBar");
                resultTypes.Insert(2, "FeaShell");

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
                var bscPathsFromResultTypes = Calculate.Bsc.BscPathFromResultTypes(_resultTypes, filePath, units);

                // Create FdScript
                var fdScript = FemDesign.Calculate.FdScript.ReadStr(filePath, bscPathsFromResultTypes);

                // Run FdScript
                var app = new FemDesign.Calculate.Application();
                bool hasExited = app.RunFdScript(fdScript, false, true, false);

                // Read model and results
                var model = Model.DeserializeFromFilePath(fdScript.StruxmlPath);

                IEnumerable<Results.IResult> results = Enumerable.Empty<Results.IResult>();

                List<Results.FeaNode> feaNodeRes = new List<Results.FeaNode>();
                List<Results.FeaBar> feaBarRes = new List<Results.FeaBar>();
                List<Results.FeaShell> feaShellRes = new List<Results.FeaShell>();

                if (resultTypes != null && resultTypes.Any())
                {
                    foreach (var cmd in fdScript.CmdListGen)
                    {
                        string path = cmd.OutFile;
                        try
                        {
                            if(path.Contains("FeaNode"))
                            {
                                feaNodeRes = Results.ResultsReader.Parse(path).Cast<Results.FeaNode>().ToList();
                            }
                            else if (path.Contains("FeaBar"))
                            {
                                feaBarRes = Results.ResultsReader.Parse(path).Cast<Results.FeaBar>().ToList();
                            }
                            else if (path.Contains("FeaShell"))
                            {
                                feaShellRes = Results.ResultsReader.Parse(path).Cast<Results.FeaShell>().ToList();
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

                fdFeaModel = new FemDesign.Results.FDfea(feaNodeRes, feaBarRes, feaShellRes);

                var resultGroups = results.GroupBy(t => t.GetType()).ToList();
                // Convert Data in DataTree structure
                var resultsTree = new DataTree<object>();

                var i = 0;
                foreach(var resGroup in resultGroups)
                {
                    resultsTree.AddRange(resGroup.AsEnumerable(), new GH_Path(i));
                    i++;
                }

                // Set output
                DA.SetData("FdModel", model);
                DA.SetData("FdFeaModel", fdFeaModel);
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
            get { return new Guid("e5d933c4-9217-4ffa-9f82-15a5a26c9967"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}
