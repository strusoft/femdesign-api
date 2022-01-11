// https://strusoft.com/
using System;
using Grasshopper.Kernel;
using FemDesign.GenericClasses;
using FemDesign.Reinforcement;

namespace FemDesign.Grasshopper
{
    public class StraightReinforcementLayout: GH_Component
    {
        public StraightReinforcementLayout(): base("Straight.ReinforcementLayout", "ReinforcementLayout", "Define straight reinforcement layout for surface reinforcement", "FemDesign", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Direction", "Direction", "Reinforcement layout direction. Allowed values: x/y.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Space", "Space", "Spacing between bars.", GH_ParamAccess.item);
            pManager.AddTextParameter("Face", "Face", "Surface reinforcement face. Allowed values: top/mid/bottom.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Cover", "Cover", "Reinforcement concrete cover.", GH_ParamAccess.item);
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
            double cover = 0;
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
            if (!DA.GetData(3, ref cover))
            {
                return;
            }
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
    }  
}