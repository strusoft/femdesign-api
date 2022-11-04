using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;


namespace FemDesign.Geometry
{
    [TestClass()]
    public class RegionTest
    {
        [TestCategory("FEM-Design required")]
        [TestMethod("Create Rectangle Slab")]
        public void CreateRectangle()
        {
            var rectangleWall = FemDesign.Shells.Slab.Wall(
                Point3d.Origin,
                new Point3d(10, 10, 0),
                10.0,
                0.1,
                FemDesign.Materials.MaterialDatabase.GetDefault().MaterialByName("C30/37"));

            var rectangleWall2 = FemDesign.Shells.Slab.Plate(
                Point3d.Origin,
                10.0,
                20.0,
                0.1,
                FemDesign.Materials.MaterialDatabase.GetDefault().MaterialByName("C30/37"));

            var point0 = new Point3d(0, 0, 4);
            var point1 = new Point3d(10, 0, 4);
            var point2 = new Point3d(5, 14, 4);
            var point3 = new Point3d(0, 1, 4);
            var slab = FemDesign.Shells.Slab.FromFourPoints(point0, point1, point2, point3, 0.4, FemDesign.Materials.MaterialDatabase.GetDefault().MaterialByName("C30/37"));


            var slab2 = FemDesign.Shells.Slab.Wall(point0, point1, 3, 0.4, FemDesign.Materials.MaterialDatabase.GetDefault().MaterialByName("C30/37"));


            var model = new FemDesign.Model(Country.S);
            var elements = new List<FemDesign.Shells.Slab>() { rectangleWall, rectangleWall2, slab, slab2 };
            model.AddElements(elements);
            model.Open();
        }
    }
}