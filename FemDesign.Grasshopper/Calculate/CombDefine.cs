// https://strusoft.com/
using System;
using System.Collections.Generic;
using FemDesign.Calculate;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class CombDefine : FEM_Design_API_Component
    {
        public CombDefine() : base("Comb.Define", "Comb", "Define calculation parameters for the Load combinations calculation type. To setup which analysis types to consider for a specific load combination - use LoadCombination.SetupCalculation", CategoryName.Name(), SubCategoryName.Cat7a())
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
            pManager.AddBooleanParameter("PlKeepLoadStep", "PlKeepLoadStep", "Keep reduced load step after it has been reduced by the solver", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("PlTolerance", "PlTolerance", "Global tolerance value [‰]", GH_ParamAccess.item, 1);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("Plmaxeqiter", "Plmaxeqiter", "Plastic analysis: Maximum equilibrium iteration number.", GH_ParamAccess.item, 50);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("PlShellLayers", "PlShellLayers", "Number of layers in the elasto-plastic shells", GH_ParamAccess.item, 10);
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
            pManager.AddGenericParameter("CombSettings", "CombSettings", "", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Comb", "Comb", "Comb.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            int NLEmaxiter = 30;
            DA.GetData(0, ref NLEmaxiter);

            int PLdefloadstep = 20;
            DA.GetData(1, ref PLdefloadstep);

            int PLminloadstep = 2;
            DA.GetData(2, ref PLminloadstep);
            
            bool PlKeepLoadStep = true;
            DA.GetData(3, ref PlKeepLoadStep);
            
            int PlTolerance = 1;
            DA.GetData(4, ref PlTolerance);
            
            int PLmaxeqiter = 50;
            DA.GetData(5, ref PLmaxeqiter);
            
            int PlShellLayers = 10;
            DA.GetData(6, ref PlShellLayers);
            
            bool NLSMohr = true;
            DA.GetData(7, ref NLSMohr);
            
            int NLSinitloadstep = 10;
            DA.GetData(8, ref NLSinitloadstep);
            
            int NLSminloadstep = 10;
            DA.GetData(9, ref NLSminloadstep);
            
            int NLSactiveelemratio = 5;
            DA.GetData(10, ref NLSactiveelemratio);
            
            int NLSplasticelemratio = 5;
            DA.GetData(11, ref NLSplasticelemratio);
            
            int CRloadstep = 20;
            DA.GetData(12, ref CRloadstep);
            
            int CRmaxiter = 30;
            DA.GetData(13, ref CRmaxiter);
            
            int CRstifferror = 2;
            DA.GetData(14, ref CRstifferror);

            List<FemDesign.Calculate.CombItem> combItem = new List<Calculate.CombItem>();
            DA.GetDataList(15, combItem);

            FemDesign.Calculate.Comb obj = new FemDesign.Calculate.Comb(NLEmaxiter, PLdefloadstep, PLminloadstep, PlKeepLoadStep, PlTolerance, PLmaxeqiter, PlShellLayers, NLSMohr, NLSinitloadstep, NLSminloadstep, NLSactiveelemratio, NLSplasticelemratio, CRloadstep, CRmaxiter, CRstifferror, combItem);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.CombDefine2;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{A8449E63-2DFF-440D-9D38-5D2267FCB209}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}