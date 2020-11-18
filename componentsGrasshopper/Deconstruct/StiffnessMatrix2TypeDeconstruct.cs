// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.GH
{
    public class StiffnessMatrix2TypeDeconstruct: GH_Component
    {
       public StiffnessMatrix2TypeDeconstruct(): base("StiffnessMatrix2Type.Deconstruct", "Deconstruct", "Deconstruct a shear stiffness matrix, stiffness matrix 2 type.", "FemDesign", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("StiffnessMatrix2Type", "StiffnessMatrix2Type", "StiffnessMatrix2Type.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddNumberParameter("XZ", "XZ", "XZ", GH_ParamAccess.item);
           pManager.AddNumberParameter("YZ", "YZ", "YZ", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.ModellingTools.StiffnessMatrix2Type obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }

            if (obj == null)
            {
                return;
            }

            // return
            DA.SetData(0, obj.XZ);
            DA.SetData(1, obj.YZ);
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.StiffnessMatrix2TypeDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("9b7a1437-480e-49f5-9c18-1c0591fa0025"); }
       }
    }
}