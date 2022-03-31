// https://strusoft.com/

using System.Collections.Generic;
using System.Xml.Serialization;


namespace FemDesign.LibraryItems
{
    [System.Serializable]
    public partial class PointConnectionTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType2> PredefinedTypes { get; set; }
    }

    [System.Serializable]
    public partial class PointSupportGroupTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType2> PredefinedTypes { get; set; }
    }

    [System.Serializable]
    public partial class LineConnectionTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType3> PredefinedTypes { get; set; }
    }

    [System.Serializable]
    public partial class LineSupportGroupTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType2> PredefinedTypes { get; set; }
    }

    [System.Serializable]
    public partial class SurfaceConnectionTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType1> PredefinedTypes { get; set; }
    }

    [System.Serializable]
    public partial class SurfaceSupportTypes
    {
        [XmlElement("predefined_type", Order = 1)]
        public List<Releases.RigidityDataLibType1> PredefinedTypes { get; set; }
    }
}
