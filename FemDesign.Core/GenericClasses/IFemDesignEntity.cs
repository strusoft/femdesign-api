using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FemDesign.GenericClasses
{
    /// <summary>
    /// Elements found in FEM-Design 3D Structure
    /// </summary>
    public interface IFemDesignEntity
    {
        /// <summary>
        /// Invoke when an instance is created.
        /// </summary>
        void EntityCreated();
    }
}
