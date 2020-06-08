// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class ShellEdgeConnectionDeconstruct: GH_Component
    {
       public ShellEdgeConnectionDeconstruct(): base("ShellEdgeConnection.Deconstruct", "Deconstruct", "Deconstruct a ShellEdgeConnection element.", "FemDesign", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("ShellEdgeConnection", "ShellEdgeConnection", "ShellEdgeConnection.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddTextParameter("AnalyticalID", "AnalyticalID", "Analytical element ID.", GH_ParamAccess.item);
           pManager.AddGenericParameter("Motions", "Motions", "Motions", GH_ParamAccess.item);
           pManager.AddGenericParameter("Rotations", "Rotations", "Rotations", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.Shells.ShellEdgeConnection obj = null;
            if (!DA.GetData(0, ref obj))
            {
                return;
            }
            if (obj == null)
            {
                return;
            }

            // return
            DA.SetData(0, obj.guid);
            DA.SetData(1, obj.name);

            // catch pre-defined rigidity
            try
            {
                DA.SetData(2, obj.rigidity.motions);
                DA.SetData(3, obj.rigidity.rotations);
            }
            catch
            {
                DA.SetData(2, "Pre-defined edge connection type was serialized.");
                DA.SetData(3, "Pre-defined edge connection type was serialized.");
            }
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.ShellEdgeConnectionDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("145b6831-bf19-4d89-9e81-9e5e0d137f87"); }
       }
    }
}