using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign.Results;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Results.Tests
{
    [TestClass()]
    public class BarInternalForcesTest
    {
        [TestMethod]
        public void Parse()
        {
            List<string> files = new List<string>();
            files.Add(@"C:\Users\Marco\Desktop\fdScriptUnderstanding\178-branch\Bar_Internal_Forces_LoadCombination.txt");
            files.Add(@"C:\Users\Marco\Desktop\fdScriptUnderstanding\178-branch\Bar_Internal_Forces_LoadCase.csv");
            
            foreach (string file_path in files)
            {
                var results = ResultsReader.Parse(file_path);
                Console.WriteLine(results);
            }

        }
    }
}