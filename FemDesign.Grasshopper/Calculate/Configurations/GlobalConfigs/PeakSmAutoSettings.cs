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

            evaluationUnit.RegisterInputParam(new Param_Number(), "Factor", "Factor", "Peak smoothing region factor.\n\nr = t/2 + f*v\n" +
                "Where:\n'r' is the distance between the axis of the pile and the edge of the pk. smoothing region projected onto the mid-plane of the slab;" +
                "\n't' is the thickness of the pile head;\n'v' is the slab thickness;\n'f' value can be set between 0.00 and 5.00;\nFor more details, see the FEM-Design GUI.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "BeamEndPoints", "BeamEndPoints", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "ColumnEndPoints", "ColumnEndPoints", "Pile head.\nDefault is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "TrussEndPoints", "TrussEndPoints", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "FictitiousBarEndPoints", "FictitiousBarEndPoints", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "PlateIntersectionEndPoints", "PlateIntersectionEndPoints", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "WallIntersectionEndPoints", "WallIntersectionEndPoints", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "FictShellIntersectionEndPoints", "FictShellIntersectionEndPoints", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "PointSupports", "PointSupports", "Default is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "LineSupportEndPoints", "LineSupportEndPoints", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "SurfaceSupportBreakPoints", "SurfaceSupportBreakPoints", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "PointConnections", "PointConnections", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "LineConnectionEndPoints", "LineConnectionEndPoints", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "SurfaceConnectionBreakPoints", "SurfaceConnectionBreakPoints", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            GH_ExtendableMenu gH_ExtendableMenu0 = new GH_ExtendableMenu(0, "");
            gH_ExtendableMenu0.Name = "Auto peak smoothing region around...";
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[1]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[2]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[3]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[4]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[5]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[6]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[7]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[8]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[9]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[10]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[11]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[12]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[13]);
            evaluationUnit.AddMenu(gH_ExtendableMenu0);
        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            double factor = 0.5;
            DA.GetData(0, ref factor);

            bool beam = false;
            DA.GetData(1, ref beam);

            bool pile = true;
            DA.GetData(2, ref pile);

            bool truss = false;
            DA.GetData(3, ref truss);

            bool fictBar = false;
            DA.GetData(4, ref fictBar);

            bool plateIntsec = false;
            DA.GetData(5, ref plateIntsec);

            bool wallIntsec = false;
            DA.GetData(6, ref wallIntsec);

            bool fictShell = false;
            DA.GetData(7, ref fictShell);

            bool ptSupp = true;
            DA.GetData(8, ref ptSupp);

            bool lnSupp = false;
            DA.GetData(9, ref lnSupp);

            bool srfSupp = false;
            DA.GetData(10, ref srfSupp);

            bool ptConn = false;
            DA.GetData(11, ref ptConn);

            bool lnConn = false;
            DA.GetData(12, ref lnConn);

            bool srfConn = false;
            DA.GetData(13, ref srfConn);

            var autoPkSm = new Calculate.PeaksmAuto(beam, pile, truss, fictBar, plateIntsec, wallIntsec, fictShell, ptSupp, lnSupp, srfSupp, ptConn, lnConn, srfConn, factor);

            DA.SetData(0, autoPkSm);
        }
    }
}