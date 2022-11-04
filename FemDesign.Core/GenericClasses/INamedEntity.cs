// https://strusoft.com/

namespace FemDesign
{
    /// <summary>
    /// Entities with a name/identifier. E.g "B.42" or "@MyLockedName.1"
    /// </summary>
    public interface INamedEntity
    {
        /// <summary>
        /// Name of the entity. E.g. "B.42"
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Instance number.
        /// </summary>
        int Instance { get; }

        /// <summary>
        /// Identifier part of the name.
        /// </summary>
        string Identifier { get; set; }

        /// <summary>
        /// When true, FEM-Design will not modify the Instance number of the Name. Note that this might not always be possible. 
        /// 
        /// See https://github.com/strusoft/femdesign-api/issues/81#issuecomment-1250848165 for more info.
        /// </summary>
        bool LockedIdentifier { get; set; }
    }
}