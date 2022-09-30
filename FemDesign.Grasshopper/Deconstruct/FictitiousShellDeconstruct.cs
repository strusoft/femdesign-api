// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class FictitiousShellDeconstruct: GH_Component
    {
       public FictitiousShellDeconstruct(): base("FictitiousShell.Deconstruct", "Deconstruct", "Deconstruct a fictitious Shell element.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("FictitiousShell", "FictShell", "Fictitious Shell.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddTextParameter("AnalyticalID", "AnalyticalID", "Analytical element ID.", GH_ParamAccess.item);
           pManager.AddSurfaceParameter("Surface", "Surface", "Surface.", GH_ParamAccess.item);
           pManager.AddGenericParameter("MembraneStiffness", "D", "Membrane stiffness (D)", GH_ParamAccess.item);
           pManager.AddGenericParameter("FlexuralStiffness", "K", "Flexural stiffness (K)", GH_ParamAccess.item);
           pManager.AddGenericParameter("ShearStiffness", "H", "Shear stiffness (H)", GH_ParamAccess.item);
           pManager.AddNumberParameter("Density", "Density", "Density.", GH_ParamAccess.item);
           pManager.AddNumberParameter("T1", "T1", "T1", GH_ParamAccess.item);
           pManager.AddNumberParameter("T2", "T2", "T2", GH_ParamAccess.item);
           pManager.AddNumberParameter("Alpha1", "Alpha1", "Alpha1", GH_ParamAccess.item);
           pManager.AddNumberParameter("Alpha2", "Alpha2", "Alpha2", GH_ParamAccess.item);
           pManager.AddBooleanParameter("IgnoreInStImpCalc", "IgnoreInStImpCalc", "IgnoreInStImpCalc", GH_ParamAccess.item);
           pManager.AddCurveParameter("EdgeCurves", "EdgeCurves", "EdgeCurves", GH_ParamAccess.list);
           pManager.AddGenericParameter("EdgeConnections", "EdgeConnections", "EdgeConnections", GH_ParamAccess.list);
           pManager.AddVectorParameter("LocalX", "LocalX", "LocalX", GH_ParamAccess.item);
           pManager.AddVectorParameter("LocalY", "LocalY", "LocalY", GH_ParamAccess.item);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.ModellingTools.FictitiousShell obj = null;
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
            DA.SetData(2, obj.Region.ToRhinoBrep());
            DA.SetData(3, obj.MembraneStiffness);
            DA.SetData(4, obj.FlexuralStiffness);
            DA.SetData(5, obj.ShearStiffness);
            DA.SetData(6, obj.Density);
            DA.SetData(7, obj.T1);
            DA.SetData(8, obj.T2);
            DA.SetData(9, obj.Alpha1);
            DA.SetData(10, obj.Alpha2);
            DA.SetData(11, obj.IgnoreInStImpCalculation);
            DA.SetDataList(12, obj.Region.ToRhinoCurves());
            DA.SetDataList(13, obj.Region.GetEdgeConnections());
            DA.SetData(14, obj.LocalX.ToRhino());
            DA.SetData(15, obj.LocalY.ToRhino());
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.FictShellDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("2e3394cb-dfa8-4ebf-b9e2-c389abe4c59b"); }
       }
        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}