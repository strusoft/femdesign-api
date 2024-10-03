using Grasshopper.Kernel;
using Grasshopper.Kernel.Parameters;
using Grasshopper.Kernel.Types;
using FemDesign.Grasshopper.Components.UIWidgets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using FemDesign.Grasshopper;
using System.Windows.Forms;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using FemDesign.Loads;
using Grasshopper.Kernel.Special;
using Eto.Drawing;

namespace FemDesign.Grasshopper
{
    public class CompositeSectionDefine : GH_SwitcherComponent
    {
        private List<SubComponent> _subcomponents = new List<SubComponent>();
        public override string UnitMenuName => "Section";
        protected override string DefaultEvaluationUnit => _subcomponents[1].name();
        public override Guid ComponentGuid => new Guid("{A6B804EA-F254-4ABE-BEC5-FA69E92069AA}");
        public override GH_Exposure Exposure => GH_Exposure.tertiary;
        protected override System.Drawing.Bitmap Icon => Properties.Resources.HSQProfile;

        public CompositeSectionDefine()
            : base("Composite.Define", "Define",
              "Define a new composite section.",
              CategoryName.Name(), SubCategoryName.Cat4b())
        {
            ((GH_Component)this).Hidden = true;
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.RegisterParam(new Param_GenericObject(), "CompositeSection", "Section", "Steel-concrete composite section.");
        }

        protected override void RegisterEvaluationUnits(EvaluationUnitManager mngr)
        {
            _subcomponents.Add(new EffectiveCompositeSlab());
            _subcomponents.Add(new HSQProfile());
            _subcomponents.Add(new DeltaBeamProfile());
            _subcomponents.Add(new FilledIProfile());
            _subcomponents.Add(new FilledCruciformProfile());
            _subcomponents.Add(new RHSProfile());
            _subcomponents.Add(new FilledSteelTube());
            _subcomponents.Add(new SteelTubeWithIProfile());
            _subcomponents.Add(new SteelTubeWithSteelCore());

            foreach (SubComponent item in _subcomponents)
            {
                item.registerEvaluationUnits(mngr);
            }
        }

        protected override void OnComponentLoaded()
        {
            base.OnComponentLoaded();
            foreach (SubComponent item in _subcomponents)
            {
                item.OnComponentLoaded();
            }
        }

        protected override void SolveInstance(IGH_DataAccess DA, EvaluationUnit unit)
        {
            if (unit == null)
            {
                return;
            }
            
            foreach (SubComponent item in _subcomponents)
            {
                if (unit.Name.Equals(item.name()))
                {
                    item.SolveInstance(DA, out var msg, out var level);
                    if (msg != "")
                    {
                        ((GH_ActiveObject)this).AddRuntimeMessage(level, msg);
                    }
                    return;
                }
            }
            throw new Exception("Invalid sub-component");
        }
        // Part of the code that allows to extend the menu with additional items
        // Right click on the component to see the options
        public override void AppendAdditionalMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalMenuItems(menu);
            if (evalUnits.Units.Count > 0)
            {
                Menu_AppendSeparator(menu);
                ToolStripMenuItem toolStripMenuItem = Menu_AppendItem(menu, "Section types");
                foreach (EvaluationUnit unit in evalUnits.Units)
                {
                    Menu_AppendItem(toolStripMenuItem.DropDown, unit.Name, Menu_ActivateUnit, null, true, unit.Active).Tag = unit;
                }
                Menu_AppendSeparator(menu);
            }
        }
        private void Menu_ActivateUnit(object sender, EventArgs e)
        {
            try
            {
                EvaluationUnit evaluationUnit = (EvaluationUnit)((ToolStripMenuItem)sender).Tag;
                if (evaluationUnit != null)
                {
                    SwitchUnit(evaluationUnit);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}