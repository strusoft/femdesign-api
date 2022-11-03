// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using System.Linq;

namespace FemDesign.Grasshopper
{
    public class MaterialGetMaterialByNameOBSOLETE2 : GH_Component
    {
        public MaterialGetMaterialByNameOBSOLETE2() : base("Material.GetMaterialByName", "GetMaterialByName", "Get Material from MaterialDatabase by name.", CategoryName.Name(), SubCategoryName.Cat4a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", ".", GH_ParamAccess.list);
            pManager.AddTextParameter("MaterialName", "MaterialName", "Name of Material to retrieve.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var materials = new List<Materials.Material>();
            DA.GetDataList(0, materials);
            
            string materialName = null;
            DA.GetData(1, ref materialName);

            FemDesign.Materials.Material material = null;
            try
            {
                material = materials.Where(x => x.Name == materialName).First();
            }
            catch (Exception)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"{materialName} does not exist!");
            }

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
            get { return new Guid("{5629FA53-2317-4605-9B01-B95961CD19B9}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;

    }
}