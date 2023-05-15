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
    [XmlRoot("cmddesgroup")]
    [System.Serializable]
    public class CmdDesignGroup : CmdCommand
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
        public string _color { get; set; } = "0xff0000";
        /// <summary>
        /// Color group
        /// </summary>
        [XmlIgnore]
        public Color? Color
        {
            get
            {
                Color col = System.Drawing.ColorTranslator.FromHtml("#" + this._color.Remove(0,2));
                return col;
            }
            set
            {
                this._color = "0x" + ColorTranslator.ToHtml((Color)value).Substring(1);
            }
        }
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
        /// <param name="elements"></param>
        /// <param name="type"></param>
        /// <param name="color"></param>
        public CmdDesignGroup(string name, List<FemDesign.GenericClasses.IStructureElement> elements, DesignGroupType type, Color? color = null) : this(name, elements, color)
        {
            Type = type;
        }


        /// <summary>
        /// Define a design group of bars
        /// </summary>
        /// <param name="name"></param>
        /// <param name="elements"></param>
        /// <param name="color"></param>
        public CmdDesignGroup(string name, List<FemDesign.GenericClasses.IStructureElement> elements, Color? color = null)
        {
            _validateGroup(elements);

            Name = name;
            if (elements[0] is FemDesign.Bars.Bar)
            {
                var bars = elements.Cast<FemDesign.Bars.Bar>().ToList();
                Guids = bars.Select(x => x.BarPart.Guid).ToList();
            }
            else if (elements[0] is FemDesign.Shells.Slab)
            {
                var slabs = elements.Cast<FemDesign.Shells.Slab>().ToList();
                Guids = slabs.Select(x => x.SlabPart.Guid).ToList();
            }
            else
                throw new Exception($"There is not Design Group Type eligible for {elements[0].GetType().Name}");

            if (color == null)
            {
                var rnd = new Random();
                color = System.Drawing.Color.FromArgb(255, 0, 0);
            }
            Elements = elements;
            Color = color;
            Type = assignGroupType(elements);
        }

        public static CmdDesignGroup CmdSteelBarDesignGroup(string name, List<FemDesign.GenericClasses.IStructureElement> elements, Color? color = null)
        {
            var cmdDesignGroup = new CmdDesignGroup(name, elements, DesignGroupType.SteelBars, color);
            return cmdDesignGroup;
        }

        //TODO
        // The design group require to have elements of the same type.
        // i.e. not mixed list of TimberBar, SteelBar and ConcreteBar.
        // Additionally, Concrete Bar group is required to have elements with same bar length.
        private void _validateGroup(List<FemDesign.GenericClasses.IStructureElement> elements)
        {
            isSameType(elements);
            //hasSameMaterial(elements);
        }

        private void isSameType(List<FemDesign.GenericClasses.IStructureElement> elements)
        {
            var barCount = elements.OfType<FemDesign.Bars.Bar>().Count();
            var slabCount = elements.OfType<FemDesign.Shells.Slab>().Count();
            if (barCount != elements.Count && slabCount != elements.Count)
            {
                throw new Exception("The list of elements contains objects of different types such as Bar, Slab, and Wall.The list must only include objects of the same type.");
            }
        }


        public override XElement ToXElement()
        {
            return Extension.ToXElement<CmdDesignGroup>(this);
        }

        private DesignGroupType assignGroupType(List<FemDesign.GenericClasses.IStructureElement> elements)
        {
            if (elements[0] is FemDesign.Bars.Bar)
            {
                var families = elements.Cast<FemDesign.Bars.Bar>().Select(x => x.BarPart.ComplexMaterialObj.Family).Distinct().ToList().First();

                switch (families)
                {
                    case Materials.Family.Steel:
                        return DesignGroupType.SteelBars;
                    case Materials.Family.Concrete:
                        return DesignGroupType.RCBars;
                    case Materials.Family.Timber:
                        return DesignGroupType.TimberBars;
                    default:
                        throw new NotImplementedException("Only the TimberCLTPanel type is eligible for the creation of a design group. It's not possible to create a design group for other types.");
                }
            }
            else if (elements[0] is FemDesign.Shells.Panel)
            {
                var families = elements.Cast<FemDesign.Shells.Panel>().Select(x => x.Material.Family).Distinct().ToList().First();

                if (families == Materials.Family.Timber)
                    return DesignGroupType.TimberPanelCLT;
                else
                    throw new NotImplementedException("Only the TimberCLTPanel type is eligible for the creation of a design group. It's not possible to create a design group for other types.");
            }
            else
                throw new Exception($"There is not Design Group Type eligible for {elements[0].GetType().Name}");
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
        [XmlEnum("RCSURFLONG")]
        RCSurfaceLong,
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
        [XmlEnum("STBAR")]
        SteelBars,

        //[XmlEnum("STFIRE")]
        //FireSteelBars,
        //[XmlEnum("TMFIRE")]
        //FireTorsionBars,
        //[XmlEnum("CLTFIRE")]
        //FireanelCLT
    }


}