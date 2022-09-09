// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using FemDesign.GenericClasses;

namespace FemDesign.Grasshopper
{
    public class Bars_LateralTorsionalDefine: GH_Component
    {
        public Bars_LateralTorsionalDefine(): base("BucklingLength.LateralTorsionalDefine", "LateralTorsionalDefine", "Define BucklingLength for Lateral Torsional buckling.", CategoryName.Name(), SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("LoadPosition", "LoadPosition", "top/center/bottom", GH_ParamAccess.item, "top");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("ContinuouslyRestrained", "ContinuouslyRestrained", "Continuously restrained. True/false.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Cantilever", "Cantilever", "Cantilever. True/false", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("BucklingLength", "BucklingLength", "BucklingLength.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string loadPosition = "top";
            bool continuouslyRestrained = false;
            bool cantilever = false;
            if (!DA.GetData(0, ref loadPosition))
            {
                // pass
            }
            if (!DA.GetData(1, ref continuouslyRestrained))
            {
                // pass
            }
            if (!DA.GetData(2, ref cantilever))
            {
                // pass
            }
            if (loadPosition == null)
            {
                return;
            }
            VerticalAlignment alignment = EnumParser.Parse<VerticalAlignment>(loadPosition);
            DA.SetData(0, FemDesign.Bars.Buckling.BucklingLength.LateralTorsional(alignment, continuouslyRestrained, cantilever));
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LateralTorsional;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("bc530740-2feb-4203-bce5-63fbab12c287"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.senary;

    }
}