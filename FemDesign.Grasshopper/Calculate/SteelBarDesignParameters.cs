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
    public class SteelBarDesignParameters : SubComponent
    {
        public override string name() => "SteelBarDesignParameters";
        public override string display_name() => "SteelBarDesignParameters";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "SteelBarDesignParameters");
            evaluationUnit.Icon = FemDesign.Properties.Resources.Config;
            mngr.RegisterUnit(evaluationUnit);

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Bar", "Bar", "Bar where to apply the design parameters", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "Utilisation", "Utilisation", "Utilisation", GH_ParamAccess.item, new GH_Number(0.8));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Sections", "Sections", "Sections", GH_ParamAccess.list);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = false;
        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            Bars.Bar bar = null;
            DA.GetData(0, ref bar);

            double utilisation = 0.8;
            DA.GetData(1, ref utilisation);

            List<FemDesign.Sections.Section> sections = new List<FemDesign.Sections.Section>();
            DA.GetDataList(2, sections);


            var steelDesignParam = new FemDesign.Calculate.SteelBarDesignParameters(utilisation, sections, bar);

            DA.SetData(0, steelDesignParam);
        }
    }
}