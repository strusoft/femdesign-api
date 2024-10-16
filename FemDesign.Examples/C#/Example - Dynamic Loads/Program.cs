using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;
using FemDesign.Bars;
using FemDesign.Calculate;
using FemDesign.GenericClasses;
using FemDesign.Geometry;
using FemDesign.Loads;
using FemDesign.Materials;
using FemDesign.ModellingTools;
using FemDesign.Sections;
using FemDesign.Shells;
using FemDesign.Utils;
using static FemDesign.Loads.PeriodicCase;

namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main()
        {
            // The example demonstrates how to create a model with dynamic loads in FemDesign.

            var model = new Model(Country.COMMON);


            // create a steel beam long 6 meters with pinned supports at the end using FemDesign
            var startPoint = new Point3d(0, 0, 0);
            var endPoint = new Point3d(0, 6, 0);
            var edge = new Edge(startPoint, endPoint);

            var steel = MaterialDatabase.GetDefault().Materials.Material.MaterialByName("S355");
            var section = SectionDatabase.GetDefault().Sections.Section.SectionByName("HEA300");

            var beam = new Beam(edge, steel, section);

            var supportStart = Supports.PointSupport.Rigid(edge.Points[0]);
            var supportEnd = Supports.PointSupport.Hinged(edge.Points[1]);

            model.AddElements(beam);
            model.AddSupports(supportStart, supportEnd);

            // create load cases
            var dl = new LoadCase("DL", LoadCaseType.DeadLoad, LoadCaseDuration.Permanent);
            var ll = new LoadCase("LL", LoadCaseType.Static, LoadCaseDuration.Permanent);

            model.AddLoadCases(dl, ll);

            // create load combinations
            var loadCombination = new LoadCombination("combo", LoadCombType.UltimateOrdinary, (dl, 1.35), (ll, 1.0));

            model.AddLoadCombinations(loadCombination);

            // create a point load in the middle
            var middlePoint = (edge.Points[0] + edge.Points[1]) / 2;
            var pointLoad = PointLoad.Force(middlePoint, new Vector3d(0, 0, -10), ll);

            model.AddLoads(pointLoad);

            // create an excitation force
            // The constructor requires a List<Diagram> and List<ExcitationForceCombination>.
            // public ExcitationForce(List<Diagram> diagrams, List<ExcitationForceCombination> combinations)

            var diagram1 = new Diagram("diagram1", new List<double> { 0.0, 1, 2}, new List<double> { 1, 0.1, 0.3});
            var diagram2 = new Diagram("diagram2", new List<double> { 0.0, 0.1, 0.2}, new List<double> { 1, 0.1, 0.3});

            var exLdCase1 = new ExcitationForceLoadCase(dl, 1, diagram1);
            var exLdCase2 = new ExcitationForceLoadCase(ll, 2, diagram2);

            var combination1 = new ExcitationForceCombination()
            {
                Name = "combo1",
                dT = 0.01,
                records = new List<ExcitationForceLoadCase> { exLdCase1 }
            };
            var combination2 = new ExcitationForceCombination()
            {
                Name = "combo2",
                dT = 0.02,
                records = new List<ExcitationForceLoadCase> { exLdCase1, exLdCase2 }
            };

            var excitationForce = new Loads.ExcitationForce()
            {
                Diagram = new List<Diagram> { diagram1, diagram2 },
                Combination = new List<ExcitationForceCombination> {  combination1, combination2},
            };

            model.AddLoads(excitationForce);


            // create a periodic load

            var periodicCase1 = new PeriodicCase(1, Shape.Cos, dl);
            var periodicCase2 = new PeriodicCase(5, Shape.Sin, dl);

            var periodicLoad1 = new PeriodicLoad("record1", 20, periodicCase1);
            var periodicLoad2 = new PeriodicLoad("record2", 10, periodicCase2);
            var periodicLoad3 = new PeriodicLoad("record3", 10, new List<PeriodicCase> { periodicCase1, periodicCase2} );


            model.AddLoads(periodicLoad1);
            model.AddLoads(periodicLoad2);
            model.AddLoads(periodicLoad3);

            // add mass definition

            var massLoad = new Loads.MassConversionTable( (1, dl), (1, ll) );
            model.AddLoads(massLoad);

            var periodicExcitation = Analysis.PeriodicExcitation();
            var excitation = Analysis.ExcitationForce();

            using (var femdesign = new FemDesignConnection(keepOpen: true))
            {
                femdesign.Open(model);
                femdesign.RunAnalysis(periodicExcitation);
                femdesign.RunAnalysis(excitation);
            }

        }
    }
}