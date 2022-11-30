// https://strusoft.com/
using System;
using System.Xml.Serialization;


namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// ANALCOMBITEM
    /// </summary>
    [System.Serializable]
    public partial class CombItem
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

        /// <summary>
        /// Load combination-specific settings for calculations.
        /// </summary>
        /// <param name="impfRqd">Required imperfection shapes.</param >
        /// <param name="stabReq">Required buckling shapes for stability analysis.</param>
        /// <param name="NLE">Consider elastic nonlinear behaviour of structural elements.</param>
        /// <param name="PL">Consider plastic behaviour of structural elements.</param>
        /// <param name="NLS">Consider nonlinear behaviour of soil.</param>
        /// <param name="Cr">Cracked section analysis. Note that Cr only executes properly in RCDesign with DesignCheck set to true.</param>
        /// <param name="f2nd">2nd order analysis.</param>
        /// <param name="Im">Imperfection shape for 2nd order analysis.</param>
        /// <param name="waterlevel">Ground water level.</param>
        // <param name="Amplitude">Amplitude of selected imperfection shape.</param> // TODO Amplitude?
        public CombItem(int impfRqd = 0, int stabReq = 0, bool NLE = true, bool PL = true, bool NLS = false, bool Cr = false, bool f2nd = false, int Im = 0, int waterlevel = 0)
        {
            this.NLE = Convert.ToInt32(NLE);
            this.PL = Convert.ToInt32(PL);
            this.NLS = Convert.ToInt32(NLS);
            this.Cr = Convert.ToInt32(Cr);
            this.f2nd = Convert.ToInt32(f2nd);
            this.Im = Im;
            this.Waterlevel = waterlevel;
            this.ImpfRqd = impfRqd;
            this.StabRqd = stabReq;
        }

        public static CombItem Default()
        {
            int impfRqd = 0;
            int stabRqd = 0;
            bool NLE = true;
            bool PL = true;
            bool NLS = false;
            bool Cr = false;
            bool f2nd = false;
            int im = 0;
            int waterlevel = 0;

            var combItem = new CombItem(impfRqd, stabRqd, NLE, PL, NLS, Cr, f2nd, im, waterlevel);
            return combItem;
        }
    }
}
