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
        public override string name => "SteelBarDesignParameters";
        public override string display_name => "SteelBarDesignParameters";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name, display_name, "SteelBarDesignParameters");
            evaluationUnit.Icon = FemDesign.Properties.Resources.Config;
            mngr.RegisterUnit(evaluationUnit);

            evaluationUnit.RegisterInputParam(new Param_Number(), "Utilisation", "Utilisation", "Utilisation", GH_ParamAccess.item, new GH_Number(0.8));
            evaluationUnit.Inputs[0].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Sections", "Sections", "Sections", GH_ParamAccess.list);
            evaluationUnit.Inputs[1].Parameter.Optional = false;

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Bar", "Bar", "Bar where to a", GH_ParamAccess.item);
            evaluationUnit.Inputs[1].Parameter.Optional = true;
        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            double utilisation = 0;
            DA.GetData(0, ref utilisation);

            List<FemDesign.Sections.Section> sections = new List<FemDesign.Sections.Section>();
            DA.GetDataList(1, sections);

            Bars.Bar bar = null;
            DA.GetData(2, ref bar);

            var steelDesignParam = new FemDesign.Calculate.SteelBarDesignParameters(utilisation, sections, bar);

            DA.SetData(0, steelDesignParam);
        }
    }
}