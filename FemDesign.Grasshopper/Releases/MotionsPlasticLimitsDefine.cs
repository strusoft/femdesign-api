// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class MotionsPlasticLimitsDefine: GH_Component
    {
        public MotionsPlasticLimitsDefine(): base("MotionsPlasticLimits.Define", "Define", "Define a new motions release [kN/m or kN/m/m].", "FemDesign", "Releases")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            FemDesign.Releases.MotionsPlasticLimits obj = new FemDesign.Releases.MotionsPlasticLimits(); // Get default values
            pManager.AddNumberParameter("x_neg", "x_neg", "Kx' compression [kN/m or kN/m/m].", GH_ParamAccess.item, obj.XNeg);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("x_pos", "x_pos", "Kx' tension [kN/m or kN/m/m].", GH_ParamAccess.item, obj.XPos);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("y_neg", "y_neg", "Ky' compression [kN/m or kN/m/m].", GH_ParamAccess.item, obj.YNeg);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("y_pos", "y_pos", "Ky' tension [kN/m or kN/m/m].", GH_ParamAccess.item, obj.YPos);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("z_neg", "z_neg", "Kz' compression [kN/m or kN/m/m].", GH_ParamAccess.item, obj.ZNeg);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("z_pos", "z_pos", "Kz' tension [kN/m or kN/m/m].", GH_ParamAccess.item, obj.ZPos);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("MotionsPlasticLimits", "MotionsPlasticLimits", "MotionsPlasticLimits.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Releases.MotionsPlasticLimits obj = new FemDesign.Releases.MotionsPlasticLimits();

            double x_neg = obj.XNeg, x_pos = obj.XPos, y_neg = obj.YNeg, y_pos = obj.YPos, z_neg = obj.ZNeg, z_pos = obj.ZPos;
            if(DA.GetData(0, ref x_neg))
            {
                obj.XNeg = x_neg;
            }
            if(DA.GetData(1, ref x_pos))
            {
                obj.XPos = x_pos;
            }
            if (DA.GetData(2, ref y_neg))
            {
                obj.YNeg = y_neg;
            }
            if(DA.GetData(3, ref y_pos))
            {
                obj.YPos = y_pos;
            }
            if(DA.GetData(4, ref z_neg))
            {
                obj.ZNeg = z_neg;
            }
            if(DA.GetData(5, ref z_pos))
            {
                obj.ZPos = z_pos;
            }

            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.MotionsPlasticLimitsDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("834b0c8b-5224-4719-832f-7561f91a3bd2"); }
        }
    } 
}