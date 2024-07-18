// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class FemDesignConnectionComponent : FEM_Design_API_Component
    {
        private FemDesignConnection _connection;

        public FemDesignConnectionComponent() : base("FEM-Design.Connection", "Connection", "Component that creates a direct link between Grasshopper and FEM-Design. Use it to specify the 'Connection' for LiveLink components.", CategoryName.Name(), SubCategoryName.Cat8())
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

            pManager.AddTextParameter("Verbosity", "Verbosity", "", GH_ParamAccess.item, "ScriptLogLinesOnly");
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddTextParameter("OutputDir", "OutputDir",
                "The directory to save script files. Default, the files will be written to the same directory of your .gh script.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddBooleanParameter("DeleteOutputFolder", "DeleteOutputFolder", "The directory to save script files will be deleted when the connection close or disconnect", GH_ParamAccess.item, false);
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
            // TODO: Set version as global variable in FemDesign.Core

            string fd_installation_folder = $@"C:\Program Files\StruSoft\FEM-Design 23\";
            DA.GetData("FEM-Design dir", ref fd_installation_folder);

            bool minimized = false;
            DA.GetData("Minimized", ref minimized);

            string outputDir = null;
            if (!DA.GetData("OutputDir", ref outputDir))
            {
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
            }

            bool delteDir = false;
            DA.GetData("DeleteOutputFolder", ref delteDir);

            if (_connection != null)
                _connection.Dispose();

            _connection = new FemDesignConnection(fd_installation_folder, minimized, outputDir: outputDir, tempOutputDir: delteDir);
            _connection._grasshopperMode = true;

            string verbosity = "";
            if (DA.GetData("Verbosity", ref verbosity))
            {
                if (Enum.TryParse(verbosity, out Verbosity _verbosity))
                {
                    _connection.SetVerbosity(_verbosity);
                }
                else
                {

                }
            };

            DA.SetData("Connection", _connection);
        }


        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 2, new List<string> { "EchoAndStatOnly", "BasicOnly", "InputOnly", "LogLinesOnly", "ScriptLogLinesOnly", "CalculationMessagesOnly", "ProgressWindowTitleOnly", "None", "Low", "Normal", "High", "All"}, null, GH_ValueListMode.DropDown);
        }


        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.FEM_Connection;
        public override Guid ComponentGuid => new Guid("{B2D8285D-0260-479C-91BF-2FA8DAB5A37E}");
        public override GH_Exposure Exposure => GH_Exposure.primary;
    }
}

