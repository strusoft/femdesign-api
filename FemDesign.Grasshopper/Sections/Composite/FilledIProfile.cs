using System;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Components.UIWidgets;


namespace FemDesign.Grasshopper
{
    public class FilledIProfile : SubComponent
    {
        public System.Drawing.Bitmap Icon => Properties.Resources.FilledIProfile;
        public override string name() => "FilledIProfile";
        public override string display_name() => "FilledIProfile";

        public override void registerEvaluationUnits(EvaluationUnitManager mngr)
        {
            EvaluationUnit evaluationUnit = new EvaluationUnit(name(), display_name(), "Create a composite section for an 'I' profiled steel section with filled with concrete. For more information, see FEM-Design GUI.", this.Icon);
            mngr.RegisterUnit(evaluationUnit);
            
            evaluationUnit.RegisterInputParam(new Param_String(), "SectionName", "SectionName", "Composite section name.", GH_ParamAccess.item);
            
            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Steel", "Steel", "Steel profile material.", GH_ParamAccess.item);

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "Concrete", "Concrete", "Concrete material.", GH_ParamAccess.item);

            evaluationUnit.RegisterInputParam(new Param_GenericObject(), "IProfile", "IProfile", "'I' profile steel section.", GH_ParamAccess.item);

            evaluationUnit.RegisterInputParam(new Param_Number(), "cy", "cy", "Concrete cover in Y direction [mm].", GH_ParamAccess.item, new GH_Number(80));
            evaluationUnit.Inputs[evaluationUnit.Inputs.Count - 1].Parameter.Optional = true;

            evaluationUnit.RegisterInputParam(new Param_Number(), "cz", "cz", "Concrete cover in Z direction [mm].", GH_ParamAccess.item, new GH_Number(80));
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

            Sections.Section iProf = null;
            if (!DA.GetData(3, ref iProf)) { return; }

            double cy = 80;
            DA.GetData(4, ref cy);

            double cz = 80;
            DA.GetData(5, ref cz);

            // check input data
            if (steel.Family != Materials.Family.Steel)
            {
                throw new ArgumentException($"Steel input must be steel material but it is {steel.Family}");
            }
            if (concrete.Family != Materials.Family.Concrete)
            {
                throw new ArgumentException($"Concrete input must be concrete material but it is {concrete.Family}");
            }

            // create task to create composite section
            Composites.CompositeSection compositeSection = null;
            var task = Task.Run(() =>
            {
                compositeSection = Composites.CompositeSection.FilledIProfile(name, steel, concrete, iProf, cy, cz);
            });

            task.ConfigureAwait(false);
            try
            {
                task.Wait();
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }

            // get output
            DA.SetData(0, compositeSection);
        }

        protected void setModelProps()
        {
            this.Parent_Component.ExpireSolution(true);
        }
    }
}