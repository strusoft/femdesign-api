// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class PeakSmoothingRegionDeconstruct : GH_Component
    {
        public PeakSmoothingRegionDeconstruct() : base("PeakSmoothingRegion.Deconstruct", "Deconstruct", "Deconstruct PeakSmoothingRegion.", CategoryName.Name(), "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PeakSmoothingRegion", "SmRegion", "PeakSmoothingRegion object.", GH_ParamAccess.list);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "Globally unique identifier.", GH_ParamAccess.list);
            pManager.AddBrepParameter("Region", "Region", "Region.", GH_ParamAccess.tree);
            pManager.AddBooleanParameter("Inactive", "Inactive", "If true, the smoothing region is inactive else it is active.", GH_ParamAccess.list);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            var obj = new List<FemDesign.FiniteElements.PeakSmoothingRegion>();
            if (!DA.GetDataList(0, obj)) return;

            // check input
            if (obj == null) return;

            List<Guid> guid = obj.Select(o => o.Guid).ToList();
            List<bool> inactive = obj.Select(o => o.Inactive).ToList();

            List<Brep> region = new List<Brep>();
            foreach(var elem in obj)
            {
                Brep reg = Convert.ToRhinoBrep(elem.Region);
                region.Add(reg);
            }
            
            // get output
            DA.SetDataList(0, guid);
            DA.SetDataList(1, region);
            DA.SetDataList(2, inactive);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get { return FemDesign.Properties.Resources.PeakSmoothingRegionDeconstruct; }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("3A9DE4EA-D3B8-4AF1-8BC8-F8BFBC3D058D"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.primary;
    }
}
