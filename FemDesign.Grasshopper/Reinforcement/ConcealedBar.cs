// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using FemDesign.GenericClasses;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public class ConcealedBar : FEM_Design_API_Component
    {
        public ConcealedBar() : base("ConcealedBar", "ConcealedBar", "Create a Concealed Bar (wire).", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddRectangleParameter("Rectangle", "Rectangle", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("RefConcreteSlab", "RefConcreteSlab", "", GH_ParamAccess.item);
            pManager.AddGenericParameter("AxisLongerSide", "AxisLongerSide", "", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("ConcealedBar", "ConcealedBar", "ConcealedBar.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Rectangle3d rectangle = Rectangle3d.Unset;
            if (!DA.GetData("Rectangle", ref rectangle)) return;

            FemDesign.Shells.Slab slab = null;
            if (!DA.GetData("RefConcreteSlab", ref slab)) return;

            bool axisLongerDirection = true;
            DA.GetData(3, ref axisLongerDirection);

            var point3d = rectangle.PointAt(0,0).FromRhino();

            var obj = new FemDesign.Reinforcement.ConcealedBar(slab, point3d, rectangle, axisLongerDirection);

            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{C03C50FB-33BE-45B5-B48B-ED94C3B8F1C3}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
    }
}