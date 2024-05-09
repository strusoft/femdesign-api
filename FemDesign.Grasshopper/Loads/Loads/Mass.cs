// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.Loads;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel.Special;

namespace FemDesign.Grasshopper
{
    public class MassDefine : FEM_Design_API_Component
    {
        public MassDefine() : base("Mass.Define", "Mass.Define", "Create a mass load.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Point", "Point. [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("Value", "Value", "Force. [kg]", GH_ParamAccess.item);
            pManager.AddBooleanParameter("ApplyOnEcc", "ApplyOnEcc", "Apply mass on eccentricity.", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.Register_GenericParam("MassLoad", "MassLoad", "MassLoad.");
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Point3d point = Point3d.Origin;
            DA.GetData(0, ref point);

            double value = 0;
            DA.GetData(1, ref value);

            bool applyEcc = false;
            DA.GetData(2, ref applyEcc);

            string comment = null;
            DA.GetData(3, ref comment);


            // Convert geometry
            FemDesign.Geometry.Point3d fdPoint = point.FromRhino();

            var obj = new FemDesign.Loads.Mass(fdPoint, value, applyEcc, comment);
           
            DA.SetData(0, obj);
        }


        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Mass;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{2A04387F-4213-4806-9AFE-1ADE0422E08B}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.senary;

    }
}