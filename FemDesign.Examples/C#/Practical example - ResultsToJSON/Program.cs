﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;
using FemDesign.Results;
using Newtonsoft.Json;


namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main()
        {
            // EXAMPLE 3: WRITING ANALYSIS RESULTS INTO A JSON FILE
            // This example shows how to run analysis, get the results
            // and write them into a .json file.

            // This example was last updated using the ver. 22.3.0 FEM-Design API.


            string filePath = Path.GetFullPath("myModel.str");

            // Set up the analysis
            var analysis = Calculate.Analysis.StaticAnalysis();

            // Run a specific analysis
            List<Results.NodalDisplacement> nodalDisplacement;

            var units = Results.UnitResults.Default();
            units.Displacement = Results.Displacement.mm;

            using (var femDesign = new FemDesignConnection())
            {
                femDesign.OnOutput += Console.WriteLine;

                femDesign.Open(filePath);
                nodalDisplacement = femDesign.GetResults<Results.NodalDisplacement>(units);


                // Convert the Results to JSON Format
                var jsonResult = nodalDisplacement[0].ToJSON();
                var jsonListResults = nodalDisplacement.ToJSON();

                // Deserialise the string to Nodal Displacement Object
                var myResults = JsonConvert.DeserializeObject<Results.NodalDisplacement>(jsonResult);
                var myListResults = JsonConvert.DeserializeObject<List<Results.NodalDisplacement>>(jsonListResults);

            }
        }
    }
}