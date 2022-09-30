// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class RotationsRigidPoint: GH_Component
    {
        public RotationsRigidPoint(): base("Rotations.RigidPoint", "RigidPoint", "Define a rigid rotations release for a point-type release (1e+10 kNm/rad).", CategoryName.Name(), SubCategoryName.Cat5())
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
            FemDesign.Releases.Rotations obj = FemDesign.Releases.Rotations.RigidPoint();

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.RotationsRigidPoint;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("214724d1-c9cf-4b22-8c71-d328f8e27f6c"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}