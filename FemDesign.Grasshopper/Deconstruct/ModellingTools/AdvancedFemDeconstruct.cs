// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class AdvancedFemDeconstruct : FEM_Design_API_Component
    {
       public AdvancedFemDeconstruct(): base("ModellingTools.Deconstruct", "Deconstruct", "Deconstruct a ModellingTools object. Returns Modelling tools and Covers.", CategoryName.Name(), "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("ModellingTools", "ModellingTools", "ModellingTools object from Model.Deconstruct.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
            pManager.AddGenericParameter("PointConnections", "PtConn", "Single fictitious bar element or list of fictitious bar elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("LineConnections", "LnConn", "Single fictitious bar element or list of fictitious bar elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("SurfaceConnections", "SrfConn", "Single fictitious bar element or list of fictitious bar elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("FictitiousBars", "FictBars", "Single fictitious bar element or list of fictitious bar elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("FictitiousShells", "FictShells", "Single fictitious shell element or list of fictitious shell elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Diaphragms", "Diaphragms", "Single diaphragm element or list of diaphragm elements.", GH_ParamAccess.list);
            pManager.AddGenericParameter("Covers", "Covers", "Single cover element or list of cover elements.", GH_ParamAccess.list);
        }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.ModellingTools.AdvancedFem obj = null;
            if (!DA.GetData(0, ref obj)) { return; }
            if (obj == null) { return; }

            // get output
            DA.SetDataList("PointConnections", obj.ConnectedPoints);
            DA.SetDataList("LineConnections", obj.ConnectedLines);
            DA.SetDataList("SurfaceConnections", obj.SurfaceConnections);
            DA.SetDataList("FictitiousBars", obj.FictitiousBars);
            DA.SetDataList("FictitiousShells", obj.FictitiousShells);
            DA.SetDataList("Diaphragms", obj.Diaphragms);
            DA.SetDataList("Covers", obj.Covers);
        }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.ModellingToolsDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("E966C625-CA8B-4044-82E9-2E1585D50A9D"); }
       }
        public override GH_Exposure Exposure => GH_Exposure.senary;
    }
}