// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class FictitiousBarDeconstruct: GH_Component
    {
       public FictitiousBarDeconstruct(): base("FictitiousBar.Deconstruct", "Deconstruct", "Deconstruct a fictitious bar element.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("FictitiousBar", "FictBar", "Fictitious Bar.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddTextParameter("AnalyticalID", "AnalyticalID", "Analytical element ID.", GH_ParamAccess.item);
           pManager.AddCurveParameter("Curve", "Curve", "LineCurve or ArcCurve", GH_ParamAccess.item);
           pManager.AddNumberParameter("AE", "AE", "AE", GH_ParamAccess.item);
           pManager.AddNumberParameter("ItG", "ItG", "ItG", GH_ParamAccess.item);
           pManager.AddNumberParameter("I1E", "I1E", "I1E", GH_ParamAccess.item);
           pManager.AddNumberParameter("I2E", "I2E", "I2E", GH_ParamAccess.item);
           pManager.AddGenericParameter("Connectivity", "Connectivity", "Connectivity as list containing StarConnectivity and EndConnectivity respectively", GH_ParamAccess.list);
           pManager.AddVectorParameter("LocalY", "LocalY", "LocalY", GH_ParamAccess.item); 
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.ModellingTools.FictitiousBar obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }

            // return
            DA.SetData(0, obj.Guid);
            DA.SetData(1, obj.Name);
            DA.SetData(2, obj.Edge.ToRhino());
            DA.SetData(3, obj.AE);
            DA.SetData(4, obj.ItG);
            DA.SetData(5, obj.I1E);
            DA.SetData(6, obj.I2E);
            DA.SetDataList(7, obj._connectivity);
            DA.SetData(8, obj.LocalY.ToRhino());
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.FictBarDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("3aa81ad1-53d6-494b-9307-761b75e8d8da"); }
       }
        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}