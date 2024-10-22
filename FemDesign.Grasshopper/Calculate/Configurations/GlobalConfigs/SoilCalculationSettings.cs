using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Components.UIWidgets;

namespace FemDesign.Grasshopper
{
    public class SoilCalculationSettings : SubComponent
    {
        public override string name() => "SoilCalculationSettings";
        public override string display_name() => "SoilCalculationSettings";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "For more details, see the FEM-Design GUI > Settings > Calculation > Soil calculation.");
            evaluationUnit.Icon = FemDesign.Properties.Resources.Config;
            mngr.RegisterUnit(evaluationUnit);

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "CalcAsSolid", "CalcAsSolid", "Calculate soil as solid element.\nDefault is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            bool solid = false;
            DA.GetData(0, ref solid);

            var soilCalc = new Calculate.SoilCalculation(solid);

            DA.SetData(0, soilCalc);
        }
    }
}