using System.Xml.Serialization;


namespace FemDesign.Materials
{
    [System.Serializable]
    public enum MaterialTypeEnum
    {
        SteelRolled  = 0,
        SteelColdWorked = 1,
        SteelWelded = 2,
        Concrete = 3,
        Timber = 4,
        Unknown = 65535,
        Undefined = -1
    }
}