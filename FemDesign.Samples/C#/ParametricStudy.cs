using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FemDesign.Samples
{
    public partial class SampleProgram
    {
        private static void ParametricStudy()
        {
            //Set the different paths and folders relevant to the example
            string struxmlPath = @"C:\Temp\femdesign-api\FemDesign.Samples\C#\ExampleModels\sample_slab.struxml";
            string outFolder = @"C:\Temp\femdesign-api\FemDesign.Samples\C#\ExampleModels\output\";
            string bscPath= @"C:\Temp\femdesign-api\FemDesign.Samples\C#\ExampleModels\pointsupportreactions.bsc";
            List<string> bscPaths = new List<string>();
            bscPaths.Add(bscPath);

            
            //Read original struxml model
            Model model = Model.DeserializeFromFilePath(struxmlPath);
            
            //Read slab number 5 and its material (hard coded in this example, probably better to look for a slab with a certain name, eg. P.1)
            Shells.Slab slab = model.Entities.Slabs[4];
            Materials.Material material = model.Entities.Slabs[4].Material;
            double Ecm = Convert.ToDouble(material.Concrete.Ecm);

            //Iterate over model using different E-modulus for the slab
            for (int i = 1; i < 6; i++)
            {
                //Change E-modulus
                double new_Ecm = Math.Round(0.2 *i * Ecm);
                material.Concrete.Ecm = Convert.ToString(new_Ecm);

                //Save struxml
                string outPathIndividual = outFolder + "sample_slab_out" + Convert.ToString(new_Ecm) + ".struxml";
                model.SerializeModel(outPathIndividual);


                //Run analysis
                Calculate.Analysis analysis = new Calculate.Analysis(null, null, null, null, true, false, false, false, false, false, false, false, false, false, false, false, false);
                FemDesign.Calculate.FdScript fdScript = FemDesign.Calculate.FdScript.Analysis(outPathIndividual, analysis, bscPaths, "", true);
                Calculate.Application app = new Calculate.Application();
                app.RunFdScript(fdScript, false, true, true);


                //One way of reading results, available for some result types only yet
                /*
                var results = Results.ResultsReader.Parse(@"C:\Users\JohannaRiad\OneDrive - StruSoft AB\Documents\2. Eget presentationsmaterial\1. Webinars\Internationellt Webinar februari 2022\parameterstudy\pointsupportreactions.csv");
                var pointSupportReactions = results.Cast<Results.PointSupportReaction>().ToList();
                double fz = pointSupportReactions[0].Fz;
                */

                //Read results from csv file (general method)
                int counter = 0; 
                using (var reader = new StreamReader(@"C:\Temp\femdesign-api\FemDesign.Samples\C#\ExampleModels\output\pointsupportreactions.csv"))
                {
                    Console.WriteLine("");
                    Console.WriteLine(string.Format("{0} {1}", "Emean: ", new_Ecm));
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split('\t');
                        if (counter>0 & line!="")
                        {                           
                            Console.WriteLine(string.Format("{0} {1}", values[0], values[7]));
                        }                       
                        counter++;
                    }                 
                }               
            }
        }
    }
}
