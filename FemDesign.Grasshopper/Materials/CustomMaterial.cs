// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class CustomMaterial : GH_Component
    {
        public CustomMaterial() : base("CustomMaterial", "CustomMaterial", ".", CategoryName.Name(), SubCategoryName.Cat4a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Name", "Name", ".", GH_ParamAccess.item);
            pManager.AddNumberParameter("E", "E", "Modulus of Elasticity [kN/m²]", GH_ParamAccess.item);
            pManager.AddNumberParameter("nu", "nu", "Poisson's Ratio", GH_ParamAccess.item);
            pManager.AddNumberParameter("alpha", "alpha", "Coeffiecient Thermal Expansion [1/C°]", GH_ParamAccess.item);
            pManager.AddNumberParameter("rho", "rho", "Mass Density [t/m3]", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            string name = null;
            DA.GetData(0, ref name);

            double e = 0.0;
            DA.GetData(1, ref e);

            double nu = 0.0;
            DA.GetData(2, ref nu);

            double alpha = 0.0;
            DA.GetData(3, ref alpha);

            double rho = 0.0;
            DA.GetData(4, ref rho);

            //
            var customMaterial = FemDesign.Materials.Material.CustomUniaxialMaterial(name, rho, e, nu, alpha);
            customMaterial.EntityModified();

            // set output
            DA.SetData(0, customMaterial);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.MaterialDatabaseListMaterialNames;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{9A46A639-27DD-4864-BBD3-8596D6A43DEC}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }
}