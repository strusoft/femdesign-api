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
using Grasshopper.Documentation;

namespace FemDesign.Grasshopper
{
    public class PipeResultFromBsc : GH_AsyncComponent
    {
        public PipeResultFromBsc() : base(" FEM-Design.GetResultFromBsc", "ResultFromBsc", "Extract results from a model with a .bsc file", CategoryName.Name(), SubCategoryName.Cat8())
        {
            BaseWorker = new ApplicationResultFromBsc(this);
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("BscFilePath", "BscFilePath", "File path to .bsc batch-file.", GH_ParamAccess.list);
            pManager.AddTextParameter("CsvFilePath", "CsvFilePath", "Specify where the .csv will be saved. If not specified, the results will be saved in the same folder of the .bsc file.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("Results", "Results", "", GH_ParamAccess.tree);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_readresult;

        public override Guid ComponentGuid => new Guid("{E7EEAC5F-4C80-40D3-AA16-C2E6E3BD62BC}");
        public override GH_Exposure Exposure => GH_Exposure.tertiary;
        private class ApplicationResultFromBsc : WorkerInstance
        {
            public FemDesignConnection _connection = null;

            private List<string> bscPath = new List<string>();
            private List<string> csvPath = new List<string>();

            private DataTree<object> _results = new DataTree<object>();
            private bool _runNode = true;
            private bool _success = false;

            public ApplicationResultFromBsc(GH_Component component) : base(component) { }

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

                    var results = bscPath.Zip(csvPath, (bsc, csv) => _connection.GetResultsFromBsc(bsc, csv) ).ToList();

                    int i = 0;
                    foreach( var result in results)
                    {
                        string[] lines = result.Split( new string[] { Environment.NewLine }, StringSplitOptions.None );
                        _results.AddRange(lines, new GH_Path(i));
                        i++;
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

            public override WorkerInstance Duplicate() => new ApplicationResultFromBsc(Parent);

            public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
            {
                if (!DA.GetData("Connection", ref _connection)) return;
                DA.GetDataList("BscFilePath", bscPath);
                if(!DA.GetDataList("CsvFilePath", csvPath))
                {
                    foreach(var bsc in bscPath)
                    {
                        csvPath.Add(System.IO.Path.ChangeExtension(bsc, "csv"));
                    }
                };



                DA.GetData("RunNode", ref _runNode);
            }

            public override void SetData(IGH_DataAccess DA)
            {
                foreach (var (level, message) in RuntimeMessages)
                {
                    Parent.AddRuntimeMessage(level, message);
                }

                DA.SetData("Connection", _connection);
                DA.SetDataTree(1, _results);
                DA.SetData("Success", _success);
            }
        }
    }

}