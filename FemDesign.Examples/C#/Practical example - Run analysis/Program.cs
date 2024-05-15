using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;
using FemDesign.Results;
using FemDesign.Calculate;
using System.Linq.Expressions;


namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main()
        {
            // EXAMPLE 3: Run several analysis on a model
            // This example demonstrates how to run several analysis on a model. The example runs a frequency analysis, a stability analysis and a generic analysis which can be set up
            // with different settings specified in the Calculate.CombItem class.

            // This example was last updated using the ver. 23.2.0 FEM-Design API.

            string filePath = Path.GetFullPath("model.struxml");

            // Define freq analysis settings
            var freqSettings = new Calculate.Freq
            {
                NumShapes = 3,
                AutoIter = 0,
                ShapeNormalization = ShapeNormalisation.Unit,
                X = true,
                Y = true,
                Z = true,
            };

            // Define stability analysis settings
            var stabilitySettings = new Calculate.Stability
            {
                CombNames = new List<string> { "WIND LEAD" },
                NumShapes = new List<int> { 3 },
            };


            // ADVANCE ANALYSIS SETTINGS
            // Comb Item needs to follow the same order as the load combinations
            // If you have 3 load combinations, you must have 3 CombItems

            var combItem_1 = new Calculate.CombItem
            {
                ImpfRqd = 0,
                StabRqd = 0,
                Calc = true,
                NLE = true,
                PL = true,
                NLS = false,
                Cr = false,
                f2nd = false,
                Im = 0,
                Waterlevel = 0,
            };
            var combItem_2 = new Calculate.CombItem
            {
                ImpfRqd = 0,
                StabRqd = 0,
                Calc = true,
                NLE = false,
                PL = false,
                NLS = false,
                Cr = false,
                f2nd = false,
                Im = 0,
                Waterlevel = 0,
            };
            var combItem_3 = new Calculate.CombItem
            {
                ImpfRqd = 0,
                StabRqd = 0,
                Calc = true,
                NLE = false,
                PL = false,
                NLS = false,
                Cr = false,
                f2nd = true,
                Im = 0,
                Waterlevel = 0,
            };

            var combItems = new List<Calculate.CombItem>
            {
                combItem_1,
                combItem_2,
                combItem_3,
            };
            var comb = new Calculate.Comb
            {
                NLEmaxiter = 30,
                PLdefloadstep = 20,
                PLminloadstep = 2,
                PlKeepLoadStep = true,
                PlTolerance = 1,
                PLmaxeqiter = 50,
                PlShellLayers = 10,
                NLSMohr = true,
                NLSinitloadstep = 10,
                NLSminloadstep = 10,
                NLSactiveelemratio = 5,
                NLSplasticelemratio = 5,
                CRloadstep = 20,
                CRmaxiter = 30,
                CRstifferror = 2,
                CombItem = combItems,
            };

            // ADVANCE ANALYSIS SETTINGS USING LOAD COMBINATION NAMES
            // In this case we are only interested in setting up the analysis for one load combination
            // The other load combination will have a default setting
            var combItem = new Calculate.CombItem
            {
                CombName = "ULS VERTICAL",
                ImpfRqd = 0,
                StabRqd = 0,
                Calc = true,
                NLE = false,
                PL = false,
                NLS = false,
                Cr = false,
                f2nd = false,
                Im = 0,
                Waterlevel = 0,
            };


            var combItemsWithName = new List<Calculate.CombItem>
            {
                combItem,
            };

            var combWithName = new Calculate.Comb
            {
                NLEmaxiter = 30,
                PLdefloadstep = 20,
                PLminloadstep = 2,
                PlKeepLoadStep = true,
                PlTolerance = 1,
                PLmaxeqiter = 50,
                PlShellLayers = 10,
                NLSMohr = true,
                NLSinitloadstep = 10,
                NLSminloadstep = 10,
                NLSactiveelemratio = 5,
                NLSplasticelemratio = 5,
                CRloadstep = 20,
                CRmaxiter = 30,
                CRstifferror = 2,
                CombItem = combItemsWithName,
            };

            // DEFAULT SETTINGS
            var combDefault = Calculate.Comb.Default();


            Analysis analysis;
            using (var connection = new FemDesignConnection(keepOpen: true))
            {
                connection.Open(filePath);

                analysis = new Analysis(comb: comb, calcCase: true, calcComb: true);
                connection.RunAnalysis(analysis);

                analysis = new Analysis(comb: combWithName, calcCase: true, calcComb: true);
                connection.RunAnalysis(analysis);

                analysis = new Analysis(comb: combDefault, calcCase: true, calcComb: true);
                connection.RunAnalysis(analysis);

                analysis = new Analysis(freq: freqSettings);
                connection.RunAnalysis(analysis);

                analysis = new Analysis(stability: stabilitySettings);
                connection.RunAnalysis(analysis);
            }
        }
    }
}