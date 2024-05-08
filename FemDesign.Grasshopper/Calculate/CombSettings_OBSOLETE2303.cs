// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class CombinationSettings_OBSOLETE2303 : FEM_Design_API_Component
    {
        public CombinationSettings_OBSOLETE2303() : base("Comb.Settings", "Comb.Settings", "Setup which analyses to consider during calculation of a specific load combination.", CategoryName.Name(), SubCategoryName.Cat7a())
        {

        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadCombination", "LoadCombination", "LoadCombination.", GH_ParamAccess.item);
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
            pManager.AddNumberParameter("Amplitude", "Amp", "Amplitude of selected imperfection shape. [m]", GH_ParamAccess.item, 0.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddIntegerParameter("Waterlevel", "Waterlevel", "Ground water level. [m]", GH_ParamAccess.item, 0);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("CombSettings", "CombSettings", "CombSettings.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            dynamic _loadCombination = null;
            if (!DA.GetData(0, ref _loadCombination)) return;

            string loadCombination = "";

            if (_loadCombination.Value is string str)
            {
                loadCombination = str;
            }
            else if (_loadCombination.Value is FemDesign.Loads.LoadCombination loads)
            {
                loadCombination = loads.Name;
            }



            bool nle = false;
            bool pl = false;
            bool nls = false;
            bool cr = false;
            bool f2nd = false;
            int im = 0;
            double amplitude = 0.0;
            int waterlevel = 0;


            if (!DA.GetData(1, ref nle))
            {
                // pass
            }
            if (!DA.GetData(2, ref pl))
            {
                // pass
            }
            if (!DA.GetData(3, ref nls))
            {
                // pass
            }
            if (!DA.GetData(4, ref cr))
            {
                // pass
            }
            if (!DA.GetData(5, ref f2nd))
            {
                // pass
            }
            if (!DA.GetData(6, ref im))
            {
                // pass
            }
            if (!DA.GetData(7, ref amplitude))
            {
                // pass
            }
            if (!DA.GetData(8, ref waterlevel))
            {
                // pass
            }

            if (pl == true && cr == true)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "PL and Cr can not be mutually be equal True. Cr is set to False!");
                cr = false;
            }

            var combItem = new FemDesign.Calculate.CombItem(loadCombination, 0, 0, nle, pl, nls, cr, f2nd, im, amplitude, waterlevel);


            // return
            DA.SetData(0, combItem);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {

                return FemDesign.Properties.Resources.CombSettings;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{D73ECF46-8D71-442E-845D-7E4474EE3FAF}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.hidden;
    }
}