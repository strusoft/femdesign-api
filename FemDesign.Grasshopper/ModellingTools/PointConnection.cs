// https://strusoft.com/
using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public class PointConnection : GH_Component
    {
        public PointConnection() : base("PointConnection", "PtConnect", "Construct a Point Connection", "FEM-Design", "ModellingTools")
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            
            pManager.AddPointParameter("MasterPoint", "MasterPoint", "Define a master point.", GH_ParamAccess.item);
            pManager.AddPointParameter("OtherPoint", "OtherPoint", "Define an other point (slave point).", GH_ParamAccess.item);
            pManager.AddGenericParameter("Motion", "Motion", "Default motion release is rigid (1.000e+10 kN/m).", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("Rotation", "Rotation", "Default rotation release is rigid (1.000e+10 kNm/rad).", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddGenericParameter("ElementToConnect", "ElementToConnect", "Objects to connect.", GH_ParamAccess.list);
            pManager.AddPlaneParameter("LocalPlane", "LocalPlane", "Default orientation is WorldXY Plane.", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddTextParameter("Identifier", "Identifier", "Define an identifier (position number).", GH_ParamAccess.item, "CP");
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PointConnection", "PointConnection", "Point connection.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Point3d masterPoint = Point3d.Unset;
            DA.GetData(0, ref masterPoint);

            Rhino.Geometry.Point3d otherPoint = Point3d.Unset;
            DA.GetData(1, ref otherPoint);

            Releases.Motions motions = null;
            if (!DA.GetData(2, ref motions))
            {
                motions = Releases.Motions.RigidPoint();
            }

            Releases.Rotations rotations = null;
            if (!DA.GetData(3, ref rotations))
            {
                rotations = Releases.Rotations.RigidPoint();
            }

            var elements = new List<EntityBase>();
            DA.GetDataList(4, elements);

            Plane plane = Plane.WorldXY;
            DA.GetData(5, ref plane);
            //Conversion
            FemDesign.Geometry.CoordinateSystem fdPlane = plane.FromRhinoPlane();

            string identifier = "CP";
            DA.GetData(6, ref identifier);

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

            //var rigidity = new Releases.RigidityDataType2(motions, rotations);

            var connectedPoints = new FemDesign.ModellingTools.ConnectedPoints(fdPlane, masterPoint.FromRhino(), otherPoint.FromRhino(), motions, rotations, refs, identifier);

            // output
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
            get { return new Guid("FCD121E5-3BCD-42A6-B8C8-CCEDA1DD5D80"); }
        }

        public override GH_Exposure Exposure => GH_Exposure.primary;

    }
}