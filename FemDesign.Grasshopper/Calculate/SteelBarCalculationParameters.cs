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
using static FemDesign.Calculate.ConcreteDesignConfig;

namespace FemDesign.Grasshopper
{
    public class SteelCalculationParameters : SubComponent
    {
        public override string name => "SteelCalculationParameters";
        public override string display_name => "SteelCalculationParameters";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name, display_name, "SteelBarDesignParameters");
            evaluationUnit.Icon = FemDesign.Properties.Resources.Config;
            mngr.RegisterUnit(evaluationUnit);

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Bar", "Bar", "Bar where to apply the calculation parameters", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "SectionsDistance", "SectionsDistance", "Distance between sections", GH_ParamAccess.item, new GH_Number(0.5));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_String(), "2ndOrder", "2ndOrder", "Connect 'ValueList' to get the options.\nIgnore\nConsiderIfAvailable\nConsiderAndFirstOrderDesign", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(FemDesign.Calculate.SteelBarCalculationParameters.SecondOrder)).ToList();


            evaluationUnit.RegisterInputParam(new Param_Boolean(), "PlasticCalculation", "PlasticCalculation", "PlasticCalculation", GH_ParamAccess.item, new GH_Boolean(false));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;


            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Equation6.41", "Equation6.41", "Equation6.41", GH_ParamAccess.item, new GH_Boolean(false));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;


            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Class4", "Class4", "Class4", GH_ParamAccess.item, new GH_Boolean(false));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;


            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Ignore", "Ignore", "Ignore", GH_ParamAccess.item, new GH_Boolean(false));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;


            evaluationUnit.RegisterInputParam(new Param_Number(), "Convergence", "Convergence", "Convergence", GH_ParamAccess.item, new GH_Number(1.00));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;


            evaluationUnit.RegisterInputParam(new Param_Integer(), "Iteration", "Iteration", "Iteration", GH_ParamAccess.item, new GH_Integer(50));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;


