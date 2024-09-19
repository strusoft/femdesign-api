using System;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Components.UIWidgets;


namespace FemDesign.Grasshopper
{
    public class SteelTubeWithIProfile : SubComponent
    {
        public override string name() => "SteelTubeWithIProfile";
        public override string display_name() => "SteelTubeWithIProfile";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "Create a filled steel tube composite section with I-profile. For more information, see FEM-Design GUI.");
            mngr.RegisterUnit(evaluationUnit);
            
            evaluationUnit.RegisterInputParam(new Param_String(), "SectionName", "SectionName", "Composite section name.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;
            
            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "TubeMaterial", "TubeMaterial", "Steel tube material.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "IMateral", "IMateral", "I profile material.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Concrete", "Concrete", "Concrete material.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "IProfile", "IProfile", "Steel section from database. Must be an I-shaped section type.", GH_ParamAccess.item);
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "d", "d", "Diameter of steel tube [mm].", GH_ParamAccess.item, new GH_Number(600));
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

            Materials.Material steelTube = new Materials.Material();
            if (!DA.GetData(1, ref steelTube)) { return; }

            Materials.Material iMaterial = new Materials.Material();
            if (!DA.GetData(2, ref steelTube)) { return; }

            Materials.Material concrete = new Materials.Material();
            if (!DA.GetData(3, ref concrete)) { return; }

            Sections.Section iProfile = null;
            if(!DA.GetData(4, ref iProfile)) { return; }

            double d = 600;
            DA.GetData(5, ref d);

            double t = 15;
            DA.GetData(6, ref t);

            // check input data
            if (steelTube.Family != Materials.Family.Steel || iMaterial.Family != Materials.Family.Steel)
                throw new ArgumentException($"SteelTube and IMaterial inputs must be steel materials!");
            if (concrete.Family != Materials.Family.Concrete)
                throw new ArgumentException($"Concrete input must be concrete material but it is {concrete.Family}");

            // create composite section
            Composites.CompositeSection compositeSection = Composites.CompositeSection.FilledSteelTubeWithIProfile(name, steelTube, iMaterial, concrete, iProfile, d, t);

            // get output
            DA.SetData(0, compositeSection);
        }

        protected void setModelProps()
        {
            this.Parent_Component.ExpireSolution(true);
        }
    }
}