using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Samples
{
    public partial class SampleProgram
    {
        private static void Example4EditExistingModel()
        {
            // Step 1: Read the model
            FemDesign.Model model = FemDesign.Model.DeserializeFromFilePath(@"C:\Users\SamuelNyberg\Documents\GitHub\femdesign-api\FemDesign.Samples\C#\ExampleModels\Example 4 - model.struxml");

            /*
            The Grasshopper work flow was like this:
            1) Select the storey to work with
            2) Find the pillars leading up to the floor: connection points will be the new point supports
            3) Differentiate walls (vertical slabs) from floors (horizontal slabs), doing the same with panels
            4) Removing excess walls (below+above), and leaving lines for line supports. Done using height margins
            5) Find the floor of the chosen storey, remove the rest
            6) Select the right loads, remove the rest
            7) Create and open model                                                   */

            FemDesign.StructureGrid.Storey storey = model.Entities.Storeys.Storey[3];
            double zCoord = storey.Origo.Z;


            // 1) Hitta alla pelare som har en ände i rätt z-koordinat
            // 2) Hitta deras xy-koordinater
            // 3) gör punktstöd med dessa koordinater + z-koodinaten
            // 4) lägg in i en lista

            var supportingPillars = new List<GenericClasses.IStructureElement>();
            var supports = new List<GenericClasses.ISupportElement>();
            for (int i = 0; i < supportingPillars.Count; i++)
            {
                
            }
            

            FemDesign.Model newModel = new FemDesign.Model(Country.S);
            // newModel.AddElements

            // I like to close my console
            Console.WriteLine("Checkpoint!");
            Console.ReadKey();
        }
    }
}
