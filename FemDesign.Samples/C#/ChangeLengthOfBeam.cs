using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Samples
{
    public partial class SampleProgram
    
    {
        private static void ChangeLengthOfBeam()
        {
            string struxmlPath = "ExampleModels/example_beam.struxml";
            string struxmlPathOut = @"ExampleModels/output/example_beam.struxml";

            if (!Directory.Exists("ExampleModels/output/"))
                Directory.CreateDirectory("ExampleModels/output/");

            // Read a struxml model file
            Model model = Model.DeserializeFromFilePath(struxmlPath);

            // Get the first beam (index 0) and update the coordinate of the beam endpoint
            Bars.Bar exbeam = model.Entities.Bars[0];
            exbeam.BarPart.Edge.Points[1].X = 35;

            // Update the position of the support to the same new coordinate
            Supports.PointSupport support = model.Entities.Supports.PointSupport[1];
            support.Position.X = 35;

            // Save the model as a new file
            model.SerializeModel(struxmlPathOut);

            Console.WriteLine($"Saved an updated struxml file at path {struxmlPathOut}");
        }
    }
}
