using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;
using FemDesign.Results;

namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main()
        {
            // EXAMPLE 1: CREATING A SIMPLE BEAM
            // This example will show you how to model a simple supported beam,
            // and how to save it for export to FEM-Design. Before running,
            // make sure you have a window with FEM-Design open.

            // This example was last updated using the ver. 21.6.0 FEM-Design API.


            // Define geometry
            var length = 6.00;

            var p1 = new Geometry.Point3d(0.0, 0.0, 0.0);
            var p2 = new Geometry.Point3d(length, 0.0, 0.0);
            var mid = p1 + (p2 - p1) * 0.5;

            // Create Line Geometry
            var edge = new Geometry.LineEdge(p1, p2);

            // Create Material
            var materialsDB = Materials.MaterialDatabase.DeserializeStruxml("materials.struxml");

            var materialName = "C35/45";
            var material = materialsDB.MaterialByName(materialName);

            // Create Section
            var sectionsDB = Sections.SectionDatabase.DeserializeStruxml("sections.struxml");

            var sectionName = "Concrete sections, Rectangle, 300x900";
            var section = sectionsDB.SectionByName(sectionName);

            var bar = new Bars.Beam(
                edge,
                material,
                section);


            // Create supports
            var stiffValue = 1e10;
            var motion = new FemDesign.Releases.Motions(stiffValue, stiffValue, stiffValue, stiffValue, stiffValue, stiffValue);
            var rotation = new FemDesign.Releases.Rotations(stiffValue, stiffValue, stiffValue, stiffValue, stiffValue, stiffValue);

            var s1 = new Supports.PointSupport(p1, motion, rotation);
            var s2 = Supports.PointSupport.Hinged(p2);

            // Create a List of Structural Elements
            var elements = new List<GenericClasses.IStructureElement>() { bar, s1, s2 };


            // Create load cases
            var deadload = new Loads.LoadCase("Deadload", Loads.LoadCaseType.DeadLoad, Loads.LoadCaseDuration.Permanent);
            var liveload = new Loads.LoadCase("Liveload", Loads.LoadCaseType.Static, Loads.LoadCaseDuration.Permanent);
            var stressload = new Loads.LoadCase("Stresses", Loads.LoadCaseType.Static, Loads.LoadCaseDuration.Permanent);
            var loadcases = new List<Loads.LoadCase>() { deadload, liveload, stressload };


            // Create load combinations
            var SLS = new Loads.LoadCombination("SLS", Loads.LoadCombType.ServiceabilityCharacteristic,
                (deadload, 1.0),
                (liveload, 1.0));
            var ULS = new Loads.LoadCombination("ULS", Loads.LoadCombType.UltimateOrdinary,
                (deadload, 1.2),
                (liveload, 1.5));
            var loadCombinations = new List<Loads.LoadCombination>() { SLS, ULS };


            // Create loads
            var pointForce = Loads.PointLoad.Force(mid, new Geometry.Vector3d(0.0, 0.0, -5.0), liveload);
            var pointMoment = Loads.PointLoad.Moment(p2, new Geometry.Vector3d(0.0, 5.0, 0.0), liveload);

            var lineLoadStart = new Geometry.Vector3d(0.0, 0.0, -2.0);
            var lineLoadEnd = new Geometry.Vector3d(0.0, 0.0, -4.0);
            var lineLoad = Loads.LineLoad.VariableForce(edge, lineLoadStart, lineLoadEnd, liveload);

            var lineStress = new Loads.LineStressLoad(edge, 10, stressload);

            var loads = new List<GenericClasses.ILoadElement>() {
                pointForce,
                pointMoment,
                lineLoad,
                lineStress
            };


            // Add to model
            Model model = new Model(Country.S);
            model.AddElements(elements);
            model.AddLoadCases(loadcases);
            model.AddLoadCombinations(loadCombinations);
            model.AddLoads(loads);

            // Create Analysis
            var analysis = Calculate.Analysis.StaticAnalysis();

            // Run Analysis
            using (var femDesign = new FemDesignConnection(outputDir: "model"))
            {
                femDesign.RunAnalysis(model, analysis);
                var pointSupportReactions = femDesign.GetResults<PointSupportReaction>();

                // Print results
                Console.WriteLine();
                Console.WriteLine("Id         | Reaction  ");
                foreach (var reaction in pointSupportReactions)
                {
                    Console.WriteLine($"{reaction.Id,10} | {reaction.Fz,10} kN | {reaction.CaseIdentifier,10}");
                }
            }

            // ENDING THE PROGRAM
            Console.WriteLine("\nPress any key to close console.");
            Console.ReadKey();
        }
    }
}