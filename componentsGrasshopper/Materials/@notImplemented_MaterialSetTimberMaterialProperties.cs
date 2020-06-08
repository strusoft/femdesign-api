// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace GH_FemDesign
{
    public class MaterialSetTimberMaterialProperties: GH_Component
    {
        public MaterialSetTimberMaterialProperties(): base("Material.SetTimberMaterialProperties", "SetTimberMaterialProperties", "Set creep and shrinkage parameters to a concrete Material.", "FemDesign", "Materials")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
            pManager.AddNumberParameter("ksys", "ksys", "System strength factor.", GH_ParamAccess.item);
            pManager.AddNumberParameter("k_cr", "k_cr", "k_cr. Between or equal to 0 and 1.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("service_class", "service_class", "service_class. 1,2 or 3.", GH_ParamAccess.item);
            pManager.AddNumberParameter("kdefU", "kdefU", "kdefU.", GH_ParamAccess.item);
            pManager.AddNumberParameter("kdefSq", "kdefSq", "kdefSq.", GH_ParamAccess.item);
            pManager.AddNumberParameter("kdefSf", "kdefSf", "kdefSf.", GH_ParamAccess.item);
            pManager.AddNumberParameter("kdefSc", "kdefSc", "kdefSc.", GH_ParamAccess.item);
        } 
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Materials.Material material = null;
            double ksys = 0;
            double k_cr = 0;
            int service_class = 0;
            double kdefU = 0;
            double kdefSq = 0;
            double kdefSf = 0;
            double kdefSc = 0;
            if (!DA.GetData(0, ref material)) { return; }
            if (!DA.GetData(1, ref ksys))
            {
                // pass
            }
            if (!DA.GetData(2, ref k_cr))
            {
                // pass
            }
            if (!DA.GetData(3, ref service_class))
            {
                // pass
            }
            if (!DA.GetData(4, ref kdefU))
            {
                // pass
            }
            if (!DA.GetData(5, ref kdefSq))
            {
                // pass
            }
            if (!DA.GetData(6, ref kdefSf))
            {
                // pass
            }
            if (!DA.GetData(7, ref kdefSc))
            {
                // pass
            }
            if (material == null) { return; }

            //
            FemDesign.Materials.Material modifiedMaterial = FemDesign.Materials.Material.SetTimberMaterialProperties(material, ksys, k_cr, service_class, kdefU, kdefSq, kdefSf, kdefSc);
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
            get { return new Guid("1871fa03-17fe-4580-9091-976da4a1b4ee"); }
        }
    }
}