using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign;
using FemDesign.Calculate;
using System.Xml.Serialization;
using System.IO;
using System.Net.Http;

namespace FemDesign.Documentation
{
    [TestClass()]
    public partial class ExampleDocumentation
    {
        /// <summary>
        /// The method tracks the url used in the C# documentation page for the examples.
        /// If example change location or name, an error occurs.
        /// </summary>
        [TestMethod("ExampleDocumentation")]
        public void UrlExampleDocusaurus()
        {
            var github = "https://github.com/strusoft/femdesign-api/blob/master/FemDesign.Examples/C%23/";
            var currentPath = Directory.GetCurrentDirectory();
            string examplesPath = Path.GetFullPath(Path.Combine(currentPath, @"..\..\..\FemDesign.Examples\C#"));
            var directory = System.IO.Directory.GetDirectories(examplesPath);

            foreach (var directoryPath in directory)
            {
                var dirName = new DirectoryInfo(directoryPath).Name;
                var programName = System.String.Concat(github, dirName, "/Program.cs");

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(programName).Result;

                    // Assert
                    Assert.AreEqual(response.IsSuccessStatusCode, true,
                        $"URL {programName} returned status code {response.StatusCode}");
                    Assert.IsTrue(response.StatusCode != System.Net.HttpStatusCode.NotFound);
                }

            }

        }
    }
}