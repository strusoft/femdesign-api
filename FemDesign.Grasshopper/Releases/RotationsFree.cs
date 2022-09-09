// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class RotationsFree: GH_Component
    {
        public RotationsFree(): base("Rotations.Free", "Free", "Define a Free rotations release.", CategoryName.Name(), SubCategoryName.Cat5())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Rotations", "Rotations", "Rotations.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //
            FemDesign.Releases.Rotations obj = FemDesign.Releases.Rotations.Free();

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.RotationsFree;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("9cbdc562-da0e-4ca3-8459-178532a9ee07"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}