// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
   public class MaterialGetCltPanelTypeByNameOBSOLETE : GH_Component
    {
        public MaterialGetCltPanelTypeByNameOBSOLETE() : base("Material.GetCltPanelTypeByName", "GetCltPanelTypeByName", "Get CltPanelLibraryType from MaterialDatabase by name.", CategoryName.Name(), SubCategoryName.Cat4a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("MaterialDatabase", "MaterialDatabase", "MaterialDatabase.", GH_ParamAccess.item);
            pManager.AddTextParameter("CltTypeName", "CltTypeName", "Name of CltType to retreive.", GH_ParamAccess.item);
        } 
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CltType", "CltType", "CltPanelLibraryType.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Materials.MaterialDatabase materialDatabase = null;
            string materialName = null;
            if (!DA.GetData(0, ref materialDatabase)) { return; }
            if (!DA.GetData(1, ref materialName)) { return; }
            if (materialDatabase == null || materialName == null) { return; }

            Materials.CltPanelLibraryType cltPaneltype = materialDatabase.GetCltPanelLibraryTypeByName(materialName);

            DA.SetData(0, cltPaneltype);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.MaterialGetMaterialByName;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("fefcac8c-6d7a-473d-9264-c75c66807a71"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

    }
}