// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class RotationsConstruct: GH_Component
    {
        public RotationsConstruct(): base("Rotations.Construct", "Construct", "Construct a new rotations release [kNm/rad or kNm/m/rad].", CategoryName.Name(), SubCategoryName.Cat5())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("x_neg", "x_neg", "Cx' compression [kNm/rad or kNm/m/rad].", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("x_pos", "x_pos", "Cx' tension [kNm/rad or kNm/m/rad].", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("y_neg", "y_neg", "Cy' compression [kNm/rad or kNm/m/rad].", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("y_pos", "y_pos", "Cy' tension [kNm/rad or kNm/m/rad].", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("z_neg", "z_neg", "Cz' compression [kNm/rad or kNm/m/rad].", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("z_pos", "z_pos", "Cz' tension [kNm/rad or kNm/m/rad].", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Rotations", "Rotations", "Rotations.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            double x_neg = 0, x_pos = 0, y_neg = 0, y_pos = 0, z_neg = 0, z_pos = 0;
            if(!DA.GetData(0, ref x_neg))
            {
                // pass
            }
            if(!DA.GetData(1, ref x_pos))
            {
                // pass
            }
            if(!DA.GetData(2, ref y_neg))
            {
                // pass
            }
            if(!DA.GetData(3, ref y_pos))
            {
                // pass
            }
            if(!DA.GetData(4, ref z_neg))
            {
                // pass
            }
            if(!DA.GetData(5, ref z_pos))
            {
                // pass
            }

            //
            FemDesign.Releases.Rotations obj = FemDesign.Releases.Rotations.Define(x_neg, x_pos, y_neg, y_pos, z_neg, z_pos);

            // return
            DA.SetData(0, obj);
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
            get { return new Guid("310ca6da-196c-4961-a345-9a7346bfa054"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}