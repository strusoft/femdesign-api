// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.Loads;

namespace FemDesign.Grasshopper
{
    public class LoadCategoryCreate : GH_Component
    {
        public LoadCategoryCreate() : base("LoadCategory.Create", "Create", "Creates a load category.", "FemDesign", "Loads")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of LoadCategory.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Psi0", "Psi0", "\u03A8₀ Factor for combination value of a variable action", GH_ParamAccess.item);
            pManager.AddNumberParameter("Psi1", "Psi1", "\u03A8₁ Factor for frequent value of a variable action", GH_ParamAccess.item);
            pManager.AddNumberParameter("Psi2", "Psi2", "\u03A8₂ Factor for quasi-permanent value of a variable action", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCategory", "LoadCategory", "LoadCategory.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = null;
            if (!DA.GetData(0, ref name)) return;

            double psi0 = 0, psi1 = 0, psi2 = 0;
            if (!DA.GetData("Psi0", ref psi0)) return;
            if (!DA.GetData("Psi1", ref psi1)) return;
            if (!DA.GetData("Psi2", ref psi2)) return;

            LoadCategory loadCategory = new FemDesign.Loads.LoadCategory(name, psi0, psi1, psi2);

            // return
            DA.SetData("LoadCategory", loadCategory);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LoadGroup;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("e14d1490-c18b-4f91-a9a5-525d00e27f3b"); }
        }
    }
}