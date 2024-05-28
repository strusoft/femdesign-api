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

namespace FemDesign.Grasshopper
{
    public class ExcitationLoad : FEM_Design_API_Component
    {
        public ExcitationLoad() : base("ExcitationLoad.Define", "ExcitationLoad", "", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Diagrams", "Diagrams", "Diagrams", GH_ParamAccess.list);
            pManager.AddGenericParameter("CombinationExcitation", "CombinationExcitation", "", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ExForceLoad", "ExForceLoad", "", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            
            List<FemDesign.Loads.Diagram> diagrams = new List<FemDesign.Loads.Diagram>();
            DA.GetDataList(0, diagrams);

            List<FemDesign.Loads.ExcitationForceCombination> exForceCombo = new List<FemDesign.Loads.ExcitationForceCombination>();
            DA.GetDataList(1, exForceCombo);

            var exForce = new FemDesign.Loads.ExcitationForce(diagrams, exForceCombo);

            DA.SetData(0, exForce);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ExcitationForce;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{DF412D3F-5EB3-4629-881E-B10843E3932D}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.obscure;
    }
}