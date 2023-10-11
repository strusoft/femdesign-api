// https://strusoft.com/
using System;
using System.Collections.Generic;
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
            pManager.AddSurfaceParameter("Surface", "Surface", "Surface must be flat.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Elements", "Elements", "Surface structural elements (e.g. slabs, surface supports, fictious shells, etc).", GH_ParamAccess.list);

            pManager.AddGenericParameter("Motion", "Motion", "Motion.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("PlaticLimits", "Limits", "Motion plastic limits.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddTextParameter("Identifier", "ID", "Identifier.", GH_ParamAccess.item, "CS");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("LineConnection", "LineConnection", "LineConnection.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            Brep surface = null;
            if (!DA.GetData("Surface", ref surface)) { return; }

            Releases.Motions motions = null;
            if (!DA.GetData("Motion", ref motions))
            {
                motions = Releases.Motions.RigidSurface();
            }

            string identifier = "CS";
            if (!DA.GetData("Identifier", ref identifier))
            {
                // pass
            }




            

            var elements = new List<EntityBase>();
            DA.GetDataList("Elements", elements);


            GuidListType[] refs = new GuidListType[elements.Count];
            for (int idx = 0; idx < refs.Length; idx++)
            {
                if (elements[idx] is Shells.Slab slab)
                {
                    refs[idx] = new GuidListType(slab.SlabPart);
                }
                else if (elements[idx] is Bars.Bar bar)
                {
                    refs[idx] = new GuidListType(bar.BarPart);
                }
                else
                {
                    refs[idx] = new GuidListType(elements[idx]);
                }

            }


            var rigidity = new Releases.RigidityDataType3(motions, rotations);

            var connectedLines = new FemDesign.ModellingTools.ConnectedLines(firstEdge.FromRhino(), secondEdge.FromRhino(), localX.FromRhino(), localY.FromRhino(), rigidity, refs, identifier, movingLocal, interfaceStart, interfaceEnd);

            // output
            DA.SetData(0, connectedLines);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.LineConnection;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{D72F0A21-FE61-483A-93F9-53FFEF68746B}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}