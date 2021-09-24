using System;
using System.Xml.Serialization;


namespace FemDesign
{
    [System.Serializable]
    public partial class StartEndType
    {
        [XmlAttribute("start")]
        public double Start { get; set; }

        [XmlAttribute("end")]
        public double End { get; set; }
        private StartEndType()
        {

        }
        public StartEndType(double start, double end)
        {
            this.Start = start;
            this.End = end;
        }
    }
}