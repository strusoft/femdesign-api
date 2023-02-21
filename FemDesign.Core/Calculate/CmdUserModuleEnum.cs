using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FemDesign.GenericClasses;

namespace FemDesign.Calculate
{
    /// <summary>
    /// cmdUserModule
    /// </summary>
    public enum CmdUserModule
    {
        /// <summary>
        /// RESMODE
        /// </summary>
        [Parseable("RESMODE", "res", "RES", "Res")]
        RESMODE,

        /// <summary>
        /// RCDESIGN
        /// </summary>
        [Parseable("RCDESIGN", "rc", "RC", "Rc")]
        RCDESIGN,

        /// <summary>
        /// STEELDESIGN
        /// </summary>
        [Parseable("STEELDESIGN", "steel", "STEEL", "Steel")]
        STEELDESIGN,

        /// <summary>
        /// TIMBERDESIGN
        /// </summary>
        [Parseable("TIMBERDESIGN", "timber", "TIMBER", "Timber")]
        TIMBERDESIGN
    }
}