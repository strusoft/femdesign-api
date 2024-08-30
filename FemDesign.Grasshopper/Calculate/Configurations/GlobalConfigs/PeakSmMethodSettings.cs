using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Components.UIWidgets;
using System;
using System.Linq;
using FemDesign.Calculate;

namespace FemDesign.Grasshopper
{
    public class PeakSmMethodSettings : SubComponent
    {
        public override string name() => "PeakSmoothingMethodSettings";
        public override string display_name() => "PeakSmoothingMethodSettings";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "Peak smoothing method settings. For more details, see the FEM-Design GUI > Settings > Calculation > Peak smoothing.");
            evaluationUnit.Icon = FemDesign.Properties.Resources.Config;
            mngr.RegisterUnit(evaluationUnit);

            evaluationUnit.RegisterInputParam(new Param_String(), "M", "M", "Mx', My' and Mx'y'.\n\nConnect 'ValueList' to get the options:\nDontSmooth\nHigherOrderShapeFunc\nConstShapeFunc\nSetToZero", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(PeaksmMethod.PeaksmMethodOptions)).ToList();

            evaluationUnit.RegisterInputParam(new Param_String(), "N", "N", "Nx', Ny' and Nx'y'.\n\nConnect 'ValueList' to get the options:\nDontSmooth\nHigherOrderShapeFunc\nConstShapeFunc\nSetToZero", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(PeaksmMethod.PeaksmMethodOptions)).ToList();

            evaluationUnit.RegisterInputParam(new Param_String(), "V", "V", "Vx', Vy' and Vx'y'.\n\nConnect 'ValueList' to get the options:\nDontSmooth\nHigherOrderShapeFunc\nConstShapeFunc\nSetToZero", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(PeaksmMethod.PeaksmMethodOptions)).ToList();
        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            string _moment = "HigherOrderShapeFunc";
            DA.GetData(0, ref _moment);

            var moment = GenericClasses.EnumParser.Parse<Calculate.PeaksmMethod.PeaksmMethodOptions>(_moment);

            string _normal = "HigherOrderShapeFunc";
            DA.GetData(1, ref _normal);

            var normal = GenericClasses.EnumParser.Parse<Calculate.PeaksmMethod.PeaksmMethodOptions>(_normal);

            string _shear = "HigherOrderShapeFunc";
            DA.GetData(2, ref _shear);

            var shear = GenericClasses.EnumParser.Parse<Calculate.PeaksmMethod.PeaksmMethodOptions>(_shear);

            var pkSmMethod = new Calculate.PeaksmMethod(moment, normal, shear);

            DA.SetData(0, pkSmMethod);
        }
    }
}