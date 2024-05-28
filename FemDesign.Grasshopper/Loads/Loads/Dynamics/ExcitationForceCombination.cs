// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using FemDesign;
using FemDesign.Calculate;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel.Special;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FemDesign.Loads;
using System.Collections.ObjectModel;

namespace FemDesign.Grasshopper
{
    public class ExcitationForceCombination : FEM_Design_API_Component
    {
        public ExcitationForceCombination() : base("ExcitationCombination.Define", "ExcitationCombination", "", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("dt", "dt", "", GH_ParamAccess.item, 0.01);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Factors", "Factors", "Factors", GH_ParamAccess.list);
            pManager.AddGenericParameter("Diagrams", "Diagrams", "Diagrams", GH_ParamAccess.list);
            pManager.AddGenericParameter("LoadCases", "LoadCases", "LoadCases", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ExForceCombinations", "ExForceCombinations", "", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = "";
            DA.GetData(0, ref name);

            double dt = 0.01;
            DA.GetData(1, ref dt);

            List<double> factors = new List<double>();
            DA.GetDataList(2, factors);

            List<FemDesign.Loads.Diagram> diagrams = new List<FemDesign.Loads.Diagram>();
            DA.GetDataList(3, diagrams);

            List<FemDesign.Loads.LoadCase> loadCases = new List<FemDesign.Loads.LoadCase>();
            DA.GetDataList(4, loadCases);

            if (factors.Count != diagrams.Count || factors.Count != loadCases.Count || diagrams.Count != loadCases.Count)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"The number of `Factors` ({factors.Count}), `Diagrams` ({diagrams.Count})  and `LoadCases` ({loadCases.Count}) must be equal.");
                return;
            }


            var excitationForceLoadCase = new List<FemDesign.Loads.ExcitationForceLoadCase>();

            for (int i = 0; i < factors.Count; i++)
            {
                excitationForceLoadCase.Add(new FemDesign.Loads.ExcitationForceLoadCase(loadCases[i], factors[i], diagrams[i]));
            }


            var combinations = new FemDesign.Loads.ExcitationForceCombination()
            {
                Name = name,
                dT = dt,
                records = excitationForceLoadCase,
            };

            DA.SetData(0, combinations);
        }


        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ExcitationForceCombination;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{56BECF17-D8E3-48E7-AB59-361E841D6608}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.obscure;
    }
}