// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.Loads;
using System.Linq;
using Grasshopper.Kernel.Special;

using FemDesign.Grasshopper.Extension.ComponentExtension;

namespace FemDesign.Grasshopper
{
    public class ActivatedLoadCase : GH_Component
    {
        public ActivatedLoadCase() : base("ActivatedLoadCase", "ActivatedLoadCase", "Creates an (construction stage) activated load case.", CategoryName.Name(),
            SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase to be activated.\n\nLoadCase may also be \"PTC T0\" or \"PTC T8\" to activate a PTC load", GH_ParamAccess.item);
            pManager.AddNumberParameter("UFactor", "U", "Load case factor for ULS combinations in Construction stages.", GH_ParamAccess.item, 1.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("ScFactor", "Sc", "Load case factor for SLS-Characteristic combinations in Construction stages.", GH_ParamAccess.item, 1.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("SfFactor", "Sf", "Load case factor for SLS-Frequent combinations in Construction stages.", GH_ParamAccess.item, 1.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("SqFactor", "Sq", "Load case factor for SLS-Quasi-permanent combinations in Construction stages.", GH_ParamAccess.item, 1.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Partitioning", "Partitioning", "Connect 'ValueList' to get the options.\n0 - only_in_this_stage\n1 - from_this_stage_on\n2 - shifted_from_first_stage\n3 - only_stage_activated_elem.\nDefault: from_this_stage_on.", GH_ParamAccess.item, "from_this_stage_on");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ActivatedLoadCase", "ActivatedLoadCase", "", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Loads.LoadCase loadCase = null;
            string ptcLoadCase = "";
            bool isLoadCase = DA.GetData("LoadCase", ref loadCase);
            bool isString = DA.GetData("LoadCase", ref ptcLoadCase);
            bool isPTCLoadCase = isString && (ptcLoadCase.ToUpper() == "PTC T0" || ptcLoadCase.ToUpper() == "PTC T8");
            if (!(isLoadCase || isPTCLoadCase)) return;
            ClearRuntimeMessages(); // Remove the error message. One of the GetData above will always add an error message since we try to parse mutliple types.

            double factor = 1;
            DA.GetData(1, ref factor);

            double scFactor = 1;
            DA.GetData(2, ref scFactor);

            double sfFactor = 1;
            DA.GetData(3, ref sfFactor);

            double sqFactor = 1;
            DA.GetData(4, ref sqFactor);

            string partitioning = "1";
            DA.GetData(5, ref partitioning);

            ActivationType type = FemDesign.GenericClasses.EnumParser.Parse<ActivationType>(partitioning);
            SFactorType sFactors = new SFactorType(scFactor, sfFactor, sqFactor);

            FemDesign.ActivatedLoadCase activatedLoadCase;
            if (isLoadCase)
                activatedLoadCase = new FemDesign.ActivatedLoadCase(loadCase, factor, type, sFactors);
            else if (isPTCLoadCase)
            {
                PTCLoadCase _ptcLoadCase = ptcLoadCase.ToUpper() == "PTC T0" ? PTCLoadCase.T0 : PTCLoadCase.T8;
                activatedLoadCase = new FemDesign.ActivatedLoadCase(_ptcLoadCase, factor, type, sFactors);
            }
            else throw new Exception("The input was not a LoadCase or a PTCLoadCase.");

            DA.SetData(0, activatedLoadCase);
        }

        protected override void BeforeSolveInstance()
        {
            var resultTypes = Enum.GetNames(typeof(ActivationType)).ToList();
            ValueListUtils.updateValueLists(this, 5, resultTypes, null);

        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.StageActivatedLoad;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("C55679D2-8A02-4C3C-8C05-785F6C934CF3"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}