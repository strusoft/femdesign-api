using System;
using System.Xml.Serialization;


namespace FemDesign.Reinforcement
{
    [System.Serializable]
    public partial class LongitudinalBar
    {
        [XmlElement("cross-sectional_position", Order = 1)]
        public Geometry.Point2d Position2d { get; set; }

        [XmlElement("anchorage", Order = 2)]
        public StartEndType Anchorage { get; set; }

        [XmlElement("prescribed_lengthening", Order = 3)]
        public StartEndType PrescribedLengthening { get; set; }

        [XmlAttribute("start")]
        public double Start { get; set; }

        [XmlAttribute("end")]
        public double End { get; set; }

        [XmlAttribute("auxiliary")]
        public bool Auxiliary { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        public LongitudinalBar()
        {

        }

        /// <summary>
        /// Construct longitudinal bar using start and end distance from bar start
        /// </summary>
        /// <param name="position"></param>
        /// <param name="startAnchorage">Start anchorage in meters.</param>
        /// <param name="endAnchorage">End anchorage in meters.</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="auxiliary"></param>
        public LongitudinalBar(Geometry.Point2d position, double startAnchorage, double endAnchorage, double start, double end, bool auxiliary)
        {
            this.Position2d = position;
            this.Anchorage = new StartEndType(startAnchorage, endAnchorage);
            this.Start = start;
            this.End = end;
            this.Auxiliary = auxiliary;
        }

        /// <summary>
        /// Construct longitudinal bar using start and end param from bar start
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="position"></param>
        /// <param name="startAnchorage">Start anchorage in meters.</param>
        /// <param name="endAnchorage">End anchorage in meters.</param>
        /// <param name="startParam"></param>
        /// <param name="endParam"></param>
        /// <param name="auxiliary"></param>
        public LongitudinalBar(Bars.Bar bar, Geometry.Point2d position, double startAnchorage, double endAnchorage, double startParam, double endParam, bool auxiliary)
        {
            this.Position2d = position;
            this.Anchorage = new StartEndType(startAnchorage, endAnchorage);
            var len = bar.BarPart.Edge.Length;
            this.Start = startParam * len;
            this.End = endParam * len;
            this.Auxiliary = auxiliary;
        }
    }
}