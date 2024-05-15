// https://strusoft.com/
using System;
using System.Collections.Generic;
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
        [XmlAttribute("Calc")]
        public int _calc { get; set; } = 1;

        /// <summary>
        /// Calculate load combination (linear analysis).
        /// </summary>
        [XmlIgnore]
        public bool Calc
        {
            get 
            { 
                return Convert.ToBoolean(_calc); 
            }
            set 
            { 
                _calc = Convert.ToInt32(value);
                if(!value)
                {
                    NLE = PL = NLS = Cr = f2nd = false;
                }
            }
        }

        /// <summary>
        /// If true, no calculations will be executed for load combinations.
        /// </summary>
        [XmlIgnore]
        public bool NoCalc
        {
            set
            {
                Calc = !value;
            }
        }

        [XmlAttribute("NLE")]
        public int _nle { get; set; }

        /// <summary>
        /// Consider elastic nonlinear behaviour of structural elements.
        /// </summary>
        [XmlIgnore]
        public bool NLE
        {
            get 
            { 
                return Convert.ToBoolean(_nle); 
            }
            set 
            { 
                _nle = Convert.ToInt32(value);
                if(!value)
                    PL = false;
            }
        }


        [XmlAttribute("PL")]
        public int _pl { get; set; }

        /// <summary>
        /// Consider plastic behaviour of structural elements.
        /// </summary>
        [XmlIgnore]
        public bool PL
        {
            get 
            { 
                return Convert.ToBoolean(_pl); 
            }
            set 
            { 
                _pl = Convert.ToInt32(value);
                if (value) NLE = true; 
            }
        }


        [XmlAttribute("NLS")]
        public int _nls { get; set; }

        /// <summary>
        /// Consider nonlinear behaviour of soil.
        /// </summary>
        [XmlIgnore]
        public bool NLS
        {
            get { return Convert.ToBoolean(_nls); }
            set { _nls = Convert.ToInt32(value); }
        }

        [XmlAttribute("Cr")]
        public int _cr { get; set; }

        /// <summary>
        /// Cracked section analysis. Note that Cr only executes properly in RCDesign with DesignCheck set to true.
        /// </summary>
        [XmlIgnore]
        public bool Cr
        {
            get 
            { 
                return Convert.ToBoolean(_cr); 
            }
            set 
            { 
                _cr = Convert.ToInt32(value);
                if (value) PL = false;
            }
        }


        [XmlAttribute("f2nd")]
        public int _f2nd { get; set; }

        /// <summary>
        /// 2nd order analysis.
        /// </summary>
        [XmlIgnore]
        public bool f2nd
        {
            get { return Convert.ToBoolean(_f2nd); }
            set { _f2nd = Convert.ToInt32(value); }
        }


        [XmlAttribute("Im")]
        public int Im { get; set; }

        [XmlAttribute("Waterlevel")]
        public int Waterlevel { get; set; }

        [XmlAttribute("ImpfRqd")]
        public int ImpfRqd { get; set; }

        [XmlAttribute("StabRqd")]
        public int StabRqd { get; set; }

        [XmlAttribute("Amplitudo")]
        public double Amplitude { get; set; }

        [XmlIgnore]
        public string CombName { get; set; }

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
        /// <param name="amplitude">Amplitude of selected imperfection shape.</param>
        /// <param name="waterlevel">Ground water level.</param>
        // <param name="Amplitude">Amplitude of selected imperfection shape.</param> // TODO Amplitude?
        public CombItem(int impfRqd = 0, int stabReq = 0, bool NLE = true, bool PL = true, bool NLS = false, bool Cr = false, bool f2nd = false, int Im = 0, double amplitude = 0.0, int waterlevel = 0)
        {
            this.NLE = NLE;
            this.PL = PL;
            this.NLS = NLS;
            this.Cr = Cr;
            this.f2nd = f2nd;
            this.Im = Im;
            this.Waterlevel = waterlevel;
            this.ImpfRqd = impfRqd;
            this.Amplitude = amplitude;
            this.StabRqd = stabReq;
        }

        /// <summary>
        /// Load combination-specific settings for calculations.
        /// </summary>
        /// <param name="combName">Load combination name.</param>
        /// <param name="impfRqd">Required imperfection shapes.</param >
        /// <param name="stabReq">Required buckling shapes for stability analysis.</param>
        /// <param name="NLE">Consider elastic nonlinear behaviour of structural elements.</param>
        /// <param name="PL">Consider plastic behaviour of structural elements.</param>
        /// <param name="NLS">Consider nonlinear behaviour of soil.</param>
        /// <param name="Cr">Cracked section analysis. Note that Cr only executes properly in RCDesign with DesignCheck set to true.</param>
        /// <param name="f2nd">2nd order analysis.</param>
        /// <param name="Im">Imperfection shape for 2nd order analysis.</param>
        /// <param name="amplitude">Amplitude of selected imperfection shape.</param>
        /// <param name="waterlevel">Ground water level.</param>
        // <param name="Amplitude">Amplitude of selected imperfection shape.</param> // TODO Amplitude?
        public CombItem(string combName, int impfRqd = 0, int stabReq = 0, bool NLE = true, bool PL = true, bool NLS = false, bool Cr = false, bool f2nd = false, int Im = 0, double amplitude = 0.0, int waterlevel = 0)
        {
            this.CombName = combName;
            this.NLE = NLE;
            this.PL = PL;
            this.NLS = NLS;
            this.Cr = Cr;
            this.f2nd = f2nd;
            this.Im = Im;
            this.Waterlevel = waterlevel;
            this.ImpfRqd = impfRqd;
            this.Amplitude = amplitude;
            this.StabRqd = stabReq;
        }

        public static CombItem Stability(int stabReq)
        {
            var combItem = new CombItem(stabReq: stabReq);
            return combItem;
        }

        public static CombItem Imperfection(int impfRqd)
        {
            var combItem = new CombItem(impfRqd: impfRqd);
            return combItem;
        }

        public static CombItem NonLinear(bool plastic = true)
        {
            var combItem = new CombItem(NLE: true, PL: plastic);
            return combItem;
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
            double amplitude = 0.0;
            int waterlevel = 0;

            var combItem = new CombItem(impfRqd, stabRqd, NLE, PL, NLS, Cr, f2nd, im, amplitude, waterlevel);
            return combItem;
        }
    }
}
