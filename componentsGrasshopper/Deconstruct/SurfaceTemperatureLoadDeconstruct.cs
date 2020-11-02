// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.GH
{
    public class SurfaceTemperatureLoadDeconstruct: GH_Component
    {
       public SurfaceTemperatureLoadDeconstruct(): base("SurfaceTemperatureLoad.Deconstruct", "Deconstruct", "Deconstruct a SurfaceTemperatureLoad.", "FemDesign", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("SurfaceTemperatureLoad", "SrfTmpLoad", "SurfaceTemperatureLoad. Use GenericLoadObject. SortLoads to extract SurfaceTemperatureLoads.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddSurfaceParameter("Surface", "Surface", "Surface." , GH_ParamAccess.item);
           pManager.AddGenericParameter("TopBottomLocationValue1", "TopBotVal1", "Top bottom location value.", GH_ParamAccess.item);
           pManager.AddGenericParameter("TopBottomLocationValue2", "TopBotVal2", "Top bottom location value.", GH_ParamAccess.item);
           pManager.AddGenericParameter("TopBottomLocaitonValue3", "TopBotVal3", "Top bottom location value.", GH_ParamAccess.item);
           pManager.AddTextParameter("LoadCaseGuid", "LoadCaseGuid", "LoadCase guid reference.", GH_ParamAccess.item);
           pManager.AddTextParameter("Comment", "Comment", "Comment.", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.Loads.GenericLoadObject obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }

            // return
            DA.SetData(0, obj.GetType());
            if (obj.SurfaceTemperatureLoad != null)
            {
                DA.SetData(0, obj.SurfaceTemperatureLoad.Guid);
                DA.SetData(1, obj.SurfaceTemperatureLoad.Region.ToRhinoBrep());
                
                // if uniform
                if (obj.SurfaceTemperatureLoad.TopBotLocVal.Count == 1)
                {
                    DA.SetData(2, obj.SurfaceTemperatureLoad.TopBotLocVal[0]);
                    DA.SetData(3, obj.SurfaceTemperatureLoad.TopBotLocVal[0]);
                    DA.SetData(4, obj.SurfaceTemperatureLoad.TopBotLocVal[0]);
                }

                // if variable
                else if (obj.SurfaceTemperatureLoad.TopBotLocVal.Count == 3)
                {
                    DA.SetData(2, obj.SurfaceTemperatureLoad.TopBotLocVal[0]);
                    DA.SetData(3, obj.SurfaceTemperatureLoad.TopBotLocVal[1]);
                    DA.SetData(4, obj.SurfaceTemperatureLoad.TopBotLocVal[2]);
                }

                // else
                else
                {
                    throw new System.ArgumentException("Length of load should be 1 or 3.");
                }

                DA.SetData(5, obj.SurfaceTemperatureLoad.LoadCase);
                DA.SetData(6, obj.SurfaceTemperatureLoad.Comment);
            }
            else
            {
                throw new System.ArgumentException("Type must be SurfaceTemperatureLoad. SurfaceTemperatureLoadDeconstruct failed.");
            }
            
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.SurfaceTempLoadDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("43fa13c0-63d5-49ee-9d87-752e51a8c702"); }
       }
    }
}