// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class PointSupportRigid: GH_Component
    {
        public PointSupportRigid(): base("PointSupport.Rigid", "Rigid", "Create a Rigid PointSupport element.", CategoryName.Name(),
            SubCategoryName.Cat1())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPlaneParameter("Position", "Position", "Point|Plane location to place the PointSupport. [m]\nDefault orientation is WorldXY Plane.", GH_ParamAccess.item);
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional, default value if undefined.", GH_ParamAccess.item, "S");
           pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PointSupport", "PointSupport", "Rigid PointSupport.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            //
            Plane plane = Plane.WorldXY;
            DA.GetData(0, ref plane);

            string identifier = "S";
            DA.GetData(1, ref identifier);



            // convert geometry
            var fdPlane = plane.FromRhinoPlane();
            var obj = FemDesign.Supports.PointSupport.Rigid(fdPlane, identifier);

            // return
            DA.SetData(0, obj);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PointSupportRigid;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("fda89a34-ef1f-4ccc-a563-e5892288ea7b"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}