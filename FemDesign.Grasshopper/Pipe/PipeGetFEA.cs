// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using System.Linq;
using System.Windows.Forms;

using GrasshopperAsyncComponent;

namespace FemDesign.Grasshopper
{
    public class PipeGetFeaModel : GH_AsyncComponent
    {
        public PipeGetFeaModel() : base("FEM-Design.GetFeModel", "GetFeModel", "Read the finite element data.", CategoryName.Name(), SubCategoryName.Cat8())
        {

            BaseWorker = new ApplicationGetFeaModelWorker(this);
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Units", "Units", "Specify the Result Units for some specific type. \n" +
                "Default Units are: Length.m, Angle.deg, SectionalData.m, Force.kN, Mass.kg, Displacement.m, Stress.Pa", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design connection.", GH_ParamAccess.item);
            pManager.AddGenericParameter("FiniteElement", "FiniteElement", "FEM-Design finite element model.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Success", "Success", "True if session has exited. False if session is open or was closed manually.", GH_ParamAccess.item);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_GetMesh;

        public override Guid ComponentGuid => new Guid("{6231B7A4-936A-4BA2-8302-D3BB4CA1594F}");
        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }

    public class ApplicationGetFeaModelWorker : WorkerInstance
    {
        /* INPUT/OUTPUT */
        public FemDesignConnection _connection = null;
        private FemDesign.Results.FiniteElement _fdFea = null;

        private Results.UnitResults _units = null;
        private bool _runNode = false;
        private bool _success = false;

        public ApplicationGetFeaModelWorker(GH_Component component) : base(component) { }

        public override void DoWork(Action<string, string> ReportProgress, Action Done)
        {
            if (_runNode == false)
            {
                _success = false;
                _connection = null;
                Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Run node set to false.");
                return;
            }

            if (_connection == null)
            {
                _success = false;
                _connection = null;
                Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Connection is null.");
                return;
            }

            if (_connection.IsDisconnected)
            {
                _success = false;
                _connection = null;
                Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Connection to FEM-Design have been lost.");
                return;
            }

            if (_connection.HasExited)
            {
                _success = false;
                _connection = null;
                Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "FEM-Design have been closed.");
                return;
            }

            //if (!_connection.HasResult())
            //{
            //    _success = false;
            //    Parent.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "The open model does not contain any results!");
            //    return;
            //}

            if(_units == null)
                _units = Results.UnitResults.Default();

            ReportProgress("", "");

            _fdFea = _connection.GetFeaModel(_units.Length);
            _success = true;
            Done();
        }

        public override WorkerInstance Duplicate() => new ApplicationGetFeaModelWorker(Parent);

        public override void GetData(IGH_DataAccess DA, GH_ComponentParamServer Params)
        {
            if (!DA.GetData("Connection", ref _connection)) return;
            DA.GetData("Units", ref _units);
            DA.GetData("RunNode", ref _runNode);
        }

        public override void SetData(IGH_DataAccess DA)
        {
            DA.SetData("Connection", _connection);
            DA.SetData("FiniteElement", _fdFea);
            DA.SetData("Success", _success);
        }
    }
}