using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Samples
{
    public partial class SampleProgram
    {
        private static void CreatePostTensionedCable()
        {
            var edge = new Geometry.Edge(new Geometry.FdPoint3d(0, 0, 0), new Geometry.FdPoint3d(10, 0, 0), Geometry.FdVector3d.UnitZ());

            Materials.MaterialDatabase materialsDB = Materials.MaterialDatabase.DeserializeStruxml(MaterialsPath);
            Sections.SectionDatabase sectionsDB = Sections.SectionDatabase.DeserializeStruxml(SectionsPath);

            var material = materialsDB.MaterialByName("C35/45");
            var section = sectionsDB.SectionByName("Concrete sections, Rectangle, 300x900");

            var bar = new Bars.Bar(edge, Bars.BarType.Beam, material, section, "B");

            var shape = new Reinforcement.PtcShapeType(
                start: new Reinforcement.PtcShapeStart() { Z= -0.3, Tangent = 0.0 },
                intermediates: new List<Reinforcement.PtcShapeInner>()
                {
                    new Reinforcement.PtcShapeInner() { Position = 0.11, Z = 0.0, Tangent = 0.0 },
                    new Reinforcement.PtcShapeInner() { Position = 0.22, Z = 0.1, Tangent = 0.0 },
                    new Reinforcement.PtcShapeInner() { Position = 0.67, Z = 0.2, Tangent = 0.0 },
                },
                end: new Reinforcement.PtcShapeEnd() { Z = -0.12, Tangent = 0.0}
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
                positions: new List<double>() { 0.0, 2.0, 4.0, 6.0, 8.0, 10.0 }, 
                shiftX: 0.0,
                shiftZ: 0.1
                );

            var strandData = new Reinforcement.PtcStrandLibType(
                name: "Custom ptc material",
                f_pk: 1860.0,
                a_p: 150.0,
                e_p: 195000.0,
                density: 7.810,
                relaxationClass: 2,
                rho_1000: 0.0
                );

            var ptc = new Reinforcement.Ptc(bar, shape, losses, manufacturing, strandData, numberOfStrands: 3, identifier: "PTC");

            var elements = new List<GenericClasses.IStructureElement>() { 
                bar,
                ptc
            };

            Model model = new Model(Country.S, elements);
            
            string path = System.IO.Path.GetTempFileName() + ".struxml";
            model.SerializeModel(path);

            //var app = new Calculate.Application();
            //app.OpenStruxml(path, false);
        }
    }
}
