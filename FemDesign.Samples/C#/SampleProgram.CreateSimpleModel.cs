using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Samples
{
    public partial class SampleProgram
    {
        private static void CreateSimpleModel()
        {
            // Define geometry
            var p1 = new Geometry.FdPoint3d(2.0, 2.0, 0);
            var p2 = new Geometry.FdPoint3d(10, 2.0, 0);
            var mid = p1 + (p2 - p1) * 0.5;

            // Create elements
            var edge = new Geometry.Edge(p1, p2, Geometry.FdVector3d.UnitZ());

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
            var elements = new List<GenericClasses.IStructureElement>() { bar };

            // Create supports
            var s1 = new Supports.PointSupport(
                point: p1,
                motions: Releases.Motions.RigidPoint(),
                rotations: Releases.Rotations.Free()
                );

            var s2 = new Supports.PointSupport(
                point: p2,
                motions: new Releases.Motions(yNeg: 1e10, yPos: 1e10, zNeg: 1e10, zPos: 1e10),
                rotations: Releases.Rotations.Free()
                );
            var supports = new List<GenericClasses.ISupportElement>() { s1, s2 };

            // Create load cases
            var deadload = new Loads.LoadCase("Deadload", Loads.LoadCaseType.DeadLoad, Loads.LoadCaseDuration.Permanent);
            var liveload = new Loads.LoadCase("Liveload", Loads.LoadCaseType.Static, Loads.LoadCaseDuration.Permanent);
            var loadcases = new List<Loads.LoadCase>() { deadload, liveload };

            // Create load combinations
            var slsFactors = new List<double>() { 1.0, 1.0 };
            var SLS = new Loads.LoadCombination("SLS", Loads.LoadCombType.ServiceabilityCharacteristic, loadcases, slsFactors);
            var ulsFactors = new List<double>() { 1.35, 1.5 };
            var ULS = new Loads.LoadCombination("ULS", Loads.LoadCombType.UltimateOrdinary, loadcases, ulsFactors);
            var loadCombinations = new List<Loads.LoadCombination>() { SLS, ULS };

            // Create loads
            var pointForce = new Loads.PointLoad(mid, new Geometry.FdVector3d(0.0, 0.0, -5.0), liveload, "", Loads.ForceLoadType.Force);
            var pointMoment = new Loads.PointLoad(p2, new Geometry.FdVector3d(0.0, 5.0, 0.0), liveload, "", Loads.ForceLoadType.Moment);

            var lineLoadStart = new Geometry.FdVector3d(0.0, 0.0, -2.0);
            var lineLoadEnd = new Geometry.FdVector3d(0.0, 0.0, -4.0);
            var lineLoad = new Loads.LineLoad(edge, lineLoadStart, lineLoadEnd, liveload, "", constLoadDir: true, loadProjection: true, Loads.ForceLoadType.Force);

            var loads = new List<GenericClasses.ILoadElement>() { 
                pointForce,
                pointMoment,
                lineLoad
            };

            // Add to model
            Model model = new Model(Country.S);
            model.AddElements(elements);
            model.AddSupports(supports);
            model.AddLoadCases(loadcases);
            model.AddLoadCombinations(loadCombinations);
            model.AddLoads(loads);

            // Save model then open in FEM-Design
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string path = System.IO.Path.Combine(directory, "simple_model.struxml");
            model.SerializeModel(path);

            var app = new Calculate.Application();
            app.OpenStruxml(path, true);
        }
    }
}
