using FemDesign;
using FemDesign.Calculate;
using FemDesign.Results;
using FemDesign.Sections;

using System.Linq;

namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Input
            var filePath = "model.struxml";

            // define outputs
            List<Section> sectionPool = new List<Section>();
            var items = new List<(string Name, double Utilisation, double? CO2)>();

            var units = new UnitResults(stress: Stress.MPa, sectionalData: SectionalData.mm);
            var analysis = Analysis.StaticAnalysis();
            var design = new Design(autoDesign: true, check: true);

            var sectionDatabase = Sections.SectionDatabase.GetDefault().Sections.Section;

            // define the section pool that we want to evaluate
            sectionPool.Add(sectionDatabase.SectionByName("HE-A 200"));
            sectionPool.Add(sectionDatabase.SectionByName("HE-B 200"));
            sectionPool.Add(sectionDatabase.SectionByName("HE-M 200"));
            sectionPool.Add(sectionDatabase.SectionByName("UKC 152x152x23"));
            sectionPool.Add(sectionDatabase.SectionByName("KKR 150x100x8"));

            Console.WriteLine("Starting FEM-Design...");
            using (var connection = new FemDesignConnection(minimized: true))
            {
                connection.Open(filePath);
                var designConfig = Calculate.SteelDesignConfiguration.Method1();

                Console.WriteLine("Running analysis and design for each section in the pool...\n");
                connection.RunAnalysis(analysis);

                foreach (var section in sectionPool)
                {
                    // we force femdesign to use the section we want to evaluate by setting the section in the design configuration
                    var calcConfig = new Calculate.SteelBarDesignParameters(0.8, new List<Sections.Section>{section});
                    connection.SetConfig(calcConfig);
                    connection.RunDesign(CmdUserModule.STEELDESIGN, design);

                    // get results from the design
                    var utilisation = connection.GetResults<BarSteelUtilization>(units).Select(x => x.Max).Max();

                    // we apply the new section to the model so that we can get the quantities
                    connection.ApplyDesignChanges();
                    var quantities = connection.GetQuantities<QuantityEstimationSteel>();

                    var totalWeight = quantities.Select(x => x.TotalWeight).Sum();
                    var co2 = quantities.Select(x => x.CO2Footprint).Sum();
                    var sectionName = quantities.Select(x => x.Section).First().ToUpper();

                    Console.WriteLine(sectionName);
                    Console.WriteLine("Utilisation: " + utilisation);
                    Console.WriteLine("Total weight: " + totalWeight + " kg");
                    Console.WriteLine("CO2 footprint: " + co2 + " kg CO2e");
                    Console.WriteLine("");

                    // collect output in a tuple
                    var item = (sectionName, utilisation, co2);
                    items.Add(item);
                }
            }

            // Sort by Utilisation
            var sortedByUtilisation = items.OrderBy(item => item.Utilisation).ToList();


            Console.WriteLine("DISPLAY RESULTS");
            Console.WriteLine("Sorted by Utilisation:");
            foreach (var item in sortedByUtilisation)
            {
                Console.WriteLine($"Name: {item.Name}, Utilisation: {item.Utilisation}, CO2: {item.CO2}");
            }

            // Sort by CO2
            var sortedByCO2 = items.OrderBy(item => item.CO2).ToList();

            Console.WriteLine("\nSorted by CO2:");
            foreach (var item in sortedByCO2)
            {
                Console.WriteLine($"Name: {item.Name}, Utilisation: {item.Utilisation}, CO2: {item.CO2}");
            }



        }
    }
}
