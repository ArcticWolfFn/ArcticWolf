using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCord.Models.Enums
{
    /// <summary>
    /// Status reported by bot comamnds and controllers.
    /// </summary>
    public enum StatusReport
    {
        OK,
        Warning,
        FatalError,

        /// <summary>
        /// The initialization did not complete and therefore this operation cannot be performed.
        /// </summary>
        InitNotDone,
        InitAlreadyDone,
    }
}
