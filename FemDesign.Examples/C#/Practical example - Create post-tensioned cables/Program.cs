using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;

namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main()
        {
            // PRACTICAL EXAMPLE: CREATE POST-TENSIONED CABLES
            // This example will show you how to add post-tensioned cables to your concrete beam.

            // This example was last updated 2022-11-17, using the ver. 21.6.0 FEM-Design API.


            // DEFINE GEOMETRY
            var p1 = new Geometry.Point3d(0.0, 2.0, 0);
            var p2 = new Geometry.Point3d(10, 2.0, 0);
            var edge = new Geometry.Edge(p1, p2, Geometry.Vector3d.UnitY);

            // CREATE BEAM
            Materials.MaterialDatabase materialsDB = Materials.MaterialDatabase.DeserializeStruxml("materials.struxml");
            Sections.SectionDatabase sectionsDB = Sections.SectionDatabase.DeserializeStruxml("sections.struxml");

            var material = materialsDB.MaterialByName("C35/45");
            var section = sectionsDB.SectionByName("Concrete sections, Rectangle, 300x900");

            var bar = new Bars.Bar(
                edge,
                Bars.BarType.Beam,
                material,
                sections: new Sections.Section[] { section },
                connectivities: new Bars.Connectivity[] { Bars.Connectivity.Rigid },
                eccentricities: new Bars.Eccentricity[] { Bars.Eccentricity.Default },
                identifier: "B");


            // CREATE POST-TENSIONED CABLE
            var shape = new Reinforcement.PtcShapeType(
                start: new Reinforcement.PtcShapeStart(0.0, 0.0),
                intermediates: new List<Reinforcement.PtcShapeInner>()
                {
                    new Reinforcement.PtcShapeInner(0.4, -0.20, 0.0, 0.1),
                    new Reinforcement.PtcShapeInner(0.6, -0.20, 0.0),
                },
                end: new Reinforcement.PtcShapeEnd(0.0, 0.0, 0.9)
                );

            var losses = new Reinforcement.PtcLosses(
                curvatureCoefficient: 0.05,
                wobbleCoefficient: 0.007,
                anchorageSetSlip: 0.0,
                elasticShortening: 0.0,
                creepStress: 0.0,
                shrinkageStress: 0.0,
                relaxationStress: 0.0);

            var manufacturing = new Reinforcement.PtcManufacturingType(
                positions: new List<double>() { 0.3, 0.7 },
                shiftX: 0.0,
                shiftZ: 0.1);

            var strandData = new Reinforcement.PtcStrandLibType(
                name: "Custom ptc material",
                f_pk: 1860.0,
                a_p: 150.0,
                e_p: 195000.0,
                rho: 7.810,
                relaxationClass: 2,
                rho_1000: 0.1);

            var ptc = new Reinforcement.Ptc(
                bar,
                shape,
                losses,
                manufacturing,
                strandData,
                jackingSide: Reinforcement.JackingSide.Start,
                jackingStress: 1000.0,
                numberOfStrands: 3,
                identifier: "PTC");

            var elements = new List<GenericClasses.IStructureElement>() {
                bar,
                ptc
            };


            // CREATE AND OPEN MODEL
            Model model = new Model(Country.S, elements);
            string path = "output/post_tensioned_cable.struxml";
            if (!Directory.Exists("output"))
                Directory.CreateDirectory("output");
            model.SerializeModel(path);

            using (var app = new FemDesignConnection())
            {
                app.Open(model);
                app.Disconnect();
            }
        }
    }
}
