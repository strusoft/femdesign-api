// https://strusoft.com/
using System.Xml.Serialization;
using System.Xml.Linq;
using System;
using System.Collections.Generic;
using FemDesign.Loads;

namespace FemDesign.Calculate
{

    /// <summary>
    /// command to generate load combinations from load groups.
    /// Similar "Generate" dialog from Load groups
    ///Guids designate load groups for filtering; all used is empty
    /// </summary>
    [XmlRoot("cmdldgroup2comb")]
    [System.Serializable]
    public partial class CmdLoadGroupToLoadComb : CmdCommand
    {
        public string Command = "$ LOAD LDGROUP2COMB"; // token

        [XmlElement("GUID")]
        public List<Guid> Guid { get; set; }

        [XmlAttribute("fU")]
        public bool fU { get; set; } = true;

        [XmlAttribute("fUa")]
        public bool fUa { get; set; } = true;

        [XmlAttribute("fUs")]
        public bool fUs { get; set; } = true;

        [XmlAttribute("fSq")]
        public bool fSq { get; set; } = true;

        [XmlAttribute("fSf")]
        public bool fSf { get; set; } = true;

        [XmlAttribute("fSc")]
        public bool fSc { get; set; } = true;

        [XmlAttribute("fSeisSigned")]
        public bool fSeisSigned { get; set; } = true;

        [XmlAttribute("fSeisTorsion")]
        public bool fSeisTorsion { get; set; } = true;

        [XmlAttribute("fSeisZdir")]
        public bool fSeisZdir { get; set; } = false;

        [XmlAttribute("fSkipMinDL")]
        public bool fSkipMinDL { get; set; } = true;

        [XmlAttribute("fForceTemp")]
        public bool fForceTemp { get; set; } = true;

        [XmlAttribute("fShortName")]
        public bool fShortName { get; set; } = true;


        /// <summary>
        /// Parameterless constructor for serialization.
        /// </summary>
        private CmdLoadGroupToLoadComb()
        {

        }

        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdLoadGroupToLoadComb>(this);
        }
    }


}