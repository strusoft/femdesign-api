// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;

#region dynamo
using Autodesk.DesignScript.Runtime;
#endregion

namespace FemDesign.LibraryItems
{
    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class PointConnectionTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public Releases.RigidityDataLibType2[] PredefinedTypes { get; set; }
    }

    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class PointSupportGroupTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public Releases.RigidityDataLibType2[] PredefinedTypes { get; set; }
    }

    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class LineConnectionTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType3> PredefinedTypes { get; set; }
    }

    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class LineSupportGroupTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public Releases.RigidityDataLibType2[] PredefinedTypes { get; set; }
    }

    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class SurfaceConnectionTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public Releases.RigidityDataLibType1[] PredefinedTypes { get; set; }
    }

    [System.Serializable]
    [IsVisibleInDynamoLibrary(false)]
    public class SurfaceSupportTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public Releases.RigidityDataLibType1[] PredefinedTypes { get; set; }
    }
}
