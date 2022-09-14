// https://strusoft.com/

using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class StiffnessMatrix2TypeConstruct: GH_Component
    {
       public StiffnessMatrix2TypeConstruct(): base("StiffnessMatrix2Type.Construct", "Construct", "Construct a shear stiffness matrix H.", "FEM-Design", "ModellingTools")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddNumberParameter("XZ", "XZ", "XZ component [kN/m]", GH_ParamAccess.item, 10000);
           pManager[pManager.ParamCount - 1].Optional = true;
           pManager.AddNumberParameter("YZ", "YZ", "YZ component [kN/m]", GH_ParamAccess.item, 10000);
           pManager[pManager.ParamCount - 1].Optional = true;
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddGenericParameter("H", "H", "Shear stiffness matrix H", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
           double xz = 10000;
           if (!DA.GetData(0, ref xz))
           {
               // pass
           }

            double yz = 10000;
           if (!DA.GetData(1, ref yz))
           {
               // pass
           }

           // return
           DA.SetData(0, new FemDesign.ModellingTools.StiffnessMatrix2Type(xz, yz));
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.StiffnessMatrix2Type;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("83d4f79e-31b3-4e2b-a305-c824f9607276"); }
       }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;

    }
}