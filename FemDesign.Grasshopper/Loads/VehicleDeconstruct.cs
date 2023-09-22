// https://strusoft.com/
using System;
using System.Collections.Generic;
using FemDesign.LibraryItems;
using Grasshopper.Kernel;


using FemDesign.LibraryItems;
using FemDesign.Loads;
using System.Linq;

namespace FemDesign.Grasshopper
{
    public class VehicleDeconstruct : GH_Component
    {
        public VehicleDeconstruct() : base("VehicleDeconstruct", "VehicleDeconstruct", "", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Vehicle", "Vehicle", "", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Name", "Name", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("PointLoad", "PointLoad", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("LineLoad", "LineLoad", "", GH_ParamAccess.list);
            pManager.AddGenericParameter("SurfaceLoad", "SurfaceLoad", "", GH_ParamAccess.list);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            StruSoft.Interop.StruXml.Data.Vehicle_lib_type vehicle = null;
            DA.GetData(0, ref vehicle);


            var name = vehicle.Name;

            var pointLoad = vehicle.Point_load.Select( x => (PointLoad)x);
            var lineLoad = vehicle.Line_load.Select(x => (LineLoad)x);
            var surfaceLoad = vehicle.Surface_load.Select(x => (SurfaceLoad)x);

            DA.SetData(0, name);
            DA.SetDataList(1, pointLoad);
            DA.SetDataList(2, lineLoad);
            DA.SetDataList(3, surfaceLoad);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Vehicle;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{DAD6C592-F8A4-46F1-BDB6-1F7674BC9F2C}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.obscure;

    }
}