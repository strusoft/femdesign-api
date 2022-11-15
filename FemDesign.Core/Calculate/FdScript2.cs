// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FemDesign.Results;
using System.Reflection;
using System.Xml.Linq;
using System.Text;

namespace FemDesign.Calculate
{

    public static class Extension
    {
        public static XElement ToXElement<T>(this object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (TextWriter streamWriter = new StreamWriter(memoryStream))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(streamWriter, obj);
                    return XElement.Parse(Encoding.ASCII.GetString(memoryStream.ToArray()));
                }
            }
        }

        public static T FromXElement<T>(this XElement xElement)
        {
            var xmlSerializer = new XmlSerializer(typeof(T));
            return (T)xmlSerializer.Deserialize(xElement.CreateReader());
        }
    }


    /// <summary>
    /// Base class for all commands that can be run in FEM-Design
    /// </summary>
    [XmlInclude(typeof(CmdCalculation))]
    [XmlInclude(typeof(CmdSave))]
    [XmlInclude(typeof(FdScriptHeader))]
    [XmlInclude(typeof(CmdOpen))]
    [XmlInclude(typeof(CmdUser))]
    [XmlInclude(typeof(CmdListGen))]
    [XmlRoot("fdscript", Namespace = "urn:strusoft")]
    [System.Serializable]
    public abstract partial class CmdCommand
    {
        public XElement ToXElement()
        {
            return Extension.ToXElement<CmdCommand>(this);
        }
    }

    /// <summary>
    /// Fdscript root class
    /// </summary>
    public partial class FdScript2
    {
        public FdScriptHeader Header { get; set; }
        public List<CmdCommand> Commands = new List<CmdCommand>();

        public FdScript2(string logFilePath, params CmdCommand[] commands)
        {
            Header = new FdScriptHeader(logFilePath);
            Commands = commands.ToList();
        }

        public void Add(CmdCommand command)
        {
            Commands.Add(command);
        }

        public void Serialize(string path)
        {
            XDocument doc = new XDocument();

            var root = new XElement(
                "fdscript", 
                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance")
                );

            root.Add(Header.ToXElement());

            Commands.ForEach(c => root.Add(c.ToXElement()));

            doc.Add(root);
            doc.Save(path);
        }
    }
}
