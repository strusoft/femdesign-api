// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign
{
    /// <summary>
    /// entities.
    /// </summary>
    [System.Serializable]
    public partial class Entities
    {
        // dummy elements are needed to deserialize an .struxml model correctly as order of elements is needed.
        // if dummy elements are not used for undefined types deserialization will not work properly
        // when serializing these dummy elements must be nulled. 
        [XmlElement("foundations", Order = 1)]
        public List<DummyXmlObject> Foundations {get {return null;} set {value = null;}}

        [XmlElement("bar", Order = 2)]
        public List<Bars.Bar> Bars = new List<Bars.Bar>();

        [XmlElement("column_corbel", Order = 3)]
        public List<Bars.ColumnCorbel> ColumnCorbel { get; set; }

        [XmlElement("hidden_bar", Order = 4)]
        public Reinforcement.HiddenBar[] HiddenBar { get; set; }

        [XmlElement("bar_reinforcement", Order = 5)]
        public Reinforcement.BarReinforcement[] BarReinforcement { get; set; }

        [XmlElement("slab", Order = 6)]
        public List<Shells.Slab> Slabs = new List<Shells.Slab>();

        [XmlElement("shell_buckling", Order = 7)]
        public Shells.ShellBucklingType[] ShellBuckling { get; set; }

        [XmlElement("wall_corbel", Order = 8)]
        public List<Shells.WallCorbel> WallCorbel { get; set; }

        [XmlElement("surface_reinforcement_parameters", Order = 9)]
        public List<Reinforcement.SurfaceReinforcementParameters> SurfaceReinforcementParameters = new List<Reinforcement.SurfaceReinforcementParameters>();

        [XmlElement("surface_reinforcement", Order = 10)]
        public List<Reinforcement.SurfaceReinforcement> SurfaceReinforcement = new List<Reinforcement.SurfaceReinforcement>();

        [XmlElement("punching_area", Order = 11)]
        public List<Reinforcement.PunchingArea> PunchingArea { get; set; }

        [XmlElement("punching_reinforcement", Order = 12)]
        public List<Reinforcement.PunchingReinforcement> PunchingReinforcement { get; set; }

        [XmlElement("no-shear_region", Order = 13)]
        public Reinforcement.NoShearRegionType[] NoShearRegion { get; set; }

        [XmlElement("panel", Order = 14)]
        public List<Shells.Panel> Panels = new List<Shells.Panel>();

        [XmlElement("post-tensioned_cable", Order = 15)]
        public Reinforcement.Ptc[] PostTensionedCable { get; set; }

        [XmlElement("loads", Order = 16)]
        public Loads.Loads Loads = new Loads.Loads();

        [XmlElement("supports", Order = 17)]
        public Supports.Supports Supports = new Supports.Supports();

        [XmlElement("advanced-fem", Order = 18)]
        public AdvancedFem AdvancedFem = new AdvancedFem();

        [XmlElement("storeys", Order = 19)]
        public StructureGrid.Storeys Storeys { get; set; }
        
        [XmlElement("axes", Order = 20)]
        public StructureGrid.Axes Axes { get; set; }

        // axes
        // ref planes
        // tsolids
        // regions
    }
}