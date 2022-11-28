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
            // EXAMPLE 3: CREATING A SIMPLE BEAM
            // This example shows how to model a simple supported beam,
            // and how to run an alalysis with it in FEM-Design.
            // Before running, make sure you have a window with FEM-Design open.

            // This example was last updated using the ver. 21.6.0 FEM-Design API.


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
                var myResults = JsonConvert.DeserializeObject<List<Results.NodalDisplacement>>(jsonResult);
                var myListResults = JsonConvert.DeserializeObject<List<Results.NodalDisplacement>>(jsonListResults);

                Console.ReadKey();
            }
        }
    }
}