// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class Slab_SetStiffnessModifier : GH_Component
    {
        public Slab_SetStiffnessModifier() : base("Slab.SetStiffnessModifier", "StiffnessModifier", "Set StiffnessModifier factor on Slab.", CategoryName.Name(),
             SubCategoryName.Cat2b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Slab", GH_ParamAccess.item);
            pManager.AddGenericParameter("StiffnessModifier", "StiffnessModifier", "", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Shells.Slab slab = null;
            if (!DA.GetData(0, ref slab)) { return; }

            Shells.SlabStiffnessFactors stiffnessFactors = null;
            if (!DA.GetData(1, ref stiffnessFactors)) { return; }

            // output
            slab.SlabPart.StiffnessModifiers = new List<Shells.SlabStiffnessFactors> { stiffnessFactors };
            DA.SetData(0, slab);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PlateSetStiffness;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{6813662E-C1B4-4E73-B90D-720360EC8152}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quinary;
    }
}