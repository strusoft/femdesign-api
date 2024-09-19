using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Components.UIWidgets;


namespace FemDesign.Grasshopper
{
    public class SteelTubeWithSteelCore : SubComponent
    {
        public override string name() => "SteelTubeWithSteelCore";
        public override string display_name() => "SteelTubeWithSteelCore";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "Create a filled steel tube composite section with a steel core. For more information, see FEM-Design GUI.");
            mngr.RegisterUnit(evaluationUnit);
            
            evaluationUnit.RegisterInputParam(new Param_String(), "SectionName", "SectionName", "Composite section name.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            
            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "TubeMaterial", "TubeMaterial", "Steel tube material.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "CoreMaterial", "CoreMaterial", "Steel core material.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Concrete", "Concrete", "Concrete material.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "d1", "d1", "Steel tube exterior diameter [mm].", GH_ParamAccess.item, new GH_Number(300));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "d2", "d2", "Steel core diameter [mm].", GH_ParamAccess.item, new GH_Number(160));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "t", "t", "Thickness of tube [mm].", GH_ParamAccess.item, new GH_Number(8));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

        }

        public override void SolveInstance(IGH_DataAccess DA, out string msg, out GH_RuntimeMessageLevel level)
        {
            msg = "";
            level = GH_RuntimeMessageLevel.Warning;

            // get input
            string name = null;
            if (!DA.GetData(0, ref name)) { return; }

            Materials.Material steelTube = new Materials.Material();
            if (!DA.GetData(1, ref steelTube)) { return; }

            Materials.Material steelCore = new Materials.Material();
            if (!DA.GetData(2, ref steelCore)) { return; }

            Materials.Material concrete = new Materials.Material();
            if (!DA.GetData(3, ref concrete)) { return; }

            double d1 = 300;
            DA.GetData(4, ref d1);

            double d2 = 160;
            DA.GetData(5, ref d2);

            double t = 8;
            DA.GetData(6, ref t);

            // check input data
            if (steelTube.Family != Materials.Family.Steel || steelCore.Family != Materials.Family.Steel)
                throw new ArgumentException($"SteelTube and SteelCore inputs must be steel material!");
            if (concrete.Family != Materials.Family.Concrete)
                throw new ArgumentException($"Concrete input must be concrete material but it is {concrete.Family}");

            // create composite section
            Composites.CompositeSection compositeSection = Composites.CompositeSection.FilledSteelTubeWithSteelCore(name, steelTube, steelCore, concrete, d1, d2, t);

            // get output
            DA.SetData(0, compositeSection);
        }

        protected void setModelProps()
        {
            this.Parent_Component.ExpireSolution(true);
        }
    }
}