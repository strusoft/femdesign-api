// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class SoilMaterialFromStruxml : GH_Component
    {
        public SoilMaterialFromStruxml() : base("SoilMaterial.FromStruxml", "SoilMaterial", "Load a custom MaterialDatabase which contains the Soil Material type from a .struxml file.", CategoryName.Name(), SubCategoryName.Cat4a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("FilePath", "FilePath", "File path to .struxml file.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SoilMaterial", "SoilMaterial", "", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string fillePath = null;
            DA.GetData(0, ref fillePath);

            var materialDatabase = Materials.MaterialDatabase.DeserializeStruxml(fillePath);
            var soilMaterials = materialDatabase.GetSoilMaterial();

            DA.SetDataList(0, soilMaterials);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SoilMaterial;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{6BCC0826-14D6-468B-98E2-D140E9D08E80}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}