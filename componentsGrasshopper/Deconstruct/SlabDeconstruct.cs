// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class SlabDeconstruct: GH_Component
    {
       public SlabDeconstruct(): base("Slab.Deconstruct", "Deconstruct", "Deconstruct a slab element.", "FemDesign", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("Slab", "Slab", "Slab.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddTextParameter("StructuralID", "StructuralID", "Structural element ID.", GH_ParamAccess.item);
           pManager.AddTextParameter("AnalyticalID", "AnalyticalID", "Analytical element ID.", GH_ParamAccess.item);
           pManager.AddBrepParameter("Surface", "Surface", "Surface", GH_ParamAccess.item);
           pManager.AddGenericParameter("Material", "Material", "Material", GH_ParamAccess.item);
           pManager.AddCurveParameter("EdgeCurves", "EdgeCurves", "EdgeCurves", GH_ParamAccess.list);
           pManager.AddGenericParameter("ShellEdgeConnections", "ShellEdgeConnections", "ShellEdgeConnections", GH_ParamAccess.list);
           pManager.AddGenericParameter("SurfaceReinforcement", "SurfaceReinforcement", "SurfaceReinforcement", GH_ParamAccess.list);
       }
       protected override void SolveInstance(IGH_DataAccess DA)
       {
            // get input
            FemDesign.Shells.Slab slab = null;
            if (!DA.GetData(0, ref slab))
            {
                return;
            }
            if (slab == null)
            {
                return;
            }

            // return
            DA.SetData(0, slab.Guid);
            DA.SetData(1, slab.Name);
            DA.SetData(2, slab.SlabPart.Name);
            DA.SetData(3, slab.SlabPart.GetRhinoSurface());
            DA.SetData(4, slab.Material);
            DA.SetDataList(5, slab.SlabPart.GetRhinoCurves());
            DA.SetDataList(6, slab.SlabPart.GetEdgeConnections());
            DA.SetDataList(7, slab.SurfaceReinforcement);
       }
       protected override System.Drawing.Bitmap Icon
       {
           get
           {
                return FemDesign.Properties.Resources.SlabDeconstruct;
           }
       }
       public override Guid ComponentGuid
       {
           get { return new Guid("145b6331-bf19-4d89-9e81-9e5e0d137f87"); }
       }
    }
}