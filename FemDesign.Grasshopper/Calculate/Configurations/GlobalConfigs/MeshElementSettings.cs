using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Components.UIWidgets;


namespace FemDesign.Grasshopper
{
    public class MeshElementSettings : SubComponent
    {
        public override string name() => "MeshElementSettings";
        public override string display_name() => "MeshElementSettings";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "Finite element mesh elements settings. For more details, see the FEM-Design GUI > Settings > Calculation > Mesh.");
            mngr.RegisterUnit(evaluationUnit);
            
            evaluationUnit.RegisterInputParam(new Param_Number(), "Scale", "Scale", "Scale.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            
            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Correct", "Correct", "Correct according to the minimum division numbers.\nDefault is True.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "RegionByRegion", "RegionByRegion", "Region by region.\nDefault is True. If false, the `Consider all regions together` option is set.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Integer(), "MinDivNumber", "MinDivNumber", "Lowest minimum division number.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "MaxCentralAngle", "MaxCentralAngle", "Maximal central angle for arcs.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;


            GH_ExtendableMenu gH_ExtendableMenu0 = new GH_ExtendableMenu(0, "");
            gH_ExtendableMenu0.Name = "Calculated average element size";
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[0]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[1]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[2]);
            evaluationUnit.AddMenu(gH_ExtendableMenu0);


            GH_ExtendableMenu gH_ExtendableMenu1 = new GH_ExtendableMenu(1, "");
            gH_ExtendableMenu1.Name = "Bar element";
            gH_ExtendableMenu1.Expand();
            gH_ExtendableMenu1.RegisterInputPlug(evaluationUnit.Inputs[3]);
            gH_ExtendableMenu1.RegisterInputPlug(evaluationUnit.Inputs[4]);
            evaluationUnit.AddMenu(gH_ExtendableMenu1);
        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            double scale = 6.0;
            DA.GetData(0, ref scale);

            bool correct = true;
            DA.GetData(1, ref correct);

            bool regByReg = true;
            DA.GetData(2, ref regByReg);

            int minDiv = 2;
            DA.GetData(3, ref minDiv);

            double angle = 10.00;
            DA.GetData(4, ref angle);

            var meshElem = new Calculate.MeshElements(regByReg, scale, correct, minDiv, angle);
            DA.SetData(0, meshElem);
        }

        protected void setModelProps()
        {
            this.Parent_Component.ExpireSolution(true);
        }
    }
}