using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCord.Controllers
{
    /// <summary>
    /// Base class for controllers.
    /// </summary>
    public abstract class ControllerBase
    {
        /// <summary>
        /// Indicates whether the controller was successfully initialized.
        /// </summary>
        protected bool InitDone = false;

        /// <summary>
        /// Initializes the controller. Usually called by the MainController.
        /// </summary>
        /// <returns>Controller Initialization Status</returns>
        public abstract Models.Enums.StatusReport Init();

        /// <summary>
        /// Shutdowns the controller. Usually called by the MainController.
        /// </summary>
        /// <returns>Controller Shutdown Status</returns>
        public abstract Models.Enums.StatusReport ShutDown();
    }
}
