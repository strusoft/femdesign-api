// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FemDesign.Calculate
{
    /// <summary>
    /// fdscript.xsd
    /// CmdConfig
    /// </summary>
    [XmlRoot("cmdconfig")]
    [System.Serializable]
    public partial class CmdConfig : CmdCommand
    {

        [XmlAttribute("command")]
        public string Command { get; set; } = "$ MODULECOM APPLYCFG";

        [XmlElement("CONFIG")]
        public List<CONFIG> Config = new List<CONFIG>();

        [XmlAttribute("file")]
        public string FilePath { get; set; }

        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdConfig()
        {

        }
        public CmdConfig(string filepath)
        {
            this.FilePath = System.IO.Path.GetFullPath(filepath);
        }

        public CmdConfig(params CONFIG[] configs)
        {
            Config = configs.ToList();
        }

        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdConfig>(this);
        }
    }

    //CODE GENERATED WITH THE HELP OF CHAT_GPT. WE NEED TO REVIEW IT AND ADD THE CORRECT TYPE. STRING, int, bool, DOUBLE

    [System.Serializable]
    public partial class CcmsConfig : CONFIG
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "CCMSCONFIG";

        [XmlAttribute("fIgnoreAnnexForShearStrength")]
        public bool IgnoreAnnexForShearStrength { get; set; }

        [XmlAttribute("StripeWidth")]
        public double StripeWidth { get; set; }

        public CcmsConfig(bool ignoreShearStrength, double stripeWidth)
        {
            IgnoreAnnexForShearStrength = ignoreShearStrength;
            StripeWidth = stripeWidth;
        }
    }

    [System.Serializable]
    public partial class EcrcConfig : CONFIG
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "ECRCCONFIG";
        [XmlAttribute("s2ndOrder")]
        public bool S2ndOrder { get; set; }

        public EcrcConfig(bool secondOrder)
        {
            S2ndOrder = secondOrder;
        }
    }

    [System.Serializable]
    public partial class EcDesparamPanelTmcLtFire : CONFIG
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "ECDESPARAMPANELTMCLTFIRE";

        [XmlAttribute("Beta0")]
        public double Beta0 { get; set; }

        [XmlAttribute("k2")]
        public double K2 { get; set; }

        [XmlAttribute("LimitUtilization")]
        public double LimitUtilization { get; set; }

        [XmlAttribute("tch")]
        public double Tch { get; set; }

        [XmlAttribute("tf")]
        public double Tf { get; set; }

        public EcDesparamPanelTmcLtFire(double beta0, double k2, double limitUtilisation, double tch, double tf)
        {
            Beta0 = beta0;
            K2 = k2;
            LimitUtilization = limitUtilisation;
            Tch = tch;
            Tf = tf;
        }
    }

    [System.Serializable]
    public partial class CcCoConfig : CONFIG
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "CCCOCONFIG";
    }

    [System.Serializable]
    public partial class EcstConfig : CONFIG
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "ECSTCONFIG";

        [XmlAttribute("sInteraction")]
        public bool Interaction { get; set; }

        public EcstConfig(bool interaction)
        {
            Interaction = interaction;
        }
    }

    [System.Serializable]
    public partial class CalcParamTimberPanelClt : CONFIG
    {
        [XmlAttribute("type")]
        public string Type = "CCCALCPARAMTMPANELCLT";

        [XmlAttribute("fCheckShearInteraction")]
        public bool CheckShearInteraction { get; set; }

        [XmlAttribute("fCheckTorsion")]
        public bool CheckTorsion { get; set; }

        [XmlAttribute("rPlankWidth")]
        public double PlankWidth { get; set; }
    }

    //[System.Serializable]
    //public class EcCalcParamTimberPanelFire
    //{
    //    [XmlAttribute("type")]
    //    public string Type { get; set; }

    //    [XmlAttribute("aDelaminationMultiplier_eBottom")]
    //    public string ADelaminationMultiplierEBottom { get; set; }

    //    [XmlAttribute("aDelaminationMultiplier_eTop")]
    //    public string ADelaminationMultiplierETop { get; set; }

    //    [XmlAttribute("afConsiderDelamination_eBottom")]
    //    public string AfConsiderDelaminationEBottom { get; set; }

    //    [XmlAttribute("afConsiderDelamination_eTop")]
    //    public string AfConsiderDelaminationETop { get; set; }

    //    [XmlAttribute("afFireExposed_eBottom")]
    //    public string AfFireExposedEBottom { get; set; }

    //    [XmlAttribute("afFireExposed_eTop")]
    //    public string AfFireExposedETop { get; set; }

    //    [XmlAttribute("afStructProtection_eBottom")]
    //    public string AfStructProtectionEBottom { get; set; }

    //    [XmlAttribute("afStructProtection_eTop")]
    //    public string AfStructProtectionETop { get; set; }

    //    [XmlAttribute("atchstruct_eBottom")]
    //    public string AtchStructEBottom { get; set; }

    //    [XmlAttribute("atchstruct_eTop")]
    //    public string AtchStructETop { get; set; }

    //    [XmlAttribute("beta0")]
    //    public string Beta0 { get; set; }

    //    [XmlAttribute("d0")]
    //    public string D0 { get; set; }

    //    [XmlAttribute("fAutoBeta")]
    //    public string FAutoBeta { get; set; }

    //    [XmlAttribute("fAutod0")]
    //    public string FAutod0 { get; set; }

    //    [XmlAttribute("fIgnoreReductionFactors")]
    //    public string FIgnoreReductionFactors { get; set; }

    //    [XmlAttribute("fNeglectThinLayers")]
    //    public string FNeglectThinLayers { get; set; }

    //    [XmlAttribute("rLimitThicknessForNeglection")]
    //    public string RLimitThicknessForNeglection { get; set; }

    //    [XmlAttribute("treq")]
    //    public string Treq { get; set; }
    //}



    /// <summary>
    /// Base class for all CONFIG that can be use for cmdconfig
    /// </summary>
    [System.Serializable]
    public abstract partial class CONFIG { }
}