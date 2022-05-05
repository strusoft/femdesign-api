using System;
using System.Collections.Generic;
using System.Linq;
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

        [XmlIgnore]
        public bool IsStirrups
        {
            get
            {
                if (this._longitudinalBar == null && this._stirrups == null)
                {
                    throw new System.ArgumentException($"No stirrups or longitudinal bars are defined on this bar reinforcement object {this.Guid}. This object is not correctly constructed.");
                }
                else if (this._longitudinalBar != null && this._stirrups != null)
                {
                    throw new System.ArgumentException($"Both stirrups and longitudinal bars are defined on this bar reinforcement object {this.Guid}. This is not allowed.");
                }
                else if (this._longitudinalBar != null && this._stirrups == null)
                {
                    return false;
                }
                else if (this._longitudinalBar == null && this._stirrups != null)
                {
                    return true;
                }
                else
                {
                    throw new System.ArgumentException($"Ambiguous object {this.Guid}. Can't decide if stirrups or longitudinal bars");
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

        /// <summary>
        /// Add reinforcement to bar.
        /// Internal method use by GH components and Dynamo nodes.
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="rebar"></param>
        /// <param name="overwrite">Overwrite rebar on bar if a rebar sharing guid already exists on the bar?</param>
        public static Bars.Bar AddReinforcementToBar(Bars.Bar bar, List<BarReinforcement> rebar, bool overwrite)
        {
            // check if bar is curved
            if (!bar.BarPart.Edge.IsLine())
            {
                throw new System.ArgumentException($"Bar with guid: {bar.Guid} is not straight. Reinforcement can only be added to straight bars.");
            }

            // check if bar material is concrete
            if (bar.BarPart.ComplexMaterialObj.Concrete == null)
            {
                throw new System.ArgumentException("Material of bar must be concrete");
            }

            foreach (BarReinforcement item in rebar)
            {
                // empty base bar - update with current barPart guid
                if (item.BaseBar.Guid == Guid.Empty)
                {
                    item.BaseBar.Guid = bar.BarPart.Guid;
                }

                // base bar equals current barPart guid 
                else if (item.BaseBar.Guid == bar.BarPart.Guid)
                {
                    // pass
                }

                // base bar does not equal current barPart guid - reinforcement probably added to another bar already.
                else if (item.BaseBar.Guid != bar.BarPart.Guid )
                {
                    throw new System.ArgumentException($"{item.GetType().FullName} with guid: {item.Guid} has a base bar guid: {item.BaseBar.Guid} that does not correnspond with the current bar");
                }

                // add reinforcement to current bar
                bool exists = bar.Reinforcement.Any(x => x.Guid == item.Guid);
                if (exists)
                {
                    if (overwrite)
                    {
                        bar.Reinforcement.RemoveAll(x => x.Guid == item.Guid);
                        bar.Reinforcement.Add(item);
                    }
                    else
                    {
                        throw new System.ArgumentException($"{item.GetType().FullName} with guid: {item.Guid} has already been added to the bar. Are you adding the same element twice?");
                    }
                }
                else
                {
                    bar.Reinforcement.Add(item);
                }
            }
            return bar;
        }
    }
}