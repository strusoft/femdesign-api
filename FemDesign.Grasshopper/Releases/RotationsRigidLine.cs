// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class RotationsRigidLine: GH_Component
    {
        public RotationsRigidLine(): base("Rotations.RigidLine", "RigidLine", "Define a rigid rotations release for a line-type release (1e+07 kNm/m/rad).", CategoryName.Name(), SubCategoryName.Cat5())
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
            FemDesign.Releases.Rotations obj = FemDesign.Releases.Rotations.RigidLine();

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.RotationsRigidLine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("214724d1-c9cf-4b22-8c71-d628f8e27f6c"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}