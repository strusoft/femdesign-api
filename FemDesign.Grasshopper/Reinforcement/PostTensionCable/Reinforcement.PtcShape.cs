using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FemDesign.Reinforcement
{
    public class PtcShapeComponent : GH_Component
    {
        public PtcShapeComponent(): base("PtcShape", "Shape", "Description", "FEM-Design", "Reinforcement"  )
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Start", "Start", "FemDesign.Reinforcement.PtcShapeStart", GH_ParamAccess.item);
            pManager.AddGenericParameter("Intermidiate", "Intermidiate", "FemDesign.Reinforcement.PtcShapeInner", GH_ParamAccess.list);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("End", "End", "FemDesign.Reinforcement.PtcShapeEnd", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Shape", "Shape", "FemDesign.Reinforcement.PtcShapeType", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            PtcShapeStart start = null;
            if (!DA.GetData("Start", ref start) || start == null) return;

            List<PtcShapeInner> intermediates = new List<PtcShapeInner>();
            DA.GetDataList("Intermidiate", intermediates);

            PtcShapeEnd end = null;
            if (!DA.GetData("End", ref end) || end == null) return;

            PtcShapeType shape = new PtcShapeType(start, end, intermediates);

            DA.SetData("Shape", shape);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PtcShape;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("6323fbff-e53a-40dd-b368-8d60f04fec3a"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.quinary;

    }
}