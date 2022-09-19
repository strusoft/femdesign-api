using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Linq;
using System.Collections.Generic;

namespace FemDesign.Reinforcement
{
    public class PtcShapeInnerComponent : GH_Component
    {
        public PtcShapeInnerComponent() : base("PtcShapeInner", "PtcInner", "FemDesign.Reinforcement.PtcShapeInner", "FEM-Design", "Reinforcement")
        {
        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("x'", "x'", "Parameter along axis of element", GH_ParamAccess.item);
            pManager.AddNumberParameter("z'", "z'", "Height", GH_ParamAccess.item);
            pManager.AddNumberParameter("Tangent", "Tangent", "Tangent of cable at the inner point", GH_ParamAccess.item);
            pManager.AddNumberParameter("PriorInflection x'", "PriorInflection x'", "Parameter of prior inflection point. Parameter between 0-1 along parent element. Optional", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Inner", "Inner", "FemDesign.Reinforcement.PtcShapeInner", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            double x = 0.0;
            if (!DA.GetData("x'", ref x)) return;

            double z = 0.0;
            if (!DA.GetData("z'", ref z)) return;

            double tangent = 0.0;
            if (!DA.GetData("Tangent", ref tangent)) return;

            double? priorInflectionParam = null;
            DA.GetData("PriorInflection x'", ref priorInflectionParam);
            
            PtcShapeInner inner = new PtcShapeInner(x, z, tangent, priorInflectionParam);

            DA.SetData("Inner", inner);
        }

        protected override System.Drawing.Bitmap Icon => FemDesign.Properties.Resources.PtcShapeInner;

        public override Guid ComponentGuid => new Guid("0c0d9f85-c235-4eea-84c9-b6fe15603582");

        public override GH_Exposure Exposure => GH_Exposure.quinary;

    }
}