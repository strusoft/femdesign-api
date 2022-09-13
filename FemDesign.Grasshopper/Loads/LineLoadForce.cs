// https://strusoft.com/
using System;
using GH = Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class LineLoadForce: GH_Component
    {
        public LineLoadForce(): base("LineLoad.Force", "Force", "Creates a force line load.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "Curve defining the line load.", GH_ParamAccess.item);
            pManager.AddVectorParameter("StartForce", "StartForce", "StartForce. The start force will define the direction of the line load. [kN]", GH_ParamAccess.item);
            pManager.AddVectorParameter("EndForce", "EndForce", "EndForce. Optional. If undefined LineLoad will be uniform with a force of StartForce. [kN]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("ConstLoadDir", "ConstLoadDir", "Constant load direction? If true direction of load will be constant along action line. If false direction of load will vary along action line - characteristic direction is in the middle point of line. Optional.", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("LoadProjection", "LoadProjection", "LoadProjection. \nFalse: Intensity meant along action line (eg. dead load). \nTrue: Intensity meant perpendicular to direction of load (eg. snow load).", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LineLoad", "LineLoad", "LineLoad.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = null;
            if (!DA.GetData("Curve", ref curve)) return;

            Vector3d startForce = Vector3d.Zero;
            if (!DA.GetData("StartForce", ref startForce)) return;

            Vector3d endForce = Vector3d.Zero;
            if (!DA.GetData("EndForce", ref endForce))
            {
                // if no data set endForce to startForce to create a uniform line load.
                endForce = startForce;
            }

            Loads.LoadCase loadCase = null;
            if (!DA.GetData("LoadCase", ref loadCase)) return;

            bool constLoadDir = true;
            DA.GetData("ConstLoadDir", ref constLoadDir);

            bool loadProjection = false;
            DA.GetData("LoadProjection", ref loadProjection);

            string comment = null;
            DA.GetData("Comment", ref comment);

            if (curve == null || startForce == null || endForce == null || loadCase == null) return;

            Geometry.Edge edge = Convert.FromRhinoLineOrArc1(curve);
            Geometry.Vector3d _startForce = startForce.FromRhino();
            Geometry.Vector3d _endForce = endForce.FromRhino();

            try
            {
                var obj = new Loads.LineLoad(edge, _startForce, _endForce, loadCase, Loads.ForceLoadType.Force, comment, constLoadDir, loadProjection);
                DA.SetData("LineLoad", obj);
            }
            catch (ArgumentException e)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, e.Message);
            }
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LineLoadForce;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("41b4dc32-d3c1-474d-9cfd-843fc18b799f"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}