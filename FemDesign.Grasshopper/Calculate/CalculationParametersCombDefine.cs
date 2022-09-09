// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class CalculationParametersCombDefine: GH_Component
    {
        public CalculationParametersCombDefine(): base("Comb.Define", "Define", "Define calculation parameters for the Load combinations calculation type. To setup which analysis types to consider for a specific load combination - use LoadCombination.SetupCalculation", CategoryName.Name(), SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
           pManager.AddIntegerParameter("NLEmaxiter", "NLEmaxiter", "Non-linear elastic analysis: Maximum iteration number.", GH_ParamAccess.item, 30);
           pManager[pManager.ParamCount - 1].Optional = true; 
           pManager.AddIntegerParameter("PLdefloadstep", "PLdefloadstep", "Plastic analysis: Default load step in % of the total load.", GH_ParamAccess.item, 20);
           pManager[pManager.ParamCount - 1].Optional = true; 
           pManager.AddIntegerParameter("PLminloadstep", "PLminloadstep", "Plastic analysis: Minimal load step [%]", GH_ParamAccess.item, 2);
           pManager[pManager.ParamCount - 1].Optional = true; 
           pManager.AddIntegerParameter("Plmaxeqiter", "Plmaxeqiter", "Plastic analysis: Maximum equilibrium iteration number.", GH_ParamAccess.item, 30);
           pManager[pManager.ParamCount - 1].Optional = true; 
           pManager.AddBooleanParameter("NLSMohr", "NLSMohr", "Non-linear soil: Consider Mohr-Coulomb criteria.", GH_ParamAccess.item, true);
           pManager[pManager.ParamCount - 1].Optional = true; 
           pManager.AddIntegerParameter("NLSinitloadstep", "NLSinitloadstep", "Non-linear soil: Initial load step [%]", GH_ParamAccess.item, 10);
           pManager[pManager.ParamCount - 1].Optional = true; 
           pManager.AddIntegerParameter("NLSminloadstep", "NLSminloadstep", "Non-linear soil: Minimal load step [%]", GH_ParamAccess.item, 10);
           pManager[pManager.ParamCount - 1].Optional = true; 
           pManager.AddIntegerParameter("NLSactiveelemratio", "NLSactiveelemratio", "Non-linear soil: Volume ratio of nonlinearly active elements in one step [%]", GH_ParamAccess.item, 5);
           pManager[pManager.ParamCount - 1].Optional = true; 
           pManager.AddIntegerParameter("NLSplasticelemratio", "NLSplasticelemratio", "Non-linear soil: Volume ratio of plastic elements in one step [%]", GH_ParamAccess.item, 5);
           pManager[pManager.ParamCount - 1].Optional = true; 
           pManager.AddIntegerParameter("CRloadstep", "CRloadstep", "Cracked section analysis: One load step in % of the total load.", GH_ParamAccess.item, 20);
           pManager[pManager.ParamCount - 1].Optional = true; 
           pManager.AddIntegerParameter("CRmaxiter", "CRmaxiter", "Cracked section analysis: Maximum iteration number.", GH_ParamAccess.item, 30);
           pManager[pManager.ParamCount - 1].Optional = true; 
           pManager.AddIntegerParameter("CRstifferror", "CRstifferror", "Cracked section analysis: Allowed stiffness change error [%]", GH_ParamAccess.item, 2);
           pManager[pManager.ParamCount - 1].Optional = true; 
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Comb", "Comb", "Comb.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int NLEmaxiter = 30;
            int PLdefloadstep = 20;
            int PLminloadstep = 2;
            int PLmaxeqiter = 30;
            bool NLSMohr = true;
            int NLSinitloadstep = 10;
            int NLSminloadstep = 10;
            int NLSactiveelemratio = 5;
            int NLSplasticelemratio = 5;
            int CRloadstep = 20;
            int CRmaxiter = 30;
            int CRstifferror = 2;
            if (!DA.GetData(0, ref NLEmaxiter))
            {
                // pass
            }
            if (!DA.GetData(1, ref PLdefloadstep))
            {
                // pass
            }
            if (!DA.GetData(2, ref PLminloadstep))
            {
                // pass
            }
            if (!DA.GetData(3, ref PLmaxeqiter))
            {
                // pass
            }
            if (!DA.GetData(4, ref NLSMohr))
            {
                // pass
            }
            if (!DA.GetData(5, ref NLSinitloadstep))
            {
                // pass
            }
            if (!DA.GetData(6, ref NLSminloadstep))
            {
                // pass
            }
            if (!DA.GetData(7, ref NLSactiveelemratio))
            {
                // pass
            }
            if (!DA.GetData(8, ref NLSplasticelemratio))
            {
                // pass
            }
            if (!DA.GetData(9, ref CRloadstep))
            {
                // pass
            }
            if (!DA.GetData(10, ref CRmaxiter))
            {
                // pass
            }
            if (!DA.GetData(11, ref CRstifferror))
            {
                // pass
            }

            //
            FemDesign.Calculate.Comb obj = new FemDesign.Calculate.Comb(NLEmaxiter, PLdefloadstep, PLminloadstep, PLmaxeqiter, NLSMohr, NLSinitloadstep, NLSminloadstep, NLSactiveelemratio, NLSplasticelemratio, CRloadstep, CRmaxiter, CRstifferror);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.CombDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("8576ce41-8d1d-4a12-a732-63f5674bb8d6"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}