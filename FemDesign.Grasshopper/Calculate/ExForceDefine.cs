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
    public class ExForceDefine : FEM_Design_API_Component
    {
        public ExForceDefine() : base("ExForce.Define", "ExForce", "Define calculation parameters for excitation force analysis.", CategoryName.Name(), SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddIntegerParameter("TimeStep", "Step", "Frequency of saved results (every nth step will be saved).", GH_ParamAccess.item, 5);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddNumberParameter("t_end", "t_end", "End time of calculation [s].", GH_ParamAccess.item, 20.0);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddTextParameter("Method", "Method", "Connect 'ValueList' to get the options.\nIntegration scheme method type:\nNewmark\nWilsonTheta", GH_ParamAccess.item, "Newmark");
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
            pManager.AddGenericParameter("ExForce", "ExForce", "Settings for excitation force analysis.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get input parameters
            int step = 5;
            DA.GetData(0, ref step);

            double lastMoment = 20.0;
            DA.GetData(1, ref lastMoment);

            string method = "Newmark";
            DA.GetData(2, ref method);

            double alpha = 0.5;
            DA.GetData(3, ref alpha);

            double beta = 0.25;
            DA.GetData(4, ref beta);

            double ksi = 5.0;
            DA.GetData(5, ref ksi);

            // Parse 'method' input to enum
            IntegrationSchemeMethod _method = FemDesign.GenericClasses.EnumParser.Parse<IntegrationSchemeMethod>(method);

            ExcitationForce obj = new Calculate.ExcitationForce(step, lastMoment, _method, alpha, beta, ksi);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ExcitationForceDefine2;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{1EA08508-EE0B-48BD-BDF8-A61ABB02BCDC}"); }
        }
        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 2, Enum.GetNames(typeof(IntegrationSchemeMethod)).ToList(), null, GH_ValueListMode.DropDown);
        }
        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }
}