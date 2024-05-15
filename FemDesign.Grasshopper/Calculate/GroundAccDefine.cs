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
    public class GroundAccDefine : FEM_Design_API_Component
    {
        public GroundAccDefine() : base("GroundAcc.Define", "GroundAcc", "Define calculation parameters for ground acceleration analysis.", CategoryName.Name(), SubCategoryName.Cat7a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("LevelAccResponseSpectraCalc", "LARS", "If true, the level acceleration response spectra calculation will be executed.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddNumberParameter("dT", "dT", "'Delta t' calculation parameter for Level acceleration spectra analysis [s].", GH_ParamAccess.item, 0.2);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddNumberParameter("t_end", "t_end", "'t end' calculation parameter for Level acceleration spectra analysis [s].", GH_ParamAccess.item, 5.0);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddNumberParameter("q", "q", "'q' calculation parameter for Level acceleration spectra analysis [s].", GH_ParamAccess.item, 1.0);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddBooleanParameter("TimeHistoryCalc", "TH", "If true, the time history calculation will be executed.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddIntegerParameter("TimeStep", "Step", "The number of every nth time steps when results are saved during the calculation.", GH_ParamAccess.item, 5);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddNumberParameter("LastMoment", "LastMoment", "Last time moment of the time history calculation [s].", GH_ParamAccess.item, 20.0);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddTextParameter("Method", "Method", "Connect 'ValueList' to get the options.\nIntegration scheme method type:\nNewmark\nWilsonTheta", GH_ParamAccess.item, "Newmark");
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddNumberParameter("alpha", "alpha", "'alpha' coefficient in the Rayleigh damping matrix.", GH_ParamAccess.item, 0.0);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddNumberParameter("beta", "beta", "'beta' coefficient in the Rayleigh damping matrix.", GH_ParamAccess.item, 0.0);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddNumberParameter("ksi", "ksi", "'ksi' damping factor.", GH_ParamAccess.item, 5.0);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("GroundAcc", "GroundAcc", "Settings for ground acceleration analysis.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // Get input parameters
            bool calcType1 = true;
            DA.GetData(0, ref calcType1);

            double dT = 0.2;
            DA.GetData(1, ref dT);

            double tEnd = 5.0;
            DA.GetData(2, ref tEnd);

            double q = 1.0;
            DA.GetData(3, ref q);

            bool calcType2 = true;
            DA.GetData(4, ref calcType2);

            int step = 5;
            DA.GetData(5, ref step);

            double lastMoment = 20.0;
            DA.GetData(6, ref lastMoment);

            string method = "Newmark";
            DA.GetData(7, ref method);

            double alpha = 0.0;
            DA.GetData(8, ref alpha);

            double beta = 0.0;
            DA.GetData(9, ref beta);

            double ksi = 5.0;
            DA.GetData(10, ref ksi);

            // Parse 'method' input to enum
            IntegrationSchemeMethod _method = FemDesign.GenericClasses.EnumParser.Parse<IntegrationSchemeMethod>(method);

            GroundAcc obj = new Calculate.GroundAcc(calcType1, dT, tEnd, q, calcType2, step, lastMoment, _method, alpha, beta, ksi);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.GroundAccelerationDefine2;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{78CF01FC-9D99-4E5E-8066-6BE9BAD5BF90}"); }
        }
        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 2, Enum.GetNames(typeof(IntegrationSchemeMethod)).ToList(), null, GH_ValueListMode.DropDown);
        }
        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }
}