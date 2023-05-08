using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;
using FemDesign.GenericClasses;
using System.Drawing;

namespace FemDesign.Calculate
{
    internal class CmdDesignGroup
    {
        [XmlAttribute("command")]
        public string Command = "$ CODE_COM DESGROUP"; // token, fixed
        /// <summary>
        /// Name group
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }
        /// <summary>
        /// Color group
        /// </summary>
        [XmlAttribute("color")]
        public Color? Color { get; set; }
        /// <summary>
        /// To delete a group use force="true" and empty guid list.
        /// </summary>
        [XmlAttribute("force")]
        public bool Force { get; set; } = false;
        [XmlAttribute("type")]
        public DesignGroupType Type { get; set; }
        /// <summary>
        /// Entities Guid.
        /// all entities guids must be conforming to the group type. remember that you must pass analytical entities!
        /// </summary>
        [XmlElement("GUID")]
        public List<Guid> Guids { get; set; }
        [XmlIgnore]
        public List<FemDesign.GenericClasses.IStructureElement> Elements { get; set; }

        private CmdDesignGroup()
        {

        }

        /// <summary>
        /// Define a design group of bars
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bars"></param>
        /// <param name="type"></param>
        /// <param name="color"></param>
        public CmdDesignGroup(string name, List<FemDesign.Bars.Bar> bars, DesignGroupType type, Color? color = null)
        {
            Name = name;
            Guids = bars.Select(x => x.BarPart.Guid).ToList();
            Type = type;
            if (color == null)
            {
                var rnd = new Random();
                color = System.Drawing.Color.FromArgb(255, 0, 0);
            }
            Color = color;
        }


        //public CmdDesignGroup(string name, List<FemDesign.GenericClasses.IStructureElement> elements, DesignGroupType type, Color? color = null)
        //{
        //    Name = name;
        //    Guids = elements.Cast<FemDesign.Bars.Bar>().Select(x => x.BarPart.Guid).ToList();
        //    Type = type;
        //    if (color == null)
        //    {
        //        var rnd = new Random();
        //        color = System.Drawing.Color.FromArgb(255, 0, 0);
        //    }
        //    Color = color;
        //}


        public static CmdDesignGroup CmdSteelBarDesignGroup(string name, List<FemDesign.Bars.Bar> bars, Color? color = null)
        {
            var cmdDesignGroup = new CmdDesignGroup(name, bars, DesignGroupType.SteelBars, color);
            return cmdDesignGroup;
        }

        //TODO
        // The design group require to have elements of the same type.
        // i.e. not mixed list of TimberBar, SteelBar and ConcreteBar.
        // Additionally, Concrete Bar group is required to have elements with same bar length.
        private bool _validateGroup()
        {
            return true;
        }
    }

    /// <summary>
    /// Type of elements to set on a design group.
    /// </summary>
    public enum DesignGroupType
    {
        [XmlEnum("FNISOLATED")]
        FoundationIsolated,
        [XmlEnum("FNWALL")]
        FoundationWall,
        [XmlEnum("FNSLAB")]
        FoundationSlab,
        //[XmlEnum("RCSURFLONG")]
        //RCSurfaceLong,
        [XmlEnum("RCSURFSHEAR")]
        RCSurfaceShear,
        [XmlEnum("RCPUNCH")]
        RCPunchingShear,
        [XmlEnum("STBARSHELL")]
        SteelShellBars,
        [XmlEnum("STJOINT")]
        SteelJoints,
        [XmlEnum("TMBAR")]
        TimberBars,
        [XmlEnum("TMPANEL")]
        TimberPanels,
        [XmlEnum("TMPANELCLT")]
        TimberPanelCLT,
        [XmlEnum("MS")]
        Masonry,
        [XmlEnum("COCOLUMN")]
        ConcreteColumns,
        [XmlEnum("CODELTABEAM")]
        ConcreteDeltaBeams,
        [XmlEnum("RCBAR")]
        RCBars,
        [XmlEnum("RCHIDDENBAR")]
        RCHiddenBars,
        [XmlElement("STBAR")]
        SteelBars,

        //[XmlEnum("STFIRE")]
        //FireSteelBars,
        //[XmlEnum("TMFIRE")]
        //FireTorsionBars,
        //[XmlEnum("CLTFIRE")]
        //FireanelCLT
    }


}