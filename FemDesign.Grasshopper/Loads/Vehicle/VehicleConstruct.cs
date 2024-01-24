// https://strusoft.com/
using System;
using System.Collections.Generic;
using FemDesign.LibraryItems;
using Grasshopper.Kernel;


namespace FemDesign.Grasshopper
{
    public class Vehicle : FEM_Design_API_Component
    {
        public Vehicle() : base("Vehicle.Construct", "Vehicle.Construct", "Define a custom vehicle.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Vehicle name.", GH_ParamAccess.item);
            pManager.AddGenericParameter("PointLoads", "PointLoads", "Caseless point loads.\nConstruct a point load using `caseless` text in the `LoadCase` input.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("LineLoads", "LineLoads", "Caseless line loads.\nConstruct a line load using `caseless` text in the `LoadCase` input.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("SurfaceLoads", "SurfaceLoads", "Caseless surface loads.\nConstruct a surface load using `caseless` text in the `LoadCase` input.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Vehicle", "Vehicle", "", GH_ParamAccess.list);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            string name = "";
            DA.GetData(0, ref name);

            List<FemDesign.Loads.PointLoad> pointLoads = new List<Loads.PointLoad>();
            DA.GetDataList(1, pointLoads);

            List<FemDesign.Loads.LineLoad> lineLoads = new List<Loads.LineLoad>();
            DA.GetDataList(2, lineLoads);

            List<FemDesign.Loads.SurfaceLoad> surfaceLoads = new List<Loads.SurfaceLoad>();
            DA.GetDataList(3, surfaceLoads);

            
            foreach(var load in pointLoads)
            {
                if (!load.IsCaseless)
                {
                    throw new Exception("Point loads must be caseless");
                }

            }

            foreach (var load in lineLoads)
            {
                if (!load.IsCaseless)
                {
                    throw new Exception("Line loads must be caseless");
                }
            }

            foreach (var load in surfaceLoads)
            {
                if (!load.IsCaseless)
                {
                    throw new Exception("Surface loads must be caseless");
                }
            }

            var vehicle = new StruSoft.Interop.StruXml.Data.Vehicle_lib_type(name, pointLoads, lineLoads, surfaceLoads);


            // set output
            DA.SetData(0, vehicle);
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
            get { return new Guid("{4BF8957B-8343-48B2-BADC-4D8B4611C65B}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.obscure;

    }
}