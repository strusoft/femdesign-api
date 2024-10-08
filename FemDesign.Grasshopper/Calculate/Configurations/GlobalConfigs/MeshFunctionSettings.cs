using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Components.UIWidgets;


namespace FemDesign.Grasshopper
{
    public class MeshFunctionSettings : SubComponent
    {
        public override string name() => "MeshFunctionSettings";
        public override string display_name() => "MeshFunctionSettings";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "Finite element mesh functions settings. For more details, see the FEM-Design GUI > Settings > Calculation > Mesh.");
            mngr.RegisterUnit(evaluationUnit);

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "RefineLocally", "RefineLocally", "Refine locally where needed.\nDefault is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Integer(), "MaxStep", "MaxStep", "Max. step.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Warn", "Warn", "Warn about reaching max. step.\nDefault is False.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "ReduceElementSize", "ReduceElementSize", "Reduce average element size if neccessary.\nDefault is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Integer(), "Steps", "Steps", "Steps.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Geometry", "Geometry", "Geometry.\nDefault is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "MinAngle", "MinAngle", "Min. angle. [°]", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "MaxAngle", "MaxAngle", "Max. angle. [°]", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "MaxSideRatio", "MaxSideRatio", "Max. side ratio", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Overlap&Cut", "Overlap&Cut", "Overlap & cut.\nDefault is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Topology&Gap", "Topology&Gap", "Topology & gap.\nDefault is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;


            GH_ExtendableMenu gH_ExtendableMenu0 = new GH_ExtendableMenu(0, "");
            gH_ExtendableMenu0.Name = "Generate surface mesh";
            gH_ExtendableMenu0.Expand();
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[0]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[1]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[2]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[3]);
            evaluationUnit.AddMenu(gH_ExtendableMenu0);

            GH_ExtendableMenu gH_ExtendableMenu1 = new GH_ExtendableMenu(1, "");
            gH_ExtendableMenu1.Name = "Smooth mesh";
            gH_ExtendableMenu1.Expand();
            gH_ExtendableMenu1.RegisterInputPlug(evaluationUnit.Inputs[4]);
            evaluationUnit.AddMenu(gH_ExtendableMenu1);

            GH_ExtendableMenu gH_ExtendableMenu2 = new GH_ExtendableMenu(2, "");
            gH_ExtendableMenu2.Name = "Check mesh";
            gH_ExtendableMenu2.Expand();
            gH_ExtendableMenu2.RegisterInputPlug(evaluationUnit.Inputs[5]);
            gH_ExtendableMenu2.RegisterInputPlug(evaluationUnit.Inputs[6]);
            gH_ExtendableMenu2.RegisterInputPlug(evaluationUnit.Inputs[7]);
            gH_ExtendableMenu2.RegisterInputPlug(evaluationUnit.Inputs[8]);
            gH_ExtendableMenu2.RegisterInputPlug(evaluationUnit.Inputs[9]);
            gH_ExtendableMenu2.RegisterInputPlug(evaluationUnit.Inputs[10]);
            evaluationUnit.AddMenu(gH_ExtendableMenu2);
        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            bool refine = true;
            DA.GetData(0, ref refine);

            int maxStep = 5;
            DA.GetData(1, ref maxStep);

            bool warn = false;
            DA.GetData(2, ref warn);

            bool reduce = true;
            DA.GetData(3, ref reduce);

            int steps = 3;
            DA.GetData(4, ref steps);

            bool geometry = true;
            DA.GetData(5, ref geometry);

            double minAngle = 5.0;
            DA.GetData(6, ref minAngle);

            double maxAngle = 175.0;
            DA.GetData(7, ref maxAngle);

            double ratio = 16.0;
            DA.GetData(8, ref ratio);

            bool overlap = true;
            DA.GetData(9, ref overlap);

            bool topology = true;
            DA.GetData(10, ref topology);

            var meshFunc = new Calculate.MeshFunctions(refine, maxStep, warn, reduce, steps, geometry, minAngle, maxAngle, ratio, overlap, topology);
            DA.SetData(0, meshFunc);
        }

        protected void setModelProps()
        {
            this.Parent_Component.ExpireSolution(true);
        }
    }
}