// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;
#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Sections
{
    /// <summary>
    /// Sections container used in database.
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    public class DatabaseSections
    {
        [XmlElement("section")]
        public List<Section> Section = new List<Section>();
    }
}