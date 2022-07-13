// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class LoadCategoryDeconstruct : GH_Component
    {
        public LoadCategoryDeconstruct() : base("LoadCategory.Deconstruct", "Deconstruct", "Deconstruct a LoadCategory.", "FEM-Design", "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCategory", "LoadCategory", "LoadCategory.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name.", GH_ParamAccess.item);
            pManager.AddTextParameter("CountyCode", "CountyCode", "CountyCode", GH_ParamAccess.item);
            pManager.AddNumberParameter("Psi0", "Psi0", "\u03A8₀ Factor for combination value of a variable action", GH_ParamAccess.item);
            pManager.AddNumberParameter("Psi1", "Psi1", "\u03A8₁ Factor for frequent value of a variable action", GH_ParamAccess.item);
            pManager.AddNumberParameter("Psi2", "Psi2", "\u03A8₂ Factor for quasi-permanent value of a variable action", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.Loads.LoadCategory obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }

            // return
            DA.SetData(0, obj.Name);
            DA.SetData(1, obj.Country);
            DA.SetData(2, obj.Psi0);
            DA.SetData(3, obj.Psi1);
            DA.SetData(4, obj.Psi2);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LoadCategoryDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("e3bfa4d8-6b64-47bc-a3ce-37ba74ac4653"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}