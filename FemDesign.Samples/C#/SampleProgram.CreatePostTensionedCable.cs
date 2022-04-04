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
            // Define geometry
            var p1 = new Geometry.FdPoint3d(0.0, 2.0, 0);
            var p2 = new Geometry.FdPoint3d(10, 2.0, 0);
            var edge = new Geometry.Edge(p1, p2, Geometry.FdVector3d.UnitY());

            // Create beam
            Materials.MaterialDatabase materialsDB = Materials.MaterialDatabase.DeserializeStruxml(MaterialsPath);
            Sections.SectionDatabase sectionsDB = Sections.SectionDatabase.DeserializeStruxml(SectionsPath);

            var material = materialsDB.MaterialByName("C35/45");
            var section = sectionsDB.SectionByName("Concrete sections, Rectangle, 300x900");

            var bar = new Bars.Bar(
                edge,
                Bars.BarType.Beam,
                material,
                sections: new Sections.Section[] { section },
                connectivities: new Bars.Connectivity[] { Bars.Connectivity.GetRigid() },
                eccentricities: new Bars.Eccentricity[] { Bars.Eccentricity.GetDefault() },
                identifier: "B");

            // Create Post-tensioned cable
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

            // Create model
            Model model = new Model(Country.S, elements);

            // Save model then open in FEM-Design
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = System.IO.Path.Combine(directory, "post_tensioned_cable.struxml");
            model.SerializeModel(path);

            var app = new Calculate.Application();
            app.OpenStruxml(path, true);
        }
    }
}
