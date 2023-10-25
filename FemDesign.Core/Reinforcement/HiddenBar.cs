// https://strusoft.com/
using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

using FemDesign.GenericClasses;
using FemDesign.Geometry;


namespace FemDesign.Reinforcement
{
    [System.Serializable]
    public partial class HiddenBar: NamedEntityBase, IStructureElement
    {
        private static int _hiddenBarInstances = 0;
        protected override int GetUniqueInstanceCount() => ++_hiddenBarInstances;

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

        [XmlAttribute("axisInLongerSide")]
        public bool AxisInLongerSide { get; set; }


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

        /// <summary>
        /// Concealed bar constructor.
        /// </summary>
        /// <param name="slab">Base shell element.</param>
        /// <param name="startPoint">The start point of the reactangle definition. Must be positioned on the SlabParts's region.</param>
        /// <param name="rectangle">Rectangle area where the concealed bar is specified. Must be inside the SlabPart region boundary.</param>
        /// <param name="identifier">Structural element identifier.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public HiddenBar(Shells.Slab slab, Point3d startPoint, RectangleType rectangle, string identifier = "CB")
        {
            if(slab.SlabPart.ComplexMaterial.Concrete == null)
            {
                throw new System.ArgumentException("Material of slab must be concrete!");
            }

            this.EntityCreated();
            this.BaseShell = slab.SlabPart.Guid;
            this.Start = startPoint;
            this.Rectangle = rectangle;
            this.AxisInLongerSide = true;
            this.Identifier = identifier;
        }

        /// <summary>
        /// Concealed bar constructor.
        /// </summary>
        /// <param name="slab">Base shell element.</param>
        /// <param name="startPoint">The start point of the reactangle definition. Must be positioned on the SlabParts's region.</param>
        /// <param name="rectangle">Rectangle area where the concealed bar is specified. Must be inside the SlabPart region boundary.</param>
        /// <param name="axisInLongerSide">If true, the axis of the concealed bar is parallel to the longer side of the rectangle, otherwise it is parallel to the shorter side.</param>
        /// <param name="identifier">Structural element identifier.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public HiddenBar(Shells.Slab slab, Point3d startPoint, RectangleType rectangle, bool axisInLongerSide = true, string identifier = "CB")
        {
            if (slab.SlabPart.ComplexMaterial.Concrete == null)
            {
                throw new System.ArgumentException("Material of slab must be concrete!");
            }

            this.EntityCreated();
            this.BaseShell = slab.SlabPart.Guid;
            this.Start = startPoint;
            this.Rectangle = rectangle;
            this.AxisInLongerSide = axisInLongerSide;
            this.Identifier = identifier;
        }

    }
}
