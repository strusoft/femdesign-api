// https://strusoft.com/
using System;
using System.Linq;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class LongitudinalBarDeconstruct: GH_Component
    {
       public LongitudinalBarDeconstruct(): base("LongitudinalBar.Deconstruct", "Deconstruct", "Deconstruct a longitudinal bar.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("LongitudinalBar", "LongitudinalBar", "LongitudinalBar of a bar element.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
            pManager.AddGenericParameter("Guid", "Guid", "Guid of LongitudinalBars", GH_ParamAccess.item);
            pManager.AddGenericParameter("BaseBar", "BaseBar", "Guid of bar part of bar on which these LongitudinalBars are distributed.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Wire", "Wire", "Wire of LongitudinalBars.", GH_ParamAccess.item);
            pManager.AddNumberParameter("YPos", "YPos", "YPos. [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("ZPos", "ZPos", "ZPos. [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("StartAnchorage", "StartAnchorage", "Anchorage mesaure at start. [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("EndAnchorage", "EndAnchorage", "Anchorage mesaure at end. [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("StartMeasurement", "StartMeasurement", "Start of LongitudinalBar distribution expressed as distance along the reference bar from start of bar.  [m]", GH_ParamAccess.item);
            pManager.AddNumberParameter("EndMeasurement", "EndMeasurement", "End of LongitudinalBar distribution expressed as a distance along the reference bar from start of bar. [m]", GH_ParamAccess.item);
            pManager.AddBooleanParameter("AuxBar", "AuxBar", "Is bar auxiliary?", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            FemDesign.Reinforcement.BarReinforcement barReinf = null;
            if (!DA.GetData("LongitudinalBar", ref barReinf))
            {
                return;
            }
            
            if (barReinf.IsStirrups)
            {
                throw new System.ArgumentException($"Passed object {barReinf.Guid} is not a longitudinal bar reinforcement object. Did you pass a stirrups bar?");
            }
            else
            {
                DA.SetData("Guid", barReinf.Guid);
                DA.SetData("BaseBar", barReinf.BaseBar.Guid);
                DA.SetData("Wire", barReinf.Wire);
                DA.SetData("YPos", barReinf.LongitudinalBar.Position2d.X);
                DA.SetData("ZPos", barReinf.LongitudinalBar.Position2d.Y);
                DA.SetData("StartAnchorage", barReinf.LongitudinalBar.Anchorage.Start);
                DA.SetData("EndAnchorage", barReinf.LongitudinalBar.Anchorage.End);
                DA.SetData("StartMeasurement", barReinf.LongitudinalBar.Start);
                DA.SetData("EndMeasurement", barReinf.LongitudinalBar.End);
                DA.SetData("AuxBar", barReinf.LongitudinalBar.Auxiliary);
            }
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.LongitudinalBarDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("cb74cb52-fe8f-4525-a3cd-5a042e5e3738"); }
       }

        public override GH_Exposure Exposure => GH_Exposure.quinary;

    }
}