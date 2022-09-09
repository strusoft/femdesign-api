// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class MaterialSetConcreteMaterialProperties: GH_Component
    {
        public MaterialSetConcreteMaterialProperties(): base("SetConcreteMaterialProperties", "SetConcreteMaterialProperties", "Set creep and shrinkage parameters to a concrete Material.", CategoryName.Name(), SubCategoryName.Cat4a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
            pManager.AddNumberParameter("creepUls", "creepUls", "Creep ULS.", GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("creepSlsQ", "creepSlsQ", "Creep SLS Quasi-permanent.", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("creepSlsF", "creepSlsF", "Creep SLS Frequent.", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("creepSlsC", "creepSlsC", "Creep SLS Characteristic.", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("shrinkage", "shrinkage", "Shrinkage.", GH_ParamAccess.item, 0);
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
            double creepUls = 0;
            double creepSlq = 0;
            double creepSlf = 0;
            double creepSlc = 0;
            double shrinkage = 0;
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
            FemDesign.Materials.Material modifiedMaterial = FemDesign.Materials.Material.ConcreteMaterialProperties(material, creepUls, creepSlq, creepSlf, creepSlc, shrinkage);
            modifiedMaterial.EntityModified();

            // set output
            DA.SetData(0, modifiedMaterial);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.MaterialSetConcreteMaterialProperties;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("5871fa03-17fe-4580-9091-976da4a1b4ee"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }
}