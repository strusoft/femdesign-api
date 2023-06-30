// https://strusoft.com/
using System;
using GH = Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace FemDesign.Grasshopper
{
    public class MovingLoad : GH_Component
    {
        public MovingLoad() : base("MovingLoad", "MovingLoad", "Creates a moving load.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("VehicleGuid", "VehicleGuid", "", GH_ParamAccess.item);
            pManager.AddTextParameter("Name", "Name", "", GH_ParamAccess.item);
            pManager.AddCurveParameter("Curve", "Curve", "Curve defining the line load.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("DivisionNumber", "DivisionNumber", "", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Return", "Return", "", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("LockDirection", "LockDirection", "", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("CutPath", "CutPath", "", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
        }
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("MovingLoad", "MovingLoad", "MovingLoad.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            // get input
            Rhino.Geometry.Curve curve = null;
            DA.GetData("Curve", ref curve);

            if (!curve.TryGetPolyline(out Polyline poly))
                throw new ArgumentException("Input Curve is not 'Line' or 'Polyline'");

            var pathPosition = new List<FemDesign.Geometry.Point3d>();
            for (int index = 0; index < poly.Count; index++)
            {
                pathPosition.Add(poly.ElementAt(index).FromRhino());
            }

            Guid vehicleGuid = new Guid();
            if (!DA.GetData("VehicleGuid", ref vehicleGuid)) return;

            string name = "";
            if (!DA.GetData("Name", ref name)) return;

            int divisionNumber = 2;
            if (!DA.GetData("DivisionNumber", ref divisionNumber)) return;

            bool returnPath = false;
            DA.GetData("Return", ref returnPath);

            bool lockDirection = false;
            DA.GetData("LockDirection", ref lockDirection);

            bool cutPath = false;
            DA.GetData("CutPath", ref cutPath);

            Point3d[] vehiclePositions;
            curve.DivideByCount(divisionNumber, true, out vehiclePositions);

            var vehiclePos = vehiclePositions.Select(x => x.FromRhino()).ToList();

            var movingLoad = new FemDesign.Loads.MovingLoad(name, vehicleGuid, pathPosition, vehiclePos, returnPath, lockDirection, cutPath);

            DA.SetData(0, movingLoad);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{66455D6A-6F26-43FF-BDC2-AB5DD9532D0E}"); }
        }
        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}