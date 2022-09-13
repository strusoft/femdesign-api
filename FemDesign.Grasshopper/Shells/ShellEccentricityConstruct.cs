// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using FemDesign.GenericClasses;

namespace FemDesign.Grasshopper
{
    public class ShellEccentricityConstruct: GH_Component
    {
        public ShellEccentricityConstruct(): base("ShellEccentricity.Construct", "Construct", "Construct a ShellEccentricity", CategoryName.Name(), SubCategoryName.Cat2b())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Alignment", "Alignment", "top/bottom/center", GH_ParamAccess.item, "center");
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("Eccentricity", "Ecc", "Eccentricity. [m]", GH_ParamAccess.item, 0.00);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("EccentricityCalculation", "EccCalc", "Consider eccentricity in calculation? True/false.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("EccentricityByCracking", "EccByCracking", "Consider eccentricity caused by cracking in cracked section analysis? True/false.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ShellEccentricity", "ShellEccentricity", "ShellEccentricity.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // 
            string alignment = "center";
            double ecc = 0;
            bool eccCalc = false;
            bool eccByCracking = false;
            if (!DA.GetData(0, ref alignment))
            {
                // pass
            }
            if (!DA.GetData(1, ref ecc))
            {
                // pass
            }
            if (!DA.GetData(2, ref eccCalc))
            {
                // pass
            }
            if (!DA.GetData(3, ref eccByCracking))
            {
                // pass
            }
            if (alignment == null)
            {
                return;
            }


            VerticalAlignment _alignment = EnumParser.Parse<VerticalAlignment>(alignment);
            FemDesign.Shells.ShellEccentricity shellEcc = new FemDesign.Shells.ShellEccentricity(_alignment, ecc, eccCalc, eccByCracking);

            // return
            DA.SetData(0, shellEcc);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ShellEccentricityDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("6b203b89-93b2-410f-8a51-2575a27f9a0f"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}