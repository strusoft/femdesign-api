// https://strusoft.com/
using System;
using GH = Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class LineStressLoad : GH_Component
    {
        public LineStressLoad() : base("LineStressLoad.Define", "Define", "Creates a line stress load.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve", "Curve", "Curve defining the line stress load.", GH_ParamAccess.item);
            pManager.AddVectorParameter("Direction", "Direction", "Reference axis for Moment Stress. Default is LocalY", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("n1", "n1", "Force at start. Or the force at both the start and end of the curve. [kN]\n+ compression\n- tension", GH_ParamAccess.item);
            pManager.AddNumberParameter("n2", "n2", "Force at end. Optional. [kN]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("m1", "m1", "Moment at start. Or the moment at both the start and end of the curve. [kNm]", GH_ParamAccess.item, 0.0);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("m2", "m2", "Moment at end. Optional. [kNm]", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddGenericParameter("LoadCase", "LoadCase", "LoadCase.", GH_ParamAccess.item);

            pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("StressLoad", "StressLoad", "Line stress load.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve curve = null;
            if (!DA.GetData("Curve", ref curve)) return;

            var edge = Convert.FromRhino(curve);
            var direction = edge.CoordinateSystem.LocalY.ToRhino();
            DA.GetData("Direction", ref direction);

            double n1 = 0.0;
            if (!DA.GetData("n1", ref n1)) return;

            double n2 = n1;
            DA.GetData("n2", ref n2);

            double m1 = 0.0;
            DA.GetData("m1", ref m1);

            double m2 = m1;
            DA.GetData("m2", ref m2);

            Loads.LoadCase loadCase = null;
            if (!DA.GetData("LoadCase", ref loadCase)) return;

            string comment = null;
            DA.GetData("Comment", ref comment);

            try
            {
                var obj = new Loads.LineStressLoad(edge, direction.FromRhino(), n1, n2, m1, m2, loadCase, comment);
                DA.SetData("StressLoad", obj);
            }
            catch (ArgumentException e)
            {
                this.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, e.Message);
            }
        }
        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.LineStressLoadConstruct;
        public override Guid ComponentGuid => new Guid("22908c81-003d-4281-bc8c-c85029d13af6");
        public override GH_Exposure Exposure => GH_Exposure.secondary;

    }
}