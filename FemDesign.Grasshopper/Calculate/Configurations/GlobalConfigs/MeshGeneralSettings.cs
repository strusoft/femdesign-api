using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Components.UIWidgets;

namespace FemDesign.Grasshopper
{
    public class MeshGeneralSettings : SubComponent
    {
        public override string name() => "MeshGeneralSettings";
        public override string display_name() => "MeshGeneralSettings";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "Finite element mesh general settings. For more details, see the FEM-Design GUI > Settings > Calculation > Mesh.");
            evaluationUnit.Icon = FemDesign.Properties.Resources.Config;
            mngr.RegisterUnit(evaluationUnit);

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "AdjustMeshToLoads", "AdjustMeshToLoads", "Adjust mesh to load positions.\nDefault is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            bool adjust = true;
            DA.GetData(0, ref adjust);

            var meshGen = new Calculate.MeshGeneral(adjust);

            DA.SetData(0, meshGen);
        }
    }
}