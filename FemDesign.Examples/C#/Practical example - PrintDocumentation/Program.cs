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
            // PRACTICAL EXAMPLE: Create Documentation through the API
            // This example will show you how to print a .docx file with an already applied template

            // This example was last updated using the ver. 21.9.0 FEM-Design API.

            // LOADING UP THE MODEL
            string strFile = "test.str";
            string docxFilePath = "documentation.docx";
            string dscTemplateFilePath = "template.dsc";

            // RUN THE ANALYSIS
            using (var femDesign = new FemDesignConnection())
            {
                femDesign.OnOutput += Console.WriteLine;
                femDesign.Open(strFile);
                femDesign.SaveDocx(docxFilePath);
            }

            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}