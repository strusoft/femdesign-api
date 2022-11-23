using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;
using FemDesign.GenericClasses;

namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // EXAMPLE 10: CREATING A CONSTRUCTION STAGE MODEL
            // This example will show you how to set up a construction stage model
            // and how to save it for export to FEM-Design. Before running,
            // make sure you have a window with FEM-Design open.

            // This example was last updated using the ver. 21.6.0 FEM-Design API.

            // Define geometry
            var p1 = new Geometry.Point3d(0.0, 0.0, 0);
            var p2 = new Geometry.Point3d(5.0, 0.0, 0);
            var p3 = new Geometry.Point3d(2.5, 2.5, 0);

            // Create elements
            var edge1 = new Geometry.Edge(p1, p2, Geometry.Vector3d.UnitZ);
            var edge2 = new Geometry.Edge(p2, p3, Geometry.Vector3d.UnitZ);
            var edge3 = new Geometry.Edge(p3, p1, Geometry.Vector3d.UnitZ);

            Materials.MaterialDatabase materialsDB = Materials.MaterialDatabase.DeserializeStruxml("materials.struxml");
            Sections.SectionDatabase sectionsDB = Sections.SectionDatabase.DeserializeStruxml("sections.struxml");

            var material = materialsDB.MaterialByName("C35/45");
            var section = sectionsDB.SectionByName("Concrete sections, Rectangle, 300x900");

            var bar1 = new Bars.Beam(
                edge1,
                material,
                sections: new Sections.Section[] { section },
                connectivities: new Bars.Connectivity[] { Bars.Connectivity.Rigid },
                eccentricities: new Bars.Eccentricity[] { Bars.Eccentricity.Default },
                identifier: "B");

            var bar2 = new Bars.Beam(
                edge2,
                material,
                sections: new Sections.Section[] { section },
                connectivities: new Bars.Connectivity[] { Bars.Connectivity.Rigid },
                eccentricities: new Bars.Eccentricity[] { Bars.Eccentricity.Default },
                identifier: "B");

            var bar3 = new Bars.Beam(
                edge3,
                material,
                sections: new Sections.Section[] { section },
                connectivities: new Bars.Connectivity[] { Bars.Connectivity.Rigid },
                eccentricities: new Bars.Eccentricity[] { Bars.Eccentricity.Default },
                identifier: "B");

            var elements = new List<GenericClasses.IStructureElement>() { bar1, bar2, bar3 };

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

            var s3 = new Supports.PointSupport(
                point: p3,
                motions: new Releases.Motions(yNeg: 1e10, yPos: 1e10, zNeg: 1e10, zPos: 1e10),
                rotations: Releases.Rotations.Free()
                );

            var supports = new List<GenericClasses.ISupportElement>() { s1, s2, s3 };

            // Create Load Cases
            var deadLoadCase = new Loads.LoadCase("DL", Loads.LoadCaseType.DeadLoad, Loads.LoadCaseDuration.Permanent);
            var windCase = new Loads.LoadCase("WIND", Loads.LoadCaseType.Static, Loads.LoadCaseDuration.Permanent);
            var imposedCase = new Loads.LoadCase("IMPOSED", Loads.LoadCaseType.Static, Loads.LoadCaseDuration.Permanent);
            var loadcases = new List<Loads.LoadCase>() { deadLoadCase, windCase, imposedCase };

            // Create loads
            var pointMoment = new Loads.PointLoad(p2, new Geometry.Vector3d(0.0, 5.0, 0.0), deadLoadCase, null, Loads.ForceLoadType.Moment);

            var lineLoadStart = new Geometry.Vector3d(0.0, 0.0, -2.0);
            var lineLoadEnd = new Geometry.Vector3d(0.0, 0.0, -4.0);
            var lineLoad = new Loads.LineLoad(edge2, lineLoadStart, lineLoadEnd, windCase, Loads.ForceLoadType.Force, "", constLoadDir: true, loadProjection: true);

            var loads = new List<GenericClasses.ILoadElement>() { pointMoment, lineLoad };

            // Elements per stage, please not that most structure elements are also
            // an IStageElement and can be added to a specific stage.
            var elementsStage1 = new List<IStageElement>() { s1, s2, bar1 };
            var elementsStage2 = new List<IStageElement>() { s3, bar2 };
            var elementsStage3 = new List<IStageElement>() { bar3 };

            // Load cases per stage
            var stageOneLoadCases = new List<ActivatedLoadCase>() {
                new ActivatedLoadCase(deadLoadCase, 1.0, ActivationType.FromThisStageOn)
            };
            var stageTwoLoadCases = new List<ActivatedLoadCase>() {
                new ActivatedLoadCase(windCase, 1.0, ActivationType.OnlyInThisStage),
                new ActivatedLoadCase(imposedCase, 1.0, ActivationType.FromThisStageOn)
            };

            // Create the stages
            var stage1 = new Stage(1, "STAGE_1", stageOneLoadCases, elementsStage1);
            var stage2 = new Stage(2, "STAGE_2", stageTwoLoadCases, elementsStage2);
            var stage3 = new Stage(3, "STAGE_3", elements: elementsStage3);
            var stage4 = new Stage(4, "STAGE_4");

            var stages = new List<Stage>() { stage1, stage2, stage3, stage4 };


            // Create load combinations
            var SLS = new Loads.LoadCombination("SLS", Loads.LoadCombType.ServiceabilityCharacteristic,
                (deadLoadCase, 1.0),
                (windCase, 1.0),
                (imposedCase, 1.0));
            var ULS = new Loads.LoadCombination("ULS", Loads.LoadCombType.UltimateOrdinary,
                (deadLoadCase, 1.35),
                (windCase, 1.5),
                (imposedCase, 1.2));
            var CSCombination = new Loads.LoadCombination("Stage combination", Loads.LoadCombType.UltimateOrdinary,
                (deadLoadCase, 1.35),
                (windCase, 1.5),
                (imposedCase, 1.2));
            CSCombination.SetStageLoadCase(stage1, 1.0); // A single construction stage may be added to the load combination

            var loadCombinations = new List<Loads.LoadCombination>() { SLS, ULS, CSCombination };

            // Create the model and set the construction stages
            Model model = new Model(Country.S);
            model.AddElements(elements);
            model.AddSupports(supports);
            model.AddLoadCases(loadcases);
            model.AddLoadCombinations(loadCombinations);
            model.AddLoads(loads);
            model.SetConstructionStages(stages,
                assignModifedElement: false,
                assignNewElement: false,
                ghostMethod: false);

            // Set up the analysis
            var constructionStageAnalysis = Calculate.Analysis.ConstructionStages(ghost: false);
            var units = Results.UnitResults.Default();
            units.Displacement = Results.Displacement.mm;

            using (var femDesign = new FemDesign.FemDesignConnection(outputDir: "beam"))
            {
                femDesign.Open(model);
                femDesign.RunAnalysis(constructionStageAnalysis);

                var displacements = femDesign.GetResults<Results.BarDisplacement>();

                double maxValue = displacements.Min(r => r.Ez); // Largest negative displacement
                var maxDisplacement = displacements.Find(r => r.Ez == maxValue);
                Console.WriteLine($"Max displacement: {displacements.Max(r => -r.Ez)}{units.Displacement} in '{maxDisplacement.CaseIdentifier}'");

                // ENDING THE PROGRAM
                Console.WriteLine("\nPress any key to close console...");
                Console.ReadKey();
            }
        }
    }
}
