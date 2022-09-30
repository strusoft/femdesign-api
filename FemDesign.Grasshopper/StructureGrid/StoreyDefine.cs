// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class StoreyDefine: GH_Component
    {
        public StoreyDefine(): base("Storey.Define", "Define", "Define a storey.", "FEM-Design", "ModellingTools")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of storey", GH_ParamAccess.item);
            pManager.AddPointParameter("Origo", "Origo", "Origo of storey. Storeys can only have unique Z-coordinates. If several storeys are placed in a model their origos should share XY-coordinates.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Direction", "Direction of storey x'-axis in the XY-plane. If several storeys are placed in a model their direction should be identical. Optional, default value is GCS x-axis.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("DimensionX", "DimensionX", "Dimension in x'-direction. [m]", GH_ParamAccess.item, 50);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("DimensionY", "DimensionY", "Dimension in y'-direction. [m]", GH_ParamAccess.item, 30);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Storey", "Storey", "Storey.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            string name = null;
            if (!DA.GetData(0, ref name))
            {
                return;
            }

            Rhino.Geometry.Point3d origo = Rhino.Geometry.Point3d.Origin;
            if (!DA.GetData(1, ref origo))
            {
                return;
            }

            Rhino.Geometry.Vector3d direction = Rhino.Geometry.Vector3d.XAxis;
            if (!DA.GetData(2, ref direction))
            {
                // pass
            }

            double dimX = 50;
            if (!DA.GetData(3, ref dimX))
            {
                // pass
            }

            double dimY = 30;
            if (!DA.GetData(4, ref dimY))
            {
                // pass
            }

            if (name == null)
            {
                return;
            }

            // convert geometry
            FemDesign.Geometry.Point3d p = origo.FromRhino();
            FemDesign.Geometry.Vector3d v = direction.FromRhino();

            // return
            FemDesign.StructureGrid.Storey obj = new FemDesign.StructureGrid.Storey(p, v, dimX, dimY, name);
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.StoreyDefine;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("65f147bb-c8a2-40f2-a4cc-75dc380edccc"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}