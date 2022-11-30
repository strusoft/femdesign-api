// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;

using System.Linq;

namespace FemDesign.Grasshopper
{
    public class ModelRunDesign : GH_Component
    {
        public ModelRunDesign() : base("Application.RunDesign", "RunDesign", "Run analysis and design of model. .csv list files and .docx documentation files are saved in the same work directory as StruxmlPath.", CategoryName.Name(), SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Mode", "Mode", "Design mode: rc, steel or timber.", GH_ParamAccess.item);
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel to open.", GH_ParamAccess.item);
            pManager.AddTextParameter("FilePathStruxml", "FilePath", "File path where to save the model as .struxml.\nIf not specified, the file will be saved using the name and location folder of your .gh script.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Analysis", "Analysis", "Analysis.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Design", "Design", "Design.", GH_ParamAccess.item);
            pManager.AddTextParameter("ResultTypes", "ResultTypes", "Results to be extracted from model. This might require the model to have been analysed. Item or list.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Units", "Units", "Specify the Result Units for some specific type. \n" +
            "Default Units are: Length.m, Angle.deg, SectionalData.m, Force.kN, Mass.kg, Displacement.m, Stress.Pa", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("DocxTemplatePath", "DocxTemplatePath", "File path to documenation template file (.dsc) to run. Optional parameter.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("EndSession", "EndSession", "If true FEM-Design will close after execution.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("CloseOpenWindows", "CloseOpenWindows", "If true all open windows will be closed without prior warning.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel.", GH_ParamAccess.item);
            pManager.Register_GenericParam("FdFeaModel", "FdFeaModel", "FemDesign Finite Element Geometries (nodes, bars, shells).");
            pManager.AddGenericParameter("Results", "Results", "Results.", GH_ParamAccess.tree);
            pManager.AddBooleanParameter("HasExited", "HasExited", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // 
            string mode = null;
            FemDesign.Model model = null;
            string filePath = null;
            FemDesign.Calculate.Analysis analysis = null;
            FemDesign.Calculate.Design design = null;
            List<string> resultTypes = new List<string>();
            string docxTemplatePath = "";
            bool endSession = false;
            bool closeOpenWindows = false;
            bool runNode = false;


            // get data
            if (!DA.GetData(0, ref mode))
            {
                return;
            }
            if (!DA.GetData(1, ref model))
            {
                return;
            }
            if (!DA.GetData(2, ref filePath))
            {
                bool fileExist = OnPingDocument().IsFilePathDefined;
                if (!fileExist)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Save your .gh script or specfy a FilePath.");
                    return;
                }
                filePath = OnPingDocument().FilePath;
                filePath = System.IO.Path.ChangeExtension(filePath, "struxml");
            }
            if (!DA.GetData(3, ref analysis))
            {
                return;
            }
            if (!DA.GetData(4, ref design))
            {
                return;
            }
            if (!DA.GetDataList(5, resultTypes))
            {
                // pass
            }

            var units = Results.UnitResults.Default();
            DA.GetData(6, ref units);

            if (!DA.GetData(7, ref docxTemplatePath))
            {
                // pass
            }
            if (!DA.GetData(8, ref endSession))
            {
                // pass
            }
            if (!DA.GetData(9, ref closeOpenWindows))
            {
                // pass
            }
            DA.GetData(10, ref runNode);
            if(runNode == false)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "RunNode is set to false!");
                return;
            }
            if (mode == null || model == null || filePath == null || analysis == null)
            {
                return;
            }

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
            if (notValidResultTypes.Count != 0)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "The following strings are not valid result types: " + string.Join(", ", notValidResultTypes));
                return;
            }

            var bscPathsFromResultTypes = Calculate.Bsc.BscPathFromResultTypes(_resultTypes, filePath, units);

            bool rtn = false;
            var resultsTree = new DataTree<object>();
            Results.FDfea fdFeaModel = null;



            //
            if (runNode)
            {
                model.SerializeModel(filePath);
                analysis.SetLoadCombinationCalculationParameters(model);

                bool endDesignSession = design.ApplyChanges == true ? true : endSession;
                rtn = model.FdApp.RunDesign(mode, filePath, analysis, design, bscPathsFromResultTypes, docxTemplatePath, endDesignSession, closeOpenWindows);


                // Create FdScript
                var fdScript = FemDesign.Calculate.FdScript.ReadStr(filePath, bscPathsFromResultTypes);

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
                            if (path.Contains("FeaNode"))
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
                var i = 0;
                foreach (var resGroup in resultGroups)
                {
                    resultsTree.AddRange(resGroup.AsEnumerable(), new GH_Path(i));
                    i++;
                }

                if (design.ApplyChanges)
                {
                    filePath = System.IO.Path.ChangeExtension(filePath, "str");
                    (model, _) = FemDesign.Model.ReadStr(filePath, null);
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Remark, "It is recommended to run a new analysis when 'design changes' are applied. The model might have a different stress/force distribution due to change in stiffness.");
                }
            }


            // Set output
            DA.SetData("FdModel", model);
            DA.SetData("FdFeaModel", fdFeaModel);
            DA.SetDataTree(2, resultsTree);
            DA.SetData(3, rtn);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ModelRunDesign;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("E188C2DD-9091-4697-9798-B30296D57728"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;
    }
}