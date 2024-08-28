using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Components.UIWidgets;


namespace FemDesign.Grasshopper
{
    public class MeshPrepareSettings : SubComponent
    {
        public override string name() => "MeshPrepareSettings";
        public override string display_name() => "MeshPrepareSettings";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "Finite element mesh preparation settings. For more details, see the FEM-Design GUI > Settings > Calculation > Mesh.");
            mngr.RegisterUnit(evaluationUnit);

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Check mesh", "Check mesh", "Default is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Autoregenerate mesh", "Autoregenerate mesh", "Regenerate mesh automatically on the changed objects.\nDefault is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Optimal rebuild", "Optimal rebuild", "Optimal rebuild surface mesh after refine.\nDefault is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Smooth mesh", "Smooth mesh", "Smooth mesh after optimal rebuild.\nDefault is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Peak smoothing", "Peak smoothing", "Default is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Beams", "Beams", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Columns", "Columns", "Default is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Trusses", "Trusses", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Fictitious bars", "Fictitious bars", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Free edges", "Free edges", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Region borders", "Region borders", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            
            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Point supports", "Point supports", "Default is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Line supports", "Line supports", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Surface support edges", "Surface support edges", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Edge connections", "Edge connections", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Point connections", "Point connections", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Line connections", "Line connections", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Surface connection edges", "Surface connection edges", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Point loads", "Point loads", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Line loads", "Line loads", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Surface load edges", "Surface load edges", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Fixed points", "Fixed points", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Fixed lines", "Fixed lines", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
                        

            GH_ExtendableMenu gH_ExtendableMenu0 = new GH_ExtendableMenu(0, "");
            gH_ExtendableMenu0.Name = "Refine mesh after regenerate around...";
            gH_ExtendableMenu0.Expand();
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
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[14]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[15]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[16]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[17]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[18]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[19]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[20]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[21]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[22]);
            evaluationUnit.AddMenu(gH_ExtendableMenu0);
        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            bool autoCheck = true;
            DA.GetData(0, ref autoCheck);

            bool autoRegen = true;
            DA.GetData(0, ref autoRegen);

            bool autoRebuild = true;
            DA.GetData(0, ref autoRebuild);

            bool autoSmooth = true;
            DA.GetData(0, ref autoSmooth);

            bool peakSmth = true;
            DA.GetData(0, ref peakSmth);

            bool beams = false;
            DA.GetData(0, ref beams);

            bool columns = true;
            DA.GetData(0, ref columns);

            bool trusses = false;
            DA.GetData(0, ref trusses);

            bool fictBars = false;
            DA.GetData(0, ref fictBars);

            bool freeEdges= false;
            DA.GetData(0, ref freeEdges);

            bool regionBorders = false;
            DA.GetData(0, ref regionBorders);

            bool ptSupp = true;
            DA.GetData(0, ref ptSupp);

            bool LnSupp = false;
            DA.GetData(0, ref LnSupp);

            bool SrfSuppEdges = false;
            DA.GetData(0, ref SrfSuppEdges);

            bool EdgeConn = false;
            DA.GetData(0, ref EdgeConn);

            bool PtConn = false;
            DA.GetData(0, ref PtConn);

            bool LnConn = false;
            DA.GetData(0, ref LnConn);

            bool SrfConnEdges = false;
            DA.GetData(0, ref SrfConnEdges);

            bool PtLoads = false;
            DA.GetData(0, ref PtLoads);

            bool LnLoads = false;
            DA.GetData(0, ref LnLoads);

            bool SrfLoadEdges = false;
            DA.GetData(0, ref SrfLoadEdges);

            bool FixedPts = false;
            DA.GetData(0, ref FixedPts);

            bool FixedLns = false;
            DA.GetData(0, ref FixedLns);



            var meshFunc = new Calculate.MeshPrepare(autoRegen, peakSmth, beams, columns, trusses, fictBars, freeEdges, regionBorders, ptSupp, LnSupp, 
                SrfSuppEdges, EdgeConn, PtConn, LnConn, SrfConnEdges, PtLoads, LnLoads, SrfLoadEdges, FixedPts, FixedLns, autoRebuild, autoSmooth, autoCheck);
            DA.SetData(0, meshFunc);
        }

        protected void setModelProps()
        {
            this.Parent_Component.ExpireSolution(true);
        }
    }
}