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
        // connected_points

        // connected_lines

        // surface_connection

        // virtual_bar
        [XmlElement("virtual_bar", Order = 1)]
        public List<ModellingTools.FictitiousBar> FictitiousBar = new List<ModellingTools.FictitiousBar>();

        // virtual_shell
        [XmlElement("virtual_shell", Order = 2)]
        public List<ModellingTools.FictitiousShell> FictitiousShell = new List<ModellingTools.FictitiousShell>();

        // diaphragm

        // steel_joint

        /// <summary>
        /// List of Cover (cover_type)
        /// </summary>
        [XmlElement("cover", Order = 3)]
        public List<Cover> Cover = new List<Cover>();
    }
}