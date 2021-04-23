// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.GH
{
    public class MotionsRigidLine: GH_Component
    {
        public MotionsRigidLine(): base("Motions.RigidLine", "RigidLine", "Define a rigid motions release for a line-type release (1.000e+7).", "FemDesign", "Releases")
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
            FemDesign.Releases.Motions obj = FemDesign.Releases.Motions.RigidLine();

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("bf81bc4e-e76d-482d-a983-37afbc0148bb"); }
        }
    } 
}