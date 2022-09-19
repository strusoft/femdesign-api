// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class CoverDeconstruct: GH_Component
    {
       public CoverDeconstruct(): base("Cover.Deconstruct", "Deconstruct", "Deconstruct a cover element.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("Cover", "Cover", "Cover.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           
           pManager.AddTextParameter("ID", "ID", "ID.", GH_ParamAccess.item);
           pManager.AddBrepParameter("Surface", "Surface", "Surface", GH_ParamAccess.item);
           pManager.AddGenericParameter("EdgeCurves", "EdgeCurves", "EdgeCurves", GH_ParamAccess.list);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.Cover cover = null;
            if (!DA.GetData(0, ref cover))
            {
                return;
            }
            if (cover == null)
            {
                return;
            }

            // return
            
            DA.SetData(0, cover.Guid);
            DA.SetData(1, cover.Name);
            DA.SetData(2, cover.GetRhinoSurface());
            DA.SetDataList(3, cover.GetRhinoCurves());
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.CoverDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("145f6331-bf19-4d89-9e81-9e5e0d137f87"); }
       }

        public override GH_Exposure Exposure => GH_Exposure.quinary;

    }
}