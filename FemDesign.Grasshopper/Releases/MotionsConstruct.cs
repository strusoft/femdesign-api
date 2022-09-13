// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class MotionsConstruct: GH_Component
    {
        public MotionsConstruct(): base("Motions.Construct", "Construct", "Construct a new motions release [kN/m or kN/m/m].", CategoryName.Name(), SubCategoryName.Cat5())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("x_neg", "x_neg", "Kx' compression [kN/m or kN/m/m].", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("x_pos", "x_pos", "Kx' tension [kN/m or kN/m/m].", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("y_neg", "y_neg", "Ky' compression [kN/m or kN/m/m].", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("y_pos", "y_pos", "Ky' tension [kN/m or kN/m/m].", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("z_neg", "z_neg", "Kz' compression [kN/m or kN/m/m].", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("z_pos", "z_pos", "Kz' tension [kN/m or kN/m/m].", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Motions", "Motions", "Motions.", GH_ParamAccess.item);
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
            FemDesign.Releases.Motions obj = FemDesign.Releases.Motions.Define(x_neg, x_pos, y_neg, y_pos, z_neg, z_pos);

            // return
            DA.SetData(0, obj);
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
            get { return new Guid("807b2b9d-eafb-4b59-9eda-89d7150c6427"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}