using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;

namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main()
        {
            // PRACTICAL EXAMPLE: CHANGE LENGTH OF BEAM
            // this simple example will show you how to edit the length of a beam from within a C# script.

            // This example was last updated using the ver. 21.4.0 FEM-Design API.


            // FILE PATH SETUP
            // Find the file you want to edit, and prepare a path for the edited file
            string struxmlPath = "example_beam.struxml";
            string struxmlPathOut = "output/example_beam_new_length.struxml";

            if (!Directory.Exists("output"))
                Directory.CreateDirectory("output");


            // READ FILE
            Model model = Model.DeserializeFromFilePath(struxmlPath);


            // EDIT BEAM
            // Get the first beam (index 0) and update the coordinate of the beam endpoint
            Bars.Bar exbeam = model.Entities.Bars[0];
            exbeam.BarPart.Edge.Points[1].X = 35;


            // EDIT SUPPORT
            // Update the position of the support to the same new coordinate
            Supports.PointSupport support = model.Entities.Supports.PointSupport[1];
            support.Position.X = 35;


            // SAVE THE MODEL AS A NEW FILE
            model.SerializeModel(struxmlPathOut);


            // ENDING THE PROGRAM
            Console.WriteLine($"Saved an updated struxml file at path bin/debug/{struxmlPathOut}");
            Console.WriteLine("\nPress any key to close console.");
            Console.ReadKey();
        }
    }
}
