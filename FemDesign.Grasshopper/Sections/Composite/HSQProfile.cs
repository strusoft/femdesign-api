using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Components.UIWidgets;


namespace FemDesign.Grasshopper
{
    public class HSQProfile : SubComponent
    {
        public override string name() => "HSQProfile";
        public override string display_name() => "HSQProfile";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "Create a composite section for HSQ cross-sections. For more information, see FEM-Design GUI.");
            mngr.RegisterUnit(evaluationUnit);
            
            evaluationUnit.RegisterInputParam(new Param_String(), "SectionName", "SectionName", "Composite section name.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            
            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Steel", "Steel", "Steel material.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Concrete", "Concrete", "Concrete material.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "b", "b", "Intermediate width of the bottom flange [mm].", GH_ParamAccess.item, new GH_Number(200));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "bt", "bt", "Top flange width [mm].", GH_ParamAccess.item, new GH_Number(240));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "o1", "o1", "Left overhang [mm].", GH_ParamAccess.item, new GH_Number(150));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "o2", "o2", "Right overhang [mm].", GH_ParamAccess.item, new GH_Number(150));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "h", "h", "Web hight [mm].", GH_ParamAccess.item, new GH_Number(360));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "tw", "tw", "Web thickness [mm].", GH_ParamAccess.item, new GH_Number(10));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "tfb", "tfb", "Bottom flange thickness [mm].", GH_ParamAccess.item, new GH_Number(20));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "tft", "tft", "Top flange thickness [mm].", GH_ParamAccess.item, new GH_Number(20));
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

            double b = 200;
            DA.GetData(3, ref b);

            double bt = 240;
            DA.GetData(4, ref bt);

            double o1 = 150;
            DA.GetData(5, ref o1);

            double o2 = 150;
            DA.GetData(6, ref o2);

            double h = 360;
            DA.GetData(7, ref h);

            double tw = 10;
            DA.GetData(8, ref tw);

            double tfb = 20;
            DA.GetData(9, ref tfb);

            double tft = 20;
            DA.GetData(10, ref tft);

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
            Composites.CompositeSection compositeSection = Composites.CompositeSection.FilledHSQProfile(name, steel, concrete, b, bt, o1, o2, h, tw, tfb, tft);

            // get output
            DA.SetData(0, compositeSection);
        }

        protected void setModelProps()
        {
            this.Parent_Component.ExpireSolution(true);
        }
    }
}