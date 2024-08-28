using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Components.UIWidgets;

namespace FemDesign.Grasshopper
{
    public class PeakSmAutoSettings : SubComponent
    {
        public override string name() => "PeakSmoothingAutoSettings";
        public override string display_name() => "PeakSmoothingAutoSettings";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "Auto peak smoothing region around the specified objects. For more details, see the FEM-Design GUI > Settings > Calculation > Peak smoothing.");
            evaluationUnit.Icon = FemDesign.Properties.Resources.Config;
            mngr.RegisterUnit(evaluationUnit);

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Beam end points", "Beam", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Column end points", "Column", "Pile head.\nDefault is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Truss end points", "Truss", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Fictitious bar end points", "FictBar", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Plate intersection end points", "PlateIntersect", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Wall intersection end points", "WallIntersect", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Fict. shell intersection end points", "FictShellIntersect", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Point supports", "PtSupp", "Default is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Line support end points", "LnSupp", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Surface support break points", "SrfSupp", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Point connections", "PtConn", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Line connection end points", "LnConn", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Surface connection break points", "SrfConn", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "Factor", "f", "Peak smoothing region factor.\n\nr = t/2 + f*v\n" +
                "Where:\n'r' is the distance between the axis of the pile and the edge of the pk. smoothing region projected onto the mid-plane of the slab;" +
                "\n't' is the thickness of the pile head;\n'v' is the slab thickness;\n'f' value can be set between 0.00 and 5.00;\nFor more details, see the FEM-Design GUI.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            bool beam = false;
            DA.GetData(0, ref beam);

            bool pile = true;
            DA.GetData(0, ref pile);

            bool truss = false;
            DA.GetData(0, ref truss);

            bool fictBar = false;
            DA.GetData(0, ref fictBar);

            bool plateIntsec = false;
            DA.GetData(0, ref plateIntsec);

            bool wallIntsec = false;
            DA.GetData(0, ref wallIntsec);

            bool fictShell = false;
            DA.GetData(0, ref fictShell);

            bool ptSupp = true;
            DA.GetData(0, ref ptSupp);

            bool lnSupp = false;
            DA.GetData(0, ref lnSupp);

            bool srfSupp = false;
            DA.GetData(0, ref srfSupp);

            bool ptConn = false;
            DA.GetData(0, ref ptConn);

            bool lnConn = false;
            DA.GetData(0, ref lnConn);

            bool srfConn = false;
            DA.GetData(0, ref srfConn);

            double factor = 0.5;
            DA.GetData(0, ref factor);

            var autoPkSm = new Calculate.PeaksmAuto(beam, pile, truss, fictBar, plateIntsec, wallIntsec, fictShell, ptSupp, lnSupp, srfSupp, ptConn, lnConn, srfConn, factor);

            DA.SetData(0, autoPkSm);
        }
    }
}