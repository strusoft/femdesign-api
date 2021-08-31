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

            var material = new Materials.Material();
            var section = new Sections.Section();
            var bar = new Bars.Bar(edge, Bars.BarType.Beam, material, section, "B");

            var shape = new Reinforcement.PtcShapeType();

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

            var strand = new Reinforcement.PtcStrandData(
                f_pk: 1860.0,
                a_p: 150.0,
                e_p: 195000.0,
                density: 7.810,
                relaxationClass: 2,
                rho_1000: 0.0
                );

            var ptc = new Reinforcement.Ptc(bar, shape, losses, manufacturing, strand, numberOfStrands: 3, identifier: "PTC");

            var elements = new List<GenericClasses.IStructureElement>() { bar, ptc };

            Model model = new Model(Country.S, elements);
        }
    }
}
