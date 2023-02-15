using System;
using System.Drawing;
using System.Xml.Serialization;

namespace StruSoft.Interop.StruXml.Data
{
    public partial class Style_type
    {
        /// <value>
        /// Store custom layers
        /// </value>
        [XmlIgnore]
        public Layer_type LayerObj;
        
        /// <value>
        /// Set colour from System.Drawing.Color
        /// </value>
        [XmlIgnore]
        public Color SetColor
        {
            set
            {
                this.Colour = ColorTranslator.ToHtml((Color)value).Substring(1);
            }
        }

        /// <value>
        /// Get colour as System.Drawing.Color
        /// </value>
        [XmlIgnore]
        public Color GetColor
        {
            get
            {
                Color col = System.Drawing.ColorTranslator.FromHtml("#" + this.Colour);
                return col;
            }
        }
    }
}
