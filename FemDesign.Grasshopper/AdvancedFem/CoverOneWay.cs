// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class CoverOneWay : GH_Component
    {
        public CoverOneWay() : base("Cover.OneWay", "OneWay", "Create a one way cover.", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface. Surface must be flat.", GH_ParamAccess.item);
            pManager.AddGenericParameter("SupportingElements", "SupportingElements", "bar elements or shell elements. List cannot be nested.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("LoadBearingDirection", "LoadBearingDirection", "Vector of load bearing direction.", GH_ParamAccess.item, Vector3d.XAxis);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier.", GH_ParamAccess.item, "CO");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Cover", "Cover", "One way Cover.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Brep brep = null;
            var structures = new List<FemDesign.GenericClasses.IStructureElement>();
            Vector3d vector = Vector3d.XAxis;
            string identifier = "CO";

            if (!DA.GetData(0, ref brep))
            {
                return;
            }
            if (!DA.GetDataList(1, structures))
            {
                // pass
            }
            if (!DA.GetData(2, ref vector))
            {
                // pass
            }
            if (!DA.GetData(3, ref identifier))
            {
                // pass
            }


            // convert geometry
            FemDesign.Geometry.Region region = brep.FromRhino();
            FemDesign.Geometry.Vector3d fdVector3d = vector.FromRhino().Normalize();

            //
            FemDesign.Cover obj = FemDesign.Cover.OneWayCover(region, structures, fdVector3d, identifier);

            // return
            DA.SetData(0, obj);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.CoverOneWay;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{7F1B0264-54F0-4D31-BEB3-23E5F151FE09}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}

