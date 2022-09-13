// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class BarsConnectivityHinged: GH_Component
    {
        public BarsConnectivityHinged(): base("Connectivity.Hinged", "Hinged", "Define hinged releases for a bar element.", CategoryName.Name(),
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
            DA.SetData(0, FemDesign.Bars.Connectivity.Hinged);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ConnectivityHinged;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("a37aad78-7306-486f-8ef2-57bfd986307d"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}