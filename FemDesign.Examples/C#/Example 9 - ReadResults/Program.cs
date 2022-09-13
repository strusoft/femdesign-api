using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using FemDesign;

namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main()
        {
            // EXAMPLE 9: READ RESULTS
            // This example will show you how to model a simple supported beam,
            // and read some of the results.

            // This example was last updated 2022-07-08, using the ver. 21.3.0 FEM-Design API.

            #region DEFINE GEOMETRY
            // Define geometry
            var p1 = new Geometry.FdPoint3d(2.0, 2.0, 0);
            var p2 = new Geometry.FdPoint3d(10, 2.0, 0);
            var mid = p1 + (p2 - p1) * 0.5;


            // Create elements
            var edge = new Geometry.Edge(p1, p2, Geometry.FdVector3d.UnitZ);
            Materials.MaterialDatabase materialsDB = Materials.MaterialDatabase.DeserializeStruxml("materials.struxml");
            Sections.SectionDatabase sectionsDB = Sections.SectionDatabase.DeserializeStruxml("sections.struxml");

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
            bar.BarPart.LocalY = Geometry.FdVector3d.UnitY;
            var elements = new List<GenericClasses.IStructureElement>() { bar };
            #endregion

            #region DEFINE SUPPORTS
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
            #endregion

            #region DEFINE LOAD CASES/COMBINATIONS
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
            var pointForce = new Loads.PointLoad(mid, new Geometry.FdVector3d(0.0, 0.0, -5.0), liveload, null, Loads.ForceLoadType.Force);
            var pointMoment = new Loads.PointLoad(p2, new Geometry.FdVector3d(0.0, 5.0, 0.0), liveload, null, Loads.ForceLoadType.Moment);

            var lineLoadStart = new Geometry.FdVector3d(0.0, 0.0, -2.0);
            var lineLoadEnd = new Geometry.FdVector3d(0.0, 0.0, -4.0);
            var lineLoad = new Loads.LineLoad(edge, lineLoadStart, lineLoadEnd, liveload, Loads.ForceLoadType.Force, "", constLoadDir: true, loadProjection: true);

            var loads = new List<GenericClasses.ILoadElement>() {
                pointForce,
                pointMoment,
                lineLoad
            };
            #endregion

            #region ASSEMBLE
            // Add to model
            Model model = new Model(Country.S);
            model.AddElements(elements);
            model.AddSupports(supports);
            model.AddLoadCases(loadcases);
            model.AddLoadCombinations(loadCombinations);
            model.AddLoads(loads);
            #endregion

            #region SETTINGS

            // define the file name
            string fileName = "SimpleBeam.struxml";
            fileName = Path.GetFullPath(fileName);

            // Define the Units
            // it is an optional operation and it can be omitted
            // Default Units can be seen looking at FemDesign.Results.UnitResults.Default()

            var units = new FemDesign.Results.UnitResults(Results.Length.m, Results.Angle.deg, Results.SectionalData.mm, Results.Force.daN, Results.Mass.kg, Results.Displacement.cm, Results.Stress.MPa);

            // Select the results to extract
            var resultTypes = new List<Type>
            {
                typeof(Results.PointSupportReaction),
                typeof(Results.NodalDisplacement)
            };

            var bscPathsFromResultTypes = Calculate.Bsc.BscPathFromResultTypes(resultTypes, fileName, units);
            #endregion

            #region ANALYSIS
            // Running the analysis
            var analysisSettings = FemDesign.Calculate.Analysis.StaticAnalysis();

            model.SerializeModel(fileName);
            var fdScript = FemDesign.Calculate.FdScript.Analysis(fileName, analysisSettings, bscPathsFromResultTypes, null, true);

            var app = new FemDesign.Calculate.Application();
            app.RunFdScript(fdScript, false, true);
            model.SerializeModel(fileName);

            // Read model and results
            model = Model.DeserializeFromFilePath(fdScript.StruxmlPath);
            #endregion

            #region EXTRACT RESULTS

            IEnumerable<Results.IResult> results = Enumerable.Empty<Results.IResult>();
            
            foreach (var cmd in fdScript.CmdListGen)
            {
                string path = cmd.OutFile;
                var _results = Results.ResultsReader.Parse(path);
                results = results.Concat(_results);
            }
            #endregion

            #region DO SOMETHING WITH RESULTS
            // Display Results on Screen
            // The results are grouped by their type
            var resultGroups = results.GroupBy(t => t.GetType()).ToList();
            foreach(var resultGroup in resultGroups)
            {
                Console.WriteLine(resultGroup.Key.Name);
                Console.WriteLine();
                foreach (var result in resultGroup)
                {
                    Console.WriteLine(result);
                }
                Console.WriteLine();
                Console.WriteLine();
            }


            // Select a specific result
            Console.WriteLine("Vertical Reaction Forces");
            var zReactions = results.Where(t => t.GetType() == typeof(Results.PointSupportReaction)).Cast<Results.PointSupportReaction>();
            foreach(var zReaction in zReactions)
            {
                Console.WriteLine($"Node {zReaction.Id}: {zReaction.Fz} {units.Force}");
            }
            #endregion

            // ENDING THE PROGRAM
            Console.WriteLine("\nPress any key to close console.");
            Console.ReadKey();
        }
    }
}
