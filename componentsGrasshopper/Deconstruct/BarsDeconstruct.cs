// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class BarsDeconstruct: GH_Component
    {
       public BarsDeconstruct(): base("Bars.Deconstruct", "Deconstruct", "Deconstruct a bar element.", "FemDesign", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("Bar", "Bar", "Bar.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddTextParameter("StructuralID", "StructuralID", "Structural element ID.", GH_ParamAccess.item);
           pManager.AddTextParameter("AnalyticalID", "AnalyticalID", "Analytical element ID.", GH_ParamAccess.item);
           pManager.AddCurveParameter("Curve", "Curve", "LineCurve or ArcCurve", GH_ParamAccess.item);
           pManager.AddGenericParameter("Material", "Material", "Material", GH_ParamAccess.item);
           pManager.AddGenericParameter("Section", "Section", "Section", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.Bars.Bar bar = null;
            if (!DA.GetData(0, ref bar))
            {
                return;
            }
            if (bar == null)
            {
                return;
            }

            // return
            DA.SetData(0, bar.guid);
            DA.SetData(1, bar.name);
            DA.SetData(2, bar.barPart.name);
            DA.SetData(3, bar.GetRhinoCurve());
            DA.SetData(4, bar.material);
            DA.SetData(5, bar.section);
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.BarDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("145b6331-bf19-4d89-9e81-9e5e0d137f67"); }
       }
    }
}