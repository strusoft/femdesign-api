// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class MotionsFree: GH_Component
    {
        public MotionsFree(): base("Motions.Free", "Free", "Define a free motions release.", CategoryName.Name(), SubCategoryName.Cat5())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Motions", "Motions", "Motions.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //
            FemDesign.Releases.Motions obj = FemDesign.Releases.Motions.Free();

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.MotionsFree;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("7f26ab11-7a13-442d-89c5-bc95ce1ce3f1"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}