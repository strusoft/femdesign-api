// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.ModellingTools
{
    /// <summary>
    /// Connections and virtual objects
    /// </summary>
    [System.Serializable]
    public partial class AdvancedFem
    {
        [XmlElement("connected_points", Order = 1)]
        public List<ConnectedPoints> ConnectedPoints { get; set; } = new List<ConnectedPoints>();

        [XmlElement("connected_lines", Order = 2)]
        public List<ConnectedLines> ConnectedLines { get; set; } = new List<ConnectedLines>();

        [XmlElement("surface_connection", Order = 3)]
        public List<SurfaceConnection> SurfaceConnections { get; set; } = new List<SurfaceConnection>();

        [XmlElement("virtual_bar", Order = 4)]
        public List<FictitiousBar> FictitiousBars = new List<FictitiousBar>();

        [XmlElement("virtual_shell", Order = 5)]
        public List<FictitiousShell> FictitiousShells = new List<FictitiousShell>();

        [XmlElement("diaphragm", Order = 6)]
        public List<Diaphragm> Diaphragms { get; set; } = new List<Diaphragm>();


        [XmlElement("steel_joint", Order = 7)]
        public List<StruSoft.Interop.StruXml.Data.Steel_joint_type> SteelJointType { get; set; }

        /// <summary>
        /// List of Cover (cover_type)
        /// </summary>
        [XmlElement("cover", Order = 8)]
        public List<Cover> Covers = new List<Cover>();

    }
}