// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class MotionsPredefined : FEM_Design_API_Component
    {
        public MotionsPredefined(): base("Motions.Predefined", "Motions", "Provide predifined translation stiffness values for point-, line- and surface-type releases.", CategoryName.Name(), SubCategoryName.Cat1())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Free", "F", "Define a free translation release (0.000e+0).", GH_ParamAccess.item);
            pManager.AddGenericParameter("RigidPoint", "Pt", "Define a rigid translation release for a point-type release (1.000e+10 kN/m).", GH_ParamAccess.item);
            pManager.AddGenericParameter("RigidLine", "Ln", "Define a rigid translation release for a line-type release (1.000e+7 kN/m/m).", GH_ParamAccess.item);
            pManager.AddGenericParameter("RigidSurface", "Srf", "Define a rigid translation release for a surface-type release (1.000e+5 kN/m2/m).", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Releases.Motions motFree = FemDesign.Releases.Motions.Free();
            FemDesign.Releases.Motions motPoint = FemDesign.Releases.Motions.RigidPoint();
            FemDesign.Releases.Motions motLine = FemDesign.Releases.Motions.RigidLine();
            FemDesign.Releases.Motions motSurf = FemDesign.Releases.Motions.RigidSurface();

            // return
            DA.SetData(0, motFree);
            DA.SetData(1, motPoint);
            DA.SetData(2, motLine);
            DA.SetData(3, motSurf);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.MotionsDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("D67136BF-CECA-4AE5-99AF-3A5306AACEC2"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}