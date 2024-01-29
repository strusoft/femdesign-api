// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class RotationsPlasticLimitsDefine: FEM_Design_API_Component
    {
        public RotationsPlasticLimitsDefine(): base("RotationsPlasticLimits.Define", "Define", "Define rotational stiffness values for plastic limits [kNm or kNm/m].", CategoryName.Name(), SubCategoryName.Cat5())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("x_neg", "x_neg", "Kx' compression [kNm or kNm/m]. Default value empty means no plastic limit.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("x_pos", "x_pos", "Kx' tension [kNm or kNm/m]. Default value empty means no plastic limit.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("y_neg", "y_neg", "Ky' compression [kNm or kNm/m]. Default value empty means no plastic limit.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("y_pos", "y_pos", "Ky' tension [kNm or kNm/m]. Default value empty means no plastic limit.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("z_neg", "z_neg", "Kz' compression [kNm or kNm/m]. Default value empty means no plastic limit.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("z_pos", "z_pos", "Kz' tension [kNm or kNm/m]. Default value empty means no plastic limit.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("RotationsPlasticLimits", "RotationsPlasticLimits", "RotationsPlasticLimits.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double? x_neg = 0.00;
            double? x_pos = 0.00;

            double? y_neg = 0.00;
            double? y_pos = 0.00;

            double? z_neg = 0.00;
            double? z_pos = 0.00;

            DA.GetData(0, ref x_neg);
            DA.GetData(1, ref x_pos);
            DA.GetData(2, ref y_neg);
            DA.GetData(3, ref y_pos);
            DA.GetData(4, ref z_neg);
            DA.GetData(5, ref z_pos);

            FemDesign.Releases.RotationsPlasticLimits obj = new FemDesign.Releases.RotationsPlasticLimits(x_neg, x_pos, y_neg, y_pos, z_neg, z_pos);

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
            get { return new Guid("88b1043f-c344-4b8b-b7af-883996b6ae9f"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;

    }
}