using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.Materials
{
    [System.Serializable()]
    [IsVisibleInDynamoLibrary(false)]
    public class GlcDataType
    {
        [XmlElement("layer", Order = 1)]
        public List<MechProps> Layers { get; set; }

        public GlcDataType()
        {

        }
    }
}