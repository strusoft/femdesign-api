// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class LoadCombinationSetupCalculation: GH_Component
    {
        public LoadCombinationSetupCalculation(): base("LoadCombination.SetupCalculation", "SetupCalculation", "Setup which analyses to consider during calculation of a specific load combination.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCombination", "LoadCombination", "LoadCombination.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("ImpfRqd", "ImpfRqd", "Required imperfection shapes.", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("StabRqd", "StabRqd", "Required buckling shapes for stability analysis.", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true; 
            pManager.AddBooleanParameter("NLE", "NLE", "Consider elastic non-linear behaviour of structural elements.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true; 
            pManager.AddBooleanParameter("PL", "PL", "Consider plastic behaviour of structural elements.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true; 
            pManager.AddBooleanParameter("NLS", "NLS", "Consider non-linear behaviour of soil.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true; 
            pManager.AddBooleanParameter("Cr", "Cr", "Cracked section analysis. Note that Cr only executes properly in RCDesign with DesignCheck set to true.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true; 
            pManager.AddBooleanParameter("f2nd", "f2nd", "2nd order analysis.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true; 
            pManager.AddIntegerParameter("Im", "Im", "Imperfection shape for 2nd order analysis.", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true; 
            pManager.AddIntegerParameter("Waterlevel", "Waterlevel", "Ground water level. [m]", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCombination", "LoadCombination", "Loadcombination.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            FemDesign.Loads.LoadCombination loadCombination = null;
            int impfRqd = 0;
            int stabRqd = 0;
            bool nle = false;
            bool pl = false;
            bool nls = false;
            bool cr = false;
            bool f2nd = false;
            int im = 0;
            int waterlevel = 0;
            if (!DA.GetData(0, ref loadCombination))
            {
                return;
            }
            if (!DA.GetData(1, ref impfRqd))
            {
                // pass
            }
            if (!DA.GetData(2, ref stabRqd))
            {
                // pass
            }
            if (!DA.GetData(3, ref nle))
            {
                // pass
            }
            if (!DA.GetData(4, ref pl))
            {
                // pass
            }
            if (!DA.GetData(5, ref nls))
            {
                // pass
            }
            if (!DA.GetData(6, ref cr))
            {
                // pass
            }
            if (!DA.GetData(7, ref f2nd))
            {
                // pass
            }
            if (!DA.GetData(8, ref im))
            {
                // pass
            }
            if (!DA.GetData(9, ref waterlevel))
            {
                // pass
            }
            if (loadCombination == null)
            {
                return;
            }

            //
            var clone = loadCombination.DeepClone();

            clone.CombItem = new FemDesign.Calculate.CombItem(impfRqd, stabRqd, nle, pl, nls, cr, f2nd, im, waterlevel);

            // return
            DA.SetData(0, clone);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {

                return FemDesign.Properties.Resources.LoadCombinationSetCalculationParameters;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("ffb78f7b-eb88-4309-92e4-0d76d201106b"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quarternary;
    }
}