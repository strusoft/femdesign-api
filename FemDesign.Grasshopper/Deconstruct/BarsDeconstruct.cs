// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
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
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.list);
           pManager.AddCurveParameter("Curve", "Curve", "LineCurve or ArcCurve", GH_ParamAccess.list);
           pManager.AddGenericParameter("Type", "Type", "Bar type", GH_ParamAccess.list);
           pManager.AddGenericParameter("Material", "Material", "Material", GH_ParamAccess.list);
           pManager.AddGenericParameter("Section", "Section", "Section", GH_ParamAccess.list);
           pManager.AddGenericParameter("Connectivity", "Connectivity", "Connectivity", GH_ParamAccess.list);
           pManager.AddGenericParameter("Eccentricity", "Eccentricity", "Eccentricity", GH_ParamAccess.list);
           pManager.AddGenericParameter("LocalY", "LocalY", "LocalY", GH_ParamAccess.list);
           pManager.AddGenericParameter("Stirrups", "Stirrups", "Stirrups.", GH_ParamAccess.list);
           pManager.AddGenericParameter("LongitudinalBars", "LongBars", "Longitudinal bars.", GH_ParamAccess.list);
           pManager.AddGenericParameter("PTC", "PTC", "Post-tensioning cables.", GH_ParamAccess.list);
           pManager.AddTextParameter("Identifier", "Identifier", "Structural element ID.", GH_ParamAccess.list);
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



            // The following code is to convert 'item' to 'list object'
            // It is required to construct the bar without graftening the data

            List<object> guidList = new List<object>();
            guidList.Add(bar.Guid);

            List<object> curveList = new List<object>();
            curveList.Add(bar.GetRhinoCurve());

            List<object> typeList = new List<object>();
            typeList.Add(bar.Type);

            List<object> materialList = new List<object>();
            materialList.Add(bar.BarPart.Material);

            List<object> localYList = new List<object>();
            localYList.Add(bar.BarPart.LocalY.ToRhino());


            // return
            DA.SetDataList(0, guidList);
            DA.SetDataList(1, curveList);
            DA.SetDataList(2, typeList);
            DA.SetDataList(3, materialList);
            DA.SetDataList(4, bar.BarPart.Sections);
            DA.SetDataList(5, bar.BarPart.Connectivities);
            DA.SetDataList(6, bar.BarPart.Eccentricities);
            DA.SetDataList(7, localYList);
            DA.SetDataList(8, bar.Stirrups);
            DA.SetDataList(9, bar.LongitudinalBars);
            DA.SetDataList(10, bar.Ptc);
            DA.SetDataList(11, bar.Identifier);
            
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