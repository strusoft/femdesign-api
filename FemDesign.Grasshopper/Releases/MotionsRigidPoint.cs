// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class MotionsRigidPoint: GH_Component
    {
        public MotionsRigidPoint(): base("Motions.RigidPoint", "RigidPoint", "Define a rigid motions release for a point-type release (1.000e+10).", CategoryName.Name(), SubCategoryName.Cat5())
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
            FemDesign.Releases.Motions obj = FemDesign.Releases.Motions.RigidPoint();

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.MotionsRigidPoint;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("bf81bc4e-e76d-482d-a983-37afbc0548bb"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}