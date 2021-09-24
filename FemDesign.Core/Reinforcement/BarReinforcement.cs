using System;
using System.Xml.Serialization;
using FemDesign.GenericClasses;


namespace FemDesign.Reinforcement
{
    [System.Serializable]
    public partial class BarReinforcement: EntityBase, IStructureElement
    {
        [XmlElement("base_bar", Order = 1)]
        public GuidListType BaseBar { get; set; }

        [XmlElement("wire", Order = 2)]
        public Wire Wire { get; set; }

        // choice stirrups
        [XmlElement("stirrups", Order = 3)]
        public Stirrups _stirrups;
        [XmlIgnore]
        public Stirrups Stirrups
        {
            get
            {
                return this._stirrups;
            }
            set
            {
                if (this.LongitudinalBar != null)
                {
                    throw new System.ArgumentException("Can't set stirrups to bar reinforcement as it already contains stirrups. Must be either or.");
                }
                else
                {
                    this._stirrups = value;
                }
            }
        }

        // choice longitudinal bar
        [XmlElement("longitudinal_bar", Order = 4)]
        public LongitudinalBar _longitudinalBar;
        [XmlIgnore]
        public LongitudinalBar LongitudinalBar
        {
            get
            {
                return this._longitudinalBar;
            }
            set
            {
                if (this.Stirrups != null)
                {
                    throw new System.ArgumentException("Can't set longitudinal bars to bar reinforcement as it already contains stirrups. Must be either or.");
                }
                else
                {
                    this._longitudinalBar = value;
                }
            }
        }

        /// <summary>
        /// Parameterless constructor for serialization
        /// </summary>
        public BarReinforcement()
        {

        }

        /// <summary>
        /// Construct stirrup bar reinforcement
        /// </summary>
        public BarReinforcement(Guid baseBar, Wire wire, Stirrups stirrups)
        {
            this.EntityCreated();
            this.BaseBar = new GuidListType(baseBar);
            this.Wire = wire;
            this.Stirrups = stirrups;
        }

        /// <summary>
        /// Construct stirrup bar reinforcement
        /// </summary>
        public BarReinforcement(Bars.Bar bar, Wire wire, Stirrups stirrups)
        {
            this.EntityCreated();
            this.BaseBar = new GuidListType(bar.BarPart.Guid);
            this.Wire = wire;
            this.Stirrups = stirrups;
        }
        
        /// <summary>
        /// Construct longitudinal bar reinforcement
        /// </summary>
        public BarReinforcement(Guid baseBar, Wire wire, LongitudinalBar longBar)
        {
            this.EntityCreated();
            this.BaseBar = new GuidListType(baseBar);
            this.Wire = wire;
            this.LongitudinalBar = longBar;
        }

        /// <summary>
        /// Construct longitudinal bar reinforcement
        /// </summary>
        public BarReinforcement(Bars.Bar bar, Wire wire, LongitudinalBar longBar)
        {
            this.EntityCreated();
            this.BaseBar = new GuidListType(bar.BarPart.Guid);
            this.Wire = wire;
            this.LongitudinalBar = longBar;
        }
    }
}