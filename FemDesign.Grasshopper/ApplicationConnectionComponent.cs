// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class ApplicationConnectionComponent : GH_Component
    {
        private ApplicationConnection _connection;

        public ApplicationConnectionComponent() : base("FEM-Design", "FEM-Design", $"FEM-Design application connection ({typeof(ApplicationConnection).FullName})", CategoryName.Name(), SubCategoryName.Cat6())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("FEM-Design dir", "FEM-Design dir", 
                "Path to directory of FEM-Design installation.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddBooleanParameter("Minimized", "Minimized", 
                "Should FEM-Design window be minimized?", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddTextParameter("OutputDir", "OutputDir", 
                "The directory to save script files. If set to null, the files will be will be written to a temporary directory and deleted after.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connection", "Connection", "FEM-Design application connection.", GH_ParamAccess.item);
        }
        public override void RemovedFromDocument(GH_Document document)
        {
            // Close FEM-Design connection when the component is removed
            _connection?.Dispose();
            _connection = null;

            base.RemovedFromDocument(document);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // TODO set version as global variable in FemDesign.Core
            string fd_installation_folder = @"C:\Program Files\StruSoft\FEM-Design 21\";
            DA.GetData("FEM-Design dir", ref fd_installation_folder);

            bool minimized = false;
            DA.GetData("Minimized", ref minimized);

            string outputDir = null;
            DA.GetData("OutputDir", ref outputDir);


            if (_connection != null)
                _connection.Dispose();

            _connection = new ApplicationConnection(fd_installation_folder, minimized, outputDir);

            DA.SetData("Connection", _connection);
        }

        protected override System.Drawing.Bitmap Icon => null;
        public override Guid ComponentGuid => new Guid("ae8824c3-26bf-42fd-9052-5f407b3540c1");
        public override GH_Exposure Exposure => GH_Exposure.primary;
    }
}

