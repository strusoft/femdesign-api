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
            pManager.AddTextParameter("Name", "Name", "Name.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("ksys", "ksys", "System strenght factor", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("kcr", "kcr", " reduction_factor_type\nEC5-1-1: 6.1.7", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("kdef U", "kdef U", "kdef (U, Ua, Us).", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("kdef Sq", "kdef Sq", "kdef (Sq).", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("kdef Sf", "kdef Sf", "kdef (Sf).", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("kdef Sc", "kdef Sc", "kdef (Sc).", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("serviceClass", "serviceClass", "Service class 1/2/3.", GH_ParamAccess.item);
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
            string name = "";
            double ksys = 0;
            double kcr = 0;
            double kdefU = 0;
            double kdefSq = 0.60;
            double kdefSf = 0.60;
            double kdefSc = 0.60;
            int serviceClass = 1;


            if (!DA.GetData(0, ref material)) { return; }
            var newMaterial = material.DeepClone();
            newMaterial.EntityCreated();

            if (DA.GetData("Name", ref name))
            {
                newMaterial.Name = name;
            }
            else
            {
                newMaterial.Name += "_modified";
            }


            if (DA.GetData("ksys", ref ksys)) { newMaterial.Timber.ksys = ksys; }
            if (DA.GetData("kcr", ref kcr)) { newMaterial.Timber.k_cr = kcr; }
            if (DA.GetData("ksys", ref kdefU)) { newMaterial.Timber.kdefU = kdefU; }
            if (DA.GetData("ksys", ref kdefSq)) { newMaterial.Timber.kdefSq = kdefSq; }
            if (DA.GetData("ksys", ref kdefSf)) { newMaterial.Timber.kdefSf = kdefSf; }
            if (DA.GetData("ksys", ref kdefSc)) { newMaterial.Timber.kdefSc = kdefSc; }
            if (DA.GetData("serviceClass", ref serviceClass)) { newMaterial.Timber.ServiceClass = (int)(FemDesign.Materials.TimberServiceClassEnum)(serviceClass - 1); }

            // set output
            DA.SetData(0, newMaterial);
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

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
    }
}