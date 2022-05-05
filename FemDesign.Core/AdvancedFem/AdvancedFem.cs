// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign
{
    /// <summary>
    /// Connections and virtual objects
    /// </summary>
    [System.Serializable]
    public partial class AdvancedFem
    {
        [XmlElement("connected_points", Order = 1)]
        public List<ModellingTools.ConnectedPoints> ConnectedPoints { get; set; } = new List<ModellingTools.ConnectedPoints>();

        [XmlElement("connected_lines", Order = 2)]
        public List<ModellingTools.ConnectedLines> ConnectedLines { get; set; } = new List<ModellingTools.ConnectedLines>();

        [XmlElement("surface_connection", Order = 3)]
        public ModellingTools.SurfaceConnection[] SurfaceConnections { get; set; }

        [XmlElement("virtual_bar", Order = 4)]
        public List<ModellingTools.FictitiousBar> FictitiousBars = new List<ModellingTools.FictitiousBar>();

        [XmlElement("virtual_shell", Order = 5)]
        public List<ModellingTools.FictitiousShell> FictitiousShells = new List<ModellingTools.FictitiousShell>();

        [XmlElement("diaphragm", Order = 6)]
        public List<ModellingTools.Diaphragm> Diaphragms { get; set; } = new List<ModellingTools.Diaphragm>();


        [XmlElement("steel_joint", Order = 7)]
        public List<StruSoft.Interop.StruXml.Data.Steel_joint_type> SteelJointType { get; set; }


        /// <summary>
        /// List of Cover (cover_type)
        /// </summary>
        [XmlElement("cover", Order = 8)]
        public List<Cover> Covers = new List<Cover>();

    }
}