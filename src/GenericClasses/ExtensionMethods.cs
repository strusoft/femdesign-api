// https://strusoft.com/
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace FemDesign
{
    // Extension method used to clone instances of objects.
    public static class ExtensionMethods
    {
        public static T DeepClone<T>(this T obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                // return
                return (T) formatter.Deserialize(ms);
            }
        }
    }
}
