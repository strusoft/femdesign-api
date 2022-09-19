// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class SurfaceReinforcementBySurface: GH_Component
    {
        public SurfaceReinforcementBySurface(): base("SurfaceReinforcement.BySurface", "BySurface", "Create straight surface reinforcement for a portion of a slab. This surface reinforcement will cover the passed surface of any slab it is applied to. Note that the surface must be contained within the target slab.", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Straight", "Straight", "Straight reinforcement layout.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Wire", "Wire", "Wire.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SurfaceReinforcement", "SurfaceReinforcement", "Surface reinforcement.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            Brep brep = null;
            FemDesign.Reinforcement.Straight straight = null;
            FemDesign.Reinforcement.Wire wire = null;
            if (!DA.GetData(0, ref brep))
            {
                return;
            }
            if (!DA.GetData(1, ref straight))
            {
                return;
            }
            if (!DA.GetData(2, ref wire))
            {
                return;
            }
            if (brep == null || straight == null || wire == null)
            {
                return;
            }
            
            // convert geometry
            FemDesign.Geometry.Region region = brep.FromRhino();          

            //
            FemDesign.Reinforcement.SurfaceReinforcement obj = FemDesign.Reinforcement.SurfaceReinforcement.DefineStraightSurfaceReinforcement(region, straight, wire);
        
            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SurfaceReinforcementBySurface;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("2cd988d4-a126-4f2f-81e3-e87ffb1a9dc6"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }   
}