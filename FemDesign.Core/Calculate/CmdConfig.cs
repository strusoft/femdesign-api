using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using FemDesign.Results;
using System.Reflection;
using System.Xml.Linq;
using System.Text;

namespace FemDesign.Calculate
{
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

        private CcmsConfig()
        {

        }

        public CcmsConfig(bool ignoreShearStrength, double stripeWidth)
        {
            IgnoreAnnexForShearStrength = ignoreShearStrength;
            StripeWidth = stripeWidth;
        }
    }

    [XmlRoot("cmdconfig")]
    [System.Serializable]
    public partial class DesParamBarSteel : CONFIG
    {
        [XmlAttribute("type")]
        public string Type = "CCDESPARAMBARST";

        [XmlAttribute("LimitUtilization")]
        public double UtilizationLimit { get; set; } = 1.0;
        [XmlAttribute("vSection_itemcnt")]
        public int SectionCount { get; set; }

        [XmlAttribute("vSection_csec_{i}")]
        public List<Guid> _vSectionCsec { get; set; }

        [XmlIgnore]
        private List<FemDesign.Sections.Section> _familySections;

        [XmlIgnore]
        public List<FemDesign.Sections.Section> FamilySections
        {
            get
            {
                return _familySections;
            }
            set
            {
                if(value != null)
                {
                    _vSectionCsec = new List<Guid>();
                    _familySections = new List<FemDesign.Sections.Section>();
                    foreach (var section in value)
                    {
                        _vSectionCsec.Add(section.Guid);
                        _familySections.Add(section);
                    }
                    
                }
            }
        }

        private DesParamBarSteel()
        {

        }

        public DesParamBarSteel(double limitUtilization, List<Sections.Section> sections)
        {
            UtilizationLimit = limitUtilization;
            FamilySections = sections;
            SectionCount = sections.Count;
        }
    }


    [XmlRoot("cmdconfig")]
    [System.Serializable]
    public partial class EcrcConfig : CONFIG
    {
        [XmlAttribute("type")]
        public string Type = "ECRCCONFIG";

        [XmlAttribute("s2ndOrder")]
        public bool S2ndOrder { get; set; }

        private EcrcConfig()
        {

        }

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

        private EcDesparamPanelTmcLtFire()
        {

        }

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

        public CcCoConfig()
        {

        }

    }

    [System.Serializable]
    public partial class EcstConfig : CONFIG
    {
        [XmlAttribute("type")]
        public string Type { get; set; } = "ECSTCONFIG";

        [XmlAttribute("sInteraction")]
        public bool Interaction { get; set; }

        private EcstConfig()
        {

        }

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

        private CalcParamTimberPanelClt()
        {

        }

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
    /// 
    [XmlInclude(typeof(CcmsConfig))]
    [XmlInclude(typeof(EcrcConfig))]
    [XmlInclude(typeof(EcDesparamPanelTmcLtFire))]
    [XmlInclude(typeof(CcCoConfig))]
    [XmlInclude(typeof(EcstConfig))]
    [XmlInclude(typeof(CalcParamTimberPanelClt))]
    [XmlInclude(typeof(DesParamBarSteel))]
    [System.Serializable]
    public abstract partial class CONFIG
    {
    
    }
} 