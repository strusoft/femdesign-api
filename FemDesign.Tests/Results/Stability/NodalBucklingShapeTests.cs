using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;

namespace FemDesign.Results
{
    [TestClass]
    public class NodalBucklingShapeTest
    {
        [TestMethod]
        public void ReadBucklingData()
        {
            // LOADING UP THE MODEL
            string struxmlPath = @"D:\Andi\API_Work\719_BucklingShape\Test\SimpleFrame_Structure.struxml";
            Model model = Model.DeserializeFromFilePath(struxmlPath);


            // CHOOSING WHAT ANALYSIS TO RUN
            FemDesign.Calculate.Analysis analysis = new FemDesign.Calculate.Analysis(
                calcCase: true,
                calcComb: true,
                calcStab: true
                );


            // SETUP BY LOAD CALCULATION SETTINGS
            // These settings are found in the FEM-Design calculation window.
            bool NLE = true;
            bool PL = true;
            bool NLS = false;
            bool Cr = false;
            bool _2nd = false;


            // SETTING UP LOAD COMBINATIONS
            // In this example, we use the same settings (CombItem)
            // for all load combinations, applied with a simple loop.
            var combItem = new FemDesign.Calculate.CombItem(0, 10, NLE, PL, NLS, Cr, _2nd);
            model.Entities.Loads.LoadCombinations.ForEach(lComb =>
            {
                lComb.CombItem = combItem;
            });

            analysis.SetLoadCombinationCalculationParameters(model);


            // RUN THE ANALYSIS
            using (var femDesign = new FemDesignConnection(outputDir: "My analyzed model", keepOpen: false))
            {
                // Run analysis and read buckling data
                femDesign.RunAnalysis(model, analysis);
                var resultsBuckling = femDesign.GetResults<Results.NodalBucklingShape>();

                Assert.IsNull(resultsBuckling);
            }
        }
    }
}