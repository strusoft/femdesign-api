using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Components.UIWidgets;


namespace FemDesign.Grasshopper
{
    public class EffectiveCompositeSlab : SubComponent
    {
        public override string name() => "EffectiveCompositeSlab";
        public override string display_name() => "EffectiveCompositeSlab";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "Create a composite section for bars based on the effective width of a composite slab. For more information, see FEM-Design GUI.");
            mngr.RegisterUnit(evaluationUnit);
            
            evaluationUnit.RegisterInputParam(new Param_String(), "SectionName", "SectionName", "Composite section name.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            
            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Steel", "Steel", "Steel material.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Concrete", "Concrete", "Concrete material.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "SteelProfile", "SteelProfile", "Steel profile.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "t", "t", "Slab thickness.", GH_ParamAccess.item, new GH_Number(150));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "bEff", "bEff", "Concrete slab effective width.", GH_ParamAccess.item, new GH_Number(800));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "th", "th", "Hunch thickness.", GH_ParamAccess.item, new GH_Number(60));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "bt", "bt", "Hunch width at the top.", GH_ParamAccess.item, new GH_Number(400));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "bb", "bb", "Hunch width at the bottom.", GH_ParamAccess.item, new GH_Number(200));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Boolean(), "Filled", "Filled", "True if the steel section part is filled with concrete, false if not. The quality of the filling material is the same as the concrete of the slab.", GH_ParamAccess.item, new GH_Boolean(false));
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

            Sections.Section steelProf = null;
            if (!DA.GetData(3, ref steelProf)) { return; }

            double t = 150;
            DA.GetData(4, ref t);

            double bEff = 800;
            DA.GetData(5, ref bEff);

            double th = 60;
            DA.GetData(6, ref th);

            double bt = 400;
            DA.GetData(7, ref bt);

            double bb = 200;
            DA.GetData(8, ref bb);

            bool filled = false;
            DA.GetData(9, ref filled);

            // check input data
            if (steel.Family != Materials.Family.Steel)
            {
                throw new ArgumentException($"Steel input must be steel material but it is {steel.Family}");
            }
            if (concrete.Family != Materials.Family.Concrete)
            {
                throw new ArgumentException($"Concrete input must be concrete material but it is {concrete.Family}");
            }

            // create composite section
            Composites.CompositeSection compositeSection = Composites.CompositeSection.EffectiveCompositeSlab(name, steel, concrete, steelProf, t, bEff, th, bt, bb, filled);

            // get output
            DA.SetData(0, compositeSection);
        }

        protected void setModelProps()
        {
            this.Parent_Component.ExpireSolution(true);
        }
    }
}