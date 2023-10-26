using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FemDesign;
using System.Reflection;

namespace FemDesign.Examples
{
    internal class Program
    {
        static void Main()
        {
            // EXAMPLE 2: ANALYSING A MODEL
            // This example will show you how to run an analysis
            // with a given model.

            // This example was last updated using the ver. 21.6.0 FEM-Design API.

            // LOADING UP THE MODEL
            string strFile = "test.str";
            string docxFilePath = "documentation.docx";
            string dscTemplateFilePath = "template.dsc";

            // RUN THE ANALYSIS
            using (var femDesign = new FemDesignConnection())
            {
                femDesign.Open(strFile);
                femDesign.SaveDocx(docxFilePath);
            }

            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}