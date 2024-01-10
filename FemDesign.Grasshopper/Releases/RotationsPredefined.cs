// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class RotationsPredefined : FEM_Design_API_Component
    {
        public RotationsPredefined(): base("Rotations.Predefined", "Rotations", "Provide predifined rotation stiffness values for point-, line- and surface-type releases.", CategoryName.Name(), SubCategoryName.Cat1())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Free", "F", "Define a free rotation release.", GH_ParamAccess.item);
            pManager.AddGenericParameter("RigidPoint", "Pt", "Define a rigid rotation release for a point-type release (1e+10 kNm/rad).", GH_ParamAccess.item);
            pManager.AddGenericParameter("RigidPLine", "Ln", "Define a rigid rotation release for a line-type release (1e+07 kNm/m/rad).", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Releases.Rotations rotFree = FemDesign.Releases.Rotations.Free();
            FemDesign.Releases.Rotations rotPoint = FemDesign.Releases.Rotations.RigidPoint();
            FemDesign.Releases.Rotations rotLine = FemDesign.Releases.Rotations.RigidLine();

            // return
            DA.SetData(0, rotFree);
            DA.SetData(1, rotPoint);
            DA.SetData(2, rotLine);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.RotationsDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("283CA555-FF55-42FE-8AA1-295EE1129D97"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quinary;

    }
}