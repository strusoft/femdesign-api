using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.Loads
{
    /// <summary>
    /// Used for keeping track of the relationsship of the load cases in a group
    /// </summary>
    public enum ELoadGroupRelationship
    {
        /// <summary> If all cases are to be applied together </summary>
        Entire,
        /// <summary> If all cases are to be applied mutually exclusive </summary>
        Alternative,
    }
}
