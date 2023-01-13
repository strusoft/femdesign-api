// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class Bars_SetStiffnessModifier : GH_Component
    {
        public Bars_SetStiffnessModifier() : base("Bars.SetStiffnessModifier", "StiffnessModifier", "Set StiffnessModifier factor on Beam.", CategoryName.Name(),
             SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "Bar", GH_ParamAccess.item);
            pManager.AddGenericParameter("StiffnessModifier", "StiffnessModifier", "", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Bar", "Bar", "Bar.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            Bars.Bar bar = null;
            if (!DA.GetData(0, ref bar)) { return; }

            Bars.BarStiffnessFactors stiffnessFactors = null;
            if (!DA.GetData(1, ref stiffnessFactors)) { return; }

            // output
            bar.BarPart.StiffnessModifiers = new List<Bars.BarStiffnessFactors> { stiffnessFactors };
            DA.SetData(0, bar);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.BeamSetStiffness;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{388DB204-30B4-4337-B98F-734F5E83538F}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;
    }
}