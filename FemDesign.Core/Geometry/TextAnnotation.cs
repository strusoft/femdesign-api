

using System;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using StruSoft.Interop.StruXml.Data;

namespace FemDesign.Geometry
{
    [Serializable]
    public class TextAnnotation: GenericClasses.IStructureElement
    {
        [XmlElement("position")]
        public Point3d Position { get; set; }
        [XmlElement("local_x")]
        public Vector3d LocalX { get; set; }
        [XmlElement("local_y")]
        public Vector3d LocalY { get; set; }
        [XmlElement("style")]
        public StruSoft.Interop.StruXml.Data.Style_type StyleType { get; set; }
        [XmlAttribute("guid")]
        public System.Guid Guid { get; set; }
        [XmlAttribute("text")]
        public string _text;
        [XmlIgnore]
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                var regex = "[&#x0009;&#x000A;&#x000d; -&#xfffd;]{0,1023}";
                var match = Regex.Match(value, regex);
                if (match.Success)
                {
                    _text = value;
                }
                else
                {
                    throw new System.ArgumentException($"text value: {value} has bad formatting. Expected regex pattern: {regex}");
                }
            }
        }
        public TextAnnotation()
        {

        }
        public void Initialize()
        {
            Position = Point3d.Origin;
            LocalX = Vector3d.UnitX;
            LocalY = Vector3d.UnitY;
            StyleType = new Style_type
            {
                Layer = "0",
                Font = new Font_type
                {
                    Script = Script_type.Default
                }
            };
            Guid = System.Guid.NewGuid();
        }

        public TextAnnotation(Point3d position, Vector3d localX, Vector3d localY, string text)
        {
            this.Initialize();
            Position = position;
            LocalX = localX;
            LocalY = localY;
            Text = text;
        }
        public TextAnnotation(Point3d position, Vector3d localX, Vector3d localY, Style_type styleType, string text)
        {
            this.Initialize();
            Position = position;
            LocalX = localX;
            LocalY = localY;
            StyleType = styleType;
            Text = text;
        }

        public override string ToString()
        {
            return $"TextAnnotation: {this.Text} at {this.Position}";
        }

        public void EntityCreated()
        {
            throw new NotImplementedException();
        }

        public void EntityModified()
        {
            throw new NotImplementedException();
        }

        public static implicit operator Text_type(TextAnnotation t) => new Text_type{
            Position = t.Position,
            Local_x = t.LocalX,
            Local_y = t.LocalY,
            Style = t.StyleType,
            Guid = t.Guid.ToString(),
            Text = t.Text,
        };
    }
}
