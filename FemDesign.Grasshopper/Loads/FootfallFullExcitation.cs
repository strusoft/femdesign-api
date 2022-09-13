// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class FootfallFullExcitation : GH_Component
    {
        public FootfallFullExcitation() : base("Footfall.FullExcitation", "FullExcitation", "Create a footfall full excitation point.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Point", "Point. [m]", GH_ParamAccess.item);
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Footfall", "Footfall", "Footfall.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Point3d point = Point3d.Origin;
            string identifier = null;
            string comment = null;
            if (!DA.GetData(0, ref point)) { return; }
            if (!DA.GetData(1, ref identifier)) { /* Pass */}
            if (!DA.GetData(2, ref comment)) { /* Pass */}

            // convert geometry
            FemDesign.Geometry.Point3d fdPoint = point.FromRhino();

            // Create a Footfall full excitation
            var obj = new Loads.Footfall(fdPoint, identifier, comment);
            DA.SetData(0, obj);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.FootfallFullExcitation;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("e31ca922-e45a-452e-a7fd-112fcbb34300"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.septenary;

    }
}