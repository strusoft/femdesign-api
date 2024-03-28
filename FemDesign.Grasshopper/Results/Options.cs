// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign;
using FemDesign.GenericClasses;
using FemDesign.Calculate;


using FemDesign.Grasshopper.Extension.ComponentExtension;

namespace FemDesign.Grasshopper
{
    public class ResultOption : FEM_Design_API_Component
    {
        public ResultOption() : base("ResultOption", "Option", "Specify the output result locations. 'ResultPoints' options requires some prefedined result points to be present in your FEM-Design Model.", CategoryName.Name(), SubCategoryName.Cat7b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Bar", "Bar", "Connect 'ValueList' to get the options.\n0 : OnlyNodes\n1 : ByStep\n2 : ResultPoints.", GH_ParamAccess.item, "ByStep");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Surface", "Surface", "Connect 'ValueList' to get the options.\n0 : Center\n1 : Vertices\n2 : ResultPoints.", GH_ParamAccess.item, "Vertices");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Step", "Step", "Distance between output nodes for 1-d elements. It will be use if 'ByStep' is selected.", GH_ParamAccess.item, 0.50);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Option", "Option", "", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string bar = "ByStep";
            DA.GetData(0, ref bar);

            string surface = "Vertices";
            DA.GetData(1, ref surface);

            double step = 0.50;
            DA.GetData(2, ref step);


            BarResultPosition barRes = EnumParser.Parse<Calculate.BarResultPosition>(bar);
            ShellResultPosition srfRes = EnumParser.Parse<Calculate.ShellResultPosition>(surface);

            var options = new FemDesign.Calculate.Options(barRes, srfRes, step);

            // output
            DA.SetData(0, options);
        }
        protected override void BeforeSolveInstance()
        {
            ValueListUtils.UpdateValueLists(this, 0, Enum.GetNames(typeof(FemDesign.Calculate.BarResultPosition)).ToList(), null);


            ValueListUtils.UpdateValueLists(this, 1, Enum.GetNames(typeof(FemDesign.Calculate.ShellResultPosition)).ToList(), null);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Options;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{EE3E370C-B20B-4521-A61A-0D41D0F4EC24}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}