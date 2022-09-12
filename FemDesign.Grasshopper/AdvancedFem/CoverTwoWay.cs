// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class CoverTwoWay : GH_Component
    {
        public CoverTwoWay() : base("Cover.TwoWay", "TwoWay", "Create a two way cover.", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface. Surface must be flat.", GH_ParamAccess.item);
            pManager.AddGenericParameter("SupportingElements", "SupportingElements", "bar elements or shell elements. List cannot be nested.", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier.", GH_ParamAccess.item, "CO");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Cover", "Cover", "Two way Cover.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Brep brep = null;
            var structures = new List<FemDesign.GenericClasses.IStructureElement>();
            string identifier = "CO";
            if (!DA.GetData(0, ref brep))
            {
                return;
            }
            if (!DA.GetDataList(1, structures))
            {
                // pass
            }
            if (!DA.GetData(2, ref identifier))
            {
                // pass
            }


            // convert geometry
            FemDesign.Geometry.Region region = brep.FromRhino();

            //
            FemDesign.Cover obj = FemDesign.Cover.TwoWayCover(region, structures, identifier);

            // return
            DA.SetData(0, obj);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.CoverTwoWay;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{63D6BD65-94F5-4B78-95D8-141630B8FF9E}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.tertiary;


    }
}