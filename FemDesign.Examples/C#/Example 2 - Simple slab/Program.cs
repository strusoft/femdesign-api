using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;
using FemDesign.Calculate;
using FemDesign.GenericClasses;
using FemDesign.Geometry;
using FemDesign.Loads;
using FemDesign.Materials;
using FemDesign.ModellingTools;
using FemDesign.Shells;

namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main()
        {
            // EXAMPLE 3b: CREATING A SIMPLE SLAB
            // This example shows how to model a simple slab,
            // and how to run an analysis.

            // This example was last updated using the ver. 22.11.0 FEM-Design API.


            // Define geometry
            var anchorPoint = new Geometry.Point3d(0, 0, 0);
            var widthX = 6.00;
            var widthY = 6.00;
            var thickness = 0.30;

            //Define properties
            var materialDatabase = FemDesign.Materials.MaterialDatabase.GetDefault();
            var material = materialDatabase.MaterialByName("C25/30");

            // Define elements
            var slab = FemDesign.Shells.Slab.Plate(anchorPoint, widthX, widthY, thickness, material);

            var edge = slab.Region.Contours[0].Edges[0];
            var lineSupport = new FemDesign.Supports.LineSupport(edge, Releases.Motions.RigidLine(), Releases.Rotations.RigidLine(), false);

            var elements = new List<IStructureElement> { slab, lineSupport };
            

            // Define load cases
            var loadCaseDL = new LoadCase("DL", LoadCaseType.DeadLoad, LoadCaseDuration.Permanent);
            var loadCaseLL = new LoadCase("LL", LoadCaseType.Static, LoadCaseDuration.Permanent);
            var loadCases = new List<LoadCase> { loadCaseDL, loadCaseLL };

            // Define load combination
            var loadComb = new LoadCombination("ULS_1", LoadCombType.UltimateOrdinary, (loadCaseDL, 1.35), (loadCaseLL, 1.5));


            // Define loads
            var pos = new Point3d(2, 2, 0);
            var force = new Vector3d(0, 0, -50);
            var pointLoad = new FemDesign.Loads.PointLoad(pos, force, loadCaseLL, "", ForceLoadType.Moment);

            var loads = new List<ILoadElement> { pointLoad};
            

            // Assemble the model
            var model = new Model(Country.S);
            model.AddElements(elements);
            model.AddLoads(loads);
            model.AddLoadCases(loadCases);
            model.AddLoadCombinations(loadComb);


            // Define the analysis settings
            var analysis = Analysis.StaticAnalysis(calcCase: true, calccomb: true);

            // create a direct link to FEM-Design and comunicate with it
            using (var femDesign = new FemDesign.FemDesignConnection())
            {
                // Inside the "using..." we can send commands to FEM-Design.
                femDesign.Open(model);
                femDesign.RunAnalysis(analysis);

                // Read results
                var lineSupportReactions = femDesign.GetResults<Results.LineSupportResultant>();

                // Print results
                Console.WriteLine();
                Console.WriteLine("Id         | Reaction   | Case Identifier");
                foreach (var reaction in lineSupportReactions)
                {
                    Console.WriteLine($"{reaction.Id,10} | {reaction.Fy,10} | {reaction.CaseIdentifier,10}");
                }
            }
        }
    }
}
