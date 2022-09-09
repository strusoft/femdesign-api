// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.GenericClasses;

namespace FemDesign.Grasshopper
{
    public class BarsPressuredTopFlangeDefine: GH_Component
    {
        public BarsPressuredTopFlangeDefine(): base("BucklingLength.PressuredTopFlangeDefine", "PressuredTopFlangeDefine", "Define BucklingLength for Pressured Top Flange.", CategoryName.Name(), SubCategoryName.Cat2a())
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
            DA.SetData(0, FemDesign.Bars.Buckling.BucklingLength.PressuredTopFlange(alignment, beta, continuouslyRestrained));
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PressuredFlangeTop;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("4d0e4c80-c9d5-4490-a316-9ce8df2b304e"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.senary;

    }
}