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

            // This example was last updated 2022-04-27, using the ver. 21.1.0 FEM-Design API.

            Model model = new Model(Country.S);

            // Define geometry
            var p1 = new Geometry.FdPoint3d(0.0, 0.0, 0);
            var p2 = new Geometry.FdPoint3d(5.0, 0.0, 0);
            var p3 = new Geometry.FdPoint3d(2.5, 2.5, 0);

            // Create elements
            var edge1 = new Geometry.Edge(p1, p2, Geometry.FdVector3d.UnitZ());
            var edge2 = new Geometry.Edge(p2, p3, Geometry.FdVector3d.UnitZ());
            var edge3 = new Geometry.Edge(p3, p1, Geometry.FdVector3d.UnitZ());

            Materials.MaterialDatabase materialsDB = Materials.MaterialDatabase.DeserializeStruxml("materials.struxml");
            Sections.SectionDatabase sectionsDB = Sections.SectionDatabase.DeserializeStruxml("sections.struxml");

            var material = materialsDB.MaterialByName("C35/45");
            var section = sectionsDB.SectionByName("Concrete sections, Rectangle, 300x900");

            var bar1 = new Bars.Bar(
                edge1,
                Bars.BarType.Beam,
                material,
                sections: new Sections.Section[] { section },
                connectivities: new Bars.Connectivity[] { Bars.Connectivity.GetRigid() },
                eccentricities: new Bars.Eccentricity[] { Bars.Eccentricity.GetDefault() },
                identifier: "B");
            //bar1.BarPart.LocalY = Geometry.FdVector3d.UnitY();

            var bar2 = new Bars.Bar(
                edge2,
                Bars.BarType.Beam,
                material,
                sections: new Sections.Section[] { section },
                connectivities: new Bars.Connectivity[] { Bars.Connectivity.GetRigid() },
                eccentricities: new Bars.Eccentricity[] { Bars.Eccentricity.GetDefault() },
                identifier: "B");
            //bar2.BarPart.LocalY = Geometry.FdVector3d.UnitX();

            var bar3 = new Bars.Bar(
                edge3,
                Bars.BarType.Beam,
                material,
                sections: new Sections.Section[] { section },
                connectivities: new Bars.Connectivity[] { Bars.Connectivity.GetRigid() },
                eccentricities: new Bars.Eccentricity[] { Bars.Eccentricity.GetDefault() },
                identifier: "B");
            //bar3.BarPart.LocalY = Geometry.FdVector3d.UnitX();



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
            var windCase = new Loads.LoadCase("WIND", Loads.LoadCaseType.DeadLoad, Loads.LoadCaseDuration.Permanent);
            var loadcases = new List<Loads.LoadCase>() { deadLoadCase, windCase };


            // Create load combinations
            var slsFactors = new List<double>() { 1.0, 1.0 };
            var SLS = new Loads.LoadCombination("SLS", Loads.LoadCombType.ServiceabilityCharacteristic, loadcases, slsFactors);
            var ulsFactors = new List<double>() { 1.35, 1.5 };
            var ULS = new Loads.LoadCombination("ULS", Loads.LoadCombType.UltimateOrdinary, loadcases, ulsFactors);
            var loadCombinations = new List<Loads.LoadCombination>() { SLS, ULS };


            // Create loads
            var pointMoment = new Loads.PointLoad(p2, new Geometry.FdVector3d(0.0, 5.0, 0.0), deadLoadCase, null, Loads.ForceLoadType.Moment);

            var lineLoadStart = new Geometry.FdVector3d(0.0, 0.0, -2.0);
            var lineLoadEnd = new Geometry.FdVector3d(0.0, 0.0, -4.0);
            var lineLoad = new Loads.LineLoad(edge2, lineLoadStart, lineLoadEnd, windCase, Loads.ForceLoadType.Force, "", constLoadDir: true, loadProjection: true);

            var loads = new List<GenericClasses.ILoadElement>() { pointMoment, lineLoad};


            // Group Element by Stage
            // Not all the element are implemented yet
            var elementsStageOne = new List<IStageElement>() { s1, s2, bar1 };
            var elementsStageTwo = new List<IStageElement>() { s3, bar2 };
            var elementsStageThree = new List<IStageElement>() { bar3 };


            //
            var deadActiveLoadCase = new ActivatedLoadCase(deadLoadCase, 1.0, PartitioningType.only_in_this_stage);
            var stage1 = new Stage(1, "STAGE_1", deadActiveLoadCase, elementsStageOne);

            var windActiveLoadCase = new ActivatedLoadCase(windCase, 1.0, PartitioningType.only_in_this_stage);
            var stage2 = new Stage(2, "STAGE_2", windActiveLoadCase, elementsStageTwo);

            var stage3 = new Stage(3, "STAGE_3", null, elementsStageThree);
            var stage4 = new Stage(3, "STAGE_4", null, null);
            var stage5 = new Stage(3, "STAGE_5", null, null);

            var stages = new List<Stage>() { stage1, stage2, stage3, stage4, stage5 };

            var constructionStages = new ConstructionStages(
                stages,
                assignModifedElement: false,
                assignNewElement: false,
                ghostMethod: false);


            model.AddElements(elements);
            model.AddSupports(supports);
            model.AddLoadCases(loadcases);
            model.AddLoadCombinations(loadCombinations);
            model.AddLoads(loads);
            model.AddConstructionStages(constructionStages);

            //Console.Write(model.SerializeToString());

            // Set up the analysis
            //var analysisType = Calculate.Analysis.Eigenfrequencies();
            //var analysisType = Calculate.Analysis.StaticAnalysis();
            var analysisType = Calculate.Analysis.ConstructionStages();

            // Optional Settings for the Discretisation
            var config = Calculate.CmdGlobalCfg.Default();
            config.MeshElements.DefaultDivision = 5;


            // Define Result to be extract
            var results = new List<Results.ResultType>() { Results.ResultType.NodalDisplacement };

            // Run a specific analysis
            model.RunAnalysis(analysisType, resultTypes: results, cmdGlobalCfg: config);
        }
    }
}
