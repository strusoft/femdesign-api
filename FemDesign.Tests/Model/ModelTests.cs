using Microsoft.VisualStudio.TestTools.UnitTesting;
using FemDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;


namespace FemDesign.Tests
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

        [TestMethod("Construct Model")]
        public void ModelTest()
        {
            Model model = new Model(Country.S);
            
            Assert.IsNotNull(model, "Can construct model");
            Assert.IsTrue(model.Country == FemDesign.Country.S, "Should construct model with country code preserved");
        }

        /// <summary>
        /// Test if global test model can be deserialised from path and then serialised to string.
        /// To check which version the test file was generated in check source software attribute in file.
        /// </summary>
        [TestMethod("ReadWriteConsole")]
        public void ReadWriteConsole()
        {
            string input = "Model/global-test-model.struxml";
            Model model = Model.DeserializeFromFilePath(input);
            Console.Write(model.SerializeToString());
        }

        /// <summary>
        /// Test if global test model can be deserialised from path and then serialised to file.
        /// The file can then be opened in FEM-Design to look for errors.
        /// To check which version the test file was generated in check source software attribute in file.
        /// </summary>
        [TestMethod("ReadWriteFile")]
        public void ReadWriteFile()
        {
            string input = "Model/global-test-model.struxml";
            string output = "Model/readWriteFile.struxml";
            Model model = Model.DeserializeFromFilePath(input);
            model.SerializeModel(output);
        }
    }
}