            evaluationUnit.RegisterInputParam(new Param_String(), "StiffDirection", "StiffDirection", "Connect 'ValueList' to get the options.\nAuto\na0\na\nb\nc\nd", GH_ParamAccess.item, new GH_String("Auto"));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(FemDesign.Calculate.SteelBarCalculationParameters.BucklingCurve)).ToList();


            evaluationUnit.RegisterInputParam(new Param_String(), "WeakDirection", "WeakDirection", "Connect 'ValueList' to get the options.\nAuto\na0\na\nb\nc\nd", GH_ParamAccess.item, new GH_String("Auto"));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(FemDesign.Calculate.SteelBarCalculationParameters.BucklingCurve)).ToList();

            evaluationUnit.RegisterInputParam(new Param_String(), "TorsionalDirection", "TorsionalDirection", "Connect 'ValueList' to get the options.\nAuto\na0\na\nb\nc\nd", GH_ParamAccess.item, new GH_String("Auto"));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(FemDesign.Calculate.SteelBarCalculationParameters.BucklingCurve)).ToList();

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "EN1993-1-1:6.3.2.2", "EN1993-1-1:6.3.2.2", "According to general case (EN1993-1-1:6.3.2.2), if sections is I, hollow or rectangle", GH_ParamAccess.item, new GH_Boolean(true));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "EN1993-1-1:6.3.2.3", "EN1993-1-1:6.3.2.3", "Use EN1993-1-1:6.3.2.3 for NA specified sections", GH_ParamAccess.item, new GH_Boolean(false));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;


            evaluationUnit.RegisterInputParam(new Param_String(), "TopFlange", "TopFlange", "Connect 'ValueList' to get the options.\nAuto\na\nb\nc\nd", GH_ParamAccess.item, new GH_String("Auto"));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(FemDesign.Calculate.SteelBarCalculationParameters.BucklingCurveLt)).ToList();


            evaluationUnit.RegisterInputParam(new Param_String(), "BottomFlange", "BottomFlange", "Connect 'ValueList' to get the options.\nAuto\na\nb\nc\nd", GH_ParamAccess.item, new GH_String("Auto"));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(FemDesign.Calculate.SteelBarCalculationParameters.BucklingCurveLt)).ToList();


            GH_ExtendableMenu gH_ExtendableMenu0 = new GH_ExtendableMenu(0, "General Settings");
            gH_ExtendableMenu0.Name = "General Settings";
            //gH_ExtendableMenu0.Expand();
            evaluationUnit.AddMenu(gH_ExtendableMenu0);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[1]);
            gH_ExtendableMenu0.RegisterInputPlug(evaluationUnit.Inputs[2]);


            GH_ExtendableMenu gH_ExtendableMenu1 = new GH_ExtendableMenu(1, "Resistante of cross sections");
            gH_ExtendableMenu1.Name = "Resistante of cross-sections";
            //gH_ExtendableMenu1.Expand();
            evaluationUnit.AddMenu(gH_ExtendableMenu1);
            gH_ExtendableMenu1.RegisterInputPlug(evaluationUnit.Inputs[3]);
            gH_ExtendableMenu1.RegisterInputPlug(evaluationUnit.Inputs[4]);
            gH_ExtendableMenu1.RegisterInputPlug(evaluationUnit.Inputs[5]);

            GH_ExtendableMenu gH_ExtendableMenu2 = new GH_ExtendableMenu(2, "Stability checks");
            gH_ExtendableMenu2.Name = "Stability checks";
            evaluationUnit.AddMenu(gH_ExtendableMenu2);
            gH_ExtendableMenu2.RegisterInputPlug(evaluationUnit.Inputs[6]);


            GH_ExtendableMenu gH_ExtendableMenu3 = new GH_ExtendableMenu(3, "Effective cross-section of Class 4");
            gH_ExtendableMenu3.Name = "Effective cross-section of Class 4 sections";
            evaluationUnit.AddMenu(gH_ExtendableMenu3);
            gH_ExtendableMenu3.RegisterInputPlug(evaluationUnit.Inputs[7]);
            gH_ExtendableMenu3.RegisterInputPlug(evaluationUnit.Inputs[8]);


            GH_ExtendableMenu gH_ExtendableMenu4 = new GH_ExtendableMenu(4, "Flexural buckling");
            gH_ExtendableMenu4.Name = "Flexural buckling";
            evaluationUnit.AddMenu(gH_ExtendableMenu4);
            gH_ExtendableMenu4.RegisterInputPlug(evaluationUnit.Inputs[9]);
            gH_ExtendableMenu4.RegisterInputPlug(evaluationUnit.Inputs[10]);


            GH_ExtendableMenu gH_ExtendableMenu5 = new GH_ExtendableMenu(5, "Torsional-flexural buckling");
            gH_ExtendableMenu5.Name = "Torsional-flexural buckling";
            evaluationUnit.AddMenu(gH_ExtendableMenu5);
            gH_ExtendableMenu5.RegisterInputPlug(evaluationUnit.Inputs[11]);


            GH_ExtendableMenu gH_ExtendableMenu6 = new GH_ExtendableMenu(6, "Lateral-torsional buckling");
            gH_ExtendableMenu6.Name = "Laterial-torsional buckling";
            evaluationUnit.AddMenu(gH_ExtendableMenu6);
            gH_ExtendableMenu6.RegisterInputPlug(evaluationUnit.Inputs[12]);
            gH_ExtendableMenu6.RegisterInputPlug(evaluationUnit.Inputs[13]);
            gH_ExtendableMenu6.RegisterInputPlug(evaluationUnit.Inputs[14]);
            gH_ExtendableMenu6.RegisterInputPlug(evaluationUnit.Inputs[15]);
        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            double sectionsDistance = 0.5;
            DA.GetData(1, ref sectionsDistance);

            string secondOrder = "ConsiderIfAvailable";
            DA.GetData(2, ref secondOrder);

            var _secondOrder = FemDesign.GenericClasses.EnumParser.Parse<SteelBarCalculationParameters.SecondOrder>(secondOrder);

            bool plasticCalculation = false;
            DA.GetData(3, ref plasticCalculation);

            bool equation641 = false;
            DA.GetData(4, ref equation641);

            bool class4 = false;
            DA.GetData(5, ref class4);

            bool ignore = false;
            DA.GetData(6, ref ignore);

            double convergence = 1.00;
            DA.GetData(7, ref convergence);

            int iteration = 50;
            DA.GetData(8, ref iteration);

            string stiffDirection = "Auto";
            DA.GetData(9, ref stiffDirection);

            var _stiffDirection = FemDesign.GenericClasses.EnumParser.Parse<SteelBarCalculationParameters.BucklingCurve>(stiffDirection);


            string weakDirection = "Auto";
            DA.GetData(10, ref weakDirection);

            var _weakDirection = FemDesign.GenericClasses.EnumParser.Parse<SteelBarCalculationParameters.BucklingCurve>(weakDirection);


            string torsionalDirection = "Auto";
            DA.GetData(11, ref torsionalDirection);

            var _torsionalDirection = FemDesign.GenericClasses.EnumParser.Parse<SteelBarCalculationParameters.BucklingCurve>(torsionalDirection);


            bool en1993_1_1_6_3_2_2 = true;
            DA.GetData(12, ref en1993_1_1_6_3_2_2);

            bool en1993_1_1_6_3_2_3 = false;
            DA.GetData(13, ref en1993_1_1_6_3_2_3);

            string topFlange = "Auto";
            DA.GetData(14, ref topFlange);

            var _topFlange = FemDesign.GenericClasses.EnumParser.Parse<SteelBarCalculationParameters.BucklingCurveLt>(topFlange);

            string bottomFlange = "Auto";
            DA.GetData(15, ref bottomFlange);

            var _bottomFlange = FemDesign.GenericClasses.EnumParser.Parse<SteelBarCalculationParameters.BucklingCurveLt>(bottomFlange);

            Bar bar = null;
            DA.GetData(0, ref bar);

            var steelDesignParam = new FemDesign.Calculate.SteelBarCalculationParameters(sectionsDistance, _secondOrder, plasticCalculation, equation641, class4, ignore, convergence, iteration, _stiffDirection, _weakDirection, _torsionalDirection, en1993_1_1_6_3_2_2, en1993_1_1_6_3_2_3, _topFlange, _bottomFlange);

            if(bar != null)
            {
                steelDesignParam.SetParametersOnBars(bar);
            }

            DA.SetData(0, steelDesignParam);
        }
    }
}