// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class StraightDeconstruct: GH_Component
    {
       public StraightDeconstruct(): base("Straight.Deconstruct", "Deconstruct", "Deconstruct a Straight element.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("Straight", "Straight", "Straight.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Direction", "Direction", "Direction.", GH_ParamAccess.item);
           pManager.AddNumberParameter("Space", "Space", "Space [m]", GH_ParamAccess.item);
           pManager.AddTextParameter("Face", "Face", "Face.", GH_ParamAccess.item);
           pManager.AddNumberParameter("Cover", "Cover", "Cover.", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.Reinforcement.Straight obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }

            // return
            DA.SetData(0, obj.Direction);
            DA.SetData(1, obj.Space);
            DA.SetData(2, obj.Face);
            DA.SetData(3, obj.Cover);
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.StraightDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("1d497956-ae87-4fe7-9208-346bd0ad6d08"); }
       }

        public override GH_Exposure Exposure => GH_Exposure.quinary;

    }
}