// https://strusoft.com/
using System;
using System.Collections.Generic;
using FemDesign.Materials;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class TimberPanelTypeFromStruxml : FEM_Design_API_Component
    {
        public TimberPanelTypeFromStruxml() : base("TimberPlateLibrary", "TimberPlateLibrary", "Load Clt or Orthotropic panel Default or FromStruxml.", CategoryName.Name(), SubCategoryName.Cat4a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("FilePath", "FilePath", "File path to .struxml file.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CltPanelType", "CltPanelType", "CltPanelLibraryType.", GH_ParamAccess.item);
            pManager.AddGenericParameter("OrthotropicShell", "OrthotropicShell", "OrthotropicPanelLibraryType", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string filePath = null;
            DA.GetData(0, ref filePath);

            FemDesign.Materials.MaterialDatabase materialDatabase;
            List<Materials.CltPanelLibraryType> cltPaneltype;
            List<Materials.OrthotropicPanelLibraryType> orthotropicPaneltype;

            if (filePath == null)
            {
                materialDatabase = FemDesign.Materials.MaterialDatabase.DefaultTimberPlateLibrary();
            }
            else
            {
                materialDatabase = FemDesign.Materials.MaterialDatabase.DeserializeStruxml(filePath);
            }

            cltPaneltype = materialDatabase.GetCltPanelLibrary();
            orthotropicPaneltype = materialDatabase.GetOrthotropicPanelLibrary();


            DA.SetDataList(0, cltPaneltype);
            DA.SetDataList(1, orthotropicPaneltype);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.MaterialTimberPlateMaterial;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{4CD9372C-170D-4745-BE24-14DAAEB14CED}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}