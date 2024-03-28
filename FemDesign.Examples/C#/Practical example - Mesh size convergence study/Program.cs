using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;
using FemDesign.AuxiliaryResults;
using FemDesign.Results;

namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main()
        {
            // PRACTICAL EXAMPLE: MESH SIZE CONVERGENCE STUDY
            // In this example, we will analyse the mesh size convergence for a model, to allow for 
            // an optimal analysis setting. The program will output files to \bin\Debug\Output.

            // This example was last updated 2022-04-27, using the ver. 21.1.0 FEM-Design API.


            // INPUTS:
            // All inputs used in the program.
            string modelPath = "Model with labelled sections.struxml";
            double maxAllowedDeviation = 0.02;
            string labeledSectionIdentifier = "LS.1";
            List<double> meshSizes = new List<double> { 2.0, 1.0, 0.5, 0.25, 0.15, 0.10 };
            int forceIdIndex = 2;
            // The force index stem from the .bsc file made from FEM-Design, where they are ordered like this:
            // ID		Mx' 	My'	    Mx'y' 	Nx' 	Ny' 	Nx'y'	Tx'z' 	Ty'z'	Comb.
            // Index    [2]     [3]     [4]     [5]     [6]     [7]     [8]     [9]     [10]


            // PREPARATIONS:
            // Some setup necessary to run the analysis and save the results.

            string fileName = Path.GetFileName(modelPath);
            FemDesign.Calculate.Analysis analysisSettings = FemDesign.Calculate.Analysis.StaticAnalysis();
            Model model = Model.DeserializeFromFilePath(modelPath);

            Dictionary<double, List<double>> allForces = new Dictionary<double, List<double>>() {};


            using (var connection = new FemDesign.FemDesignConnection())
            {

                // RUNNING THE ANALYSIS FOR EACH MESH SIZE
                foreach (double size in meshSizes)
                {
                    model.Entities.Slabs[0].SlabPart.MeshSize = size;

                    // Serializing new model
                    string currentPath = Path.GetFullPath(Path.Combine(fileName.Replace(".struxml", $"_{size.ToString(System.Globalization.CultureInfo.InvariantCulture)}.struxml")));
                    model.SerializeModel(currentPath);

                    // Running the analysis

                    connection.Open(model);
                    connection.RunAnalysis(analysisSettings);
                    

                    var forceResults = connection.GetLoadCombinationResults<LabelledSectionInternalForce>();

                    // Preprarations
                    List<double> currentForces = forceResults.Select(x => x.Tyz).ToList();
                    allForces.Add(size, currentForces);
                }
            }

            // PREPARATIONS
            // Variables needed for the convergence calculation.
            int counter = 0;
            string critMet = "false";
            List<double> totalForce = new List<double>();
            List<double> previousForces = new List<double>();
            List<double> totalDeviation = new List<double>();
            List<double> deviationCalculation = new List<double>();
            List<double> deviationPercentage = new List<double>();

            // Results header
            Console.WriteLine("\nRESULTS:");
            Console.WriteLine($"{"Mesh size", 10} | {"Mx' tot.",10} | {"Dev.tot.",10} | {"Dev %",10} | Meets crit.");
            Console.WriteLine($"{"[m]",10} | {"[kNm/m]",10} | {"[kNm/m]",10} | {"[-]",10} | {"[-]",10}");


            // CONVERGENCE CALCULATION LOOP
            foreach (var forcesList in allForces)
            {
                // Calculate deviation between last and current forces
                if (counter > 0)
                {
                    for (int i = 0; i < forcesList.Value.Count; i++)
                    {
                        deviationCalculation.Add(Math.Abs(forcesList.Value[i]-previousForces[i]));
                    }
                    totalDeviation.Add(deviationCalculation.Sum());
                    deviationCalculation.Clear();

                }
                else
                {
                    totalDeviation.Add(0.000);
                }

                // Calculate the relation between the force and deviation totals
                totalForce.Add(forcesList.Value.Sum());
                deviationPercentage.Add(totalDeviation[counter] / totalForce[counter]);

                // Check if our criteria is met
                if (counter > 0 & deviationPercentage[counter] <= maxAllowedDeviation) { critMet = "true"; }

                // Display results
                Console.WriteLine($"{forcesList.Key,10} | {totalForce[counter] / 1000,10:##0.0} | {totalDeviation[counter] / 1000,10:##0.0} | {deviationPercentage[counter]*100,10:##0.0} | {critMet, 10}");

                // Setup for next loop
                previousForces.Clear();
                previousForces.AddRange(forcesList.Value);
                counter++;
            }

        }
    }
}
