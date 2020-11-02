// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.GH
{
    public class LineTemperatureLoadDeconstruct: GH_Component
    {
       public LineTemperatureLoadDeconstruct(): base("LineTemperatureLoad.Deconstruct", "Deconstruct", "Deconstruct a LineTemperatureLoad.", "FemDesign", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("LineTemperatureLoad", "LnTmpLoad", "LineTemperatureLoad. Use GenericLoadObject.SortLoads to extract LineTemperatureLoads.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddCurveParameter("Curve", "Curve", "Curve." , GH_ParamAccess.item);
           pManager.AddVectorParameter("Direction", "Direction", "Direction.", GH_ParamAccess.item);
           pManager.AddGenericParameter("TopBottomLocationValue1", "TopBotLocVal1", "Top bottom location value.", GH_ParamAccess.item);
           pManager.AddGenericParameter("TopBottomLocationValue2", "TopBotLocVal2", "Top bottom locaiton value.", GH_ParamAccess.item);
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
            if (obj.LineTemperatureLoad != null)
            {
                DA.SetData(0, obj.LineTemperatureLoad.Guid);
                DA.SetData(1, obj.LineTemperatureLoad.Edge.ToRhino());
                DA.SetData(2, obj.LineTemperatureLoad.Direction.ToRhino());
                DA.SetData(3, obj.LineTemperatureLoad.TopBotLocVal[0]);
                DA.SetData(4, obj.LineTemperatureLoad.TopBotLocVal[1]);
                DA.SetData(5, obj.LineTemperatureLoad.LoadCase);
                DA.SetData(6, obj.LineTemperatureLoad.Comment);
            }
            else
            {
                throw new System.ArgumentException("Type must be LineTemperatureLoad. LineTemperatureLoadDeconstruct failed.");
            }
            
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.LineTempLoadDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("018c237a-2af8-449f-b188-680c67e7ffb9"); }
       }
    }
}