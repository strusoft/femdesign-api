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
            // EXAMPLE 3: CREATING A SIMPLE BEAM
            // This example shows how to model a simple supported beam,
            // and how to run an alalysis with it in FEM-Design.
            // Before running, make sure you have a window with FEM-Design open.

            // This example was last updated using the ver. 21.6.0 FEM-Design API.

            #region Simple beam model
            // Define geometry
            var start = new Geometry.Point3d(2.0, 2.0, 0);
            var end = new Geometry.Point3d(10, 2.0, 0);
            var p3 = new Geometry.Point3d(4.0, 2.0, 0);
            var mid = start + (end - start) * 0.5;

            // Create elements
            var edge = new Geometry.Edge(start, end, Geometry.Vector3d.UnitZ);
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

            // Create supports
            var s1 = new Supports.PointSupport(
                point: start,
                motions: Releases.Motions.RigidPoint(),
                rotations: Releases.Rotations.RigidPoint()
                );

            var s2 = new Supports.PointSupport(
                point: end,
                motions: new Releases.Motions(yNeg: 1e10, yPos: 1e10, zNeg: 1e10, zPos: 1e10),
                rotations: Releases.Rotations.Free()
                );

            var s3 = new Supports.PointSupport(
                point: p3,
                motions: new Releases.Motions(yNeg: 1e10, yPos: 1e10, zNeg: 1e10, zPos: 1e10),
                rotations: Releases.Rotations.Free()
                );


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
            var pointMoment = new Loads.PointLoad(end, new Geometry.Vector3d(0.0, 5.0, 0.0), liveload, null, Loads.ForceLoadType.Moment);

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
            var elements = new List<GenericClasses.IStructureElement>() { bar, s1, s2 }; // We will add support s3 later in this example
            var model = new Model(Country.S, elements, loads, loadcases, loadCombinations);
            #endregion

            #region Analysis
            // Set up the analysis
            var analysis = Calculate.Analysis.StaticAnalysis();

            // Run a specific analysis
            List<Results.BarDisplacement> results1, results2;
            var config = Calculate.CmdGlobalCfg.Default();
            config.MeshElements.ElemSizeDiv = 10;
            var units = Results.UnitResults.Default();
            units.Displacement = Results.Displacement.mm;

            using (var femDesign = new FemDesignConnection())
            {
                femDesign.OnOutput += Console.WriteLine;

                // Update FEM-Design settings
                femDesign.SetGlobalConfig(config);

                // First we run the analysis of the first beam
                femDesign.OutputDir = "beam/";

                femDesign.Open(model);
                femDesign.RunAnalysis(analysis);
                results1 = femDesign.GetLoadCombinationResults<Results.BarDisplacement>(ULS, units);

                // Then we add the third support and run the analysis again. The files will be saved in a different output folder.
                model.AddElements(s3);
                femDesign.OutputDir = "beam 3 supports/";
                
                femDesign.Open(model);
                femDesign.RunAnalysis(analysis);
                results2 = femDesign.GetLoadCombinationResults<Results.BarDisplacement>(ULS, units);
            }
            #endregion

            #region Display results
            Console.WriteLine("Max bar displacement per case/comb:");

            Console.WriteLine();
            Console.WriteLine("Beam 1 (2 supports)");
            foreach (var group in results1.GroupBy(r => r.CaseIdentifier))
            {
                double min = group.Min(r => r.Ez);
                Console.WriteLine($"{group.Key}: {min:0.000}{units.Displacement}");
            }

            Console.WriteLine();
            Console.WriteLine("Beam 2 (3 supports)");
            foreach(var group in results2.GroupBy(r => r.CaseIdentifier))
            {
                double min = group.Min(r => r.Ez);
                Console.WriteLine($"{group.Key}: {min:0.000}{units.Displacement}");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            #endregion
        }
    }
}