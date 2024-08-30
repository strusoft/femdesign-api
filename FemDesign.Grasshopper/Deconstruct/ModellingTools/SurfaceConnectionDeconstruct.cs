// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class SurfaceConnectionDeconstruct : FEM_Design_API_Component
    {
        public SurfaceConnectionDeconstruct() : base("SurfaceConnection.Deconstruct", "Deconstruct", "Deconstruct a SurfaceConnection.", CategoryName.Name(), "Deconstruct")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("SurfaceConnection", "SrfConnect", "SurfaceConnection from ModellingTools.", GH_ParamAccess.item);
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Guid", "Guid", "Guid.", GH_ParamAccess.item);
            pManager.AddGenericParameter("ConnectedElementsReference", "Ref", "GUIDs of connected surface structural elements (e.g. slabs, surface supports, fictious shells, etc).", GH_ParamAccess.list);
            pManager.AddSurfaceParameter("Surface", "Srf", "SurfaceConnection definition surface.", GH_ParamAccess.item);
            pManager.AddGenericParameter("Motion", "Mot", "Motion release.", GH_ParamAccess.item);
            pManager.AddGenericParameter("MotionsPlasticLimits", "PlaLimM", "Plastic limits forces for motion springs.", GH_ParamAccess.item);
            pManager.AddVectorParameter("LocalX", "X", "Local x-axis.", GH_ParamAccess.item);
            pManager.AddVectorParameter("LocalZ", "Z", "Local z-axis.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Distance", "d", "Distance in meter.", GH_ParamAccess.item);
            pManager.AddTextParameter("Identifier", "ID", "Identifier.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            FemDesign.ModellingTools.SurfaceConnection obj = null;
            if(!DA.GetData(0, ref obj)) { return; }
            if(obj == null) { return; }

            // get output
            DA.SetData(0, obj.Guid);
            DA.SetDataList(1, obj.References);
            DA.SetData(2, obj.Region.ToRhinoBrep());
            DA.SetData(3, obj.Rigidity.Motions);
            DA.SetData(4, obj.Rigidity.PlasticLimitForces);
            DA.SetData(5, obj.LocalX.ToRhino());
            DA.SetData(6, obj.LocalZ.ToRhino());
            DA.SetData(7, obj.Distance);
            DA.SetData(8, obj.Name);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.SurfaceConnectionDeconstruct;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{5994409D-2C7D-48E4-8968-4FBAA71A98FA}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.senary;

    }
}