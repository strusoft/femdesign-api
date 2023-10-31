// https://strusoft.com/
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using FemDesign.GenericClasses;
using FemDesign.Geometry;


namespace FemDesign.Reinforcement
{
    [System.Serializable]
    public partial class ConcealedBar: NamedEntityBase, IStructureElement
    {
        private static int _concealedBarInstances = 0;
        protected override int GetUniqueInstanceCount() => ++_concealedBarInstances;

        [XmlElement("rectangle", Order = 1)]
        public RectangleType Rectangle { get; set; }

        [XmlElement("start", Order = 2)]
        public Point3d Start { get; set; }

        [XmlElement("buckling_data", Order = 3)]
        public Bars.Buckling.BucklingData BucklingData { get; set; }

        [XmlElement("end", Order = 4)]
        public string End = "";

        [XmlAttribute("base_shell")]
        public Guid BaseShell { get; set; }

        [XmlAttribute("axis_in_longer_side")]
        [DefaultValue(true)]
        public bool AxisInLongerSide { get; set; } = true;


        [XmlIgnore]
        public List<BarReinforcement> Reinforcement = new List<BarReinforcement>();

        [XmlIgnore]
        public List<BarReinforcement> Stirrups
        {
            get
            {
                return this.Reinforcement.Where(x => x.Stirrups != null).ToList();
            }
        }
        [XmlIgnore]
        public List<BarReinforcement> LongitudinalBars
        {
            get
            {
                return this.Reinforcement.Where(x => x.LongitudinalBar != null).ToList();
            }
        }

        private ConcealedBar()
        {

        }

        /// <summary>
        /// Concealed bar constructor.
        /// </summary>
        /// <param name="slab">Base shell element.</param>
        /// <param name="rectangle">Rectangle area where the concealed bar is specified. Must be inside the SlabPart region boundary.</param>
        /// <param name="axisInLongerSide">If true, the axis of the concealed bar is parallel to the longer side of the rectangle, otherwise it is parallel to the shorter side.</param>
        /// <param name="identifier">Structural element identifier.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public ConcealedBar(Shells.Slab slab, RectangleType rectangle, bool axisInLongerSide = true, string identifier = "CB")
        {
            if (slab.SlabPart.ComplexMaterial.Concrete == null)
            {
                throw new System.ArgumentException("Slab material must be concrete!");
            }

            this.EntityCreated();
            this.BaseShell = slab.SlabPart.Guid;
            this.AxisInLongerSide = axisInLongerSide;
            //if(axisInLongerSide)
            //{
            //    this.Start = rectangle.BaseCorner + new Vector3d(0, rectangle.DimY / 2, 0);
            //}
            //else
            //{
            //    this.Start = rectangle.BaseCorner + new Vector3d(rectangle.DimX / 2, 0, 0);
            //}
            this.Rectangle = rectangle;
            this.Identifier = identifier;
        }

    }
}
