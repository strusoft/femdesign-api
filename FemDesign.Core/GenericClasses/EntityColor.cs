// https://strusoft.com/
using System;
using System.Drawing;
using System.Globalization;
using System.Xml.Serialization;
using FemDesign.GenericClasses;
using Newtonsoft.Json.Linq;

namespace FemDesign
{
    /// <summary>
    /// entity_attribs
    /// </summary>
    [Serializable]
    public partial class EntityColor
    {
        private EntityColor()
        {
        }

        [XmlAttribute("tone")]
        public string _tone;

        [XmlIgnore]
        public Color Tone
        {
            get
            {
                Color col = System.Drawing.ColorTranslator.FromHtml("#" + this._tone);
                return col;
            }
            set
            {
                this._tone = ColorTranslator.ToHtml((Color)value).Substring(1);
            }
        }

        [XmlAttribute("penwidth")]
        public double PenWidth { get; set; }

    }
}

