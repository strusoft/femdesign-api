// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class DiaphragmDeconstruct : GH_Component
    {
       public DiaphragmDeconstruct(): base("Diaphragm.Deconstruct", "Deconstruct", "Deconstruct a slab element.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("Diaphragm", "Diaphragm", "Diaphragm.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddBrepParameter("Surface", "Surface", "Surface", GH_ParamAccess.item);
           pManager.AddTextParameter("Identifier", "Identifier", "Structural element ID.", GH_ParamAccess.item);     
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.ModellingTools.Diaphragm diaphragm = null;
            if (!DA.GetData(0, ref diaphragm))
            {
                return;
            }
            if (diaphragm == null)
            {
                return;
            }

            DA.SetData("Guid", diaphragm.Guid);
            DA.SetData("Surface", diaphragm.Region.ToRhinoBrep());
            DA.SetData("Identifier", diaphragm.Name);
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.DiaphragmDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("77d800b5-756e-41ea-b95e-1058e83f1f5b"); }
       }
        public override GH_Exposure Exposure => GH_Exposure.senary;
    }
}