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

            var guidList = new List<object>() { bar.Guid };

            var curveList = new List<object>() { bar.GetRhinoCurve() };

            var typeList = new List<object>() { bar.Type };

            var materialList = new List<object>() { bar.BarPart.Material };

            var localYList = new List<object>() { bar.BarPart.LocalY.ToRhino() };


            // return
            DA.SetData(0, bar.Guid);
            DA.SetData(1, bar.GetRhinoCurve());
            DA.SetData(2, bar.Type);
            DA.SetData(3, bar.BarPart.ComplexMaterialObj);
            DA.SetDataList(4, bar.BarPart.ComplexSectionObj.Sections);
            DA.SetData(5, bar.BarPart.ComplexCompositeObj);
            DA.SetDataList(6, bar.BarPart.Connectivity);
            DA.SetDataList(7, bar.BarPart.ComplexSectionObj.Eccentricities);
            DA.SetData(8, bar.BarPart.LocalY.ToRhino());
            DA.SetDataList(9, bar.Stirrups);
            DA.SetDataList(10, bar.LongitudinalBars);
            DA.SetDataList(11, bar.Ptc);
            DA.SetData(12, bar.Identifier);
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
           get { return new Guid("8317fff1-b65c-4d27-8604-8a13665a348f"); }
       }
    }
}