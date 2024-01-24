using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign.LibraryItems;

using StruSoft.Interop.StruXml.Data;
using FemDesign.Geometry;
using FemDesign.Utils;
using FemDesign.Loads;

namespace FemDesign.Vehicle
{
    [TestClass()]
    public class VehicleTests
    {
        [TestMethod("DeserialiseVehicle")]
        public void DeserialiseResources()
        {
            var vehicles = VehicleDatabase.DeserializeFromResource();
            Assert.IsTrue( vehicles.Count == 18 );
        }

        [TestMethod("ConstructVehicle")]
        public void ConstructVehicle()
        {
            StruSoft.Interop.StruXml.Data.Vehicle_lib_type vehicle;

            var name = "myVehicle";

            // point loads
            var pt = new Point3d( 0, 0, 0 );
            var force = new Vector3d( 0, 0, 5 );
            var caselessPointLoad = FemDesign.Loads.PointLoad.CaselessPointLoad(pt, force);

            var pt1 = new Point3d(1, 0, 0);
            var force1 = new Vector3d(0, 0, 8);
            var caselessPointLoad1 = FemDesign.Loads.PointLoad.CaselessPointLoad(pt1, force1);

            var caselessPointLoads = new List<FemDesign.Loads.PointLoad> { caselessPointLoad, caselessPointLoad1 };

            // line loads
            var pt2 = new Point3d(0, 5, 0);

            var edge = new Edge(pt, pt1);
            var caselessLineLoad = FemDesign.Loads.LineLoad.CaselessUniformForce(edge, force);

            var edge1 = new Edge(pt, pt2);
            var caselessLineLoad1 = FemDesign.Loads.LineLoad.CaselessUniformForce(edge1, force);

            var caselessLineLoads = new List<FemDesign.Loads.LineLoad> { caselessLineLoad, caselessLineLoad1 };



            // surface loads

            var region = FemDesign.Geometry.Region.RectangleXY(pt, 4, 4);
            var caselessSurfaceLoad = FemDesign.Loads.SurfaceLoad.CaselessUniform(region, force);
            var caselessSurfaceLoads = new List<FemDesign.Loads.SurfaceLoad> { caselessSurfaceLoad };


            vehicle = new StruSoft.Interop.StruXml.Data.Vehicle_lib_type(name, caselessPointLoads, caselessLineLoads, caselessSurfaceLoads);



            FemDesign.LibraryItems.VehicleTypes vehicleTypes = new VehicleTypes();
            vehicleTypes.PredefinedTypes = new List<Vehicle_lib_type>
            {
                vehicle
            };

            Console.WriteLine(vehicleTypes.SerializeObject());
        }
    }
}
