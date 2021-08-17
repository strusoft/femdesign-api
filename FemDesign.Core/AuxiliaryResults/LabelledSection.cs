using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using FemDesign;
using FemDesign.Geometry;
using FemDesign.GenericClasses;

namespace FemDesign.AuxiliaryResults
{
    /// <summary>
    /// Labelled section. Used for extracting detailed results along a section line or polyline.
    /// </summary>
    [System.Serializable]
    public class LabelledSection : EntityBase, IStructureElement
    {
        [XmlIgnore]
        private static int instances = 0;
        public static void ResetInstanceCount() => instances = 0;
        [XmlElement("line_segment")]
        public LineSegment _lineSegment;
        [XmlElement("polyline")]
        public Polyline _polyline;
        /// <summary>
        /// The verticies of the labelled section. 2 verticies for line geometry and 3 or more for polyline geometry.
        /// </summary>
        [XmlIgnore]
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
        /// Parameterless contructor for serialization
        /// </summary>
        private LabelledSection() { }

        /// <summary>
        /// Construct a labelled section
        /// </summary>
        /// <param name="verticies">Verticies</param>
        /// <param name="identifier">Identifier</param>
        public LabelledSection(List<FdPoint3d> verticies, string identifier = "LS")
        {
            Initialize(verticies, identifier);
        }

        /// <inheritdoc cref="LabelledSection(List{FdPoint3d}, string)"/>
        public LabelledSection(string identifier = "LS", params FdPoint3d[] verticies)
        {
            Initialize(verticies.ToList(), identifier);
        }

        private void Initialize(List<FdPoint3d> verticies, string identifier)
        {
            instances++;
            this.EntityCreated();

            Verticies = verticies;
            Name = $"{identifier}.{instances}";
        }
    }
}
