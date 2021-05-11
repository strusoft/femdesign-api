// https://strusoft.com/
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Autodesk.DesignScript.Runtime;

namespace FemDesign.Bars
{
    /// <summary>
    /// BarType enum
    /// </summary>
    [IsVisibleInDynamoLibrary(false)]
    [System.Serializable]
    public enum BarType
    {
        [XmlEnum("beam")]
        Beam,
        [XmlEnum("column")]
        Column,
        [XmlEnum("truss")]
        Truss
    };
}
