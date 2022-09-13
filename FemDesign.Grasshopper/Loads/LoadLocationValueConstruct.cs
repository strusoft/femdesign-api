// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class LoadLocationValueConstruct: GH_Component
    {
        public LoadLocationValueConstruct(): base("LoadLocationValue.Construct", "Construct", "Construct a LoadLocationValue object. LoadLocationValue objects are used to define a SurfaceLoad with variable intensity [q1, q2, q3].", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Point", "Position of Load. [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("Intensity", "Intensity", "Intensity of load. [kN/m²]", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LoadLocationValue", "LoadLocationValue", "LoadLocationValue.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Point3d point = Point3d.Origin;
            double val = 0;
            if (!DA.GetData(0, ref point)) { return; }
            if (!DA.GetData(1, ref val)) { return; }
            if (point == null) { return; }

            //  transform geometry
            FemDesign.Geometry.Point3d fdPoint = point.FromRhino();

            //
            FemDesign.Loads.LoadLocationValue obj = new FemDesign.Loads.LoadLocationValue(fdPoint, val);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LoadDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("d7c9d6be-3474-41f6-b58c-0029fd24729f"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quinary;

    }
}