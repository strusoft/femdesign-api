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
        public ModellingTools.ConnectedPoints[] ConnectedPoints { get; set; }

        [XmlElement("connected_lines", Order = 2)]
        public ModellingTools.ConnectedLines[] ConnectedLines { get; set; }

        // surface_connection

        // virtual_bar
        [XmlElement("virtual_bar", Order = 3)]
        public List<ModellingTools.FictitiousBar> FictitiousBars = new List<ModellingTools.FictitiousBar>();

        // virtual_shell
        [XmlElement("virtual_shell", Order = 4)]
        public List<ModellingTools.FictitiousShell> FictitiousShells = new List<ModellingTools.FictitiousShell>();

        // diaphragm
        [XmlElement("diaphragm", Order = 5)]
        public ModellingTools.Diaphragm[] Diaphragms { get; set; }

        // steel_joint

        /// <summary>
        /// List of Cover (cover_type)
        /// </summary>
        [XmlElement("cover", Order = 6)]
        public List<Cover> Covers = new List<Cover>();
    }
}