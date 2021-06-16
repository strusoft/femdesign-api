// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class MaterialTimberFactorsDefine : GH_Component
    {
        public MaterialTimberFactorsDefine() : base("TimberFactors.Define", "Define", "Define timber factor parameters for a timber panel type.", "FemDesign", "Materials")
        {
            
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Gamma mu", "Gamma mu", "Gamma mu.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Gamma ma", "Gamma ma", "Gamma ma.", GH_ParamAccess.item);
            pManager.AddNumberParameter("kdef U", "kdef U", "kdef (U, Ua, Us).", GH_ParamAccess.item);
            pManager.AddNumberParameter("kdef Sq", "kdef Sq", "kdef (Sq).", GH_ParamAccess.item);
            pManager.AddNumberParameter("kdef Sf", "kdef Sf", "kdef (Sf).", GH_ParamAccess.item);
            pManager.AddNumberParameter("kdef Sc", "kdef Sc", "kdef (Sc).", GH_ParamAccess.item);
            pManager.AddIntegerParameter("serviceClass", "serviceClass", "Service class.", GH_ParamAccess.item);
            pManager.AddNumberParameter("systemFactor", "systemFactor", "System factor.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TimberFactors", "TimberFactors", "TimberFactors.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double gammaMU = 0.0, gammaMAs = 0.0, kdefU = 0.0, kdefSq = 0.0, kdefSf = 0.0, kdefSc = 0.0, systemFactor = 1.0;
            int serviceClass = 0;

            if (!DA.GetData("Gamma mu", ref gammaMU)) return;
            if (!DA.GetData("Gamma ma", ref gammaMAs)) return;
            if (!DA.GetData("kdef U", ref kdefU)) return;
            if (!DA.GetData("kdef Sq", ref kdefSq)) return;
            if (!DA.GetData("kdef Sf", ref kdefSf)) return;
            if (!DA.GetData("kdef Sc", ref kdefSc)) return;

            if (!DA.GetData("serviceClass", ref serviceClass)) return;

            if (!DA.GetData("systemFactor", ref systemFactor)) return;

            FemDesign.Materials.TimberFactors obj = new Materials.TimberFactors(gammaMU, gammaMAs, kdefU, kdefSq, kdefSf, kdefSc, serviceClass, systemFactor);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("b0a30079-81b6-4a7c-b9b6-3c659a62a13f"); }
        }
    }
}