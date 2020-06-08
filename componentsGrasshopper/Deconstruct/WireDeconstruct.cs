// https://strusoft.com/
using System;
using Grasshopper.Kernel;

namespace FemDesign.GH
{
    public class WireDeconstruct: GH_Component
    {
       public WireDeconstruct(): base("Wire.Deconstruct", "Deconstruct", "Deconstruct a Wire element.", "FemDesign", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("Wire", "Wire", "Wire.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddNumberParameter("Diameter", "Diameter", "Diameter.", GH_ParamAccess.item);
           pManager.AddGenericParameter("ReinforcingMaterial", "ReinforcingMaterial", "ReinforcingMaterial", GH_ParamAccess.item);
           pManager.AddTextParameter("Profile", "Profile", "Profile.", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.Reinforcement.Wire obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }

            // return
            DA.SetData(0, obj.diameter);
            DA.SetData(1, obj.reinforcingMaterial);
            DA.SetData(2, obj.profile);
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.WireDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("99775a89-cdbf-47cd-9647-db12125a2eb7"); }
       }
    }
}