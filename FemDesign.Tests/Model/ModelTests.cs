using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using Microsoft.XmlDiffPatch;

using FemDesign.GenericClasses;

namespace FemDesign.Models
{
    [TestClass()]
    public class ModelTests
    {
        public static Stream Serialize(object source)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            formatter.Serialize(stream, source);
            return stream;
        }

        public static T Deserialize<T>(Stream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            stream.Position = 0;
            return (T)formatter.Deserialize(stream);
        }

        private bool AreEqual(object obj1, object obj2)
        {
            if (obj1 == null && obj2 == null) return true;
            if (obj1 == null || obj2 == null) return false;

            if (!obj1.GetType().Equals(obj2.GetType()))
            {
                return false;
            }

            Type type = obj1.GetType();
            if (type.IsPrimitive || typeof(string).Equals(type))
            {
                return obj1.Equals(obj2);
            }
            if (type.IsArray)
            {
                Array first = obj1 as Array;
                Array second = obj2 as Array;
                var en = first.GetEnumerator();
                int i = 0;
                while (en.MoveNext())
                {
                    if (!AreEqual(en.Current, second.GetValue(i)))
                        return false;
                    i++;
                }
            }
            else
            {
                foreach (PropertyInfo pi in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
                {
                    var val = pi.GetValue(obj1);
                    var tval = pi.GetValue(obj2);
                    if (!AreEqual(val, tval))
                        return false;
                }
                foreach (FieldInfo fi in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
                {
                    var val = fi.GetValue(obj1);
                    var tval = fi.GetValue(obj2);
                    if (!AreEqual(val, tval))
                        return false;
                }
            }
            return true;
        }

        public static T Clone<T>(T source)
        {
            return Deserialize<T>(Serialize(source));
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("Construct Model")]
        public void ModelTest()
        {
            Model model = new Model(Country.S);

            Assert.IsNotNull(model, "Can construct model");
            Assert.IsTrue(model.Country == FemDesign.Country.S, "Should construct model with country code preserved");
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("Open a Model")]
        public void Open()
        {
            Model model = new Model(Country.S);
            model.Open();
        }

        /// <summary>
        /// Test if the global model can be deep cloned.
        /// </summary>
        [TestCategory("FEM-Design required")]
        [TestMethod("DeepClone")]
        public void DeepClone()
        {
            string input = "Model/global-test-model_MASTER.struxml";
            Model model = Model.DeserializeFromFilePath(input);
            var clone = model.DeepClone();
            Console.Write(clone.SerializeToString());
        }

        private (int, int, long) GetFileInfo(string filePath)
        {
            int fileLineCount = File.ReadLines(filePath).Count();
            int numberOfCharacters = File.ReadAllLines(filePath).Sum(s => s.Length);
            FileInfo fileInfo = new FileInfo(filePath);
            long size = fileInfo.Length;

            return (fileLineCount, numberOfCharacters, size);
        }

        public bool GenerateDiffGram(string originalFile, string finalFile,
                                            XmlWriter diffGramWriter)
        {
            XmlDiff xmldiff = new XmlDiff(XmlDiffOptions.IgnoreChildOrder |
                                             XmlDiffOptions.IgnoreNamespaces |
                                             XmlDiffOptions.IgnorePrefixes);
            bool bIdentical = xmldiff.Compare(originalFile, finalFile, false, diffGramWriter);
            diffGramWriter.Close();
            return bIdentical;
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("CompareInOut")]
        public void CompareInOut()
        {
            // The input file "global-test-model_MASTER.struxml" has been previously checked for
            // equivalence with the .struxml serialised from FD taking in consideration that
            // the FD and the API serialise in different way.

            // NOTE
            // if We implement of add new attributes for the already created objects (i.e. some bar attributes)
            // the test will fail as the file will not be identical anymore

            string inputFile = "Model/global-test-model_MASTER.struxml";
            Model model = Model.DeserializeFromFilePath(inputFile);

            string outputFile = "Model/global-test-model_MASTER_OUT.struxml";
            model.SerializeModel(outputFile);

            (int fileLineCountMaster, int numberOfCharactersMaster, long sizeMaster) = GetFileInfo(inputFile);
            (int fileLineCountOut, int numberOfCharactersOut, long sizeOut) = GetFileInfo(outputFile);

            Console.WriteLine($"Master file has {fileLineCountMaster} number of lines");
            Console.WriteLine($"Output file has {fileLineCountOut} number of lines");

            Console.WriteLine($"Master file has {numberOfCharactersMaster} characters");
            Console.WriteLine($"Output file has {numberOfCharactersOut} characters");

            Console.WriteLine($"Master file is {sizeMaster} bytes");
            Console.WriteLine($"Output file is {sizeOut} bytes");

            int diffLine = fileLineCountOut - fileLineCountMaster;
            int diffCharacter = numberOfCharactersOut - numberOfCharactersMaster;
            long diffSize = sizeOut - sizeMaster;

            var diffGramWriter = XmlWriter.Create("Model/diffGram.xml");
            bool identical = GenerateDiffGram(inputFile, outputFile, diffGramWriter);

            Assert.IsTrue(diffLine == 0);
            Assert.IsTrue(diffCharacter == 0);
            Assert.IsTrue(diffSize == 0);
            Assert.IsTrue(identical);
        }

        [TestCategory("FEM-Design required")]
        [TestMethod("SerialiseFdScript")]
        // Check if the fdscript is serialised correctly doing a deserialisation!!!
        public void SerialiseFdScript()
        {
            string input = "Model/25539-surface-result.struxml";
            Model model = Model.DeserializeFromFilePath(input);

            model.RunAnalysis(Calculate.Analysis.StaticAnalysis(), endSession: true);

            var design = new FemDesign.Calculate.Design(true, true, true, false);
            model.RunDesign(Calculate.CmdUserModule.STEELDESIGN, Calculate.Analysis.StaticAnalysis(), design, endSession: false);
        }
    }
}