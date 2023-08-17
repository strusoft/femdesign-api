// https://strusoft.com/

using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;

namespace FemDesign.Composites
{
    [System.Serializable]
    public partial class ComplexComposite : EntityBase
    {
        [XmlElement("composite_section")]
        public List<ComplexCompositePart> Parts = new List<ComplexCompositePart>();

        [XmlIgnore]
        public CompositeSection[] CompositeSections
        {
            get
            {
                return this.Parts.Select(s => s.CompositeSectionObj).ToArray();
            }
            set
            {
                if()
            }
        }

        [XmlIgnore]
        public double[] Positions
        {
            get
            {
                return this.Parts.Select(p => p.Pos).ToArray();
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private ComplexComposite()
        {

        }

        /// <summary>
        /// Create complex composite from a composite section. Equal cross-section at both ends of bar.
        /// </summary>
        /// <param name="composite">Composite section</param>
        internal ComplexComposite(CompositeSection composite)
        {
            this.EntityCreated();
            this.Parts.Add(new ComplexCompositePart(0, composite));
            this.Parts.Add(new ComplexCompositePart(1, composite));

            // check parts
            this.CheckStartEnd();
        }

        public void CheckStartEnd(double[] pos)
        {
            if (pos[0] != 0)
            {
                throw new System.Exception($"First position of complex section must be 0 but is: {this.Parts[0].Pos}");
            }
            if (pos.Last() != 1)
            {
                throw new System.Exception($"Last position of complex section must be 1 but is: {this.Parts[0].Pos}");
            }
        }
    }
}
