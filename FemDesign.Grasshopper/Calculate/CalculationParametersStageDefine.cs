// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class CalculationParametersStageDefine: GH_Component
    {
        public CalculationParametersStageDefine(): base("StageSetting.Define", "StageSetting", "Define construction stage method", CategoryName.Name(), SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("ghost", "ghost", "\"Ghost\" construction method. True/false. If false incremental \"Tracking\" method is used.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("TimeDependent", "TimeDependent", "", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("CreepStrainIncrementLimit", "CreepStrainIncrementLimit", "", GH_ParamAccess.item, 2.5);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("StageSetting", "StageSetting", "Construction stages calculation method.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool ghost = false;
            DA.GetData(0, ref ghost);

            bool timeDependent = false;
            DA.GetData(1, ref timeDependent);

            double? creepStrainLimit = 2.5;
            if (!DA.GetData(2, ref creepStrainLimit))
            {
                creepStrainLimit = null;
            }

            if(timeDependent == false && creepStrainLimit != null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "'CreepStrainIncrementLimit' value will not be use. Set 'TimeDependent' == True.");
            }

            //
            var obj = new FemDesign.Calculate.Stage( ghost, timeDependent, creepStrainLimit );

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Stages;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("fb75062e-e1b0-4d68-a8d3-2597dc4712fe"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}