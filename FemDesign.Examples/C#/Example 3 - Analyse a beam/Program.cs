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
            // EXAMPLE 1: CREATING A SIMPLE BEAM
            // This example will show you how to model a simple supported beam,
            // and how to save it for export to FEM-Design.Before running,
            // make sure you have a window with FEM-Design open.

            // This example was last updated using the ver. 21.4.0 FEM-Design API.


            // Define geometry
            var p1 = new Geometry.Point3d(2.0, 2.0, 0);
            var p2 = new Geometry.Point3d(10, 2.0, 0);
            var p3 = new Geometry.Point3d(4.0, 2.0, 0);
            var mid = p1 + (p2 - p1) * 0.5;

            // Create elements
            var edge = new Geometry.Edge(p1, p2, Geometry.Vector3d.UnitZ);
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
            bar.BarPart.LocalY = Geometry.Vector3d.UnitY;
            var elements = new List<GenericClasses.IStructureElement>() { bar };


            // Create supports
            var s1 = new Supports.PointSupport(
                point: p1,
                motions: Releases.Motions.RigidPoint(),
                rotations: Releases.Rotations.RigidPoint()
                );

            var s2 = new Supports.PointSupport(
                point: p2,
                motions: new Releases.Motions(yNeg: 1e10, yPos: 1e10, zNeg: 1e10, zPos: 1e10),
                rotations: Releases.Rotations.Free()
                );

            var s3 = new Supports.PointSupport(
                point: p3,
                motions: new Releases.Motions(yNeg: 1e10, yPos: 1e10, zNeg: 1e10, zPos: 1e10),
                rotations: Releases.Rotations.Free()
                );
            var supports = new List<GenericClasses.ISupportElement>() { s1, s2, s3 };


            // Create load cases
            var deadload = new Loads.LoadCase("Deadload", Loads.LoadCaseType.DeadLoad, Loads.LoadCaseDuration.Permanent);
            var liveload = new Loads.LoadCase("Liveload", Loads.LoadCaseType.Static, Loads.LoadCaseDuration.Permanent);
            var loadcases = new List<Loads.LoadCase>() { deadload, liveload };

            var combItem = Calculate.CombItem.Default();
            var combItemNLE = new Calculate.CombItem(NLE: true);

            // Create load combinations
            var slsFactors = new List<double>() { 1.0, 1.0 };
            var SLS = new Loads.LoadCombination("SLS", Loads.LoadCombType.ServiceabilityCharacteristic, loadcases, slsFactors, combItem);
            var ulsFactors = new List<double>() { 1.35, 1.5 };
            var ULS = new Loads.LoadCombination("ULS", Loads.LoadCombType.UltimateOrdinary, loadcases, ulsFactors, combItemNLE);
            var loadCombinations = new List<Loads.LoadCombination>() { SLS, ULS };


            // Create loads
            var pointForce = new Loads.PointLoad(mid, new Geometry.Vector3d(0.0, 0.0, -5.0), liveload, null, Loads.ForceLoadType.Force);
            var pointMoment = new Loads.PointLoad(p2, new Geometry.Vector3d(0.0, 5.0, 0.0), liveload, null, Loads.ForceLoadType.Moment);

            var lineLoadStart = new Geometry.Vector3d(0.0, 0.0, -2.0);
            var lineLoadEnd = new Geometry.Vector3d(0.0, 0.0, -4.0);
            var lineLoad = new Loads.LineLoad(edge, lineLoadStart, lineLoadEnd, liveload, Loads.ForceLoadType.Force, "", constLoadDir: true, loadProjection: true);

            var obj = new FemDesign.Loads.MassConversionTable(new List<double>() { 1.0 }, new List<Loads.LoadCase>() { deadload });

            var loads = new List<GenericClasses.ILoadElement>() {
                pointForce,
                pointMoment,
                lineLoad,
                obj
            };

            // Add to model
            Model model = new Model(Country.S);
            model.AddElements(elements);
            model.AddSupports(supports);
            model.AddLoadCases(loadcases);
            model.AddLoadCombinations(loadCombinations);
            model.AddLoads(loads);

            // Set up the analysis
            //var analysisType = Calculate.Analysis.Eigenfrequencies();
            var analysisType = Calculate.Analysis.StaticAnalysis();

            // Optional Settings for the Discretisation
            var config = Calculate.CmdGlobalCfg.Default();
            config.MeshElements.DefaultDivision = 5;


            // Define Result to be extract
            var results = new List<Type>() { typeof(Results.BarDisplacement) };

            // Run a specific analysis
            model.RunAnalysis(analysisType, resultTypes: results, cmdGlobalCfg: config, endSession: true);
        }
    }
}