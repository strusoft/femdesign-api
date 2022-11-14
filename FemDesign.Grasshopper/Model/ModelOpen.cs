// https://strusoft.com/
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class ModelOpen : GH_Component
    {
        private enum ProgramState { Unknown, Creation, Finished }
        private volatile ProgramState _state = ProgramState.Creation;
        private Task _task;

        public ModelOpen() : base("Model.Open", "Open", "Open model in FEM-Design.", CategoryName.Name(), SubCategoryName.Cat6())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            int connectionIndex = pManager.AddGenericParameter("Connection", "Connection", "FEM-Design application connection.", GH_ParamAccess.item);
            pManager.AddGenericParameter("FdModel", "FdModel", "FdModel to open.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("RunNode", "RunNode", "If true node will execute. If false node will not execute.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design application connection.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Finished", "Finished", "Model was opened successfully.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ApplicationConnection connection = null;
            DA.GetData("Connection", ref connection);

            Model model = null;
            DA.GetData("FdModel", ref model);

            bool runNode = false;
            DA.GetData("RunNode", ref runNode);

            if (runNode == false)
            {
                DA.SetData("Finished", false);
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "RunNode is set to false!");
                return;
            }

            if (_state == ProgramState.Creation)
            {
                // Create an async task that opens the model on a background thread.
                // This is needed in order not to block the grasshopper UI thread indefinitely.
                _task = Task.Run(() => connection.OpenAsync(model));

                // When the task is done we want to let grasshopper know the output must be updated (expire solution)
                _task.ContinueWith(task =>
                {
                    _state = ProgramState.Finished;
                    Rhino.RhinoApp.InvokeOnUiThread(new Action(() => this.ExpireSolution(true)));
                });

                // Until that happens, the node will output false
                DA.SetData("Finished", false);
            }
            else if (_state == ProgramState.Finished)
            {
                // When the task has been finished
                DA.SetData("Finished", true);
                DA.SetData("Connection", connection);
                _state = ProgramState.Creation;
            }
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.ModelOpen;
        public override Guid ComponentGuid => new Guid("11fd183e-f7bf-442f-89d6-5ff86bafcf38");
        public override GH_Exposure Exposure => GH_Exposure.secondary;
    }
}