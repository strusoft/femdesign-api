using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Components.UIWidgets;


namespace FemDesign.Grasshopper
{
    public class FilledSteelTube : SubComponent
    {
        public System.Drawing.Bitmap Icon => Properties.Resources.FilledSteelTube;
        public override string name() => "FilledSteelTube";
        public override string display_name() => "FilledSteelTube";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "Create a filled steel tube composite section. For more information, see FEM-Design GUI.", this.Icon);
            mngr.RegisterUnit(evaluationUnit);
            
            evaluationUnit.RegisterInputParam(new Param_String(), "SectionName", "SectionName", "Composite section name.", GH_ParamAccess.item);
            
            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Steel", "Steel", "Steel material.", GH_ParamAccess.item);

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Concrete", "Concrete", "Concrete material.", GH_ParamAccess.item);

            evaluationUnit.RegisterInputParam(new Param_Number(), "d", "d", "Diameter of steel tube [mm].", GH_ParamAccess.item, new GH_Number(400));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "t", "t", "Thickness of tube [mm].", GH_ParamAccess.item, new GH_Number(15));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            // get input
            string name = null;
            if (!DA.GetData(0, ref name)) { return; }

            Materials.Material steel = new Materials.Material();
            if (!DA.GetData(1, ref steel)) { return; }

            Materials.Material concrete = new Materials.Material();
            if (!DA.GetData(2, ref concrete)) { return; }

            double d = 400;
            DA.GetData(3, ref d);

            double t = 15;
            DA.GetData(4, ref t);

            // check input data
            if (steel.Family != Materials.Family.Steel)
                throw new ArgumentException($"Steel input must be steel material but it is {steel.Family}");
            if (concrete.Family != Materials.Family.Concrete)
                throw new ArgumentException($"Concrete input must be concrete material but it is {concrete.Family}");

            // create composite section
            Composites.CompositeSection compositeSection = Composites.CompositeSection.FilledSteelTube(name, steel, concrete, d, t);

            // get output
            DA.SetData(0, compositeSection);
        }

        protected void setModelProps()
        {
            this.Parent_Component.ExpireSolution(true);
        }
    }
}