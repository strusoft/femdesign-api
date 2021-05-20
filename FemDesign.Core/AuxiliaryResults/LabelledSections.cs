using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign;
using FemDesign.Geometry;
using FemDesign.GenericClasses;

namespace FemDesign.AuxiliaryResults
{
    [System.Serializable]
    public class LabelledSection : EntityBase, IStructureElement
    {
        [XmlIgnore]
        private static int instances = 0;
        public static void ResetInstanceCount() => instances = 0;
        [XmlElement("line_segment")]
        public LineSegment _lineSegment;
        [XmlElement("line_segment")]
        public Polyline _polyline;
        public List<FdPoint3d> Verticies { 
            get
            {
                var verticies = new List<FdPoint3d>();
                if (_lineSegment != null)
                    verticies.AddRange(_lineSegment.Verticies);
                if (_polyline != null)
                    verticies.AddRange(_polyline.Verticies);
                return verticies;
            }
            set
            {
                if (value.Count == 2)
                {
                    _lineSegment = new LineSegment(value[0], value[1]);
                    _polyline = null;
                }
                else if (value.Count > 2)
                {
                    _lineSegment = null;
                    _polyline = new Polyline(value);
                }
                else
                    throw new ArgumentException($"LabelledSection must have at least 2 verticies.");
            }
        }

        /// <summary>
        /// Identifier
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }
        
        /// <summary>
        /// Construct labelled section
        /// </summary>
        /// <param name="verticies"></param>
        /// <param name="identifier"></param>
        public LabelledSection(List<FdPoint3d> verticies, string identifier = "LS")
        {
            Initialize(verticies, identifier);
        }

        private void Initialize(List<FdPoint3d> verticies, string identifier)
        {
            instances++;
            this.EntityCreated();

            Verticies = verticies;
            Name = identifier + "." + instances.ToString();
        }
    }
}
