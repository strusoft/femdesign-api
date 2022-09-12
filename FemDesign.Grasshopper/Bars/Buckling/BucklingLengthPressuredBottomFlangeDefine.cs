// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.GenericClasses;

namespace FemDesign.Grasshopper
{
    public class BarsPressuredBottomFlangeDefine: GH_Component
    {
        public BarsPressuredBottomFlangeDefine(): base("BucklingLength.PressuredBottomFlangeDefine", "PressuredBottomFlangeDefine", "Define BucklingLength for Pressured Bottom Flange.", CategoryName.Name(), SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Beta", "Beta", "Beta factor.", GH_ParamAccess.item, 1);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("LoadPosition", "LoadPosition", "Load position. top/center/bottom", GH_ParamAccess.item, "top");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("ContinuouslyRestrained", "ContinuouslyRestrained", "Continuously restrained. True/false.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BucklingLength", "BucklingLength", "BucklingLength.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double beta = 1;
            string loadPosition = "top";
            bool continuouslyRestrained = false;
            if (!DA.GetData(0, ref beta))
            {
                // pass
            }
            if (!DA.GetData(1, ref loadPosition))
            {
                // pass
            }
            if (!DA.GetData(2, ref continuouslyRestrained))
            {
                // pass
            }
            if (loadPosition == null)
            {
                return;
            }

            VerticalAlignment alignment = EnumParser.Parse<VerticalAlignment>(loadPosition);
            DA.SetData(0, FemDesign.Bars.Buckling.BucklingLength.PressuredBottomFlange(alignment, beta, continuouslyRestrained));
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PressuredFlangeBottom;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("955edc91-e2d0-4a54-b3e8-73e99354a56c"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.senary;

    }
}