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
        public List<CombItem> CombItem = new List<CombItem>();

        
        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private Comb()
        {
            
        }
        private Comb(int _NLEmaxiter, int _PLdefloadstep, int _PLminloadstep, int _PLmaxeqiter, bool _NLSMohr, int _NLSinitloadstep, int _NLSminloadstep, int _NLSactiveelemratio, int _NLSplasticelemratio, int _CRloadstep, int _CRmaxiter, int _CRstifferror)
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

        /// <summary>Define calculation parameters for the Load combinations calculation type. To setup which analysis types to consider for a specific load combination - use LoadCombination.SetupCalculation (found under the Calculate category).</summary>
        /// <remarks>Create</remarks>
        /// <param name="NLEmaxiter">Non-linear elastic analysis: Maximum iteration number.</param>
        /// <param name="PLdefloadstep">Plastic analysis: Default load step in % of the total load.</param>
        /// <param name="PLminloadstep">Plastic analysis: Minimal load step [%]</param>
        /// <param name="PLmaxeqiter">Plastic analysis: Maximum equilibrium iteration number.</param>
        /// <param name="NLSMohr">Non-linear soil: Consider Mohr-Coulomb criteria.</param>
        /// <param name="NLSinitloadstep">Non-linear soil: Initial load step [%]</param>
        /// <param name="NLSminloadstep">Non-linear soil: Minimal load step [%]</param>
        /// <param name="NLSactiveelemratio">Non-linear soil: Volume ratio of nonlinearly active elements in one step [%]</param>
        /// <param name="NLSplasticelemratio">Non-linear soil: Volume ratio of plastic elements in one step [%]</param>
        /// <param name="CRloadstep">Cracked section analysis: One load step in % of the total load.</param>
        /// <param name="CRmaxiter">Cracked section analysis: Maximum iteration number.</param>
        /// <param name="CRstifferror">Cracked section analysis: Allowed stiffness change error [%]</param>
        public static Comb Define(int NLEmaxiter = 30, int PLdefloadstep = 20, int PLminloadstep = 2, int PLmaxeqiter = 30, bool NLSMohr = true, int NLSinitloadstep = 10, int NLSminloadstep = 10, int NLSactiveelemratio = 5, int NLSplasticelemratio = 5, int CRloadstep = 20, int CRmaxiter = 30, int CRstifferror = 2)
        {
            return new Comb(NLEmaxiter, PLdefloadstep, PLminloadstep, PLmaxeqiter, NLSMohr, NLSinitloadstep, NLSminloadstep, NLSactiveelemratio, NLSplasticelemratio, CRloadstep, CRmaxiter, CRstifferror);
        }

        /// <summary>Add load combination parameters to calculation options.</summary>
        /// <remarks>Private</remarks>
        /// <param name="loadCombination">LoadCombination</param>
        public void AddLoadCombinationParameters(Loads.LoadCombination loadCombination)
        {
            if (loadCombination.CombItem == null)
            {
                // pass
            }
            else
            {
                this.CombItem.Add(loadCombination.CombItem);
            }      
        }
    }
}