using System.Xml.Serialization;


namespace FemDesign.Materials
{
    [System.Serializable()]
    public partial class LimitStresses: MechProps
    {
        [XmlAttribute("fm0k")]
        public double fm0k { get; set; }
        [XmlAttribute("fm90k")]
        public double fm90k { get; set; }
        [XmlAttribute("ft0k")]
        public double ft0k { get; set; }
        [XmlAttribute("ft90k")]
        public double ft90k { get; set; }
        [XmlAttribute("fc0k")]
        public double fc0k { get; set; }
        [XmlAttribute("fc90k")]
        public double fc90k { get; set; }
        [XmlAttribute("fxyk")]
        public double fxyk { get; set; }
        [XmlAttribute("fvk")]
        public double fvk { get; set; }
        [XmlAttribute("fRk")]
        public double fRk { get; set; }
        [XmlAttribute("fTork")]
        public double fTork { get; set; }

        public LimitStresses()
        {
            
        }
    }
}