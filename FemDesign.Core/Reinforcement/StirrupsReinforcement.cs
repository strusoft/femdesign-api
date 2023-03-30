using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using FemDesign.GenericClasses;

namespace FemDesign.Reinforcement
{
    
    [XmlRoot("database", Namespace = "urn:strusoft")]
    [System.Serializable]
    public partial class StirrupReinforcement : BarReinforcement
    {
        private StirrupReinforcement()
        {
        }
        public StirrupReinforcement(Guid baseBar, Wire wire, Stirrups stirrups) : base(baseBar, wire, stirrups)
        {
        }
        public StirrupReinforcement(Bars.Bar bar, Wire wire, Stirrups stirrups) : base(bar, wire, stirrups)
        {
        }
        public StirrupReinforcement(Reinforcement.BarReinforcement reinf)
        {
            BaseBar = reinf.BaseBar;
            Wire = reinf.Wire;
            Stirrups = reinf.Stirrups;
        }
    }
}
