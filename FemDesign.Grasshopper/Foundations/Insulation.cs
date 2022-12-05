// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class Insulation : GH_Component
    {
        public Insulation() : base("Insulation", "Insulation", "",
            CategoryName.Name(),
            SubCategoryName.Cat0())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("E", "E", "E Modulus [kN/m2]", GH_ParamAccess.item);
            pManager.AddNumberParameter("Thickness", "Thickness", "Thickness [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("Density", "Density", "Unit Mass [t/m3]", GH_ParamAccess.item);
            pManager.AddGenericParameter("LimitStress", "LimitStress", "LimitStress [N/mm2]", GH_ParamAccess.item);
            pManager.AddNumberParameter("GammaMu", "GammaMu", "", GH_ParamAccess.item, 1.2);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("GammaMuas", "GammaMuas", "", GH_ParamAccess.item, 1.0);
            pManager[pManager.ParamCount - 1].Optional = true;

        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Insulation", "Insulation", "Insulation.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double e = 0.0;
            DA.GetData(0, ref e);

            double thickness = 0.0;
            DA.GetData(0, ref thickness);

            double density = 0.0;
            DA.GetData(0, ref density);

            double gamma_m_u = 1.2;
            DA.GetData(0, ref gamma_m_u);

            double gamma_m_uas = 1.0;
            DA.GetData(0, ref gamma_m_uas);

            double limitStress = 0.0;
            DA.GetData(0, ref limitStress);


            var insulation = new FemDesign.Foundations.Insulation(e, thickness, density, limitStress, gamma_m_u, gamma_m_uas);

            // output
            DA.SetData(0, insulation);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Insulation;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{2277FD90-7F69-4311-8601-EDEDF5F127FA}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}
