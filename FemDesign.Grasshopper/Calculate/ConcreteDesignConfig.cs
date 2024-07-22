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
using FemDesign.Loads;
using FemDesign.Calculate;

namespace FemDesign.Grasshopper
{
    public class ConcreteDesignConfiguration : SubComponent
    {
        public override string name => "ConcreteDesignConfiguration";
        public override string display_name => "ConcreteDesignConfiguration";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name, display_name, "ConcreteDesignConfiguration");
            evaluationUnit.Icon = FemDesign.Properties.Resources.Config;
            mngr.RegisterUnit(evaluationUnit);

            evaluationUnit.RegisterInputParam(new Param_String(), "CalculationMethod", "CalculationMethod", "Connect 'ValueList' to get the options.\nNominalStiffness\nNominalCurvature", GH_ParamAccess.item, new GH_String("NominalStiffness"));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(FemDesign.Calculate.ConcreteDesignConfig.CalculationMethod)).ToList();

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "CrackQuasiPermanent", "CrackQuasiPermanent", "", GH_ParamAccess.item, new GH_Boolean(false));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "CrackFrequent", "CrackFrequent", "", GH_ParamAccess.item, new GH_Boolean(false));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "CrackCharacteristic", "CrackCharacteristic", "", GH_ParamAccess.item, new GH_Boolean(false));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "ReopeningCracks", "ReopeningCracks", "", GH_ParamAccess.item, new GH_Boolean(false));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            string calculationMethod = "";
            DA.GetData(0, ref calculationMethod);

            ConcreteDesignConfig.CalculationMethod _calculationMethod = FemDesign.GenericClasses.EnumParser.Parse<ConcreteDesignConfig.CalculationMethod>(calculationMethod);

            bool crackQuasiPermanent = false;
            DA.GetData(1, ref crackQuasiPermanent);

            bool crackFrequent = false;
            DA.GetData(2, ref crackFrequent);

            bool crackCharacteristic = false;
            DA.GetData(3, ref crackCharacteristic);

            bool reopeningCracks = false;
            DA.GetData(4, ref reopeningCracks);


            var concreteDesignParameters = new FemDesign.Calculate.ConcreteDesignConfig(_calculationMethod, crackQuasiPermanent, crackFrequent, crackCharacteristic, reopeningCracks);

            DA.SetData(0, concreteDesignParameters);
        }
    }
}