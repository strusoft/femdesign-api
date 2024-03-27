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
        /// <summary>
        /// Maximum iteration number
        /// </summary>
        [XmlAttribute("NLEmaxiter")]
        public int NLEmaxiter { get; set; } = 30;
        
        /// <summary>
        /// Deafult load step in % of the total load
        /// </summary>
        [XmlAttribute("PLdefloadstep")]
        public int PLdefloadstep { get; set; } = 20;

        /// <summary>
        /// Minimum load step [%]
        /// </summary>
        [XmlAttribute("PLminloadstep")]
        public int PLminloadstep { get; set; } = 2;

        /// <summary>
        /// Keep reduced load step after it has been reduced by the solver
        /// </summary>
        [XmlAttribute("PlKeepLoadStep")]
        public bool PlKeepLoadStep { get; set; } = true;

        /// <summary>
        /// Golbal tolerance value [‰]
        /// </summary>
        [XmlAttribute("PlTolerance")]
        public int PlTolerance { get; set; } = 1;

        /// <summary>
        /// Maximum equilibrium iteration number
        /// </summary>
        [XmlAttribute("PLmaxeqiter")]
        public int PLmaxeqiter { get; set; } = 50;

        /// <summary>
        /// Number of layers in the elasto-plastic shells
        /// </summary>
        [XmlAttribute("PlShellLayers")]
        public int PlShellLayers { get; set; } = 10;
        
        /// <summary>
        /// Consider Mohr-Coulomb criteria
        /// </summary>
        [XmlAttribute("NLSMohr")]
        public bool NLSMohr { get; set; } = true;
        
        /// <summary>
        /// Initial load step [%]
        /// </summary>
        [XmlAttribute("NLSinitloadstep")]
        public int NLSinitloadstep { get; set; } = 10;
        
        /// <summary>
        /// Minimum load step [%]
        /// </summary>
        [XmlAttribute("NLSminloadstep")]
        public int NLSminloadstep { get; set; } = 10;
        
        /// <summary>
        /// Volume ratio of nonlinearly active elements in one step [%]
        /// </summary>
        [XmlAttribute("NLSactiveelemratio")]
        public int NLSactiveelemratio { get; set; } = 5;
        
        /// <summary>
        /// Volume ratio of plastic elements in one step [%]
        /// </summary>
        [XmlAttribute("NLSplasticelemratio")]
        public int NLSplasticelemratio { get; set; } = 5;

        /// <summary>
        /// One load step in % of the total load
        /// </summary>
        [XmlAttribute("CRloadstep")]
        public int CRloadstep { get; set; } = 20;

        /// <summary>
        /// Maximum iteration number
        /// </summary>
        [XmlAttribute("CRmaxiter")]
        public int CRmaxiter { get; set; } = 30;
        
        /// <summary>
        /// Allowed stiffness change error [%]
        /// </summary>
        [XmlAttribute("CRstifferror")]
        public int CRstifferror { get; set; } = 2;

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
            this.NLSMohr = _NLSMohr;
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