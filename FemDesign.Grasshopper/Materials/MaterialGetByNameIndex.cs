// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Linq;

namespace FemDesign.Grasshopper
{
    public class MaterialGetMaterialByName : GH_Component
    {
        public MaterialGetMaterialByName() : base("Material.GetMaterialByName|Index", "GetMaterialByName|Index", "Get Material from MaterialDatabase by name.", CategoryName.Name(), SubCategoryName.Cat4a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Materials", "Materials", ".", GH_ParamAccess.list);
            pManager.AddGenericParameter("MaterialName|Index", "MaterialName|Index", "Name of Material to retrieve or positional index in the list.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var materials = new List<Materials.Material>();
            DA.GetDataList(0, materials);

            dynamic materialInput = null;
            DA.GetData(1, ref materialInput);
            
            materialInput = materialInput.Value;

            var material = Materials.Material.GetMaterialByNameOrIndex(materials, materialInput);

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
            get { return new Guid("{9D0A102B-29AD-48A8-A51C-459444262E9B}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}