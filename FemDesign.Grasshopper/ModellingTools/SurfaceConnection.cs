// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class SurfaceConnection : FEM_Design_API_Component
    {
        public SurfaceConnection() : base("SurfaceConnection", "SrfConnect", "Construct a Surface Connection.", CategoryName.Name(), "ModellingTools")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ElementsToConnect", "Elements", "Surface structural elements to be connected (e.g. slabs, surface supports, fictious shells, etc).", GH_ParamAccess.list);
            pManager.AddSurfaceParameter("Surface", "Srf", "Surface must be flat.", GH_ParamAccess.item);
            
            pManager.AddGenericParameter("Motion", "Mot", "Default motion release is rigid (1.000e+5 kN/m2/m).", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("MotionsPlasticLimits", "PlaLimM", "Plastic limits forces for motion springs. No plastic limits defined by default.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddVectorParameter("LocalX", "X", "Set local x-axis. Vector must be perpendicular to surface local z-axis. Local y-axis will be adjusted accordingly. Optional, local x-axis from surface coordinate system used if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddVectorParameter("LocalZ", "Z", "Set local z-axis. Vector must be perpendicular to surface local x-axis. Local y-axis will be adjusted accordingly. Optional, local z-axis from surface coordinate system used if undefined.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddTextParameter("Identifier", "ID", "Identifier.", GH_ParamAccess.item, "CS");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("SurfaceConnection", "SrfConnect", "SurfaceConnection.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            var elements = new List<EntityBase>();
            DA.GetDataList(0, elements);

            Brep surface = null;
            if (!DA.GetData(1, ref surface)) { return; }
            if (surface == null) { return; }

            Releases.Motions motions = null;
            if (!DA.GetData(2, ref motions))
            {
                motions = Releases.Motions.RigidSurface();
            }

            Releases.MotionsPlasticLimits limits = new Releases.MotionsPlasticLimits(null, null, null, null, null, null);
            DA.GetData(3, ref limits);

            Vector3d localX = Vector3d.Zero;
            DA.GetData(4, ref localX);

            Vector3d localZ = Vector3d.Zero;
            DA.GetData(5, ref localZ);

            string identifier = "CS";
            DA.GetData(6, ref identifier);
            if(identifier == null) { return; }
                               

            var fdSuface = surface.FromRhino();
            var rigidity = new Releases.RigidityDataType1(motions, limits);

            GuidListType[] refs = new GuidListType[elements.Count];
            for (int idx = 0; idx < refs.Length; idx++)
            {
                if (elements[idx] is Shells.Slab slab)
                {
                    refs[idx] = new GuidListType(slab.SlabPart);
                }
                else
                {
                    refs[idx] = new GuidListType(elements[idx]);
                }
            }

            var obj = new FemDesign.ModellingTools.SurfaceConnection(fdSuface, rigidity, refs, identifier);

            if(!localX.Equals(Vector3d.Zero))
            {
                obj.LocalX = localX.FromRhino();
            }
            if (!localZ.Equals(Vector3d.Zero))
            {
                obj.LocalZ = localZ.FromRhino();
            }


            // output
            DA.SetData(0, obj);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SurfaceConnection;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{D72F0A21-FE61-483A-93F9-53FFEF68746B}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}