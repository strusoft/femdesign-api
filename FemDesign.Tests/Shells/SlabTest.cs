using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FemDesign.Shells
{
    /// <summary>
    /// Summary description for SlabTest
    /// </summary>
    [TestClass()]
    public class SlabTest
    {
        public static Model ReadModel()
        {
            string input = "Shells/slabModel.struxml";
            var model = Model.DeserializeFromFilePath(input);
            return model;
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("UpdateMaterial")]
        public void UpdateMaterial()
        {
            var model = ReadModel();
            var slabs = model.Entities.Slabs;
            var panel = model.Entities.Panels;

            Materials.MaterialDatabase materialsDB = Materials.MaterialDatabase.DeserializeStruxml("Bars//materials.struxml");

            var s235 = materialsDB.MaterialByName("S 235");

            foreach (var slab in slabs)
            {
                slab.UpdateMaterial(s235);
            }
            using (var connection = new FemDesign.FemDesignConnection())
            {
                connection.Open(model, true);
            }
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("UpdateThickness")]
        public void UpdateThickness()
        {
            var model = ReadModel();

            var slabOne = model.Entities.Slabs[0];
            var slabTwo = model.Entities.Slabs[1];

            slabOne.UpdateThickness(0.15);

            var ptOne = slabTwo.SlabPart.Region.Contours[0].Edges[0].Points[0];
            var ptTwo = slabTwo.SlabPart.Region.Contours[0].Edges[0].Points[1];
            var ptThree = slabTwo.SlabPart.Region.Contours[0].Edges[3].Points[0];
            
            var points = new List<Geometry.Point3d> { ptOne, ptTwo, ptThree };
            var values = new List<double> { 1, 2, 0.50 };
            slabTwo.UpdateThickness(points, values);
            using (var connection = new FemDesign.FemDesignConnection())
            {
                connection.Open(model, true);
            }
        }
    }
}
