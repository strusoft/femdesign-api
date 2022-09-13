// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class EdgeConnectionHinged: GH_Component
    {
        public EdgeConnectionHinged(): base("EdgeConnection.Hinged", "Hinged", "Create a Hinged EdgeConnection.", CategoryName.Name(), SubCategoryName.Cat2b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("EdgeConnection", "EdgeConnection", "EdgeConnection.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //
            FemDesign.Shells.EdgeConnection obj = FemDesign.Shells.EdgeConnection.Hinged;

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.EdgeConnectionHinged;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("cedaca89-caa4-40a0-916d-533dc3bf7f8b"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}