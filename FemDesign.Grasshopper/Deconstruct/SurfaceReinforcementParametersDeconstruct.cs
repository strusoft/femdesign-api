// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class SurfaceReinforcementParametersDeconstruct: GH_Component
    {
       public SurfaceReinforcementParametersDeconstruct(): base("SurfaceReinforcementParameters.Deconstruct", "Deconstruct", "Deconstruct a SurfaceReinforcementParameters element.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("SurfaceReinforcementParameters", "SrfReinfParams", "SurfaceReinforcement.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid", GH_ParamAccess.item);
           pManager.AddBooleanParameter("SingleLayerReinforcement", "SingleLayerReinf", "Single layer reinforcement?", GH_ParamAccess.item);
           pManager.AddVectorParameter("XDirection", "XDir", "X direction of reinforcement", GH_ParamAccess.item);
           pManager.AddVectorParameter("YDirection", "YDir", "Y direction of reinforcement", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
           Reinforcement.SurfaceReinforcementParameters obj = null;
           if (!DA.GetData(0, ref obj))
           {
               return;
           }
           
           DA.SetData(0, obj.Guid);
           DA.SetData(1, obj.SingleLayerReinforcement);
           DA.SetData(2, obj.XDirection.ToRhino());
           DA.SetData(3, obj.YDirection.ToRhino());            
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.ReinforcementParametersDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("46da9ba3-61ae-4d2e-a2bc-312ca158eb8a"); }
       }
        public override GH_Exposure Exposure => GH_Exposure.quinary;

    }
}