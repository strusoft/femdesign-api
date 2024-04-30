// https://strusoft.com/
using System;
using System.Collections.Generic;
using FemDesign.Soil;
using Grasshopper.Kernel;

using FemDesign.Materials;

namespace FemDesign.Grasshopper
{
    public class MaterialSetConcretePlasticity : FEM_Design_API_Component
    {
        public MaterialSetConcretePlasticity() : base("SetConcretePlasticity", "SetConcretePlasticity", "Set plasticity parameters to a concrete Material.", CategoryName.Name(), SubCategoryName.Cat4a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Elasto-Plastic", "Elasto-Plastic", "Elasto-Plastic Behaviour", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Plastic-Hardening", "Plastic-Hardening", "Plastic-Hardening", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Crushing", "Crushing", "Crushing", GH_ParamAccess.item, "Prager");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Tension-Strength", "Tension-Strength", "Tension strength in plastic flow rule", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("ReducedShearStiffness", "ReducedShearStiffness", "Reduced transverse shear stiffness in case of cracking", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("UltimateStrainRebars", "UltimateStrainRebars", "Ultimate Strain in Rebars", GH_ParamAccess.item, true);


        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var concrete = new Material();
            DA.GetData(0, ref concrete);

            if(concrete.Concrete == null)
            {
                throw new System.ArgumentException("Material is not a concrete material.");
            }

            bool elastoPlastic = true;
            DA.GetData(1, ref elastoPlastic);

            bool plasticHardening = true;
            DA.GetData(2, ref plasticHardening);

            string crushing = "Prager";
            DA.GetData(3, ref crushing);

            bool tensionStrength = true;
            DA.GetData(4, ref tensionStrength);

            bool reducedShearStiffness = false;
            DA.GetData(5, ref reducedShearStiffness);

            bool ultimateStrainRebars = true;
            DA.GetData(6, ref ultimateStrainRebars);

            var modifiedMaterial = concrete.SetConcretePlasticity(elastoPlastic, plasticHardening, (CrushingCriterion)Enum.Parse(typeof(CrushingCriterion), crushing), tensionStrength, TensionStiffening.Hinton, ReducedCompression.Vecchio1, reducedShearStiffness, ultimateStrainRebars);

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
            get { return new Guid("{76728C09-5064-4646-936C-C215CA807FD2}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }
}