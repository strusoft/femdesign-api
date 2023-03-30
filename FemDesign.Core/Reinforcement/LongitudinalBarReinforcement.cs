using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Reinforcement
{
    [XmlRoot("database", Namespace = "urn:strusoft")]
    [System.Serializable]
    public partial class LongitudinalBarReinforcement : BarReinforcement
    {
        private LongitudinalBarReinforcement()
        {
        }
        public LongitudinalBarReinforcement(Guid baseBar, Wire wire, LongitudinalBar longBar) : base(baseBar, wire, longBar)
        {
        }
        public LongitudinalBarReinforcement(Bars.Bar bar, Wire wire, LongitudinalBar longBar) : base(bar, wire, longBar)
        {
        }
        public LongitudinalBarReinforcement(Reinforcement.BarReinforcement reinf)
        {
            BaseBar = reinf.BaseBar;
            Wire = reinf.Wire;
            LongitudinalBar = reinf.LongitudinalBar;
        }
       
    }
}
