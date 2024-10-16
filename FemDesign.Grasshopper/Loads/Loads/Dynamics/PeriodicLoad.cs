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
    public class PeriodicLoad : FEM_Design_API_Component
    {
        public PeriodicLoad() : base("PeriodicLoad.Define", "PeriodicLoad.Define", "", CategoryName.Name(), SubCategoryName.Cat3())
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("Frequency", "Frequency", "Hz", GH_ParamAccess.item);
            pManager.AddGenericParameter("PeriodicCases", "PeriodicCases", "", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("PeriodicLoad", "PeriodicLoad", "");
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = "";
            DA.GetData(0, ref name);

            double frequency = 0.0;
            DA.GetData(1, ref frequency);

            List<FemDesign.Loads.PeriodicCase> periodicCases = new List<FemDesign.Loads.PeriodicCase>();
            DA.GetDataList(2, periodicCases);

            var periodicLoads = new FemDesign.Loads.PeriodicLoad(name, frequency, periodicCases);

            DA.SetData(0, periodicLoads);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PeriodicExcitationLoad;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{F6A3C3AC-8399-4894-9B77-BF0A65CF4F46}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.obscure;


    }
}