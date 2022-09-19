// https://strusoft.com/

using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class StiffnessMatrix4TypeConstruct: GH_Component
    {
       public StiffnessMatrix4TypeConstruct(): base("StiffnessMatrix4Type.Construct", "Construct", "Construct a membrane (D) or flexural (K) stiffness matrix.", "FEM-Design", "ModellingTools")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddNumberParameter("XX", "XX", "XX component [kN/m] or [kN]", GH_ParamAccess.item, 10000);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddNumberParameter("XY", "XY", "XY component kN/m] or [kN]", GH_ParamAccess.item, 5000);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddNumberParameter("YY", "YY", "YY component [kN/m]", GH_ParamAccess.item, 10000);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddNumberParameter("GXY", "GXY", "GXY component [kN/m]", GH_ParamAccess.item, 10000);
           pManager[pManager.ParamCount - 1].Optional = true;
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddGenericParameter("D/K", "D/K", "Membrane (D) or flexural (K) stiffness matrix", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
           double xx = 10000;
           if (!DA.GetData(0, ref xx))
           {
               // pass
           }

           double xy = 5000;
           if (!DA.GetData(1, ref xy))
           {
               // pass
           }

           double yy = 10000;
           if (!DA.GetData(2, ref yy))
           {
               // pass
           }

           double gxy = 10000;
           if (!DA.GetData(3, ref gxy))
           {
               // pass
           }

           // return
           DA.SetData(0, new FemDesign.ModellingTools.StiffnessMatrix4Type(xx, xy, yy, gxy));
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.StiffnessMatrix4Type;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("e75c2f12-4d2e-4a7d-b8fa-3a80510893df"); }
       }
        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}