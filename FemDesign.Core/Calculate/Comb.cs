// https://strusoft.com/
using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.Calculate
{

    /// <summary>
    /// fdscript.xsd
    /// ANALCOMB
    /// </summary>
    public partial class Comb
    {
        [XmlAttribute("NLEmaxiter")]
        public int NLEmaxiter { get; set; } // int
        [XmlAttribute("PLdefloadstep")]
        public int PLdefloadstep { get; set; } // int
        [XmlAttribute("PLminloadstep")]
        public int PLminloadstep { get; set; } // int
        [XmlAttribute("PLmaxeqiter")]
        public int PLmaxeqiter { get; set; } // int
        [XmlAttribute("CRloadstep")]
        public int CRloadstep { get; set; } // int
        [XmlAttribute("CRmaxiter")]
        public int CRmaxiter { get; set; } // int
        [XmlAttribute("CRstifferror")]
        public int CRstifferror { get; set; } // int
        [XmlAttribute("NLSMohr")]
        public int NLSMohr { get; set; } // bool
        [XmlAttribute("NLSinitloadstep")]
        public int NLSinitloadstep { get; set; } // int
        [XmlAttribute("NLSminloadstep")]
        public int NLSminloadstep { get; set; } // int
        [XmlAttribute("NLSactiveelemratio")]
        public int NLSactiveelemratio { get; set; } // int
        [XmlAttribute("NLSplasticelemratio")]
        public int NLSplasticelemratio { get; set; } // int

        [XmlElement("combitem")]
        public List<CombItem> CombItem { get; set; }
        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Comb()
        {
            
        }
        public Comb(int _NLEmaxiter = 30, int _PLdefloadstep = 20, int _PLminloadstep = 2, int _PLmaxeqiter = 30, bool _NLSMohr = true, int _NLSinitloadstep = 10, int _NLSminloadstep = 10, int _NLSactiveelemratio = 5, int _NLSplasticelemratio = 5, int _CRloadstep = 20, int _CRmaxiter = 30, int _CRstifferror = 2, List<CombItem> CombItem = null)
        {
            this.NLEmaxiter = _NLEmaxiter;
            this.PLdefloadstep = _PLdefloadstep;
            this.PLminloadstep = _PLminloadstep;
            this.PLmaxeqiter = _PLmaxeqiter;
            this.NLSMohr = Convert.ToInt32(_NLSMohr);
            this.NLSinitloadstep = _NLSinitloadstep;
            this.NLSminloadstep = _NLSminloadstep;
            this.NLSactiveelemratio = _NLSactiveelemratio;
            this.NLSplasticelemratio = _NLSplasticelemratio;
            this.CRloadstep = _CRloadstep;
            this.CRmaxiter = _CRmaxiter;
            this.CRstifferror = _CRstifferror;

            this.CombItem = CombItem ?? new List<CombItem>();
        }

        /// <summary>
        /// Set default calculation parameters for load combinations.
        /// </summary>
        /// <remarks>Create</remarks>
        /// <returns></returns>
        public static Comb Default()
        {
            int NLEmaxiter = 30;
            int PLdefloadstep = 20;
            int PLminloadstep = 2;
            int PLmaxeqiter = 30;
            bool NLSMohr = true;
            int NLSinitloadstep = 10;
            int NLSminloadstep = 10;
            int NLSactiveelemratio = 5;
            int NLSplasticelemratio = 5;
            int CRloadstep = 20;
            int CRmaxiter = 30;
            int CRstifferror = 2;
            return new Comb(NLEmaxiter, PLdefloadstep, PLminloadstep, PLmaxeqiter, NLSMohr, NLSinitloadstep, NLSminloadstep, NLSactiveelemratio, NLSplasticelemratio, CRloadstep, CRmaxiter, CRstifferror);
        }

    }
}