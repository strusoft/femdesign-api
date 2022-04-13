using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Results;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign;

namespace FemDesign.Results.Tests
{
    [TestClass()]
    public class BarDisplacementTests
    {
        [TestMethod()]
        public void Parse()
        {

            // string file_path = @"C:\Users\Marco\Desktop\fdScriptUnderstanding\fdBarDisp.csv";
            string file_path = @"C:\Users\Marco\Desktop\fdScriptUnderstanding\fdBarDisp_CaseAndCombo.csv";
            


            var results = ResultsReader.Parse(file_path);

            Console.WriteLine(results);
        }
    }
}