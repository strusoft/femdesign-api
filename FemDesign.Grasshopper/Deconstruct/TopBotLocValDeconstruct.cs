// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class TopBotLocValDeconstruct: GH_Component
    {
       public TopBotLocValDeconstruct(): base("TopBotLocVal.Deconstruct", "Deconstruct", "Deconstruct a TopBottomLocationValue.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("TopBottomLocationValue", "TopBotLocVal", "TopBottomLocationValue.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddPointParameter("Point", "Point", "Point." , GH_ParamAccess.item);
           pManager.AddNumberParameter("TopValue", "TopVal", "TopValue.", GH_ParamAccess.item);
           pManager.AddNumberParameter("BottomValue", "BotVal", "BotValue.", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.Loads.TopBotLocationValue obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }

            // return
            DA.SetData(0, obj.GetFdPoint().ToRhino());
            DA.SetData(1, obj.TopVal);
            DA.SetData(2, obj.BottomVal);
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.TopBottomValueDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("46e7462c-80a8-4894-a43f-1a00fb5a4e52"); }
       }
    }
}