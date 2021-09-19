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
        /// <summary> Correlated </summary>
        Correlated,
        /// <summary> Uncorrelated </summary>
        Uncorrelated,
    }
}