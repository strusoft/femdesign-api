using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;


namespace FemDesign.Geometry.RegionTest
{
    [TestClass()]
    public class RegionTest
    {
        [TestMethod("Create Rectangle Slab")]
        public void CreateRectangle()
        {
            //
            var rectangleWall = FemDesign.Shells.Slab.Wall(
                10.0,
                20.0,
                0.1,
                FemDesign.Materials.MaterialDatabase.GetDefault().MaterialByName("C30/37"));

            var rectangleWall2 = FemDesign.Shells.Slab.Plate(
                10.0,
                20.0,
                0.1,
                FemDesign.Materials.MaterialDatabase.GetDefault().MaterialByName("C30/37"));



            var model = new FemDesign.Model(Country.S);
            var elements = new List<FemDesign.Shells.Slab>() { rectangleWall, rectangleWall2};
            model.AddElements(elements);
            var struxml = model.SerializeToString();
        }
    }
}