// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class BarsConnectivityDefineSemiRigid: GH_Component
    {
        public BarsConnectivityDefineSemiRigid(): base("Connectivity.SemiRigid", "SemiRigid", "Define semi-rigid end releases for a bar element.", CategoryName.Name(),
            SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("t_x", "t_x", "Release stiffness. Translation local-x axis. Optional, default value is fully rigid. [kN/m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount -1].Optional = true;
            pManager.AddNumberParameter("t_y", "t_z", "Release stiffness. Translation local-y axis. Optional, default value is fully rigid. [kN/m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount -1].Optional = true;
            pManager.AddNumberParameter("t_z", "t_z", "Release stiffness. Translation local-z axis. Optional, default value is fully rigid. [kN/m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount -1].Optional = true;
            pManager.AddNumberParameter("r_x", "r_x", "Release stiffness. Rotation around local-x axis. Optional, default value is fully rigid. [kNm/rad]", GH_ParamAccess.item);
            pManager[pManager.ParamCount -1].Optional = true;
            pManager.AddNumberParameter("r_y", "r_y", "Release stiffness. Rotation around local-y axis. Optional, default value is fully rigid. [kNm/rad]", GH_ParamAccess.item);
            pManager[pManager.ParamCount -1].Optional = true;
            pManager.AddNumberParameter("r_z", "r_z", "Release stiffness. Rotation around local-z axis. Optional, default value is fully rigid. [kNm/rad]", GH_ParamAccess.item);
            pManager[pManager.ParamCount -1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connectivity", "Connectivity", "Bar element release", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            bool mx = false, my = false, mz = false, rx = false, ry = false, rz = false;
            double mxRelease = 0.00, myRelease = 0.00, mzRelease = 0.00, rxRelease = 0.00, ryRelease = 0.00, rzRelease = 0.00;
            if (!DA.GetData(0, ref mxRelease))
            {
                mx = true;
            }
            if (!DA.GetData(1, ref myRelease))
            {
                my = true;
            }
            if (!DA.GetData(2, ref mzRelease))
            {
                mz = true;
            }
            if (!DA.GetData(3, ref rxRelease))
            {
                rx = true;
            }
            if (!DA.GetData(4, ref ryRelease))
            {
                ry = true;
            }
            if (!DA.GetData(5, ref rzRelease))
            {
                rz = true;
            }

            // connectivity
            FemDesign.Bars.Connectivity obj = new Bars.Connectivity(mx, my, mz, rx, ry, rz, mxRelease, myRelease, mzRelease, rxRelease, ryRelease, rzRelease);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.ConnectivityDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("2440d612-86bd-4113-ab16-2ff018c17a62"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}