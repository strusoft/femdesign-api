// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class PointConnection : FEM_Design_API_Component
    {
        public PointConnection() : base("PointConnection", "PtConnect", "Construct a Point Connection.", CategoryName.Name(), "ModellingTools")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("ElementsToConnect", "Elements", "Structural elements to be connected (bars, slabs, supports, etc.).", GH_ParamAccess.list);
            
            pManager.AddPointParameter("MasterPoint", "MPoint", "Define a master point.", GH_ParamAccess.item);
            pManager.AddPointParameter("SlavePoint", "SPoint", "Define a slave point.", GH_ParamAccess.item);
            
            pManager.AddGenericParameter("Motion", "Mot", "Default motion release is rigid (1.000e+10 kN/m).", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("MotionsPlasticLimits", "PlaLimM", "Plastic limits forces for motion springs. No plastic limits defined by default.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddGenericParameter("Rotation", "Rot", "Default rotation release is rigid (1.000e+10 kNm/rad).", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("RotationsPlaticLimits", "PlaLimR", "Plastic limits moments for rotation springs. No plastic limits defined by default.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;

            pManager.AddPlaneParameter("LocalPlane", "Plane", "Default orientation is WorldXY Plane.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            
            pManager.AddTextParameter("Identifier", "ID", "Define an identifier.", GH_ParamAccess.item, "CP");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PointConnection", "PointConnection", "Point connection.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            var elements = new List<EntityBase>();
            DA.GetDataList(0, elements);
            
            Point3d masterPoint = Point3d.Unset;
            DA.GetData(1, ref masterPoint);

            Point3d slavePoint = Point3d.Unset;
            DA.GetData(2, ref slavePoint);

            Releases.Motions motions = null;
            if (!DA.GetData(3, ref motions))
            {
                motions = Releases.Motions.RigidPoint();
            }

            Releases.MotionsPlasticLimits motLimits = null;
            DA.GetData(4, ref motLimits);

            Releases.Rotations rotations = null;
            if (!DA.GetData(5, ref rotations))
            {
                rotations = Releases.Rotations.RigidPoint();
            }

            Releases.RotationsPlasticLimits rotLimits = null;
            DA.GetData(6, ref rotLimits);

            Plane plane = Plane.WorldXY;
            DA.GetData(7, ref plane);
            //Conversion
            FemDesign.Geometry.Plane fdPlane = plane.FromRhinoPlane();

            string identifier = "CP";
            DA.GetData(8, ref identifier);


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

            var connectedPoints = new FemDesign.ModellingTools.ConnectedPoints(fdPlane, masterPoint.FromRhino(), slavePoint.FromRhino(), motions, motLimits, rotations, rotLimits, refs, identifier);


            // get output
            DA.SetData(0, connectedPoints);

        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.PointConnection;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{13F2ED2A-364C-4B5B-9D62-D725ED7CE7A1}"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}