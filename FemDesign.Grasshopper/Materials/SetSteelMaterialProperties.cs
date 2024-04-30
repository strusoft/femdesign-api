// https://strusoft.com/
using System;
using System.Collections.Generic;
using FemDesign.Soil;
using Grasshopper.Kernel;

using FemDesign.Materials;

namespace FemDesign.Grasshopper
{
    public class MaterialSetSteelMaterialProperties : FEM_Design_API_Component
    {
        public MaterialSetSteelMaterialProperties() : base("SetSteelMaterialProperties", "SetSteelMaterialProperties", "Set plasticity parameters to a steel Material.", CategoryName.Name(), SubCategoryName.Cat4a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Elasto-PlasticBehaviour", "Elasto-PlasticBehaviour", "Elasto-Plastic Behaviour.\n1 or 4 values. In case of 4 values, the data will be remapped to U, Sq, Sf, Sc combinations.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("StrainLimit", "StrainLimit", "Elasto-plastic strain limit in tension/compression [%]. Default is 2.5\n 0.00 means no strain limit.\n1 or 4 values. In case of 4 values, the data will be remapped to U, Sq, Sf, Sc combinations.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var material = new FemDesign.Materials.Material();
            DA.GetData(0, ref material);

            if(material.Steel == null)
            {
                throw new System.ArgumentException("Material is not a steel material.");
            }

            var _elastoPlasticBehaviour = new List<bool>();
            if(!DA.GetDataList(1, _elastoPlasticBehaviour))
            {
                // default value
                _elastoPlasticBehaviour = new List<bool> { true };
            }

            var _strainLimit = new List<double>();
            if(!DA.GetDataList(2, _strainLimit))
            {
                // default value
                _strainLimit = new List<double> { 2.5 };
            };

            if(_elastoPlasticBehaviour.Count == 1 && _strainLimit.Count == 1)
            {
                var plastic = _elastoPlasticBehaviour[0];
                var strain = _strainLimit[0];
                var modifiedMaterial = material.SetSteelPlasticity(plastic, strain);

                // set output
                DA.SetData(0, modifiedMaterial);
            }
            else
            {
                var modifiedMaterial = material.SetSteelPlasticity( _elastoPlasticBehaviour, _strainLimit );
                DA.SetData(0, modifiedMaterial);
            }

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.MaterialSetSteelMaterialProperties;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{F3F891AB-3FDD-4C37-ABEE-C5CC120EFC4C}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }
}