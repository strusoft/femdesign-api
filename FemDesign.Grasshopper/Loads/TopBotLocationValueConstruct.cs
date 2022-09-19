// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class TopBotLocationValueConstruct: GH_Component
    {
        public TopBotLocationValueConstruct(): base("TopBotLocationValue.Construct", "Construct", "Define a top bottom location value", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Point", "Point", "Point of top bottom location value. [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("TopVal", "TopVal", "Top value. [\x00B0C]", GH_ParamAccess.item);
            pManager.AddNumberParameter("BottomVal", "BottomVal", "Bottom value.  [\x00B0C]", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("TopBotLocationValue", "TopBotLocationValue", "TopBotLocationValue.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Point3d point = Point3d.Origin;
            if (!DA.GetData(0, ref point))
            {
                return;
            }

            double topVal = 0;
            if (!DA.GetData(1, ref topVal))
            {
                return;
            }

            double bottomVal = 0;
            if (!DA.GetData(2, ref bottomVal))
            {
                return;
            }

            // convert geometry
            Geometry.Point3d p = point.FromRhino();

            // create obj
            Loads.TopBotLocationValue obj = new Loads.TopBotLocationValue(p, topVal, bottomVal);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.TopBottomValue;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("ec2c2062-0b9c-4052-a61d-75c2d496e670"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quinary;

    }
}