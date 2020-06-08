// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.GH
{
    public class SlabWall: GH_Component
    {
        public SlabWall(): base("Slab.Wall", "Wall", "Create a wall element.", "FemDesign", "Shells")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface must be flat and vertical", GH_ParamAccess.item);
            pManager.AddNumberParameter("Thickness", "Thickness", "Thickness.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Material", "Material", "Material.", GH_ParamAccess.item);
            pManager.AddGenericParameter("ShellEccentricity", "Eccentricity", "ShellEccentricity. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("ShellOrthotropy", "Orthotropy", "ShellOrthotropy. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("ShellEdgeConnection", "EdgeConnection", "ShellEdgeConnection. Optional.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "Identifier. Optional.", GH_ParamAccess.item, "P");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Slab", "Slab", "Slab.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            Brep surface = null;
            double thickness = 0;
            FemDesign.Materials.Material material = null;
            FemDesign.Shells.ShellEccentricity eccentricity = FemDesign.Shells.ShellEccentricity.Default();
            FemDesign.Shells.ShellOrthotropy orthotropy = FemDesign.Shells.ShellOrthotropy.Default();
            FemDesign.Shells.ShellEdgeConnection edgeConnection = FemDesign.Shells.ShellEdgeConnection.Rigid();
            string identifier = "P";
            if(!DA.GetData(0, ref surface)) { return; }
            if(!DA.GetData(1, ref thickness)) { return; }
            if(!DA.GetData(2, ref material)) { return; }
            if(!DA.GetData(3, ref eccentricity))
            {
                // pass
            }
            if(!DA.GetData(4, ref orthotropy))
            {
                // pass
            }
            if(!DA.GetData(5, ref edgeConnection))
            {
                // pass
            }
            if(!DA.GetData(6, ref identifier))
            {
                // pass
            }
            if (surface == null || material == null || eccentricity == null || orthotropy == null || edgeConnection == null || identifier == null) { return; }

            //
            FemDesign.Geometry.Region region = FemDesign.Geometry.Region.FromRhino(surface);

            //
            List<FemDesign.Shells.Thickness> thicknessObj = new List<FemDesign.Shells.Thickness>();
            thicknessObj.Add(new FemDesign.Shells.Thickness(region.coordinateSystem.origin, thickness));

            //
            FemDesign.Shells.Slab obj = FemDesign.Shells.Slab.Wall(identifier, material, region, edgeConnection, eccentricity, orthotropy, thicknessObj);

            // return
            DA.SetData(0, obj);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.Wall;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("e26cd4b0-0582-4ea5-8705-3b9695937277"); }
        }
    }
}