// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class MaterialSetTimberMaterialProperties : GH_Component
    {
        public MaterialSetTimberMaterialProperties() : base("SetTimberMaterialProperties", "SetTimberMaterialProperties", "Set creep and shrinkage parameters to a timber Material.", CategoryName.Name(), SubCategoryName.Cat4a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
            pManager.AddNumberParameter("creepUls", "creepUls", "Creep ULS.", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("creepSlsQ", "creepSlsQ", "Creep SLS Quasi-permanent.", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;


        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Materials.Material material = null;


            if (!DA.GetData(0, ref material)) { return; }
            if (!DA.GetData(1, ref creepUls))
            {
                // pass
            }
            if (!DA.GetData(2, ref creepSlq))
            {
                // pass
            }
            if (!DA.GetData(3, ref creepSlf))
            {
                // pass
            }
            if (!DA.GetData(4, ref creepSlc))
            {
                // pass
            }
            if (!DA.GetData(5, ref shrinkage))
            {
                // pass
            }
            if (material == null) { return; }

            //
            FemDesign.Materials.Material modifiedMaterial = FemDesign.Materials.Material.TimberMaterialProperties(material, _ksys, _k_cr, serviceClass, _kdefU, _kdefSq, _kdefSf, _kdefSc);
            modifiedMaterial.EntityModified();

            // set output
            DA.SetData(0, modifiedMaterial);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.MaterialSetTimberMaterialProperties;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{D5ACE67A-954F-411B-A3E3-27AB5F6DFF26}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }
}