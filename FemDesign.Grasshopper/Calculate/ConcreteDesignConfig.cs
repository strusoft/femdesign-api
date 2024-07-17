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

            evaluationUnit.RegisterInputParam(new Param_Integer(), "CalculationMethod", "CalculationMethod", "0: Nominal stiffness\n1: Nominal curvature", GH_ParamAccess.item, new GH_Integer(0));
            evaluationUnit.Inputs[0].Parameter.Optional = true;

            Param_Integer typeComb = evaluationUnit.Inputs[0].Parameter as Param_Integer;
            typeComb.AddNamedValue("Nominal stiffness", 0);
            typeComb.AddNamedValue("Nominal curvature", 1);

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "CrackQuasiPermanent", "CrackQuasiPermanent", "", GH_ParamAccess.item, new GH_Boolean(false));
            evaluationUnit.Inputs[1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "CrackFrequent", "CrackFrequent", "", GH_ParamAccess.item, new GH_Boolean(false));
            evaluationUnit.Inputs[2].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "CrackCharacteristic", "CrackCharacteristic", "", GH_ParamAccess.item, new GH_Boolean(false));
            evaluationUnit.Inputs[3].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "ReopeningCracks", "ReopeningCracks", "", GH_ParamAccess.item, new GH_Boolean(false));
            evaluationUnit.Inputs[4].Parameter.Optional = true;
        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            int calculationMethod = 0;
            DA.GetData(0, ref calculationMethod);

            if (!Enum.IsDefined(typeof(FemDesign.Calculate.ConcreteDesignConfig.CalculationMethod), calculationMethod))
            {
                throw new System.ArgumentException("CalculationMethod not valid. 0 = Nominal stiffness, 1 = Nominal curvature");
            }

            var _calculationMethod = (FemDesign.Calculate.ConcreteDesignConfig.CalculationMethod)calculationMethod;

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