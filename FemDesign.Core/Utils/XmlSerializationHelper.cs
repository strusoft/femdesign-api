using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FemDesign.Utils
{   public static class XmlSerialization
    {
        public static string SerializeObjectToXml<T>(this T obj)
        {
            // Create an XmlSerializer for the type of the object
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            // Use a StringWriter to hold the XML data
            using (StringWriter writer = new StringWriter())
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                // Serialize the object to the StringWriter
                serializer.Serialize(writer, obj, ns);

                // Return the XML string
                return writer.ToString();
            }
        }
    }
}