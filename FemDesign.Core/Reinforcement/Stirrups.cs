using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Reinforcement
{
    [System.Serializable]
    public partial class Stirrups
    {
        [XmlElement("region", Order = 1)]
        public List<Geometry.Region> Regions = new List<Geometry.Region>();

        [XmlAttribute("start")]
        public double Start { get; set; }

        [XmlAttribute("end")]
        public double End { get; set; }

        [XmlAttribute("distance")]
        public double Distance { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        public Stirrups()
        {

        }

        /// <summary>
        /// Construct stirrups by start and end distance from bar start.
        /// </summary>
        public Stirrups(Geometry.Region region, double start, double end, double distance)
        {
            this.Regions.Add(region);
            this.Start = start;
            this.End = end;
            this.Distance = distance;
        }

        /// <summary>
        /// Construct stirrups by start and end parameter on the bar.
        /// </summary>
        public Stirrups(Bars.Bar bar, Geometry.Region region, double startParam, double endParam, double distance)
        {
            this.Regions.Add(region);
            double len = bar.BarPart.Edge.Length;
            this.Start = startParam * len;
            this.End = endParam * len;
            this.Distance = distance;
        }
    }
}