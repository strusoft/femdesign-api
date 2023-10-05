// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

namespace FemDesign.Grasshopper
{
    public class SurfaceReinforcementAddToSlab: FEM_Design_API_Component
    {
        public SurfaceReinforcementAddToSlab(): base("SurfaceReinforcement.AddToSlab", "AddToSlab", "Add surface reinforcement to slab.", "FEM-Design", "Reinforcement")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Slab.", GH_ParamAccess.item);
            pManager.AddGenericParameter("SurfaceReinforcement", "SurfaceReinforcement", "SurfaceReinforcment to add to slab. Item or list.", GH_ParamAccess.list);
            pManager.AddVectorParameter("xDir", "xDir", "Reinforcement direction", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("yDir", "yDir", "Reinforcement direction", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Passed slab.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get data
            FemDesign.Shells.Slab slab = null;
            List<FemDesign.Reinforcement.SurfaceReinforcement> surfaceReinforcement = new List<FemDesign.Reinforcement.SurfaceReinforcement>();
            if (!DA.GetData(0, ref slab))
            {
                return;
            }
            if (!DA.GetDataList(1, surfaceReinforcement))
            {
                return;
            }

            Rhino.Geometry.Vector3d _xDir = Rhino.Geometry.Vector3d.Unset;
            DA.GetData(2, ref _xDir);

            Rhino.Geometry.Vector3d _yDir = Rhino.Geometry.Vector3d.Unset;
            DA.GetData(3, ref _yDir);

            if (slab == null || surfaceReinforcement == null)
            {
                return;
            }

            FemDesign.Geometry.Vector3d xDir;
            if (_xDir == Rhino.Geometry.Vector3d.Unset)
                xDir = slab.SlabPart.LocalX;
            else
                xDir = _xDir.FromRhino();

            FemDesign.Geometry.Vector3d yDir;
            if (_yDir == Rhino.Geometry.Vector3d.Unset)
                yDir = slab.SlabPart.LocalY;
            else
                yDir = _yDir.FromRhino();

            FemDesign.Shells.Slab obj = FemDesign.Reinforcement.SurfaceReinforcement.AddReinforcementToSlab(slab, surfaceReinforcement, xDir, yDir);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.RebarAddToElement;
                ;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{928F0C21-878A-4B27-A61C-79832AEB2C88}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.tertiary;
    }   
}