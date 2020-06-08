// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

namespace FemDesign
{
    /// <summary>
    /// entities.
    /// </summary>
    public class Entities
    {
        // dummy elements are needed to deserialize an .struxml model correctly as order of elements is needed.
        // if dummy elements are not used for undefined types deserialization will not work properly
        // when serializing these dummy elements must be nulled. 
        [XmlElement("foundations", Order = 1)]
        public List<DummyXmlObject> foundations {get {return null;} set {value = null;}}
        [XmlElement("bar", Order = 2)]
        public List<Bars.Bar> bar = new List<Bars.Bar>();
        [XmlElement("column_corbel", Order = 3)]
        public List<DummyXmlObject> columnCorbel {get {return null;} set {value = null;}}
        [XmlElement("hidden_bar", Order = 4)]
        public List<DummyXmlObject> hiddenBar {get {return null;} set {value = null;}}
        [XmlElement("bar_reinforcement", Order = 5)]
        public List<DummyXmlObject> barReinforcement {get {return null;} set {value = null;}}
        [XmlElement("slab", Order = 6)]
        public List<Shells.Slab> slab = new List<Shells.Slab>();
        [XmlElement("shell_buckling", Order = 7)]
        public List<DummyXmlObject> shellBuckling {get {return null;} set {value = null;}}
        [XmlElement("wall_corbel", Order = 8)]
        public List<DummyXmlObject> wallCorbel {get {return null;} set {value = null;}}
        [XmlElement("surface_reinforcement_parameters", Order = 9)]
        public List<Reinforcement.SurfaceReinforcementParameters> surfaceReinforcementParameters = new List<Reinforcement.SurfaceReinforcementParameters>();
        [XmlElement("surface_reinforcement", Order = 10)]
        public List<Reinforcement.SurfaceReinforcement> surfaceReinforcement = new List<Reinforcement.SurfaceReinforcement>();
        [XmlElement("punching_area", Order = 11)]
        public List<DummyXmlObject> punchingArea {get {return null;} set {value = null;}}
        [XmlElement("punching_reinforcement", Order = 12)]
        public List<DummyXmlObject> punchingReinforcement {get {return null;} set {value = null;}}
        [XmlElement("no-shear_region", Order = 13)]
        public List<DummyXmlObject> noShearRegion {get {return null;} set {value = null;}}
        [XmlElement("panel", Order = 14)]
        public List<DummyXmlObject> panel {get {return null;} set {value = null;}}
        [XmlElement("post-tensioned_cable", Order = 15)]
        public List<DummyXmlObject> postTensionedCable {get {return null;} set {value = null;}}
        [XmlElement("loads", Order = 16)]
        public Loads.Loads loads = new Loads.Loads();
        [XmlElement("supports", Order = 17)]
        public Supports.Supports supports = new Supports.Supports();
        [XmlElement("advanced-fem", Order = 18)]
        public AdvancedFem advancedFem = new AdvancedFem();

        // storeys
        // axes
        // ref planes
        // tsolids
        // regions
    }
}