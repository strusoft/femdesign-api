
namespace FemDesign.Loads
{
    /// <summary>
    /// Used for keeping track of the type of load combination
    /// </summary>
    public enum ELoadCombinationType
    {
        /// <summary> 6.10a in table B-3 in EKS 11 </summary>
        SixTenA,
        /// <summary> 6.10b in table B-3 in EKS 11 </summary>
        SixTenB,
        /// <summary> Characteristic combination from A1.4 </summary>
        Characteristic,
        /// <summary> Frequent combination from A1.4 </summary>
        Frequent,
        /// <summary> Quasi Permanent combination from A1.4 </summary>
        QuasiPermanent,
    }
}

