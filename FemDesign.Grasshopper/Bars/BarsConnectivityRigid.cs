// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class BarsConnectivityRigid: GH_Component
    {
        public BarsConnectivityRigid(): base("Connectivity.Rigid", "Rigid", "Define Rigid end releases for a bar element.", CategoryName.Name(),
            SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            //
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connectivity", "Connectivity", "Connectivity.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetData(0, FemDesign.Bars.Connectivity.Rigid);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ConnectivityRigid;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("4d958eab-ea4a-4753-a6d6-42f9dc27c96b"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}