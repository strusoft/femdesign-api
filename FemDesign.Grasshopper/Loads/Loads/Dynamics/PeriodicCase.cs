// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using FemDesign;
using FemDesign.Calculate;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel.Special;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FemDesign.Loads;

namespace FemDesign.Grasshopper
{
    public class PeriodicCase : FEM_Design_API_Component
    {
        public PeriodicCase() : base("PeriodicCase.Define", "PeriodicCase.Define", "", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Factor", "Factor", "", GH_ParamAccess.item, 1.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Phase", "Phase", "\"Connect 'ValueList' to get the options.\\nPhase type:\\nSin\\\nCos", GH_ParamAccess.item, "Sin");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("LoadCase", "LoadCase", "", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("PeriodicCase", "PeriodicCase", "");
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double factor = 1.0;
            DA.GetData(0, ref factor);

            string phase = "Cos";
            DA.GetData(1, ref phase);

            FemDesign.Loads.LoadCase loadCase = null;
            DA.GetData(2, ref loadCase);

            Loads.PeriodicCase.Shape _shape = FemDesign.GenericClasses.EnumParser.Parse<Loads.PeriodicCase.Shape>(phase);

            var periodicCase = new FemDesign.Loads.PeriodicCase(factor, _shape, loadCase);

            DA.SetData(0, periodicCase);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PeriodicCase;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{4305829A-FDB0-4B7D-9F0D-12278C013661}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.obscure;

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 1, Enum.GetNames(typeof(FemDesign.Loads.PeriodicCase.Shape)).ToList(), null, GH_ValueListMode.DropDown);
        }
    }
}