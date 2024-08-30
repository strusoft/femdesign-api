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

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "CheckMesh", "CheckMesh", "Default is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "AutoregenerateMesh", "AutoregenerateMesh", "Regenerate mesh automatically on the changed objects.\nDefault is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "OptimalRebuild", "OptimalRebuild", "Optimal rebuild surface mesh after refine.\nDefault is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "SmoothMesh", "SmoothMesh", "Smooth mesh after optimal rebuild.\nDefault is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "PeakSmoothing", "PeakSmoothing", "Default is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Beams", "Beams", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Columns", "Columns", "Default is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Trusses", "Trusses", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "FictitiousBars", "FictitiousBars", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "FreeEdges", "FreeEdges", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "RegionBorders", "RegionBorders", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            
            evaluationUnit.RegisterInputParam(new Param_Boolean(), "PointSupports", "PointSupports", "Default is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "LineSupports", "LineSupports", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "SurfaceSupportEdges", "SurfaceSupportEdges", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "EdgeConnections", "EdgeConnections", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "PointConnections", "PointConnections", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "LineConnections", "LineConnections", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "SurfaceConnectionEdges", "SurfaceConnectionEdges", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "PointLoads", "PointLoads", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "LineLoads", "LineLoads", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "SurfaceLoadEdges", "SurfaceLoadEdges", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "FixedPoints", "FixedPoints", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "FixedLines", "FixedLines", "Default is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
                        

            GH_ExtendableMenu gH_ExtendableMenu0 = new GH_ExtendableMenu(0, "");
            gH_ExtendableMenu0.Name = "Refine mesh after regenerate around...";
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
            DA.GetData(1, ref autoRegen);

            bool autoRebuild = true;
            DA.GetData(2, ref autoRebuild);

            bool autoSmooth = true;
            DA.GetData(3, ref autoSmooth);

            bool peakSmth = true;
            DA.GetData(4, ref peakSmth);

            bool beams = false;
            DA.GetData(5, ref beams);

            bool columns = true;
            DA.GetData(6, ref columns);

            bool trusses = false;
            DA.GetData(7, ref trusses);

            bool fictBars = false;
            DA.GetData(8, ref fictBars);

            bool freeEdges= false;
            DA.GetData(9, ref freeEdges);

            bool regionBorders = false;
            DA.GetData(10, ref regionBorders);

            bool ptSupp = true;
            DA.GetData(11, ref ptSupp);

            bool LnSupp = false;
            DA.GetData(12, ref LnSupp);

            bool SrfSuppEdges = false;
            DA.GetData(13, ref SrfSuppEdges);

            bool EdgeConn = false;
            DA.GetData(14, ref EdgeConn);

            bool PtConn = false;
            DA.GetData(15, ref PtConn);

            bool LnConn = false;
            DA.GetData(16, ref LnConn);

            bool SrfConnEdges = false;
            DA.GetData(17, ref SrfConnEdges);

            bool PtLoads = false;
            DA.GetData(18, ref PtLoads);

            bool LnLoads = false;
            DA.GetData(19, ref LnLoads);

            bool SrfLoadEdges = false;
            DA.GetData(20, ref SrfLoadEdges);

            bool FixedPts = false;
            DA.GetData(21, ref FixedPts);

            bool FixedLns = false;
            DA.GetData(22, ref FixedLns);



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