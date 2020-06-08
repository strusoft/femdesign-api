// https://strusoft.com/
using System;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// ANALCOMBITEM
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class CombItem
    {
        [XmlAttribute("NLE")]
        public int NLE { get; set; } // bool
        [XmlAttribute("PL")]
        public int PL { get; set; } // bool
        [XmlAttribute("NLS")]
        public int NLS { get; set; } // bool
        [XmlAttribute("Cr")]
        public int Cr { get; set; } // bool
        [XmlAttribute("f2nd")]
        public int f2nd { get; set; } // bool
        [XmlAttribute("Im")]
        public int Im { get; set; } // int
        [XmlAttribute("Waterlevel")]
        public int Waterlevel { get; set; } // int
        [XmlAttribute("ImpfRqd")]
        public int ImpfRqd { get; set; } // int
        [XmlAttribute("StabRqd")]
        public int StabRqd { get; set; } // int

        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CombItem()
        {
            
        }
        internal CombItem(int _ImpfRqd = 0, int _StabRqd = 0, bool _NLE = false, bool _PL = false, bool _NLS = false, bool _Cr = false, bool _f2nd = false, bool _Im = false, int _Waterlevel = 0)
        {
            this.NLE = Convert.ToInt32(_NLE);
            this.PL = Convert.ToInt32(_PL);
            this.NLS = Convert.ToInt32(_NLS);
            this.Cr = Convert.ToInt32(_Cr);
            this.f2nd = Convert.ToInt32(_f2nd);
            this.Im = Convert.ToInt32(_Im);
            this.Waterlevel = _Waterlevel;
            this.ImpfRqd = _ImpfRqd;
            this.StabRqd = _StabRqd;
        }
    }
}