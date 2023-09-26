// https://strusoft.com/

using System;
using System.Xml.Serialization;

namespace FemDesign.Composites
{
    [System.Serializable]
    public partial class ComplexCompositePart
    {
        [XmlAttribute("guid")]
        public Guid _compositeSectionRef { get; set; }
        
        [XmlIgnore]
        public Guid CompositeSectionRef 
        { 
            get { return this._compositeSectionRef; }
            set { this._compositeSectionRef = value; }
        }

        [XmlIgnore]
        public CompositeSection CompositeSectionObj { get; set; }

        [XmlAttribute("pos")]
        public double _pos;

        [XmlIgnore]
        public double Pos
        {
            get { return this._pos; }
            set { this._pos = RestrictedDouble.NonNegMax_1(value); }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ComplexCompositePart()
        { 

        }

        /// <summary>
        /// Constructor to create Composites.
        /// </summary>
        /// <param name="pos">Position parameter (0-1).</param>
        /// <param name="composite">Composite section at pos.</param>
        internal ComplexCompositePart(double pos, CompositeSection composite)
        {
            this.Pos = pos;
            this.CompositeSectionObj = composite;
            this.CompositeSectionRef = composite.Guid;
        }
    }
}
