// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
   public class MaterialGetMaterialByNameOBSOLETE: GH_Component
    {
        public MaterialGetMaterialByNameOBSOLETE(): base("Material.GetMaterialByName", "GetMaterialByName", "Get Material from MaterialDatabase by name.", CategoryName.Name(), SubCategoryName.Cat4a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("MaterialDatabase", "MaterialDatabase", "MaterialDatabase.", GH_ParamAccess.item);
            pManager.AddTextParameter("MaterialName", "MaterialName", "Name of Material to retreive.", GH_ParamAccess.item);
        } 
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Materials.MaterialDatabase materialDatabase = null;
            string materialName = null;
            if (!DA.GetData(0, ref materialDatabase)) return;
            if (!DA.GetData(1, ref materialName)) return;
            if (materialDatabase == null || materialName == null) return;

            FemDesign.Materials.Material material = materialDatabase.MaterialByName(materialName);

            DA.SetData(0, material);
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
            get { return new Guid("4f28cdd5-2078-458a-b55b-98e9c449a26d"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

    } 
}