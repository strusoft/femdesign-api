using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FemDesign;
using System.Globalization;

namespace FemDesign.Samples
{
    public partial class SampleProgram
    {
        static void CostOptimizationOfSlab()
        {
            string path = @"C:\YOUR_FILE_PATH\femdesign-api\FemDesign.Samples\C#\ExampleModels\sample_optimization.struxml";
            string bscPathQEconcrete = @"C:\YOUR_FILE_PATH\femdesign-api\FemDesign.Samples\C#\ExampleModels\QEconcrete.bsc";
            string bscPathQEreinforcement = @"C:\YOUR_FILE_PATH\femdesign-api\FemDesign.Samples\C#\ExampleModels\QEreinforcement.bsc";
            string outFolder = @"C:\YOUR_FILE_PATH\femdesign-api\FemDesign.Samples\C#\ExampleModels\output\";
            string tempPath = outFolder + "temp.struxml";
            List<string> bscPaths = new List<string>();
            bscPaths.Add(bscPathQEconcrete);
            bscPaths.Add(bscPathQEreinforcement);

            FemDesign.Model model = Model.DeserializeFromFilePath(path);
            double concreteCost = 10;
            double reinforcementCost = 70;
            double concreteWeight = 0;
            double reinforcementWeight = 0;



            //Read slab (hard coded in this example, probably better to look for a slab with a certain name, eg. P.1)
            FemDesign.Shells.Slab slab = model.Entities.Slabs[0];


            //Sets up what type of analysis should be done
            #region Analysis Setup
            // Setup for calculation of load combinations
            bool NLE = true;
            bool PL = false;
            bool NLS = false;
            bool Cr = false;
            bool _2nd = false;

            // Skapar inställningar för analysen
            var combItem = new FemDesign.Calculate.CombItem(0, 0, NLE, PL, NLS, Cr, _2nd);

            int numLoadCombs = model.Entities.Loads.LoadCombinations.Count;
            var combItems = new List<FemDesign.Calculate.CombItem>();
            for (int i = 0; i < numLoadCombs; i++)
            {
                combItems.Add(combItem);
            }

            // Behövs för datastrukturens skull
            FemDesign.Calculate.Comb comb = new FemDesign.Calculate.Comb();
            comb.CombItem = combItems.ToList();

            #endregion


            //Loop variables
            double low = 0.2;
            double high = 1;


            List<double> costs = new List<double>();

            for (double i = low; i < high; i = i + 0.05)
            {
                //Set thickness of slab
                double thickness = i;
                slab.SlabPart.Thickness[0].Value = thickness;

                //Save temporary model
                model.SerializeModel(tempPath);

                //Run analysis and get results
                RunAnalysis(tempPath, bscPaths);
                concreteWeight = ConcreteWeight();
                reinforcementWeight = ReinforcementWeight();

                //Calculate cost, write to console app and write to list
                double totalCost = concreteCost * concreteWeight + reinforcementCost * reinforcementWeight;
                Console.WriteLine(string.Format("{0} {1} {2}", "Cost: ", totalCost, thickness + "m"));
                costs.Add(totalCost);
            }



        }

        public static void RunAnalysis(string modelPath, List<string> bscFilePaths)
        {
            FemDesign.Calculate.Analysis analysis = new FemDesign.Calculate.Analysis(null, null, null, null, true, false, false, false, false, false, false, false, false, false, false, false, false);
            FemDesign.Calculate.Design design = new FemDesign.Calculate.Design(true, false);
            FemDesign.Calculate.FdScript fdScript = FemDesign.Calculate.FdScript.Design("rc", modelPath, analysis, design, bscFilePaths, "", true);
            FemDesign.Calculate.Application app = new FemDesign.Calculate.Application();
            app.RunFdScript(fdScript, false, true, true);
        }

        public static double ConcreteWeight()
        {
            //Read results from csv file
            double concreteWeight = 0;
            int counter = 0;
            using (var reader = new StreamReader(@"C:\YOUR_FILE_PATH\femdesign-api\FemDesign.Samples\C#\ExampleModels\output\QEconcrete.csv"))
            {

                //Console.WriteLine("");
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split("\t");
                    if (values[0] == "TOTAL" & line != "")
                    {
                        //Console.WriteLine(string.Format("{0} {1} {2}", values[0], "concrete", values[10]));
                        concreteWeight = double.Parse(values[10], CultureInfo.InvariantCulture);
                    }
                    counter++;
                }
            }

            return concreteWeight;
        }

        public static double ReinforcementWeight()
        {
            //Read results from csv file
            double reinforcementWeight = 0;
            int counter = 0;
            using (var reader = new StreamReader(@"C:\YOUR_FILE_PATH\femdesign-api\FemDesign.Samples\C#\ExampleModels\output\QEreinforcement.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split("\t");
                    if (values[0] == "TOTAL" & line != "")
                    {
                        //Console.WriteLine(string.Format("{0} {1} {2}", values[0], "reinforcement", values[5]));
                        reinforcementWeight = double.Parse(values[5], CultureInfo.InvariantCulture);
                    }
                    counter++;
                }
            }
            return reinforcementWeight;
        }
    }
}
