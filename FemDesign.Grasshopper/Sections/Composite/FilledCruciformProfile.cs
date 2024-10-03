using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Components.UIWidgets;


namespace FemDesign.Grasshopper
{
    public class FilledCruciformProfile : SubComponent
    {
        public System.Drawing.Bitmap Icon => Properties.Resources.FilledCruciformProfile;
        public override string name() => "FilledCruciformProfile";
        public override string display_name() => "FilledCruciformProfile";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "Create a composite section for cruciform profiled cross-sections. For more information, see FEM-Design GUI.", this.Icon);
            mngr.RegisterUnit(evaluationUnit);
            
            evaluationUnit.RegisterInputParam(new Param_String(), "SectionName", "SectionName", "Composite section name.", GH_ParamAccess.item);
            
            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Steel", "Steel", "Steel material.", GH_ParamAccess.item);

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Concrete", "Concrete", "Concrete material.", GH_ParamAccess.item);

            evaluationUnit.RegisterInputParam(new Param_Number(), "bc", "bc", "Cross-section width [mm].", GH_ParamAccess.item, new GH_Number(600));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "hc", "hc", "Cross-section height [mm].", GH_ParamAccess.item, new GH_Number(600));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "bf", "bf", "Flange width [mm].", GH_ParamAccess.item, new GH_Number(250));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "tw", "tw", "Web thickness [mm].", GH_ParamAccess.item, new GH_Number(15));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "tf", "tf", "Flange thickness [mm].", GH_ParamAccess.item, new GH_Number(25));
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

            double bc = 600;
            DA.GetData(3, ref bc);

            double hc = 600;
            DA.GetData(4, ref hc);

            double bf = 250;
            DA.GetData(5, ref bf);

            double tw = 15;
            DA.GetData(6, ref tw);

            double tf = 25;
            DA.GetData(7, ref tf);

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
            Composites.CompositeSection compositeSection = Composites.CompositeSection.FilledCruciformProfile(name, steel, concrete, bc, hc, bf, tw, tf);

            // get output
            DA.SetData(0, compositeSection);
        }

        protected void setModelProps()
        {
            this.Parent_Component.ExpireSolution(true);
        }
    }
}