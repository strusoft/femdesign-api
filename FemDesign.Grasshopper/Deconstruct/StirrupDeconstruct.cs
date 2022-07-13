// https://strusoft.com/
using System;
using System.Linq;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class StirrupDeconstruct: GH_Component
    {
       public StirrupDeconstruct(): base("Stirrups.Deconstruct", "Deconstruct", "Deconstruct a distribution of stirrups.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("Stirrups", "Stirrups", "Stirrups along a distribution of a bar element.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddGenericParameter("Guid", "Guid", "Guid of stirrups", GH_ParamAccess.item);
           pManager.AddGenericParameter("BaseBar", "BaseBar", "Guid of bar part of bar on which these stirrups are distributed.", GH_ParamAccess.item);
           pManager.AddGenericParameter("Wire", "Wire", "Wire of stirrups.", GH_ParamAccess.item);
           pManager.AddBrepParameter("Profiles", "Profiles", "Profiles of stirrups", GH_ParamAccess.list);
           pManager.AddNumberParameter("StartMeasurement", "StartMeasurement", "Start of stirrup distribution expressed as distance along the reference bar from start of bar [m]", GH_ParamAccess.item);
           pManager.AddNumberParameter("EndMeasurement", "EndMeasurement", "End of stirrup distribution expressed as a distance along the reference bar from start of bar [m]", GH_ParamAccess.item);
           pManager.AddNumberParameter("Spacing", "Spacing", "Spacing of stirrups along distribution. [m]", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            FemDesign.Reinforcement.BarReinforcement barReinf = null;
            if (!DA.GetData("Stirrups", ref barReinf))
            {
                return;
            }
            
            if (!barReinf.IsStirrups)
            {
                throw new System.ArgumentException($"Passed object {barReinf.Guid} is not a stirrups bar reinforcement object. Did you pass a longitudinal bar?");
            }
            else
            {
                DA.SetData("Guid", barReinf.Guid);
                DA.SetData("BaseBar", barReinf.BaseBar.Guid);
                DA.SetData("Wire", barReinf.Wire);
                DA.SetDataList("Profiles", barReinf.Stirrups.Regions.Select(x => x.ToRhinoBrep()));
                DA.SetData("StartMeasurement", barReinf.Stirrups.Start);
                DA.SetData("EndMeasurement", barReinf.Stirrups.End);
                DA.SetData("Spacing", barReinf.Stirrups.Distance);
            }
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.StirrupsDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("f85d4396-9a13-4d80-b265-1f3ac6ccb3ae"); }
       }

        public override GH_Exposure Exposure => GH_Exposure.quinary;

    }
}