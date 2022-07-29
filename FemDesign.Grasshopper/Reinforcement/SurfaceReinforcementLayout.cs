// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using FemDesign.GenericClasses;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public class SurfaceReinforcementLayout: GH_Component
    {
        public SurfaceReinforcementLayout(): base("SurfaceReinforcement.Layout", "ReinforcementLayout", "Define straight reinforcement layout for surface reinforcement", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Direction", "Direction", "Reinforcement layout direction. Allowed values: x/y.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Space", "Space", "Spacing between bars. [m]", GH_ParamAccess.item);
            pManager.AddTextParameter("Face", "Face", "Surface reinforcement face. Allowed values: top/mid/bottom.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Cover", "Cover", "Reinforcement concrete cover. [m]", GH_ParamAccess.item, 0.02);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Straight", "Straight", "Straight surface reinforcement layout.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            string direction = null;
            double space = 0;
            string face = null;
            double cover = 0.02;
            if (!DA.GetData(0, ref direction))
            {
                return;
            }
            if (!DA.GetData(1, ref space))
            {
                return;
            }
            if (!DA.GetData(2, ref face))
            {
                return;
            }
            DA.GetData(3, ref cover);

            if (direction == null || face == null)
            {
                return;
            }

            Face _face = EnumParser.Parse<Face>(face);
            ReinforcementDirection _direction = EnumParser.Parse<ReinforcementDirection>(direction);
            FemDesign.Reinforcement.Straight obj = new FemDesign.Reinforcement.Straight(_direction, space, _face, cover);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.StraightReinforcementLayout;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("03982d38-6424-49a1-a8cb-a19359c754d4"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }  
}