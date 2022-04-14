using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FemDesign.Results.Tests
{
    [TestClass()]
    public class BarStressTests
    {
        [TestMethod()]
        public void Parse()
        {
            string file_path = @"C:\Users\Marco\Desktop\fdScriptUnderstanding\311-barStresses\bar_stresses.txt";

            var results = ResultsReader.Parse(file_path);

            Console.WriteLine(results);
        }
    }
}