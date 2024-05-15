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
    public class PeriodicExDefine : FEM_Design_API_Component
    {
        public PeriodicExDefine() : base("PeriodicEx.Define", "PeriodicEx", "Define calculation parameters for periodic excitation analysis.", CategoryName.Name(), SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("dT", "dT", "'Delta t' calculation parameter [s].", GH_ParamAccess.item, 0.01);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddNumberParameter("t_end", "t_end", "'t end' calculation parameter [s].", GH_ParamAccess.item, 5.0);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddTextParameter("DmpType", "DmpType", "Connect 'ValueList' to get the options.\nDamping type:\nRayleigh\nKelvinVoigt", GH_ParamAccess.item, "Rayleigh");
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddNumberParameter("alpha", "alpha", "'alpha' coefficient in the Rayleigh damping matrix.", GH_ParamAccess.item, 0.5);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddNumberParameter("beta", "beta", "'beta' coefficient in the Rayleigh damping matrix.", GH_ParamAccess.item, 0.25);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddNumberParameter("ksi", "ksi", "'ksi' damping factor.", GH_ParamAccess.item, 5.0);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PeriodicEx", "PeriodicEx", "Settings for periodic excitation analysis.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get input parameters
            double dT = 0.01;
            DA.GetData(0, ref dT);

            double tEnd = 5.0;
            DA.GetData(1, ref tEnd);

            string type = "Rayleigh";
            DA.GetData(2, ref type);

            double alpha = 0.5;
            DA.GetData(3, ref alpha);

            double beta = 0.25;
            DA.GetData(4, ref beta);

            double ksi = 5.0;
            DA.GetData(5, ref ksi);

            // Parse 'method' input to enum
            DampingType _type = FemDesign.GenericClasses.EnumParser.Parse<DampingType>(type);

            PeriodicExcitation obj = new Calculate.PeriodicExcitation(dT, tEnd, _type, alpha, beta, ksi);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PeriodicExcitationDefine2;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{DC6FE7D7-D4BB-4E1C-9084-9AD70E6D9FE9}"); }
        }
        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 2, Enum.GetNames(typeof(DampingType)).ToList(), null, GH_ValueListMode.DropDown);
        }
        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }
}