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
using FemDesign.Loads;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel.Special;
using static FemDesign.Calculate.ConcreteDesignConfig;
using FemDesign.GenericClasses;
using FemDesign.Calculate;

namespace FemDesign.Grasshopper
{
    public class SteelDesignConfiguration : SubComponent
    {
        public override string name => "SteelDesignConfiguration";
        public override string display_name => "SteelDesignConfiguration";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name, display_name, "SteelDesignConfiguration");
            evaluationUnit.Icon = FemDesign.Properties.Resources.Config;
            mngr.RegisterUnit(evaluationUnit);

            evaluationUnit.RegisterInputParam(new Param_String(), "Interaction", "Interaction", "Connect 'ValueList' to get the options.\nMethod1\nMethod2", GH_ParamAccess.item, new GH_String("Method1"));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].EnumInput = Enum.GetNames(typeof(FemDesign.Calculate.SteelDesignConfiguration.Method)).ToList();
        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            string interaction = "Method1";
            DA.GetData(0, ref interaction);

            var _interaction = FemDesign.GenericClasses.EnumParser.Parse<Calculate.SteelDesignConfiguration.Method>(interaction);

            var results = new FemDesign.Calculate.SteelDesignConfiguration(_interaction);
            DA.SetData(0, results);
        }
    }
}