// https://strusoft.com/
using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign
{
    /// <summary>
    /// Connections and virtual objects
    /// </summary>
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class AdvancedFem
    {        
        [XmlElement("connected_points", Order = 1)]
        public List<ModellingTools.ConnectedPoints> ConnectedPoints { get; set; }

        [XmlElement("connected_lines", Order = 2)]
        public List<ModellingTools.ConnectedLines> ConnectedLines { get; set; }

        [XmlElement("surface_connection", Order = 3)]
        public ModellingTools.SurfaceConnection[] SurfaceConnections { get; set; }

        [XmlElement("virtual_bar", Order = 4)]
        public List<ModellingTools.FictitiousBar> FictitiousBars = new List<ModellingTools.FictitiousBar>();

        [XmlElement("virtual_shell", Order = 5)]
        public List<ModellingTools.FictitiousShell> FictitiousShells = new List<ModellingTools.FictitiousShell>();

        [XmlElement("diaphragm", Order = 6)]
        public ModellingTools.Diaphragm[] Diaphragms { get; set; }

        // steel_joint

        /// <summary>
        /// List of Cover (cover_type)
        /// </summary>
        [XmlElement("cover", Order = 7)]
        public List<Cover> Covers = new List<Cover>();
    }
}