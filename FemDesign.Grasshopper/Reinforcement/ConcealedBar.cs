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
            pManager.AddGenericParameter("RefConcreteSlab", "RefConcreteSlab", "Concrete slab/wall where the concealed bar should be created", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Axis", "Axis",
                "True:  the axis of the concealed bar will follow the X axis of the rectangle.\n" +
                "False: the axis of the concealed bar will follow the Y axis of the rectangle.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "", GH_ParamAccess.item, "CB");
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
            DA.GetData("Axis", ref axisLongerDirection);

            string identifier = "CB";
            DA.GetData("Identifier", ref identifier);

            var _rectangle = rectangle.FromRhino();

            // error handling in Grasshopper
            // check if corners are in the slab
            for(int i = 0; i < 4; i++)
            {
                var pt = rectangle.Corner(i);
                var closestPt = slab.Region.ToRhinoBrep().ClosestPoint(pt);
                if(closestPt.DistanceTo(pt) >= Tolerance.LengthComparison)
                {
                    throw new Exception($"Rectangle does not lie on the slab region!");
                }
            }

            var obj = new FemDesign.Reinforcement.ConcealedBar(slab, _rectangle, axisLongerDirection, identifier);

            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.ConcealedBar;
        public override Guid ComponentGuid
        {
            get { return new Guid("{C03C50FB-33BE-45B5-B48B-ED94C3B8F1C3}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;
    }
}