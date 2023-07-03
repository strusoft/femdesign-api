// https://strusoft.com/
using System;
using GH = Grasshopper;
using Grasshopper.Kernel;
using Rhino.Geometry;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using FemDesign.LibraryItems;
using StruSoft.Interop.StruXml.Data;
using FemDesign.Grasshopper.Extension.ComponentExtension;
using Grasshopper.Kernel.Special;
using System.Xml;

namespace FemDesign.Grasshopper
{
    public class MovingLoad : GH_Component
    {
        public MovingLoad() : base("MovingLoad.Construct", "MovingLoad.Construct", "Creates a moving load.", CategoryName.Name(), SubCategoryName.Cat3())
        {

        }
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Moving load name", GH_ParamAccess.item);
            pManager.AddGenericParameter("Vehicle", "Vehicle", "Vehicle name. \"Connect 'ValueList' component to get the names.", GH_ParamAccess.item);
            pManager.AddCurveParameter("Curve", "Curve", "Curve defining the vehicle path.", GH_ParamAccess.item);
            pManager.AddIntegerParameter("DivisionNumber", "DivisionNumber", "", GH_ParamAccess.item);
            pManager.AddNumberParameter("ShiftX", "ShiftX", "", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddNumberParameter("ShiftY", "ShiftY", "", GH_ParamAccess.item);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("Return", "Return", "", GH_ParamAccess.item, false);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("LockDirection", "LockDirection", "", GH_ParamAccess.item, true);
            pManager[pManager.ParamCount - 1].Optional = true;
            pManager.AddBooleanParameter("CutPath", "CutPath", "", GH_ParamAccess.item, false);
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


            Vehicle_lib_type selectedVehicle = null;
            string _vehicleName = "";
            dynamic vehicleName = null;
            if (!DA.GetData("Vehicle", ref vehicleName)) return;

            if(vehicleName.Value is string)
            {
                _vehicleName = (string) vehicleName.Value;
                var vehicleDatabase = new Dictionary<string, Vehicle_lib_type>();

                foreach (var item in VehicleDatabase.DeserializeFromResource())
                {
                    vehicleDatabase.Add(item.Name, item);
                }

                var success = vehicleDatabase.TryGetValue(_vehicleName, out selectedVehicle);
                if (!success)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"Vehicle '{_vehicleName}' does not exist");
                    return;
                }
            }
            else if(vehicleName.Value is StruSoft.Interop.StruXml.Data.Vehicle_lib_type _vehicleLib)
            {
                selectedVehicle = _vehicleLib;
            }


            string name = "";
            if (!DA.GetData("Name", ref name)) return;

            int divisionNumber = 2;
            if (!DA.GetData("DivisionNumber", ref divisionNumber)) return;

            double shiftX = 0.00;
            DA.GetData("ShiftX", ref shiftX);

            double shiftY = 0.00;
            DA.GetData("ShiftY", ref shiftY);

            bool returnPath = false;
            DA.GetData("Return", ref returnPath);

            bool lockDirection = false;
            DA.GetData("LockDirection", ref lockDirection);

            bool cutPath = false;
            DA.GetData("CutPath", ref cutPath);

            Point3d[] vehiclePositions;
            curve.DivideByCount(divisionNumber, true, out vehiclePositions);

            var vehiclePos = vehiclePositions.Select(x => x.FromRhino()).ToList();

            var movingLoad = new FemDesign.Loads.MovingLoad(name, selectedVehicle, pathPosition, vehiclePos, shiftX, shiftY, returnPath, lockDirection, cutPath);

            DA.SetData(0, movingLoad);
        }
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return FemDesign.Properties.Resources.MovingLoad;
            }
        }
        public override Guid ComponentGuid
        {
            get { return new Guid("{66455D6A-6F26-43FF-BDC2-AB5DD9532D0E}"); }
        }

        protected override void BeforeSolveInstance()
        {
            ValueListUtils.updateValueLists(this, 1, new List<string>
            { "EC LM1 Lane 1. (Truck) [Concentrated]", "EC LM1 Lane 2. (Truck) [Concentrated]", "EC LM1 Lane 3. (Truck) [Concentrated]", "EC LM2 (Truck) [Concentrated]", "EC LM1 Lane 1. (Truck) [Distributed]", "EC LM1 Lane 2. (Truck) [Distributed]", "EC LM1 Lane 3. (Truck) [Distributed]", "EC LM2 (Truck) [Distributed]", "EC LM71 (Train)", "EC LM71 Right (Train) [e=79mm, r=1435mm]", "EC LM71 Left (Train) [e=79mm, r=1435mm]", "EC SW/0 (Train)", "EC SW/0 Right (Train) [e=79mm, r=1435mm]", "EC SW/0 Left (Train) [e=79mm, r=1435mm]", "EC SW/2 (Train)", "EC SW/2 Right (Train) [e=79mm, r=1435mm]", "EC SW/2 Left (Train) [e=79mm, r=1435mm]", "Unit"
            }, null, GH_ValueListMode.DropDown);
        }


        public override GH_Exposure Exposure => GH_Exposure.quarternary;

    }
}