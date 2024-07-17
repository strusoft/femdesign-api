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

            evaluationUnit.RegisterInputParam(new Param_Integer(), "Interaction", "Interaction", "0: Method 1\n1: Method 2", GH_ParamAccess.item, new GH_Integer(0));
            evaluationUnit.Inputs[0].Parameter.Optional = true;
            Param_Integer typeComb = evaluationUnit.Inputs[0].Parameter as Param_Integer;
            typeComb.AddNamedValue("Method 1", 0);
            typeComb.AddNamedValue("Method 2", 1);
        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            int interaction = 0;
            DA.GetData(0, ref interaction);

            if (!Enum.IsDefined(typeof(FemDesign.Calculate.SteelDesignConfiguration.Method), interaction))
            {
                throw new System.ArgumentException("Interaction not valid. 0 = Method 1, 1 = Method 2");
            }

            var _interaction = (FemDesign.Calculate.SteelDesignConfiguration.Method)interaction;

            var results = new FemDesign.Calculate.SteelDesignConfiguration(_interaction);
            DA.SetData(0, results);
        }
    }
}