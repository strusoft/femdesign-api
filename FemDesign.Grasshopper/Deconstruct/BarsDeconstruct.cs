// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class BarsDeconstruct: GH_Component
    {
       public BarsDeconstruct(): base("Bars.Deconstruct", "Deconstruct", "Deconstruct a bar element.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("Bar", "Bar", "Bar.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddCurveParameter("Curve", "Curve", "LineCurve or ArcCurve", GH_ParamAccess.item);
           pManager.AddGenericParameter("Type", "Type", "Bar type", GH_ParamAccess.item);
           pManager.AddGenericParameter("Material", "Material", "Material", GH_ParamAccess.item);
           pManager.AddGenericParameter("Section", "Section", "Section", GH_ParamAccess.list);
           pManager.AddGenericParameter("Connectivity", "Connectivity", "Connectivity", GH_ParamAccess.list);
           pManager.AddGenericParameter("Eccentricity", "Eccentricity", "Eccentricity", GH_ParamAccess.list);
           pManager.AddGenericParameter("LocalY", "LocalY", "LocalY", GH_ParamAccess.item);
           pManager.AddGenericParameter("Stirrups", "Stirrups", "Stirrups.", GH_ParamAccess.list);
           pManager.AddGenericParameter("LongitudinalBars", "LongBars", "Longitudinal bars.", GH_ParamAccess.list);
           pManager.AddGenericParameter("PTC", "PTC", "Post-tensioning cables.", GH_ParamAccess.list);
           pManager.AddTextParameter("Identifier", "Identifier", "Structural element ID.", GH_ParamAccess.item);
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
            DA.SetData(0, bar.Guid);
            DA.SetData(1, bar.GetRhinoCurve());
            DA.SetData(2, bar.Type);
            DA.SetData(3, bar.BarPart.Material);
            DA.SetDataList(4, bar.BarPart.Sections);
            DA.SetDataList(5, bar.BarPart.Connectivities);
            DA.SetDataList(6, bar.BarPart.Eccentricities);
            DA.SetData(7, bar.BarPart.LocalY.ToRhino());
            DA.SetDataList(8, bar.Stirrups);
            DA.SetDataList(9, bar.LongitudinalBars);
            DA.SetDataList(10, bar.Ptc);
            DA.SetData(11, bar.Identifier);
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
           get { return new Guid("87525a2e-598f-44ac-ad96-f2058bc37623"); }
       }
    }
}