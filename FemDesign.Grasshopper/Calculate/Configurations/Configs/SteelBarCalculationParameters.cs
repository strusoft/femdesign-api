using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Components.UIWidgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using FemDesign.Grasshopper;
using FemDesign.Bars;
using FemDesign.Calculate;


namespace FemDesign.Grasshopper
{
    public class SteelCalculationParameters : SubComponent
    {
        public override string name() => "SteelCalculationParameters";
        public override string display_name() => "SteelCalculationParameters";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "SteelCalculationParameters");
            mngr.RegisterUnit(evaluationUnit);

            evaluationUnit.RegisterInputParam(new Param_Number(), "SectionsDistance", "SectionsDistance", "Distance between sections", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_String(), "2ndOrder", "2ndOrder", "Connect 'ValueList' to get the options.\nIgnore\nConsiderIfAvailable\nConsiderAndFirstOrderDesign", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(FemDesign.Calculate.SteelBarCalculationParameters.SecondOrder)).ToList();


            evaluationUnit.RegisterInputParam(new Param_Boolean(), "PlasticCalculation", "PlasticCalculation", "PlasticCalculation", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;


            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Equation6.41", "Equation6.41", "Equation6.41", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;


            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Class4", "Class4", "Class4", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;


            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Ignore", "Ignore", "Ignore", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;


            evaluationUnit.RegisterInputParam(new Param_Number(), "Convergence", "Convergence", "Convergence", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;


            evaluationUnit.RegisterInputParam(new Param_Integer(), "Iteration", "Iteration", "Iteration", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;


            evaluationUnit.RegisterInputParam(new Param_String(), "Stiff Direction Buckling Curve", "Stiff Direction Buckling Curve", "Connect 'ValueList' to get the options.\nAuto\na0\na\nb\nc\nd", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(FemDesign.Calculate.SteelBarCalculationParameters.BucklingCurve)).ToList();


            evaluationUnit.RegisterInputParam(new Param_String(), "Weak Direction Buckling Curve", "Weak Direction Buckling Curve", "Connect 'ValueList' to get the options.\nAuto\na0\na\nb\nc\nd", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(FemDesign.Calculate.SteelBarCalculationParameters.BucklingCurve)).ToList();

            evaluationUnit.RegisterInputParam(new Param_String(), "Torsional Direction Buckling Curve", "Torsional Direction Buckling Curve", "Connect 'ValueList' to get the options.\nAuto\na0\na\nb\nc\nd", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(FemDesign.Calculate.SteelBarCalculationParameters.BucklingCurve)).ToList();

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "EN1993-1-1:6.3.2.2", "EN1993-1-1:6.3.2.2", "According to general case (EN1993-1-1:6.3.2.2), if sections is I, hollow or rectangle", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "EN1993-1-1:6.3.2.3", "EN1993-1-1:6.3.2.3", "Use EN1993-1-1:6.3.2.3 for NA specified sections", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;


            evaluationUnit.RegisterInputParam(new Param_String(), "Top Flange Buckling Curve", "Top Flange Buckling Curve", "Connect 'ValueList' to get the options.\nAuto\na\nb\nc\nd", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(FemDesign.Calculate.SteelBarCalculationParameters.BucklingCurveLt)).ToList();


            evaluationUnit.RegisterInputParam(new Param_String(), "Bottom Flange Buckling Curve", "Bottom Flange Buckling Curve", "Connect 'ValueList' to get the options.\nAuto\na\nb\nc\nd", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(FemDesign.Calculate.SteelBarCalculationParameters.BucklingCurveLt)).ToList();

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Bar", "Bar", "Bar where to apply the calculation parameters", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count -1].Parameter.Optional = true;


            GH_ExtendableMenu gH_ExtendableMenu0 = new GH_ExtendableMenu(0, "");
            gH_ExtendableMenu0.Name = "General settings";
            gH_ExtendableMenu0.Expand();
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[0]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[1]);
            evaluationUnit.AddMenu(gH_ExtendableMenu0);


            GH_ExtendableMenu gH_ExtendableMenu1 = new GH_ExtendableMenu(1, "");
            gH_ExtendableMenu1.Name = "Resistante of cross-sections";
            gH_ExtendableMenu1.Expand();
            gH_ExtendableMenu1.RegisterInputPlug(evaluationUnit.Inputs[2]);
            gH_ExtendableMenu1.RegisterInputPlug(evaluationUnit.Inputs[3]);
            gH_ExtendableMenu1.RegisterInputPlug(evaluationUnit.Inputs[4]);
            evaluationUnit.AddMenu(gH_ExtendableMenu1);

            GH_ExtendableMenu gH_ExtendableMenu2 = new GH_ExtendableMenu(2, "");
            gH_ExtendableMenu2.Name = "Stability checks";
            gH_ExtendableMenu2.RegisterInputPlug(evaluationUnit.Inputs[5]);
            evaluationUnit.AddMenu(gH_ExtendableMenu2);


            GH_ExtendableMenu gH_ExtendableMenu3 = new GH_ExtendableMenu(3, "");
            gH_ExtendableMenu3.Name = "Effective Class 4 section";
            gH_ExtendableMenu3.RegisterInputPlug(evaluationUnit.Inputs[6]);
            gH_ExtendableMenu3.RegisterInputPlug(evaluationUnit.Inputs[7]);
            evaluationUnit.AddMenu(gH_ExtendableMenu3);


            GH_ExtendableMenu gH_ExtendableMenu4 = new GH_ExtendableMenu(4, "");
            gH_ExtendableMenu4.Name = "Flexural buckling";
            gH_ExtendableMenu4.RegisterInputPlug(evaluationUnit.Inputs[8]);
            gH_ExtendableMenu4.RegisterInputPlug(evaluationUnit.Inputs[9]);
            evaluationUnit.AddMenu(gH_ExtendableMenu4);


            GH_ExtendableMenu gH_ExtendableMenu5 = new GH_ExtendableMenu(5, "");
            gH_ExtendableMenu5.Name = "Torsional-flexural buckling";
            gH_ExtendableMenu5.RegisterInputPlug(evaluationUnit.Inputs[10]);
            evaluationUnit.AddMenu(gH_ExtendableMenu5);


            GH_ExtendableMenu gH_ExtendableMenu6 = new GH_ExtendableMenu(6, "");
            gH_ExtendableMenu6.Name = "Lateral-torsional buckling";
            gH_ExtendableMenu6.RegisterInputPlug(evaluationUnit.Inputs[11]);
            gH_ExtendableMenu6.RegisterInputPlug(evaluationUnit.Inputs[12]);
            gH_ExtendableMenu6.RegisterInputPlug(evaluationUnit.Inputs[13]);
            gH_ExtendableMenu6.RegisterInputPlug(evaluationUnit.Inputs[14]);
            evaluationUnit.AddMenu(gH_ExtendableMenu6);
        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            Bar bar = null;
            DA.GetData("Bar", ref bar);

            double sectionsDistance = 0.5;
            DA.GetData("SectionsDistance", ref sectionsDistance);

            string secondOrder = "ConsiderIfAvailable";
            DA.GetData("2ndOrder", ref secondOrder);

            var _secondOrder = FemDesign.GenericClasses.EnumParser.Parse<SteelBarCalculationParameters.SecondOrder>(secondOrder);

            bool plasticCalculation = false;
            DA.GetData("PlasticCalculation", ref plasticCalculation);

            bool equation641 = false;
            DA.GetData("Equation6.41", ref equation641);

            bool class4 = false;
            DA.GetData("Class4", ref class4);

            bool ignore = false;
            DA.GetData("Ignore", ref ignore);

            double convergence = 1.00;
            DA.GetData("Convergence", ref convergence);

            int iteration = 50;
            DA.GetData("Iteration", ref iteration);

            string stiffDirection = "Auto";
            DA.GetData("Stiff Direction Buckling Curve", ref stiffDirection);

            var _stiffDirection = FemDesign.GenericClasses.EnumParser.Parse<SteelBarCalculationParameters.BucklingCurve>(stiffDirection);


            string weakDirection = "Auto";
            DA.GetData("Weak Direction Buckling Curve", ref weakDirection);

            var _weakDirection = FemDesign.GenericClasses.EnumParser.Parse<SteelBarCalculationParameters.BucklingCurve>(weakDirection);


            string torsionalDirection = "Auto";
            DA.GetData("Torsional Direction Buckling Curve", ref torsionalDirection);

            var _torsionalDirection = FemDesign.GenericClasses.EnumParser.Parse<SteelBarCalculationParameters.BucklingCurve>(torsionalDirection);


            bool en1993_1_1_6_3_2_2 = true;
            DA.GetData("EN1993-1-1:6.3.2.2", ref en1993_1_1_6_3_2_2);

            bool en1993_1_1_6_3_2_3 = false;
            DA.GetData("EN1993-1-1:6.3.2.3", ref en1993_1_1_6_3_2_3);

            string topFlange = "Auto";
            DA.GetData("Top Flange Buckling Curve", ref topFlange);

            var _topFlange = FemDesign.GenericClasses.EnumParser.Parse<SteelBarCalculationParameters.BucklingCurveLt>(topFlange);

            string bottomFlange = "Auto";
            DA.GetData("Bottom Flange Buckling Curve", ref bottomFlange);

            var _bottomFlange = FemDesign.GenericClasses.EnumParser.Parse<SteelBarCalculationParameters.BucklingCurveLt>(bottomFlange);

            var steelDesignParam = new FemDesign.Calculate.SteelBarCalculationParameters(sectionsDistance, _secondOrder, plasticCalculation, equation641, class4, ignore, convergence, iteration, _stiffDirection, _weakDirection, _torsionalDirection, en1993_1_1_6_3_2_2, en1993_1_1_6_3_2_3, _topFlange, _bottomFlange);

            if (bar != null)
            {
                steelDesignParam.SetParametersOnBars(bar);
            }

            DA.SetData(0, steelDesignParam);
        }

        protected void setModelProps()
        {
            this.Parent_Component.ExpireSolution(true);
        }
    }
}