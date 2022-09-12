// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class BarsConnectivityDefine: GH_Component
    {
        public BarsConnectivityDefine(): base("Connectivity.Define", "Define", "Define end releases for a bar element.", CategoryName.Name(),
            SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("m_x", "m_x", "Translation local-x axis. True if rigid, false if free.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("m_y", "m_z", "Translation local-y axis. True if rigid, false if free.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("m_z", "m_z", "Translation local-z axis. True if rigid, false if free.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("r_x", "r_x", "Rotation around local-x axis. True if rigid, false if free.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("r_y", "r_y", "Rotation around local-y axis. True if rigid, false if free.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("r_z", "r_z", "Rotation around local-z axis. True if rigid, false if free.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connectivity", "Connectivity", "Bar element release", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get indata
            bool m_x = false, m_y = false, m_z = false, r_x = false, r_y = false, r_z = false;
            if (!DA.GetData(0, ref m_x)) { return; }
            if (!DA.GetData(1, ref m_y)) { return; }
            if (!DA.GetData(2, ref m_z)) { return; }
            if (!DA.GetData(3, ref r_x)) { return; }
            if (!DA.GetData(4, ref r_y)) { return; }
            if (!DA.GetData(5, ref r_z)) { return; }

            // return
            DA.SetData(0, new FemDesign.Bars.Connectivity(m_x, m_y, m_z, r_x, r_y, r_z));
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
            get { return new Guid("511ac7c9-555f-441d-85f7-9ede2790606a"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}