// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using System.Linq;
using System.Windows.Forms;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel.Special;
using GrasshopperAsyncComponent;

using System.Reflection;

using FemDesign;
using FemDesign.Calculate;

namespace FemDesign.Grasshopper
{
    public class PipeGetQuantities : GH_AsyncComponent
    {
        public PipeGetQuantities() : base("FEM-Design.GetQuantities", "GetQuantities", "Get quantities from a model. .csv list files are saved in the same work directory as StruxmlPath.", CategoryName.Name(), SubCategoryName.Cat7())
        {
            BaseWorker = new ApplicationGetQuantitiesWorker(this);
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddTextParameter("QuantityType", "QuantityType", "Connect 'ValueList' to get the options.\nQuantity type:\nQuantityEstimationConcrete\nQuantityEstimationReinforcement\nQuantityEstimationSteel\nQuantityEstimationTimber\nQuantityEstimationTimberPanel\nQuantityEstimationMasonry\nQuantityEstimationGeneral\nQuantityEstimationProfiledPanel", GH_ParamAccess.item);
            pManager.AddGenericParameter("Units", "Units", "Specify the Result Units for some specific type. \n" +
                "Default Units are: Length.m, Angle.deg, SectionalData.m, Force.kN, Mass.kg, Displacement.m, Stress.Pa", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Quantities", "Quantities", "Quantities.", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => base.Icon;
        public override Guid ComponentGuid => new Guid("{81E32E19-C6A6-4E9E-A0B2-EB6CE1BA888F}");
        public override GH_Exposure Exposure => GH_Exposure.tertiary;

        protected override void BeforeSolveInstance()
        {
            var quantities = new List<string>();

            var shipped = Enum.GetValues(typeof(ListProc));
            foreach(var _item in shipped)
            {
                var item = (ListProc)_item;
                if(item.IsQuantityEstimation())
                    quantities.Add(item.ToString());
            }

            ValueListUtils.updateValueLists(this, 1, quantities, null, GH_ValueListMode.DropDown);
        }

    }

    public class ApplicationGetQuantitiesWorker : WorkerInstance
    {
        /* INPUT/OUTPUT */
        public FemDesignConnection _connection = null;
        private Results.UnitResults _units = null;
        private string _resultType;

        private List<Results.IResult> _results = new List<Results.IResult>();
        private bool _runNode = true;
        private bool _success = false;

        private Verbosity _verbosity = Verbosity.Normal;

        public ApplicationGetQuantitiesWorker(GH_Component component) : base(component) { }

        public override void DoWork(Action<string, double> ReportProgress, Action Done)
        {
            if (_runNode == false)
            {
                _success = false;
                Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Run node set to false.");
                ReportProgress(Id, 0.0);
                return;
            }

            if (_connection.IsDisconnected)
            {
                _success = false;
                Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Connection to FEM-Design have been lost.");
                return;
            }

            if (_connection.HasExited)
            {
                _success = false;
                Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "FEM-Design have been closed.");
                return;
            }

            // Run the Analysis
            var _type = $"FemDesign.Results.{_resultType}, FemDesign.Core";
            Type type = Type.GetType(_type);

            var res = _connection._getQuantities(type, _units);
            _results.AddRange(res);

            _success = true;
            Done();
        }

        public override WorkerInstance Duplicate() => new ApplicationGetQuantitiesWorker(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref _connection)) return;
            DA.GetData("QuantityType", ref _resultType);
            DA.GetData("Units", ref _units);
            DA.GetData("RunNode", ref _runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            DA.SetData("Connection", _connection);
            DA.SetDataList("Quantities", _results);
            DA.SetData("Success", _success);
        }
    }
}