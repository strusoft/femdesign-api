// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class MaterialTimberFactorsConstruct : GH_Component
    {
        public MaterialTimberFactorsConstruct() : base("TimberFactors.Construct", "Construct", "Define timber factor parameters for a timber panel type.", CategoryName.Name(), SubCategoryName.Cat4a())
        {
            
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Gamma mu", "Gamma mu", "Gamma mu.", GH_ParamAccess.item, 1.25);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Gamma ma", "Gamma ma", "Gamma ma.", GH_ParamAccess.item, 1.00);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("kdef U", "kdef U", "kdef (U, Ua, Us).", GH_ParamAccess.item, 0.00);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("kdef Sq", "kdef Sq", "kdef (Sq).", GH_ParamAccess.item, 0.60);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("kdef Sf", "kdef Sf", "kdef (Sf).", GH_ParamAccess.item, 0.60);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("kdef Sc", "kdef Sc", "kdef (Sc).", GH_ParamAccess.item, 0.60);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("serviceClass", "serviceClass", "Service class 1/2/3.", GH_ParamAccess.item, 1);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("systemFactor", "systemFactor", "System factor.", GH_ParamAccess.item, 1.00);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TimberFactors", "TimberFactors", "TimberFactors.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double gammaMU = 1.25, gammaMAs = 1.00, kdefU = 0.0, kdefSq = 0.60, kdefSf = 0.60, kdefSc = 0.60, systemFactor = 1.0;
            int serviceClass = 1;

            DA.GetData("Gamma mu", ref gammaMU);
            DA.GetData("Gamma ma", ref gammaMAs);
            DA.GetData("kdef U", ref kdefU);
            DA.GetData("kdef Sq", ref kdefSq);
            DA.GetData("kdef Sf", ref kdefSf);
            DA.GetData("kdef Sc", ref kdefSc);
            DA.GetData("serviceClass", ref serviceClass);
            DA.GetData("systemFactor", ref systemFactor);

            // parse enum
            // test value
            if (!Enum.IsDefined(typeof(FemDesign.Materials.TimberServiceClassEnum), serviceClass - 1))
            {    
                throw new System.ArgumentException($"Incorrect service class value: {serviceClass}. Value should be 1, 2 or 3");
            }
            // set enum if exception was not thrown
            var sClass = (FemDesign.Materials.TimberServiceClassEnum)(serviceClass - 1);

            // create object
            FemDesign.Materials.TimberFactors obj = new Materials.TimberFactors(gammaMU, gammaMAs, kdefU, kdefSq, kdefSf, kdefSc, sClass, systemFactor);

            // return
            DA.SetData(0, obj);
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
            get { return new Guid("31d14871-70d6-43ba-98fe-ae870ca1efb9"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}