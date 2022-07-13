// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class ThicknessValDeconstruct: GH_Component
    {
       public ThicknessValDeconstruct(): base("Thickness.Deconstruct", "Deconstruct", "Deconstruct a ThicknessLocationValue.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("ThicknessLocationValue", "ThickLocVal", "ThicknessLocationValue. [m]", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddPointParameter("Point", "Point", "Point. Position of value." , GH_ParamAccess.item);
           pManager.AddNumberParameter("Value", "Val", "Value. Thickness.", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.Shells.Thickness obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }

            // return
            DA.SetData(0, obj.GetFdPoint().ToRhino());
            DA.SetData(1, obj.Value);         
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.ThicknessDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("64956a69-48a3-4c8b-ae4f-fe22b9a53a23"); }
       }
        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}