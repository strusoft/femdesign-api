// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class BarsConnectivityConstruct : FEM_Design_API_Component
    {
        public BarsConnectivityConstruct() : base("Connectivity.Construct", "Construct", "Construct end releases for a bar element.", CategoryName.Name(),
            SubCategoryName.Cat2a())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("t_x", "t_x", "Translation local-x axis.\nTrue: rigid.\nFalse: free.\nNumber: stiffness [kN/m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("t_y", "t_y", "Translation local-y axis.\nTrue: rigid.\nFalse: free.\nNumber: stiffness [kN/m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("t_z", "t_z", "Translation local-z axis.\nTrue: rigid.\nFalse: free.\nNumber: stiffness [kN/m]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("r_x", "r_x", "Rotation around local-x axis.\nTrue: rigid.\nFalse: free.\nNumber: stiffness [kNm/rad]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("r_y", "r_y", "Rotation around local-y axis.\nTrue: rigid.\nFalse: free.\nNumber: stiffness [kNm/rad]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("r_z", "r_z", "Rotation around local-z axis.\nTrue: rigid.\nFalse: free.\nNumber: stiffness [kNm/rad]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Connectivity", "Connectivity", "Bar element release", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            Bars.Connectivity connectivity = new Bars.Connectivity();

            // get indata
            dynamic t_x = true, t_y = true, t_z = true, r_x = true, r_y = true, r_z = true;

            DA.GetData(0, ref t_x);
            DA.GetData(1, ref t_y);
            DA.GetData(2, ref t_z);
            DA.GetData(3, ref r_x);
            DA.GetData(4, ref r_y);
            DA.GetData(5, ref r_z);

            #region tx release
            try
            {
                if (typeof(bool).IsAssignableFrom(t_x.Value.GetType()))
                {
                    connectivity.Tx = t_x.Value;
                }
                else
                {
                    connectivity.Tx = false;
                    try
                    {
                        connectivity.TxRelease = double.Parse(t_x.Value);
                    }
                    catch { connectivity.TxRelease = t_x.Value; }
                }
            }
            catch
            {
                connectivity.Tx = t_x;
            }
            #endregion

            #region ty release
            try
            {
                if (typeof(bool).IsAssignableFrom(t_y.Value.GetType()))
                {
                    connectivity.Ty = t_y.Value;
                }
                else
                {
                    connectivity.Ty = false;
                    try
                    {
                        connectivity.TyRelease = double.Parse(t_y.Value);
                    }
                    catch { connectivity.TyRelease = t_y.Value; }
                }
            }
            catch { connectivity.Ty = t_y; }

            #endregion

            #region tz release
            try
            {
                if (typeof(bool).IsAssignableFrom(t_z.Value.GetType()))
                {
                    connectivity.Tz = t_z.Value;
                }
                else
                {
                    connectivity.Tz = false;
                    try
                    {
                        connectivity.TzRelease = double.Parse(t_z.Value);
                    }
                    catch { connectivity.TzRelease = t_z.Value; }
                }
            }
            catch
            {
                connectivity.Tz = t_z;
            }


            #endregion

            #region rx release
            try
            {
                if (typeof(bool).IsAssignableFrom(r_x.Value.GetType()))
                {
                    connectivity.Rx = r_x.Value;
                }
                else
                {
                    connectivity.Rx = false;
                    try
                    {
                        connectivity.RxRelease = double.Parse(r_x.Value);
                    }
                    catch { connectivity.RxRelease = r_x.Value; }
                }
            }
            catch { connectivity.Rx = r_x; }

            #endregion

            #region ry release
            try
            {
                if (typeof(bool).IsAssignableFrom(r_y.Value.GetType()))
                {
                    connectivity.Ry = r_y.Value;
                }
                else
                {
                    connectivity.Ry = false;
                    try
                    {
                        connectivity.RyRelease = double.Parse(r_y.Value);
                    }
                    catch { connectivity.RyRelease = r_y.Value; }
                }
            }
            catch { connectivity.Ry = r_y; }

            #endregion

            #region rz release
            try
            {
                if (typeof(bool).IsAssignableFrom(r_z.Value.GetType()))
                {
                    connectivity.Rz = r_z.Value;
                }
                else
                {
                    connectivity.Rz = false;
                    try
                    {
                        connectivity.RzRelease = double.Parse(r_z.Value);
                    }
                    catch { connectivity.RzRelease = r_z.Value; }
                }
            }
            catch { connectivity.Rz = r_z; }
            #endregion


            // return
            DA.SetData(0, connectivity);
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
            get { return new Guid("{ECCAB9F9-2053-4654-80D4-858BF57EAB62}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}