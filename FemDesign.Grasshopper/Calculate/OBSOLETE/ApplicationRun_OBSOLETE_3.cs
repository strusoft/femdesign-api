// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;
using FemDesign.Loads;
using FemDesign.Calculate;
using FemDesign.Results;
using System.Data.Common;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FemDesign.Grasshopper.Extension.ComponentExtension;

using GH_IO.Serialization;


namespace FemDesign.Grasshopper
{
    public class ApplicationRun_OBSOLETE_3 : FEM_Design_API_Component
    {
        public ApplicationRun_OBSOLETE_3() : base("Application.Run", "RunApplication", "Run application for a model.", CategoryName.Name(), SubCategoryName.Cat7a())
        {
            _minimised = false;
            _keepOpen = false;
        }

        public bool _minimised { get; set; }
        public bool _keepOpen { get; set; }


        public override bool Write(GH_IWriter writer)
        {
            // Save the version when this component was created
            writer.SetBoolean("minimised", _minimised);
            writer.SetBoolean("keepOpen", _keepOpen);
            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            // Read the version when this component was created
            try
            {
                _minimised = reader.GetBoolean("minimised");
                _keepOpen = reader.GetBoolean("keepOpen");
            }
            catch (NullReferenceException) { } // In case the info component was created before the VersionWhenFirstCreated was implemented.
            return base.Read(reader);
        }



        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Model", "Model", "Model to open.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Analysis", "Analysis", "Analysis.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Design", "Design", "Design.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddGenericParameter("DesignGroups", "DesignGroups", "DesignGroups.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddTextParameter("ResultTypes", "ResultTypes", "Results to be extracted from model. This might require the model to have been analysed.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddTextParameter("Bsc", "Bsc", "Bsc file path. This might require the model to have been analysed.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;


            pManager.AddGenericParameter("Options", "Options", "Settings for output location. Default is 'ByStep' and 'Vertices'", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddGenericParameter("Units", "Units", "Specify the Result Units for some specific type. \n" +
                "Default Units are: Length.m, Angle.deg, SectionalData.m, Force.kN, Mass.kg, Displacement.m, Stress.Pa", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddTextParameter("Cfg", "Cfg", "Cfg file path with design parameters for structural materials. \nYou can use the 'cfg.xml' file in located package manager library folder as a starting point..\n%AppData%\\McNeel\\Rhinoceros\\packages\\7.0\\FemDesign\\", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddTextParameter("GlobalCfg", "GlobalCfg", "GlobalCfg file path. You can use the 'cmdglobalcfg.xml' file in located package manager library folder as a starting point.\n%AppData%\\McNeel\\Rhinoceros\\packages\\7.0\\FemDesign\\", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddTextParameter("DocxTemplatePath", "DocxTemplatePath", "File path to documentation template file (.dsc). The documentation will be saved in the `FEM-Design API` folder. Optional parameter.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("SaveFilePath", "SaveFilePath", "File path where to save the model as .struxml.\nIf not specified, the file will be saved in the `FEM-Design API` folder adjacent to your .gh script.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Model", "Model", "Model.", GH_ParamAccess.item);
            pManager.Register_GenericParam("FiniteElement", "FiniteElement", "FemDesign Finite Element Geometries(nodes, bars, shells).");
            pManager.AddGenericParameter("Results", "Results", "Results.", GH_ParamAccess.tree);
        }


        public dynamic _getResults(FemDesignConnection connection, Type resultType, Results.UnitResults units = null, Options options = null, List<FemDesign.GenericClasses.IStructureElement> elements = null)
        {
            List<Results.IResult> mixedResults = new List<Results.IResult>();
            MethodInfo genericMethod = typeof(FemDesign.FemDesignConnection).GetMethod("_getResults", BindingFlags.Instance | BindingFlags.NonPublic).MakeGenericMethod(resultType);
            dynamic result = genericMethod.Invoke(connection, new object[] { units, options, elements, true });
            mixedResults.AddRange(result);
            return mixedResults;
        }


        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            // Append the item to the menu, making sure it's always enabled and checked if Absolute is True.
            ToolStripMenuItem minimisedItem = Menu_AppendItem(menu, "Minimise FEM-Design", Menu_AbsoluteClicked, null, true, _minimised);
            ToolStripMenuItem keepOpenItem = Menu_AppendItem(menu, "Keep open", keepOpenClick, null, true, _keepOpen);
        }

        private void Menu_AbsoluteClicked(object sender, EventArgs e)
        {
            _minimised = !_minimised;
            ExpireSolution(true);
        }

        private void keepOpenClick(object sender, EventArgs e)
        {
            _keepOpen = !_keepOpen;
            ExpireSolution(true);
        }


        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool runNode = false;
            DA.GetData("RunNode", ref runNode);

            if (runNode == false)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "RunNode set to false!");
                return;
            }

            dynamic _model = null;
            DA.GetData("Model", ref _model);

            FemDesign.Calculate.Analysis analysis = null;
            DA.GetData("Analysis", ref analysis);

            FemDesign.Calculate.Design design = null;
            DA.GetData("Design", ref design);

            FemDesign.Results.UnitResults units = UnitResults.Default();
            DA.GetData("Units", ref units);

            List<FemDesign.Calculate.CmdDesignGroup> designGroups = new List<Calculate.CmdDesignGroup>();
            DA.GetDataList("DesignGroups", designGroups);

            List<string> _resultType = new List<string>();
            DA.GetDataList("ResultTypes", _resultType);

            List<string> bscFilePath = new List<string>();
            DA.GetDataList("Bsc", bscFilePath);

            FemDesign.Calculate.Options options = null;
            DA.GetData("Options", ref options);

            string cfg = null;
            DA.GetData("Cfg", ref cfg);

            string globalCfg = null;
            DA.GetData("GlobalCfg", ref globalCfg);

            string dscTemplate = null;
            DA.GetData("DocxTemplatePath", ref dscTemplate);

            string saveFilePath = null;
            DA.GetData("SaveFilePath", ref saveFilePath);


            //if (analysis == null && design == null && saveFilePath == null && _resultType.Count == 0)
            //{
            //    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "At least one of the following should be provided.\n'Analysis', 'Design', 'ResultTypes' or 'SaveFilePath'");
            //    return;
            //}

            // Collect Outputs
            Model model = null;
            List<Results.IResult> results = new List<Results.IResult>();
            FemDesign.Results.FiniteElement finiteElement = null;
            var resultsTree = new DataTree<object>();


            var types = new List<Type>();
            foreach (var resType in _resultType)
            {
                var _type = $"FemDesign.Results.{resType}, FemDesign.Core";
                Type type = Type.GetType(_type);
                types.Add(type);
            }


            bool fileExist = OnPingDocument().IsFilePathDefined;
            if (!fileExist)
            {
                // hops issue
                //var folderPath = System.IO.Directory.GetCurrentDirectory();
                string tempPath = System.IO.Path.GetTempPath();
                System.IO.Directory.SetCurrentDirectory(tempPath);
            }
            else
            {
                var filePath = OnPingDocument().FilePath;
                var currentDir = System.IO.Path.GetDirectoryName(filePath);
                System.IO.Directory.SetCurrentDirectory(currentDir);
            }


            // Gets how many times SolveInstance() has been called
            var iteration = DA.Iteration;

            // Create Task
            var t = Task.Run((Action)(() =>
            {
                var connection = new FemDesign.FemDesignConnection(minimized: _minimised, keepOpen: _keepOpen);

                connection.Open(_model.Value);

                if (cfg != null)
                    connection.SetConfig(cfg);

                if (globalCfg != null)
                    connection.SetGlobalConfig(CmdGlobalCfg.DeserializeCmdGlobalCfgFromFilePath(globalCfg));

                if (analysis != null)
                    connection.RunAnalysis(analysis);


                // run design

                if (design != null)
                {
                    CmdUserModule userModule = design.Mode;
                    if (designGroups.Count() == 0)
                        connection.RunDesign(userModule, design);
                    else
                        connection.RunDesign(userModule, design, designGroups);

                    if (design.ApplyChanges == true && design.Check == true)
                    {
                        connection.RunAnalysis(analysis);
                        var _design = new Design(check: true);

                        connection.RunDesign(userModule, _design);
                    }
                }


                finiteElement = connection.GetFeaModel(units.Length);

                int i = 0;
                if (types.Count != 0)
                {
                    foreach (var type in types)
                    {
                        var res = _getResults(connection, type, units, options);
                        resultsTree.AddRange(res, new GH_Path(iteration, i));
                        i++;
                    }
                }

                if(bscFilePath.Count != 0)
                {
                    foreach(var bsc in bscFilePath)
                    {
                        var res = connection.GetResultsFromBsc(bsc);
                        resultsTree.AddRange(res, new GH_Path(iteration, i));
                        i++;
                    }

                }

                if (dscTemplate != null)
                {
                    var outputDocx = OutputFileHelper.GetDocxPath(connection.OutputDir);
                    connection.SaveDocx(outputDocx, dscTemplate);
                }

                // return the new model
                model = connection.GetModel();


                // save calculated model in .str format
                if (saveFilePath == null)
                {
                    saveFilePath = OutputFileHelper.GetStrPath(connection.OutputDir, "model_saved");
                }

                connection.Save(saveFilePath);

                connection.Dispose();
            }));


            t.ConfigureAwait(false);

            try
            {
                t.Wait();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

            // Set output
            DA.SetData("Model", model);
            DA.SetData("FiniteElement", finiteElement);
            DA.SetDataTree(2, resultsTree);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ModelRunAnalysis;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{1D5D200B-2F6F-4D81-A295-2924773782B3}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

    }
}