namespace FemDesign.Loads
{
    /// <summary>
    /// Used for keeping track the load group type
    /// </summary>
    public enum ELoadGroupType
    {
        /// <summary> Permanent </summary>
        Permanent,
        /// <summary> Variable </summary>
        Variable,
    }

    /// <summary>
    /// Used for keeping track of the relationsship of the load cases in a group
    /// </summary>
    public enum ELoadGroupRelation
    {
        /// <summary> If all cases are to be applied together </summary>
        Entire,
        /// <summary> If all cases are to be applied mutually exclusive </summary>
        Alternative,
    }

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