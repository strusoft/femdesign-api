// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.LibraryItems
{
    [System.Serializable]
    public class PointConnectionTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType2> PredefinedTypes { get; set; }
    }

    [System.Serializable]
    public class PointSupportGroupTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType2> PredefinedTypes { get; set; }
    }

    [System.Serializable]
    public class LineConnectionTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType3> PredefinedTypes { get; set; }
    }

    [System.Serializable]
    public class LineSupportGroupTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType2> PredefinedTypes { get; set; }
    }

    [System.Serializable]
    public class SurfaceConnectionTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType1> PredefinedTypes { get; set; }
    }

    [System.Serializable]
    public class SurfaceSupportTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType1> PredefinedTypes { get; set; }
    }
}
