// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class SlabDeconstruct: GH_Component
    {
       public SlabDeconstruct(): base("Slab.Deconstruct", "Deconstruct", "Deconstruct a slab element.", "FEM-Design", "Deconstruct")
       {

       }
       protected override void RegisterInputParams(GH_InputParamManager pManager)
       {
           pManager.AddGenericParameter("Slab", "Slab", "Slab.", GH_ParamAccess.item);           
       } 
       protected override void RegisterOutputParams(GH_OutputParamManager pManager)
       {
           pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
           pManager.AddBrepParameter("Surface", "Surface", "Surface", GH_ParamAccess.item);
           pManager.AddGenericParameter("ThicknessItems", "ThickItems", "Thickness items. List of LocationValues", GH_ParamAccess.list);
           pManager.AddGenericParameter("Material", "Material", "Material", GH_ParamAccess.item);
           pManager.AddGenericParameter("ShellEccentricity", "Eccentricity", "ShellEccentricity.", GH_ParamAccess.item);
           pManager.AddGenericParameter("ShellOrthotropy", "Orthotropy", "ShellOrhotropy", GH_ParamAccess.item);
           pManager.AddCurveParameter("EdgeCurves", "EdgeCurves", "EdgeCurves", GH_ParamAccess.list);
           pManager.AddGenericParameter("EdgeConnections", "EdgeConnections", "EdgeConnections", GH_ParamAccess.list);
           pManager.AddVectorParameter("LocalX", "LocalX", "LocalX", GH_ParamAccess.item);
           pManager.AddVectorParameter("LocalY", "LocalY", "LocalY", GH_ParamAccess.item);
           pManager.AddGenericParameter("SurfaceReinforcementParameters", "SrfReinfParams", "SurfaceReinforcementParameters", GH_ParamAccess.item);
           pManager.AddGenericParameter("SurfaceReinforcement", "SrfReinf", "SurfaceReinforcement", GH_ParamAccess.list);
        //    pManager.AddVectorParameter("Reinforcement x-direction", "ReinfXDir", "X-direction of reinforcement layout", GH_ParamAccess.item);
        //    pManager.AddVectorParameter("Reinforcement y-direction", "ReinfYDir", "Y-direction of reinforcement layout", GH_ParamAccess.item);
           pManager.AddTextParameter("Identifier", "Identifier", "Structural element ID.", GH_ParamAccess.item);     
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
            DA.SetData(1, slab.SlabPart.GetRhinoSurface());
            DA.SetDataList(2, slab.SlabPart.Thickness);
            DA.SetData(3, slab.Material);
            DA.SetData(4, slab.SlabPart.ShellEccentricity);
            DA.SetData(5, slab.SlabPart.ShellOrthotropy);
            DA.SetDataList(6, slab.SlabPart.GetRhinoCurves());
            DA.SetDataList(7, slab.SlabPart.GetEdgeConnections());
            DA.SetData(8, slab.SlabPart.LocalX.ToRhino());
            DA.SetData(9, slab.SlabPart.LocalY.ToRhino());
            DA.SetData(10, slab.SurfaceReinforcementParameters);
            DA.SetDataList(11, slab.SurfaceReinforcement);
            DA.SetData(12, slab.Name);

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

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}